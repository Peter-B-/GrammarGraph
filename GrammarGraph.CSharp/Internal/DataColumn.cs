using System.Collections.Immutable;

namespace GrammarGraph.CSharp.Internal;

public abstract record DataColumn
{
    public abstract DataColumnType Type { get; }
}

public record DoubleColumn(ImmutableArray<double> Values) : DataColumn
{
    public override DataColumnType Type => DataColumnType.Double;

    public override string ToString()
    {
        const int limit = 10;
        var values = Values
            .Take(limit)
            .Select(v => v.ToString("g2"));

        return $"num [{Values.Length}]: {string.Join("; ", values)}{(Values.Length > limit?", ...":"")}";
    }

    private string ToDump() => ToString();
}

public record FactorColumn(ImmutableArray<int> Indices, ImmutableArray<string> Levels) : DataColumn
{
    public override DataColumnType Type => DataColumnType.Factor;

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

    public override string ToString()
    {
        const int limit = 10;
        var values = Values
            .Take(limit);

        return $"factor [{Indices.Length}] of {Levels.Length} levels: {string.Join("; ", values)}{(Indices.Length > limit?", ...":"")}";
    }

    private string ToDump() => ToString();


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

        return new FactorColumn(builder.MoveToImmutable(), levels);
    }
}

public enum DataColumnType
{
    Double,
    Factor
}
