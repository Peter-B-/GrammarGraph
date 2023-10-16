using System.Collections.Immutable;
using System.Globalization;
using GrammarGraph.CSharp.Internal;
using Plotly.NET;

namespace GrammarGraph.CSharp.Render;

public class PlotlyRenderEngine
{
    public GenericChart.GenericChart Render<T>(GgChart<T> chart)
    {
        var layers = GetLayers(chart);

        var rawData = layers
            .Select(layer => new
            {
                Layer = layer,
                Data = CreateDataTable(chart.Data, layer.Aesthetics)
            })
            .ToImmutableArray();


        var firstLayer = rawData.First();

        return Chart2D.Chart.Point<double, double, string>(
            (firstLayer.Data.Columns[AestheticsId.X] as DoubleColumn).Values,
            (firstLayer.Data.Columns[AestheticsId.Y] as DoubleColumn).Values
            );
    }

    private DataTable CreateDataTable<T>(IEnumerable<T> data, ImmutableDictionary<AestheticsId, Mapping<T>> aesthetics)
    {
        var columns =
            aesthetics
                .Select(mapping =>
                {
                    var compile = mapping.Value.Expression.Compile();
                    var values = data
                        .Select(d => compile(d))
                        .Select(d => d.ToDouble(CultureInfo.InvariantCulture))
                        .ToImmutableArray();
                    return (mapping.Key, Data: (DataColumn)new DoubleColumn(values));
                })
                .ToImmutableDictionary(m => m.Key, m => m.Data);

        return new DataTable(columns);
    }

    private ImmutableArray<Layer<T>> GetLayers<T>(GgChart<T> chart)
    {
        return
            chart.Layers
                .Select(layer =>
                    layer with { Aesthetics = CombineAesthetics(chart, layer) }
                )
                .ToImmutableArray();
    }

    private ImmutableDictionary<AestheticsId, Mapping<T>> CombineAesthetics<T>(GgChart<T> chart, Layer<T> layer)
    {
        return chart.Aesthetics.SetItems(layer.Aesthetics);
    }
}
