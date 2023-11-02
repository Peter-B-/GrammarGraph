using System.Collections.Immutable;
using System.Linq.Expressions;
using GrammarGraph.CSharp.Exceptions;
using GrammarGraph.CSharp.Internal;
using GrammarGraph.CSharp.Render;

namespace GrammarGraph.CSharp.Facets;

public record WrapFaced<T>(Mapping<T> Map) : Facet<T>
{
    public override ImmutableArray<Panel> GetPanels(IReadOnlyList<T> data)
    {
        var column = GetFactorColumn(data);

        var noOfColumns = 3;

        var panels =
            column.Levels.Select(
                    (level, idx) => new WrapPanel(level, idx / noOfColumns, idx % noOfColumns)
                )
                .Cast<Panel>()
                .ToImmutableArray();

        return panels;
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

