using System.Collections.Immutable;
using System.Linq.Expressions;
using GrammarGraph.CSharp.Internal;
using Microsoft.FSharp.Core;
using Plotly.NET;

namespace GrammarGraph.CSharp.Render;

public class PlotlyRenderEngine
{
    private static readonly ImmutableArray<Color> Colors = ImmutableArray.Create(Color.fromRGB(166, 206, 227), Color.fromRGB(31, 120, 180), Color.fromRGB(178, 223, 138), Color.fromRGB(51, 160, 44),
        Color.fromRGB(251, 154, 153), Color.fromRGB(227, 26, 28), Color.fromRGB(253, 191, 111), Color.fromRGB(255, 127, 0), Color.fromRGB(202, 178, 214), Color.fromRGB(106, 61, 154),
        Color.fromRGB(255, 255, 153), Color.fromRGB(177, 89, 40));

    public static Type GetObjectType<T>(Expression<Func<T, object>> expr)
    {
        if (expr.Body.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked)
            if (expr.Body is UnaryExpression unary)
                return unary.Operand.Type;

        return expr.Body.Type;
    }

    public GenericChart.GenericChart Render<T>(GgChart<T> chart)
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

        var firstLayer = layerData.First();


        var xType = firstLayer.Data[AestheticsId.X].Type;
        var yType = firstLayer.Data[AestheticsId.Y].Type;

        var color = firstLayer.Data.TryGetColumn(AestheticsId.Color) switch
        {
            null => FSharpOption<Color>.None,
            DoubleColumn doubleColumn => FSharpOption<Color>.None,
            FactorColumn factorColumn => MapToColor(factorColumn)
        };

        var resultChart = (xType, yType) switch
        {
            (DataColumnType.Double, DataColumnType.Double) =>
                Chart2D.Chart.Point<double, double, string>(
                    firstLayer.Data.GetDoubleColumn(AestheticsId.X).Values,
                    firstLayer.Data.GetDoubleColumn(AestheticsId.Y).Values,
                    MarkerColor: color
                ),
            (DataColumnType.Factor, DataColumnType.Double) =>
                Chart2D.Chart.Point<string, double, string>(
                    firstLayer.Data.GetFactorColumn(AestheticsId.X).Values,
                    firstLayer.Data.GetDoubleColumn(AestheticsId.Y).Values,
                    MarkerColor: color
                ),
            (DataColumnType.Double, DataColumnType.Factor) =>
                Chart2D.Chart.Point<double, string, string>(
                    firstLayer.Data.GetDoubleColumn(AestheticsId.X).Values,
                    firstLayer.Data.GetFactorColumn(AestheticsId.Y).Values,
                    MarkerColor: color
                ),
            (DataColumnType.Factor, DataColumnType.Factor) =>
                Chart2D.Chart.Point<string, string, string>(
                    firstLayer.Data.GetFactorColumn(AestheticsId.X).Values,
                    firstLayer.Data.GetFactorColumn(AestheticsId.Y).Values,
                    MarkerColor: color
                )
        };

        return resultChart;
    }

    private FSharpOption<Color> MapToColor(FactorColumn factor)
    {
        var colorMap =
            factor.Values
                .Distinct()
                .Select((v, i) => new { Key = v, Color = Colors[i] })
                .ToDictionary(x => x.Key, x => x.Color);

        var colors =
            factor.Values
                .Select(v => colorMap[v]);

        return FSharpOption<Color>.Some(Color.fromColors(colors));
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

    private static DataColumn CreateDataColumn<T>(IEnumerable<T> data, Mapping<T> mapping)
    {
        var objectType = GetObjectType(mapping.Expression);
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
}

public record LayerData<T>(
    Layer<T> Layer,
    DataFrame Data
);
