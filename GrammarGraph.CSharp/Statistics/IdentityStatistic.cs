using System.Collections.Immutable;

namespace GrammarGraph.CSharp.Statistics;

internal record IdentityStatistic() : Statistic()
{
    public override ImmutableDictionary<AestheticsId, ImmutableArray<double>> Compute(ImmutableDictionary<AestheticsId, ImmutableArray<double>> data)
    {
        return data;
    }
}
