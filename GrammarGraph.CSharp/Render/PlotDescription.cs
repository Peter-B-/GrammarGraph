using System.Collections.Immutable;
using GrammarGraph.CSharp.Internal;

namespace GrammarGraph.CSharp.Render;

public record PlotDescription(
    ImmutableArray<Panel> Panels,
    ImmutableArray<Layer> Layers

);

public record Layer(
    ImmutableDictionary<AestheticsId, Factor> Identifiers,
    ImmutableArray<Group> Groups,
    DataFrame Data
);

public abstract record Panel(
);

public record GridPanel(
    string? RowLabel,
    string? ColLabel,
    int RowIdx,
    int ColIdx
):Panel;

public record WrapPanel(
    string Label,
    int RowIdx,
    int ColIdx
) : Panel;

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
