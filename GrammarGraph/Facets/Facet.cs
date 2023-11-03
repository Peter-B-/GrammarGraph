using System.Collections.Immutable;
using GrammarGraph.Render;

namespace GrammarGraph.Facets;

public abstract record Facet<T>
{
    // I think splitting panel generation from facets into a two step process
    // might enable setting geoms with a separate data source.
    // GetPanels will only be invoked on the top level data, while AssignToPanels can
    // can be called for each step.
    public abstract ImmutableArray<Panel> GetPanels(IReadOnlyList<T> data);
    public abstract ImmutableArray<Panel> AssignToPanels(IReadOnlyList<T> data, ImmutableArray<Panel> panels);
}
