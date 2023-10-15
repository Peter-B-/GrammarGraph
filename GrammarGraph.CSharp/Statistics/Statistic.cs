using System.Collections.Immutable;

namespace GrammarGraph.CSharp.Statistics;

public abstract record Statistic()
{
    public abstract ImmutableDictionary<AestheticsId, ImmutableArray<double>> Compute(ImmutableDictionary<AestheticsId, ImmutableArray<double>> data);
}
