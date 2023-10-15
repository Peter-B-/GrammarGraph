using System.Collections.Immutable;

namespace GrammarGraph.CSharp.Statistics;

public record EcdfStatistic() : Statistic()
{
    public override ImmutableDictionary<AestheticsId, ImmutableArray<double>> Compute(ImmutableDictionary<AestheticsId, ImmutableArray<double>> data)
    {
        // Todo: Implement
        return data;
    }
}