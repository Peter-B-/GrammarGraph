using GrammarGraph;
using GrammarGraph.Data;
using GrammarGraph.Geometry;
using GrammarGraph.Render;
using GrammarGraph.Statistics;

var chart =
        DataSets.GetDiamonds()
            .Take(1000)
            .CreateChart()
            .SetAesthetics(AestheticsId.X, d => d.Carat)
            //.SetAesthetics(AestheticsId.Y, d => d.Price)
            //.Add(b => b.Point(b => b.SetAesthetics(AestheticsId.Color, d => d.Cut)))
            .Add(b => b.Line(stat: new EcdfStatistic()))
            .InFacets(d => d.Cut)

    ;

var plot = new PlotBuilder().BuildPlot(chart);
var graph = new PlotlyRenderEngine().Render(plot);
