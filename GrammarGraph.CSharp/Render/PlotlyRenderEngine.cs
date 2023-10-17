using System.Collections.Immutable;
using System.Globalization;
using GrammarGraph.CSharp.Internal;
using Plotly.NET;

namespace GrammarGraph.CSharp.Render;

public class PlotlyRenderEngine
{
    public GenericChart.GenericChart Render<T>(GgChart<T> chart)
    {
        var combinedLayers = chart.Layers
            .Select(layer =>
                        layer with {Aesthetics = CombineAesthetics(chart, layer)}
            )
            .ToImmutableArray();


        var rawLayerData = combinedLayers
            .Select(layer => GetRawData(layer, chart.Data))
            .ToImmutableArray();

        var layerData = rawLayerData
            .Select(ApplyStatistics)
            .ToImmutableArray();

        var firstLayer = layerData.First();

        return Chart2D.Chart.Point<double, double, string>(
            firstLayer.Data.GetDoubleColumn(AestheticsId.X).Values,
            firstLayer.Data.GetDoubleColumn(AestheticsId.Y).Values
        );
    }

    private LayerData<T> ApplyStatistics<T>(LayerData<T> layerData)
    {
        var dataFrame = layerData.Layer.Stat.Compute(layerData.Data);
        return layerData with {Data = dataFrame};
    }

    private ImmutableDictionary<AestheticsId, Mapping<T>> CombineAesthetics<T>(GgChart<T> chart, Layer<T> layer)
    {
        return chart.Aesthetics.SetItems(layer.Aesthetics);
    }

    private ImmutableArray<Layer<T>> GetLayers<T>(GgChart<T> chart)
    {
        return
            chart.Layers
                .Select(layer =>
                            layer with {Aesthetics = CombineAesthetics(chart, layer)}
                )
                .ToImmutableArray();
    }

    private static LayerData<T> GetRawData<T>(Layer<T> layer, IEnumerable<T> data)
    {
        var columns =
            layer.Aesthetics
                .Select(mapping =>
                {
                    var compile = mapping.Value.Expression.Compile();
                    var values = data
                        .Select(d => compile(d))
                        .Select(d => d.ToDouble(CultureInfo.InvariantCulture))
                        .ToImmutableArray();
                    return (mapping.Key, Data: (DataColumn) new DoubleColumn(values));
                })
                .ToImmutableDictionary(m => m.Key, m => m.Data);
        return new LayerData<T>(layer, new DataFrame(columns));
    }
}

public record LayerData<T>(
    Layer<T> Layer,
    DataFrame Data
);
