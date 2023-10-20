using System.Collections.Immutable;
using GrammarGraph.CSharp.Internal;

namespace GrammarGraph.CSharp.Render;

public class PlotBuilder
{
    public PlotDescription BuildPlot<T>(GgChart<T> chart)
    {
        var combinedLayers = chart.Layers
            .Select(layer =>
                layer with { Aesthetics = CombineAesthetics(chart, layer) }
            )
            .ToImmutableArray();


        var rawLayerData = combinedLayers
            .Select(layer => GetRawData(layer, chart.Data))
            .ToImmutableArray();

        var layerData = rawLayerData
            .Select(ApplyStatistics)
            .ToImmutableArray();

        var plot = new PlotDescription(
            layerData.Select(ld => ld.Data)
                .ToImmutableArray()
        );
        return plot;
    }


    private static DataColumn CreateDataColumn<T>(IEnumerable<T> data, Mapping<T> mapping)
    {
        var objectType = PlotlyRenderEngine.GetObjectType(mapping.Expression);
        var accessor = mapping.Expression.Compile();

        if (!mapping.AsFactor)
            if (objectType == typeof(double) || objectType == typeof(float) || objectType == typeof(int))
            {
                Func<T, double> extract = objectType switch
                {
                    _ when objectType == typeof(double) => d => (double)accessor(d),
                    _ when objectType == typeof(float) => d => (float)accessor(d),
                    _ when objectType == typeof(int) => d => (int)accessor(d)
                };

                var values = data
                    .Select(d => extract(d))
                    .ToImmutableArray();
                return new DoubleColumn(values);
            }

        if (objectType == typeof(string))
        {
            var values = data
                .Select(d => (string)accessor(d));
            return FactorColumn.FromStrings(values);
        }

        {
            var values = data
                .Select(d => accessor(d).ToString() ?? "na.")
                .ToImmutableArray();
            return FactorColumn.FromStrings(values);
        }
    }

    private static LayerData<T> GetRawData<T>(Layer<T> layer, IEnumerable<T> data)
    {
        var columns =
            layer.Aesthetics
                .Select(kvp =>
                {
                    var (aestheticsId, mapping) = kvp;

                    var dataColumn = CreateDataColumn(data, mapping);
                    return (Key: aestheticsId, Data: dataColumn);
                })
                .ToImmutableDictionary(m => m.Key, m => m.Data);
        return new LayerData<T>(layer, new DataFrame(columns));
    }


    private LayerData<T> ApplyStatistics<T>(LayerData<T> layerData)
    {
        var dataFrame = layerData.Layer.Stat.Compute(layerData.Data);
        return layerData with { Data = dataFrame };
    }

    private ImmutableDictionary<AestheticsId, Mapping<T>> CombineAesthetics<T>(GgChart<T> chart, Layer<T> layer)
    {
        return chart.Aesthetics.SetItems(layer.Aesthetics);
    }
}
