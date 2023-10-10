
using Plotly.NET;

namespace GrammarGraph.CSharp;

public static class ChartExtensions
{
    public static GenericChart.GenericChart Plot<T>(this GgChart<T> chart)
    {
        var engine = new PlotlyRenderEngine();

        return engine.Render(chart);
    }
}

public class PlotlyRenderEngine
{
    public GenericChart.GenericChart Render<T>(GgChart<T> chart)
    {
        var X = Enumerable.Range(0, 10).Select(x => x * 0.1);
        var Y = Enumerable.Range(0, 10).Select(x => Random.Shared.NextDouble() * 10);
        return Chart2D.Chart.Point<double, double, string>(X, Y);
    }

}
