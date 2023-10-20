using System.Collections.Immutable;
using GrammarGraph.CSharp.Exceptions;

namespace GrammarGraph.CSharp.Internal;

public abstract record DataColumn
{
    public abstract DataColumnType Type { get; }
}

public record DoubleColumn(ImmutableArray<double> Values) : DataColumn
{
    public override DataColumnType Type => DataColumnType.Double;
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

public record DataFrame(
    ImmutableDictionary<AestheticsId, DataColumn> Columns
)
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
