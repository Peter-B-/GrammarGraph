using GrammarGraph.Exceptions;
using GrammarGraph.Internal;

namespace GrammarGraph.Extensions;

public static class DataColumnContainerExtensions
{
    public static bool Contains(this IDataColumnContainer container, AestheticsId id)
    {
        return container.Columns.ContainsKey(id);
    }

    public static DoubleColumn GetDoubleColumn(this IDataColumnContainer container, AestheticsId id)
    {
        return container.Columns[id] as DoubleColumn ??
            throw new UnexpectedDataColumnTypeException(typeof(DoubleColumn), container.Columns[id].GetType());
    }

    public static FactorColumn GetFactorColumn(this IDataColumnContainer container, AestheticsId id)
    {
        return container.Columns[id] as FactorColumn ??
            throw new UnexpectedDataColumnTypeException(typeof(FactorColumn), container.Columns[id].GetType());
    }
}
