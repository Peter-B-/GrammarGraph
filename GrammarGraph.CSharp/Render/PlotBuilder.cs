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

        var facet = chart.Facet ?? new SingleFacet<T>();
        var panels = facet.GetPanels(chartData);

        var rawLayerData = combinedLayers
            .Select(layer => new
            {
                Layer = GetRawData(layer, chartData, facet, panels),
                LayerDefinition = layer,
            })
            .ToImmutableArray();

        var layerData = rawLayerData
            .Select(item => ApplyStatistics(item.Layer, item.LayerDefinition))
            .ToImmutableArray();

        var plot = new PlotDescription(
            panels,
            layerData
        );
        return plot;
    }


    public static DataColumn CreateDataColumn<T>(IEnumerable<T> data, Mapping<T> mapping)
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


    private Layer ApplyStatistics<T>(Layer layer, Layer<T> layerDefinition)
    {
        var dataFrame = layerDefinition.Stat.Compute(layer.Data);
        return layer with { Data = dataFrame };
    }

    private ImmutableDictionary<AestheticsId, Mapping<T>> CombineAesthetics<T>(GgChart<T> chart, Layer<T> layer) =>
        chart.Aesthetics
            .SetItems(layer.Aesthetics);


    private static Layer GetRawData<T>(
        Layer<T> layer, IReadOnlyList<T> data,
        Facet<T> facet, ImmutableArray<Panel> panels
    )
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

        var panelMap = facet.AssignToPanels(data, panels);

        var group = Group.Default;
        var groups = ImmutableArray.Create(group);

        var groupMap = panelMap.Select(_ => group)
            .ToImmutableArray();

        return new Layer(groups, new DataFrame(columns, panelMap, groupMap));
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
