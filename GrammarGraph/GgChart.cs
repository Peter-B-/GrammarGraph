using System.Collections.Immutable;
using System.Linq.Expressions;
using GrammarGraph.Facets;
using GrammarGraph.Geometry;
using GrammarGraph.Render;

namespace GrammarGraph;

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

    public static GgChart<T> InFacets<T>(this GgChart<T> chart, Expression<Func<T, object>> rowMap, Expression<Func<T, object>> colMap)
    {
        return chart with
        {
            Facet = new GridFaced<T>(new Mapping<T>(rowMap, true), new Mapping<T>(colMap, true))
        };
    }

    public static GgChart<T> InFacets<T>(this GgChart<T> chart, Expression<Func<T, object>> map)
    {
        return chart with
        {
            Facet = new WrapFaced<T>(new Mapping<T>(map, true))
        };
    }
}
