using GrammarGraph.Statistics;

namespace GrammarGraph;

public static class Stat
{
    public static Statistic Identity()
    {
        return new IdentityStatistic();
    }

    public static Statistic Ecdf()
    {
        return new EcdfStatistic();
    }
}
