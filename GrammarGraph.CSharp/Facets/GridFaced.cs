using System.Linq.Expressions;

namespace GrammarGraph.CSharp.Facets;

public record GridFaced<T>(Mapping<T> RowMap, Mapping<T> ColumnMap) : Facet<T>
{
    public override IEnumerable<KeyValuePair<AestheticsId, Mapping<T>>> GetAesthetics()
    {
        yield return new KeyValuePair<AestheticsId, Mapping<T>>(AestheticsId.Intern.FacetRow, RowMap);
        yield return new KeyValuePair<AestheticsId, Mapping<T>>(AestheticsId.Intern.FacetColumn, ColumnMap);
    }
}
