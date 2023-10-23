using System.Linq.Expressions;

namespace GrammarGraph.CSharp.Facets;

public record WrapFaced<T>(Mapping<T> Map) : Facet<T>
{
    public override IEnumerable<KeyValuePair<AestheticsId, Mapping<T>>> GetAesthetics()
    {
        yield return new KeyValuePair<AestheticsId, Mapping<T>>(AestheticsId.Intern.Facet, Map);
    }
}
