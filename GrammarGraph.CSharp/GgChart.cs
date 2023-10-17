using System.Collections.Immutable;
using System.Linq.Expressions;
using GrammarGraph.CSharp.Facets;
using GrammarGraph.CSharp.Geometry;
using GrammarGraph.CSharp.Render;

namespace GrammarGraph.CSharp;

public record GgChart<T>(
    IEnumerable<T> Data,
    ImmutableList<Layer<T>> Layers,
    ImmutableList<Scale> Scales,
    ImmutableDictionary<AestheticsId, Mapping<T>> Aesthetics,
    ImmutableList<Coordinate> Coordinates,
    Facet<T>? Facet,
    ImmutableList<Label> Labels,
    Theme Theme)
{
    private object ToDump()
    {
        return this.Plot();
    }
}

public static class GgChart
{
    public static GgChart<T> CreateChart<T>(this IEnumerable<T> data)
    {
        return new GgChart<T>(data, ImmutableList<Layer<T>>.Empty,
            ImmutableList<Scale>.Empty,
            ImmutableDictionary<AestheticsId, Mapping<T>>.Empty,
            ImmutableList<Coordinate>.Empty,
            null,
            ImmutableList<Label>.Empty,
            Theme.Default
        );
    }

    public static GgChart<T> SetAesthetics<T>(this GgChart<T> chart, AestheticsId id, Expression<Func<T, object>> mapping)
    {
        return chart with
        {
            Aesthetics = chart.Aesthetics.SetItem(id, new Mapping<T>(mapping))
        };
    }

    public static GgChart<T> AddLayer<T>(this GgChart<T> chart, Layer<T> layer)
    {
        var layers = chart.Layers.Add(layer);
        return chart with { Layers = layers };
    }

    public static GgChart<T> Add<T>(this GgChart<T> chart, Func<GeometryBuilder<T>, Layer<T>> geomFactory)
    {
        var layer = geomFactory(GeometryBuilder.Create<T>());

        return chart with
        {
            Layers = chart.Layers.Add(layer)
        };
    }

    public static GgChart<T> InFacets<T>(this GgChart<T> chart, Expression<Func<T, IConvertible>> rowMap, Expression<Func<T, IConvertible>> colMap)
    {
        return chart with
        {
            Facet = new GridFaced<T>(rowMap, colMap)
        };
    }

    public static GgChart<T> InFacets<T>(this GgChart<T> chart, Expression<Func<T, IConvertible>> map)
    {
        return chart with
        {
            Facet = new WrapFaced<T>(map)
        };
    }
}
