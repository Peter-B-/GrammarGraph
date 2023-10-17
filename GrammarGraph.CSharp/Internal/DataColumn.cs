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

public record FactorColumn(ImmutableArray<string> Values) : DataColumn
{
    public override DataColumnType Type => DataColumnType.Factor;
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
        Columns.TryGetValue(id, out DataColumn? col);
        return col;
    }
}
