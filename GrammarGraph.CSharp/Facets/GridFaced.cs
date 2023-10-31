using System.Collections.Immutable;
using GrammarGraph.CSharp.Exceptions;
using GrammarGraph.CSharp.Internal;
using GrammarGraph.CSharp.Render;

namespace GrammarGraph.CSharp.Facets;

public record GridFaced<T>(Mapping<T> RowMap, Mapping<T> ColumnMap) : Facet<T>
{
    public override ImmutableArray<Panel> AssignToPanels(IReadOnlyList<T> data, ImmutableArray<Panel> panels)
    {
        var panelsTyped = panels.As<GridPanel>();

        var panelDict = panelsTyped
            .ToDictionary(p => p.RowLabel, )

    }

    public override ImmutableArray<Panel> GetPanels(IReadOnlyList<T> data)
    {
        var rowColumn = PlotBuilder.CreateDataColumn(data, RowMap) as FactorColumn ??
            throw new GraphicsConfigurationException("Facet row must map as factor but did not.");
        var colColumn = PlotBuilder.CreateDataColumn(data, ColumnMap) as FactorColumn ??
            throw new GraphicsConfigurationException("Facet column must map as factor but did not.");

        var panels =
            rowColumn.Levels.SelectMany(
                    (rowLevel, rowIdx) => colColumn.Levels
                        .Select((columnLevel, colIdx) => new GridPanel(rowLevel, columnLevel, rowIdx, colIdx))
                )
                .ToImmutableArray();

        return ImmutableArray<Panel>.CastUp(panels);
    }
}

public record SingleFacet<T> : Facet<T>
{
    public override ImmutableArray<Panel> AssignToPanels(IReadOnlyList<T> data, ImmutableArray<Panel> panels)
    {
        var panel = panels.Single();

        var builder = ImmutableArray.CreateBuilder<Panel>(data.Count);
        for (var i = 0; i < data.Count; i++)
            builder.AddRange(panel);
        return builder.DrainToImmutable();
    }

    public override ImmutableArray<Panel> GetPanels(IReadOnlyList<T> data)
    {
        var panel = new GridPanel(null, null, 0, 0);
        return ImmutableArray.Create((Panel) panel);
    }
}
