using System.Collections.Immutable;
using System.Linq.Expressions;
using Plotly.NET;

namespace GrammarGraph.CSharp;

public record GgChart<T>(
    IEnumerable<T> Data,
    ImmutableList<Layer<T>> Layers,
    ImmutableList<Scale> Scales,
    ImmutableDictionary<AestheticsId, Mapping<T>> Mappings,
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

public record AestheticsId(string Id)
{
    public static AestheticsId Color => new(Known.Color);
    public static AestheticsId Fill => new(Known.Fill);
    public static AestheticsId Size => new(Known.Size);
    public static AestheticsId Shape => new(Known.Shape);
    public static AestheticsId X => new(Known.X);
    public static AestheticsId Y => new(Known.Y);

    public static implicit operator AestheticsId(string id)
    {
        return new AestheticsId(id);
    }

    public static class Known
    {
        public static string Color => "color";
        public static string Fill => "fill";
        public static string Size => "size";
        public static string Shape => "shape";
        public static string X => "x";
        public static string Y => "y";
    }
}

public class Theme
{
    public static Theme Default => new();
}

public record Layer<T>(
    Geometry<T> Geometry,
    Stat<T> Stat,
    ImmutableDictionary<AestheticsId, Mapping<T>> Mappings
);

public record Label;

public abstract record Facet<T>;

public record Stat<T>(
    //ImmutableDictionary<AestheticsId, Mapping<T>> ComputedMappings
);

public static class Stat
{
    public static Stat<T> Identity<T>()
    {
        return new Stat<T>();
    }
}

public record GridFaced<T>(Expression<Func<T, IConvertible>> RowMap, Expression<Func<T, IConvertible>> ColMap) : Facet<T>;

public record WrapFaced<T>(Expression<Func<T, IConvertible>> Map) : Facet<T>;

public record Coordinate;

public record Mapping<T>(
    Expression<Func<T, IConvertible>> Expression
);

public record Scale;

public abstract record Geometry<T>;

public record LineGeometry<T> : Geometry<T>;

public record PointGeometry<T> : Geometry<T>;

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

    public static GgChart<T> SetAesthetics<T>(this GgChart<T> chart, AestheticsId id, Expression<Func<T, IConvertible>> mapping)
    {
        return chart with
        {
            Mappings = chart.Mappings.SetItem(id, new Mapping<T>(mapping))
        };
    }

    public static GgChart<T> AddLayer<T>(this GgChart<T> chart, Layer<T> layer)
    {
        var layers = chart.Layers.Add(layer);
        return chart with { Layers = layers };
    }

    public static Layer<T> CreateLayer<T>(Geometry<T> geometry, Stat<T> stat, ImmutableDictionary<AestheticsId, Mapping<T>> mappings)
    {
        return new Layer<T>(geometry, stat, mappings);
    }

    public static GgChart<T> WithGeom<T>(this GgChart<T> chart, Func<GeometryBuilder<T>, Layer<T>> geomFactory)
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

public record GeometryBuilder<T>;

public static class GeometryBuilder
{
    public static GeometryBuilder<T> Create<T>()
    {
        return new GeometryBuilder<T>();
    }

    public static Layer<T> Line<T>(this GeometryBuilder<T> builder, Func<Layer<T>, Layer<T>>? config = null)
    {
        return Create(config, new PointGeometry<T>());
    }

    public static Layer<T> Point<T>(this GeometryBuilder<T> builder, Func<Layer<T>, Layer<T>>? config = null)
    {
        return Create(config, new LineGeometry<T>());
    }

    private static Layer<T> Create<T>(Func<Layer<T>, Layer<T>>? config, Geometry<T> geometry)
    {
        var layer = new Layer<T>(
            geometry,
            Stat.Identity<T>(),
            ImmutableDictionary<AestheticsId, Mapping<T>>.Empty);

        if (config != null)
            layer = config(layer);

        return layer;
    }
}

public record GeometryConfig<T>;

public static class GeometryConfig
{
    public static Layer<T> WithAesthetics<T>(this Layer<T> layer, AestheticsId id, Expression<Func<T, IConvertible>> mapping)
    {
        return layer with
        {
            Mappings = layer.Mappings.SetItem(id, new Mapping<T>(mapping))
        };
    }
}
