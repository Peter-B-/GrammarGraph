using System.Collections.Immutable;
using GrammarGraph.CSharp.Internal;

namespace GrammarGraph.CSharp.Render;

public record PlotDescription(
    ImmutableArray<DataFrame> Layers
);