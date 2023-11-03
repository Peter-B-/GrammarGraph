using System.Collections.Immutable;
using GrammarGraph.Extensions;
using GrammarGraph.Internal;

namespace GrammarGraph.Render;

public record PlotDescription(
    ImmutableArray<Panel> Panels,
    ImmutableArray<Layer> Layers

);

public record Layer(
    ImmutableArray<Group> Groups,
    DataFrame Data
);

public abstract record Panel(
);

public record GridPanel(
    string RowLabel,
    string ColLabel,
    int RowIdx,
    int ColIdx
):Panel;

public record WrapPanel(
    string Label,
    int RowIdx,
    int ColIdx
) : Panel;

public record SinglePanel : Panel;

public record Factor(
    int Index,
    ImmutableArray<string> Levels
)
{
    public string Value => Levels[Index];

    public override string ToString()
    {
        var factors = Levels
            .Take(6)
            .JoinStrings(", ");
        if (Levels.Length > 6)
            factors += ", ...";
        return $"{Value} / [{factors}]";
    }
}
