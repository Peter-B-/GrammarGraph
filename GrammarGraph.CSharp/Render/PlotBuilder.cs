using System.Collections.Immutable;
using GrammarGraph.CSharp.Facets;
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

        var chartData = chart.Data as IReadOnlyList<T> ?? chart.Data.ToList();

        //var panelInfo = GetPanelInfo(chart);
        //GetPanels(panelInfo, combinedLayers, chartData);


        var rawLayerData = combinedLayers
            .Select(layer => GetRawData(layer, chartData))
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

    //private ImmutableArray<PanelData<T>> GetPanels<T>(PanelInfo<T> panelInfo, ImmutableArray<Layer<T>> combinedLayers, IReadOnlyList<T> chartData)
    //{
    //    chartData
    //        .GroupBy()

    //    return Enumerable.Range(0, panelInfo.Panels)
    //        .Select(panel =>
    //            new PanelData<T>(panelInfo.GetRow(panel), panelInfo.GetColumn(panel),
    //                panelInfo.Labels[panel],
    //                layers)
    //        )
    //        .ToImmutableArray();
    //}

    private PanelInfo<T> GetPanelInfo<T>(GgChart<T> chart)
    {
        return chart.Facet switch
        {
            null => PanelInfo<T>.Single,
            GridFaced<T> gridFaced => throw new NotImplementedException(),
            WrapFaced<T> wrapFaced => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };
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
        return chart.Aesthetics
            .SetItems(layer.Aesthetics)
            .SetItems(chart.GetFacetAesthetics());
    }
}

internal static class ChartExtensionsIntern
{
    public static IEnumerable<KeyValuePair<AestheticsId, Mapping<T>>> GetFacetAesthetics<T>(this GgChart<T> chart)
    {
        return chart.Facet?.GetAesthetics() ?? Enumerable.Empty<KeyValuePair<AestheticsId, Mapping<T>>>();
    }
}

public record PanelData<T>(
    int Row,
    int Column,
    PanelLabel Label,
    ImmutableArray<LayerData<T>> Layers)
{
}

public record PanelInfo<T>(
    int Panels,
    int Rows,
    int Columns,
    ImmutableDictionary<int, PanelLabel> Labels,
    Func<T, int> GetPanelId
)
{
    public static PanelInfo<T> Single => new(
        1,
        1, 1,
        ImmutableDictionary<int, PanelLabel>.Empty.Add(0, new PanelLabel(null, null)),
        _ => 0
    );
}

public record PanelLabel(string? RowLabel, string? ColumnLabel);
