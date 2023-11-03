using GrammarGraph.Internal;

namespace GrammarGraph.Statistics;

public abstract record Statistic
{
    public abstract DataFrame Compute(DataFrame data);
}
