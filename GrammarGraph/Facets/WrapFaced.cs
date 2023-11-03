using System.Collections.Immutable;
using GrammarGraph.Exceptions;
using GrammarGraph.Internal;
using GrammarGraph.Render;

namespace GrammarGraph.Facets;

public record WrapFaced<T>(Mapping<T> Map) : Facet<T>
{
    public override PanelCollection GetPanels(IReadOnlyList<T> data)
    {
        var column = GetFactorColumn(data);

        var noOfColumns = 3;

        var panels =
            column.Levels.Select(
                    (level, idx) => new WrapPanel(level, idx / noOfColumns, idx % noOfColumns)
                )
                .ToImmutableArray();

        return new PanelCollection(
            panels.Max(p => p.RowIdx) + 1,
            panels.Max(p => p.ColIdx) + 1,
            ImmutableArray<Panel>.CastUp(panels)
        );


    }

    private FactorColumn GetFactorColumn(IReadOnlyList<T> data)
    {
        var column = PlotBuilder.CreateDataColumn(data, Map) as FactorColumn ??
            throw new GraphicsConfigurationException("Facet row must map as factor but did not.");
        return column;
    }

    public override ImmutableArray<Panel> AssignToPanels(IReadOnlyList<T> data, ImmutableArray<Panel> panels)
    {
        var column = GetFactorColumn(data);

        return column.Indices
            .Select(i => panels[i])
            .ToImmutableArray();
    }
}

