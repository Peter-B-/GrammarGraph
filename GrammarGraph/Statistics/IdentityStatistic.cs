using GrammarGraph.Internal;

namespace GrammarGraph.Statistics;

internal record IdentityStatistic : Statistic
{
    public override DataFrame Compute(DataFrame data) => data;
}
