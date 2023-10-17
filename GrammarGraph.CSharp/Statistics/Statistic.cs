using System.Collections.Immutable;
using GrammarGraph.CSharp.Internal;
using GrammarGraph.CSharp.Render;

namespace GrammarGraph.CSharp.Statistics;

public abstract record Statistic()
{
    public abstract DataFrame Compute(DataFrame data);
}
