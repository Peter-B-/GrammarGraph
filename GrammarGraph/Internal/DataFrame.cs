using System.Collections.Immutable;
using GrammarGraph.Exceptions;
using GrammarGraph.Extensions;
using GrammarGraph.Render;
using JetBrains.Annotations;

namespace GrammarGraph.Internal;

public record DataFrame(
    ImmutableDictionary<AestheticsId, DataColumn> Columns,
    ImmutableArray<Panel> Panels,
    ImmutableArray<Group> Groups)
{
    public DataColumn this[AestheticsId aestheticsId] => Columns[aestheticsId];

    public bool Contains(AestheticsId id)
    {
        return Columns.ContainsKey(id);
    }

    public DoubleColumn GetDoubleColumn(AestheticsId id)
    {
        return Columns[id] as DoubleColumn ??
            throw new UnexpectedDataColumnTypeException(typeof(DoubleColumn), Columns[id].GetType());
    }

    public FactorColumn GetFactorColumn(AestheticsId id)
    {
        return Columns[id] as FactorColumn ??
            throw new UnexpectedDataColumnTypeException(typeof(FactorColumn), Columns[id].GetType());
    }

    public DataColumn? TryGetColumn(AestheticsId id)
    {
        Columns.TryGetValue(id, out var col);
        return col;
    }
}

[NoReorder]
public class DataFrameItem(DataFrame dataFrame, int index)
{
    public int Index => index;

    public Panel Panel => dataFrame.Panels[Index];
    public Group Group => dataFrame.Groups[Index];
}

public record AestheticsFactor(AestheticsId Aesthetics, Factor Factor)
{
    public override string ToString() => $"{Aesthetics}: {Factor}";

    private string ToDump() => this.ToString();
}

public record AestheticsFactorColumn(AestheticsId Aesthetics, FactorColumn FactorColumn)
{
    public override string ToString() => $"{Aesthetics}: {FactorColumn}";

    private string ToDump() => this.ToString();
}

public record Group(ImmutableArray<AestheticsFactor> Identifiers)
{
    public override string ToString()
    {
        if (Identifiers.IsEmpty) return "default";

        return Identifiers
            .Select(kvp => $"{kvp.Aesthetics}: {kvp.Factor.Value}")
            .JoinStrings("; ");
    }

    private string ToDump() => ToString();

    public static Group Default => new(ImmutableArray<AestheticsFactor>.Empty);

    public bool MatchesIndices(int[] indices)
    {
        if (indices.Length != Identifiers.Length)
            throw new ArgumentException($"{nameof(indices)} must be of same length as {nameof(Identifiers)}.", nameof(indices));

        for (var idx = 0; idx < Identifiers.Length; idx++)
            if (indices[idx] != Identifiers[idx].Factor.Index)
                return false;

        return true;
    }
}
