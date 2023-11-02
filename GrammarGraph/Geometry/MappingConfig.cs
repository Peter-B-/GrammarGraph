using System.Linq.Expressions;

namespace GrammarGraph.CSharp.Geometry;

public static class MappingConfig
{
    public static GgChart<T> SetAesthetics<T>(this GgChart<T> layer, AestheticsId id, Expression<Func<T, object>> mapping)
    {
        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(mapping, true))
        };
    }

    public static GgChart<T> SetAesthetics<T>(this GgChart<T> layer, AestheticsId id, Expression<Func<T, int>> mapping, bool asFactor=false)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var castedMapping = Expression.Lambda<Func<T, object>>(
            Expression.Convert(Expression.Invoke(mapping, param),
                typeof(object)
            ),
            param);

        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(castedMapping, asFactor))
        };
    }

    public static GgChart<T> SetAesthetics<T>(this GgChart<T> layer, AestheticsId id, Expression<Func<T, float>> mapping, bool asFactor=false)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var castedMapping = Expression.Lambda<Func<T, object>>(
            Expression.Convert(Expression.Invoke(mapping, param),
                typeof(object)
            ),
            param);

        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(castedMapping, asFactor))
        };
    }

    public static GgChart<T> SetAesthetics<T>(this GgChart<T> layer, AestheticsId id, Expression<Func<T, double>> mapping, bool asFactor=false)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var castedMapping = Expression.Lambda<Func<T, object>>(
            Expression.Convert(Expression.Invoke(mapping, param),
                typeof(object)
            ),
            param);

        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(castedMapping, asFactor))
        };
    }

    public static Layer<T> SetAesthetics<T>(this Layer<T> layer, AestheticsId id, Expression<Func<T, object>> mapping)
    {
        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(mapping, true))
        };
    }

    public static Layer<T> SetAesthetics<T>(this Layer<T> layer, AestheticsId id, Expression<Func<T, int>> mapping, bool asFactor=false)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var castedMapping = Expression.Lambda<Func<T, object>>(
            Expression.Convert(Expression.Invoke(mapping, param),
                typeof(object)
            ),
            param);

        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(castedMapping, asFactor))
        };
    }

    public static Layer<T> SetAesthetics<T>(this Layer<T> layer, AestheticsId id, Expression<Func<T, float>> mapping, bool asFactor=false)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var castedMapping = Expression.Lambda<Func<T, object>>(
            Expression.Convert(Expression.Invoke(mapping, param),
                typeof(object)
            ),
            param);

        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(castedMapping, asFactor))
        };
    }

    public static Layer<T> SetAesthetics<T>(this Layer<T> layer, AestheticsId id, Expression<Func<T, double>> mapping, bool asFactor=false)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var castedMapping = Expression.Lambda<Func<T, object>>(
            Expression.Convert(Expression.Invoke(mapping, param),
                typeof(object)
            ),
            param);

        return layer with
        {
            Aesthetics = layer.Aesthetics.SetItem(id, new Mapping<T>(castedMapping, asFactor))
        };
    }
}
