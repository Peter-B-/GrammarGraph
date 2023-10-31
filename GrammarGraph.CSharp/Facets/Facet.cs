using System.Collections.Immutable;
using GrammarGraph.CSharp.Render;

namespace GrammarGraph.CSharp.Facets;

public abstract record Facet<T>
{
    public abstract ImmutableArray<Panel> GetPanels(IReadOnlyList<T> data);
    public abstract ImmutableArray<Panel> AssignToPanels(IReadOnlyList<T> data, ImmutableArray<Panel> panels);
}
