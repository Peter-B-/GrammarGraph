using System.Collections.Immutable;

namespace GrammarGraph.CSharp.Internal;

public abstract record DataColumn;

public record DoubleColumn(ImmutableArray<double> Values) : DataColumn;

public record FactorColumn(ImmutableArray<string> Values) : DataColumn;

public record DataTable(
    ImmutableDictionary<AestheticsId, DataColumn> Columns
);
