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
        var column = PlotBuilder.CreateDataColumn(data, Map) as FactorColumn ??
            throw new GraphicsConfigurationException("Facet row must map as factor but did not.");

        var noOfColumns = 3;

        var panels =
            column.Levels.Select(
                    (level, idx) => new WrapPanel(level, idx / noOfColumns, idx % noOfColumns)
                )
                .Cast<Panel>()
                .ToImmutableArray();

        return panels;
    }

    public override ImmutableArray<Panel> AssignToPanels<T1>(IReadOnlyList<T1> data, ImmutableArray<Panel> panels)
    {
        throw new NotImplementedException();
    }
}

