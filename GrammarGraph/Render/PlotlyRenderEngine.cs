using System.Collections.Immutable;
using System.Linq.Expressions;
using GrammarGraph.Internal;
using Microsoft.FSharp.Core;
using Plotly.NET;

namespace GrammarGraph.Render;

public class PlotlyRenderEngine
{
    private static readonly ImmutableArray<Color> Colors =
        ImmutableArray.Create(Color.fromRGB(166, 206, 227), Color.fromRGB(31, 120, 180), Color.fromRGB(178, 223, 138), Color.fromRGB(51, 160, 44),
            Color.fromRGB(251, 154, 153), Color.fromRGB(227, 26, 28), Color.fromRGB(253, 191, 111), Color.fromRGB(255, 127, 0), Color.fromRGB(202, 178, 214), Color.fromRGB(106, 61, 154),
            Color.fromRGB(255, 255, 153), Color.fromRGB(177, 89, 40));

    public static Type GetObjectType<T>(Expression<Func<T, object>> expr)
    {
        if (expr.Body.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked)
            if (expr.Body is UnaryExpression unary)
                return unary.Operand.Type;

        return expr.Body.Type;
    }

    public GenericChart.GenericChart Render(PlotDescription plot)
    {
        var firstLayer = plot.Layers.First();

        var data = firstLayer.Data;


        var xType = data[AestheticsId.X].Type;
        var yType = data[AestheticsId.Y].Type;

        var color = data.TryGetColumn(AestheticsId.Color) switch
        {
            null => FSharpOption<Color>.None,
            DoubleColumn doubleColumn => FSharpOption<Color>.None,
            FactorColumn factorColumn => MapToColor(factorColumn)
        };

        var resultChart = (xType, yType) switch
        {
            (DataColumnType.Double, DataColumnType.Double) =>
                Chart2D.Chart.Point<double, double, string>(
                    data.GetDoubleColumn(AestheticsId.X).Values,
                    data.GetDoubleColumn(AestheticsId.Y).Values,
                    MarkerColor: color
                ),
            (DataColumnType.Factor, DataColumnType.Double) =>
                Chart2D.Chart.Point<string, double, string>(
                    data.GetFactorColumn(AestheticsId.X).Values,
                    data.GetDoubleColumn(AestheticsId.Y).Values,
                    MarkerColor: color
                ),
            (DataColumnType.Double, DataColumnType.Factor) =>
                Chart2D.Chart.Point<double, string, string>(
                    data.GetDoubleColumn(AestheticsId.X).Values,
                    data.GetFactorColumn(AestheticsId.Y).Values,
                    MarkerColor: color
                ),
            (DataColumnType.Factor, DataColumnType.Factor) =>
                Chart2D.Chart.Point<string, string, string>(
                    data.GetFactorColumn(AestheticsId.X).Values,
                    data.GetFactorColumn(AestheticsId.Y).Values,
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
}
