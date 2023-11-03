using System.Collections.Immutable;
using System.Linq.Expressions;
using GrammarGraph.Extensions;
using GrammarGraph.Internal;
using Microsoft.FSharp.Core;
using Plotly.NET;

namespace GrammarGraph.Render;

public class PlotlyRenderEngine
{
    private static readonly ImmutableArray<Color> Colors =
        ImmutableArray.Create(Color.fromRGB(166, 206, 227), Color.fromRGB(31, 120, 180), Color.fromRGB(178, 223, 138), Color.fromRGB(51, 160, 44),
                              Color.fromRGB(251, 154, 153), Color.fromRGB(227, 26, 28), Color.fromRGB(253, 191, 111), Color.fromRGB(255, 127, 0), Color.fromRGB(202, 178, 214),
                              Color.fromRGB(106, 61, 154),
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
        var layerCharts =
        plot.Layers
            .SelectMany(layer =>
                        layer.Data.Group()
                            .GroupBy(g => g.Panel)
                            .Select(gr => new PanelChart(gr.Key, RenderGroups(gr, layer.Geometry)))
                            .ToList()
                        );

        var combinedChartDict = layerCharts
            .GroupBy(lc => lc.Panel)
            .Select(gr => new PanelChart(gr.Key, Chart.Combine(gr.Select(g => g.Traces))))
            .ToDictionary(pc => pc.Panel, pc => pc.Traces);


        var panelCharts = plot.Panels.Panels
            .Select(p => combinedChartDict.TryGetValue(p, out var traces)?traces:Chart.Invisible())
            .ToList();

        return
            Chart.Grid<IEnumerable<GenericChart.GenericChart>>(
                    plot.Panels.Rows,
                    plot.Panels.Columns,
                    Pattern: FSharpOption<StyleParam.LayoutGridPattern>.Some(StyleParam.LayoutGridPattern.Coupled)
                )
                .Invoke(panelCharts);
    }

    private GenericChart.GenericChart CreateChart(PanelGroupData data, IGeometryLogic geometry)
    {
        var resultChart = geometry.CreateChart(data);

        var traceName = data.Group.Identifiers
            .Select(id => id.Factor.Value)
            .JoinStrings(", ")
            ;
        var showLegend = data.Group.Identifiers.Any();

        resultChart.WithTraceInfo(FSharpOption<string>.Some(traceName),
                                  ShowLegend: FSharpOption<bool>.Some(showLegend)
                                  );
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

    private GenericChart.GenericChart RenderGroups(IEnumerable<PanelGroupData> data, IGeometryLogic geometry)
    {
        var groupCharts = data
            .Select(groupData => CreateChart(groupData, geometry));

        return Chart.Combine(groupCharts);
    }
}

public record PanelChart(Panel Panel, GenericChart.GenericChart Traces);
