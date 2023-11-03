using System.Collections.Generic;
using System.Collections.Immutable;
using GrammarGraph.Exceptions;
using GrammarGraph.Extensions;

namespace GrammarGraph.Internal;

public abstract record DataColumn
{
    public abstract DataColumnType Type { get; }
    public abstract int Length { get; }
    public abstract DataColumn Subset(ImmutableArray<int> indices);
}

public record DoubleColumn(ImmutableArray<double> Values) : DataColumn
{
    public override DataColumnType Type => DataColumnType.Double;
    public override int Length => Values.Length;

    public override DataColumn Subset(ImmutableArray<int> indices) =>
        new DoubleColumn(Values.Subset(indices));

    public override string ToString()
    {
        const int limit = 10;
        var values = Values
            .Take(limit)
            .Select(v => v.ToString("g2"));

        return $"num [{Values.Length}]: {string.Join("; ", values)}{(Values.Length > limit ? ", ..." : "")}";
    }

    private string ToDump() => ToString();
}

public record FactorColumn(ImmutableArray<int> Indices, ImmutableArray<string> Levels, IEqualityComparer<string> Comparer) : DataColumn
{
    public override DataColumnType Type => DataColumnType.Factor;
    public override int Length => Indices.Length;

    public IEnumerable<string> Values => Indices.Select(i => Levels[i]);

    public static FactorColumn FromStrings(IEnumerable<string> items, IEqualityComparer<string>? comparer = null)
    {
        var itemsList = items.ToList();
        var levels =
            itemsList
                .Distinct(comparer)
                .ToImmutableArray();

        return FromStrings(itemsList, levels, comparer);
    }


    public static FactorColumn FromStrings(IEnumerable<string> items, ImmutableArray<string> levels, IEqualityComparer<string>? comparer = null)
    {
        comparer ??= StringComparer.Ordinal;

        var itemsList = items as IList<string> ?? items.ToList();

        var levelsMap = levels
            .Select((l, i) => new { Level = l, Idx = i })
            .ToDictionary(i => i.Level, i => i.Idx, comparer);

        var builder = ImmutableArray.CreateBuilder<int>(itemsList.Count);

        builder.AddRange(itemsList
                             .Select(level => levelsMap[level])
        );

        return new FactorColumn(builder.MoveToImmutable(), levels, comparer);
    }

    public override DataColumn Subset(ImmutableArray<int> indices) =>
        new FactorColumn(Indices.Subset(indices), Levels, Comparer);

    public override string ToString()
    {
        const int limit = 10;
        var values = Values
            .Take(limit);

        return $"factor [{Indices.Length}] of {Levels.Length} levels: {string.Join("; ", values)}{(Indices.Length > limit ? ", ..." : "")}";
    }

    private string ToDump() => ToString();
}

public enum DataColumnType
{
    Double,
    Factor
}

public static class DataColumnFactory
{
    public static DataColumn Merge(IEnumerable<DataColumn> parts)
    {
        var partList = parts as IReadOnlyList<DataColumn> ?? parts.ToList();

        var types = partList.Select(p => p.GetType())
            .Distinct()
            .ToList();

        if (types.Count > 1)
            throw new GraphicsLogicException("Cannot merge DataColumns of varying types.");

        var itemCount = partList.Sum(p => p.Length);

        return partList[0] switch
        {
            DoubleColumn => MergeDouble(partList.Cast<DoubleColumn>(), itemCount),
            FactorColumn => MergeFactor(partList.Cast<FactorColumn>(), itemCount),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static DataColumn MergeFactor(IEnumerable<FactorColumn> parts, int itemCount)
    {
        var first = parts.FirstOrDefault();
        if (first == null) throw new GraphicsLogicException("Need at least one FactorColumn for merge");

        var indexBuilder = ImmutableArray.CreateBuilder<int>(itemCount);
        foreach (var part in parts)
            indexBuilder.AddRange(part.Indices);

        return new FactorColumn(indexBuilder.MoveToImmutable(), first.Levels, first.Comparer);
    }

    private static DataColumn MergeDouble(IEnumerable<DoubleColumn> parts, int itemCount)
    {
        var valueBuilder = ImmutableArray.CreateBuilder<double>(itemCount);
        foreach (var part in parts)
            valueBuilder.AddRange(part.Values);

        return new DoubleColumn(valueBuilder.MoveToImmutable());
    }
}
