using GrammarGraph.CSharp.Data;

namespace GrammarGraph.CSharp;

public static class UsageSample
{
    public static void Run()
    {
        var data = DataSets.GetDiamonds();

        var chart =
                data
                    .CreateChart()
                    .SetAesthetics(AestheticsId.X, d => d.Carat)
                    .SetAesthetics(AestheticsId.Y, d => d.Price)
                    .WithGeom(b => b.Point())
                    .WithGeom(b => b.Line(
                                  g => g.WithAesthetics(AestheticsId.Color, d => d.Color)
                              ))
                    .InFacets(d => d.Cut)
            ;
    }
}
