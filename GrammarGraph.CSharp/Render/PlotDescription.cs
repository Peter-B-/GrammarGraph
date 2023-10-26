using System.Collections.Immutable;
using GrammarGraph.CSharp.Internal;

namespace GrammarGraph.CSharp.Render;

public record PlotDescription(
    ImmutableArray<Panel> Panels
);

public record Layer(
    ImmutableDictionary<AestheticsId, Factor> Identifiers,
    DataFrame Data
);

public record Trace(
    ImmutableDictionary<AestheticsId, Factor> Identifiers,
    DataFrame Data
);

public record Panel(
    ImmutableDictionary<AestheticsId, Factor> Identifiers,
    ImmutableArray<Layer> Layers
);

public record Factor(
    int Index,
    ImmutableArray<string> Levels
)
{
    public string Value => Levels[Index];

    public override string ToString()
    {
        return $"{Value} factor of {Levels.Length} levels";
    }
}
