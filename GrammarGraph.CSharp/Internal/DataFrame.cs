using System.Collections.Immutable;
using GrammarGraph.CSharp.Exceptions;
using GrammarGraph.CSharp.Render;
using JetBrains.Annotations;

namespace GrammarGraph.CSharp.Internal;

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

public record Group
{
}
