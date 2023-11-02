using GrammarGraph.CSharp.Internal;

namespace GrammarGraph.CSharp.Statistics;

internal record IdentityStatistic : Statistic
{
    public override DataFrame Compute(DataFrame data)
    {
        return data;
    }
}
