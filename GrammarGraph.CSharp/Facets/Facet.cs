namespace GrammarGraph.CSharp.Facets;

public abstract record Facet<T>
{
    public abstract IEnumerable<KeyValuePair<AestheticsId, Mapping<T>>> GetAesthetics();
}
