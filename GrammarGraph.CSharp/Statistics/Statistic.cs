using GrammarGraph.CSharp.Internal;

namespace GrammarGraph.CSharp.Statistics;

public abstract record Statistic
{
    public abstract DataFrame Compute(DataFrame data);
}
