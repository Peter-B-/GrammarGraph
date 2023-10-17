using GrammarGraph.CSharp.Data;
using GrammarGraph.CSharp.Data.Diamonds;
using GrammarGraph.CSharp.Geometry;

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
                    .Add(b => b.Point(l => l.SetAesthetics(AestheticsId.Color, d => d.Color)))
                    .Add(b => b.Line())
                    .InFacets(d => d.Cut)
            ;

        data
            .CreateChart()
            .SetAesthetics(AestheticsId.X, d => d.Carat)
            .SetAesthetics(AestheticsId.Y, d => d.Price)
            .Add(b =>
                b.Point(stat: Stat.Identity())
            )
            .Add(b => b.Step(stat: Stat.Ecdf()))
            // .WithStatistics(b =>
            //     b.Ecdf(g => g.Point())
            //         )
            .InFacets(d => d.Cut)
            ;

    }
}
