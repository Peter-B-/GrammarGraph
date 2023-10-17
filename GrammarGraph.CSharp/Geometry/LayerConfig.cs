using System.Linq.Expressions;

namespace GrammarGraph.CSharp.Geometry;

public static class LayerConfig
{
    public static Layer<T> WithAesthetics<T>(this Layer<T> layer, AestheticsId id, Expression<Func<T, IConvertible>> mapping)
    {
        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(mapping))
        };
    }
}
