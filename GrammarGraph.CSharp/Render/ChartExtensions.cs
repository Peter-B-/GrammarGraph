using Plotly.NET;

namespace GrammarGraph.CSharp.Render;

public static class ChartExtensions
{
    public static GenericChart.GenericChart Plot<T>(this GgChart<T> chart)
    {
        var engine = new PlotlyRenderEngine();

        return engine.Render(chart);
    }
}
