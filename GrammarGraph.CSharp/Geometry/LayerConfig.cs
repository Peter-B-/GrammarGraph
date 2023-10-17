using System.Linq.Expressions;

namespace GrammarGraph.CSharp.Geometry;

public static class LayerConfig
{
    public static Layer<T> SetAesthetics<T>(this Layer<T> layer, AestheticsId id, Expression<Func<T, object>> mapping)
    {
        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(mapping))
        };
    }
}
