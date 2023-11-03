using System.Collections.Immutable;
using GrammarGraph.Exceptions;
using GrammarGraph.Extensions;
using GrammarGraph.Internal;
using GrammarGraph.Render;

namespace GrammarGraph.Facets;

public record GridFaced<T>(Mapping<T> RowMap, Mapping<T> ColumnMap) : Facet<T>
{
    public override ImmutableArray<Panel> AssignToPanels(IReadOnlyList<T> data, ImmutableArray<Panel> panels)
    {
        var panelsTyped = panels.As<GridPanel>();

        var panelGrid = panelsTyped.ToGrid();

        var (rowColumn, colColumn) = GetColumns(data);

        return data
            .Select((_, idx) =>
            {
                var rowIdx = rowColumn.Indices[idx];
                var colIdx = colColumn.Indices[idx];
                return panelGrid[rowIdx][colIdx] as Panel;
            })
            .ToImmutableArray();
    }

    public override PanelCollection GetPanels(IReadOnlyList<T> data)
    {
        var (rowColumn, colColumn) = GetColumns(data);

        var panels =
            rowColumn.Levels.SelectMany(
                    (rowLevel, rowIdx) => colColumn.Levels
                        .Select((columnLevel, colIdx) => new GridPanel(rowLevel, columnLevel, rowIdx, colIdx))
                )
                .ToImmutableArray();

        return
            new PanelCollection(rowColumn.Length, colColumn.Length,
                                ImmutableArray<Panel>.CastUp(panels));
    }

    private (FactorColumn rowColumn, FactorColumn colColumn) GetColumns(IReadOnlyList<T> data)
    {
        var rowColumn = PlotBuilder.CreateDataColumn(data, RowMap) as FactorColumn ??
            throw new GraphicsConfigurationException("Facet row must map as factor but did not.");
        var colColumn = PlotBuilder.CreateDataColumn(data, ColumnMap) as FactorColumn ??
            throw new GraphicsConfigurationException("Facet column must map as factor but did not.");
        return (rowColumn, colColumn);
    }
}

public record SingleFacet<T> : Facet<T>
{
    public override ImmutableArray<Panel> AssignToPanels(IReadOnlyList<T> data, ImmutableArray<Panel> panels) =>
        ImmutableArrayFactory.Repeat(panels.Single(), data.Count);

    public override PanelCollection GetPanels(IReadOnlyList<T> data) =>
        new PanelCollection(1, 1, ImmutableArray.Create((Panel) new SinglePanel()));
}

public static class GridPanelExtensions
{
    /// <summary>
    ///     Create a 2D jagged array of GridPabel[rowIdx][colIdx].
    /// </summary>
    public static ImmutableArray<ImmutableArray<GridPanel>> ToGrid(this IReadOnlyList<GridPanel> panels)
    {
        var rows = panels.Max(p => p.RowIdx) + 1;
        var cols = panels.Max(p => p.ColIdx) + 1;

        var rowBuilder = ImmutableArray.CreateBuilder<ImmutableArray<GridPanel>>(rows);
        var colBuilder = ImmutableArray.CreateBuilder<GridPanel>(cols);

        var rowGroups = panels
            .GroupBy(p => p.RowIdx)
            .OrderBy(gr => gr.Key);

        var row = 0;
        foreach (var rowGroup in rowGroups)
        {
            if (rowGroup.Key != row) throw new InvalidOperationException($"The rowIdx was expected to be {row} but was {rowGroup.Key}");

            var colGroups = rowGroup
                .GroupBy(p => p.ColIdx)
                .OrderBy(gr => gr.Key);

            var col = 0;
            foreach (var colGroup in colGroups)
            {
                if (colGroup.Key != col) throw new InvalidOperationException($"The colIdx was expected to be {col} but was {colGroup.Key}");
                var panel = colGroup.Single();
                colBuilder.Add(panel);
                col++;
            }

            rowBuilder.Add(colBuilder.DrainToImmutable());
            row++;
        }

        return rowBuilder.ToImmutable();
    }
}
