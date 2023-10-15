using GrammarGraph.CSharp.Statistics;

namespace GrammarGraph.CSharp;

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