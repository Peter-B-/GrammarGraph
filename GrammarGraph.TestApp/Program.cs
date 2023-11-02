using GrammarGraph.CSharp;
using GrammarGraph.CSharp.Data;
using GrammarGraph.CSharp.Geometry;
using GrammarGraph.CSharp.Render;

var chart =
        DataSets.GetDiamonds()
            .Take(100)
            .CreateChart()
            .SetAesthetics(AestheticsId.X, d => d.Carat)
            .SetAesthetics(AestheticsId.Y, d => d.Price)
            .Add(b => b.Point(b => b
                                  .SetAesthetics(AestheticsId.Color, d => d.Cut)
                                  .SetAesthetics(AestheticsId.Shape, d => d.Clarity)
                 ))
            .Add(b => b.Line())
            .InFacets(d => d.Cut, d => d.Clarity)
    ;

var plot = new PlotBuilder().BuildPlot(chart);
