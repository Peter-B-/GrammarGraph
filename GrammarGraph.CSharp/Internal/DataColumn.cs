using System.Collections.Immutable;
using GrammarGraph.CSharp.Exceptions;

namespace GrammarGraph.CSharp.Internal;

public abstract record DataColumn;

public record DoubleColumn(ImmutableArray<double> Values) : DataColumn;

public record FactorColumn(ImmutableArray<string> Values) : DataColumn;

public record DataFrame(
    ImmutableDictionary<AestheticsId, DataColumn> Columns
)
{
    public DataColumn this[AestheticsId aestheticsId] => Columns[aestheticsId];
    public bool Contains(AestheticsId id) => Columns.ContainsKey(id);
    public DoubleColumn GetDoubleColumn(AestheticsId id) => Columns[id] as DoubleColumn ?? throw new UnexpectedDataColumnTypeException(typeof(DoubleColumn), Columns[id].GetType());
    public FactorColumn GetFactorColumn(AestheticsId id) => Columns[id] as FactorColumn ?? throw new UnexpectedDataColumnTypeException(typeof(FactorColumn), Columns[id].GetType());
}