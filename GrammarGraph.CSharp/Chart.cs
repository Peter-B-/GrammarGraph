using System.Collections.Immutable;
using System.Linq.Expressions;

namespace GrammarGraph.CSharp;

public record Chart<T>(
    IEnumerable<T> Data,
    ImmutableList<Geometry<T>> Geometries,
    ImmutableList<Scale> Scales,
    ImmutableDictionary<AestheticsId, Mapping<T>> Mappings,
    ImmutableList<Coordinate> Coordinates,
    Facet? Facet,
    ImmutableList<Label> Labels,
    Theme Theme);

public record AestheticsId(string Id)
{
    public static AestheticsId Color => new(Known.Color);
    public static AestheticsId Fill => new(Known.Fill);
    public static AestheticsId Size => new(Known.Size);
    public static AestheticsId X => new(Known.X);
    public static AestheticsId Y => new(Known.Y);

    public static implicit operator AestheticsId(string id) => new(id);

    public static class Known
    {
        public static string Color => "color";
        public static string Fill => "fill";
        public static string Size => "size";
        public static string X => "x";
        public static string Y => "y";
    }
}

public class Theme
{
    public static Theme Default => new Theme();
}

public record Label;

public record Facet;

public record Coordinate;

public record Mapping<T>(Expression<Func<T, IConvertible>> Expression);

public record Scale;

public record Geometry<T>(
    ImmutableDictionary<AestheticsId, Mapping<T>> Mappings
);

public static class Chart
{
    public static Chart<T> CreateChart<T>(this IEnumerable<T> data) =>
        new(data, ImmutableList<Geometry<T>>.Empty,
            ImmutableList<Scale>.Empty,
            ImmutableDictionary<AestheticsId, Mapping<T>>.Empty,
            ImmutableList<Coordinate>.Empty,
            null,
            ImmutableList<Label>.Empty,
            Theme.Default
        );

    public static Chart<T> WithAesthetics<T>(this Chart<T> chart, AestheticsId id, Expression<Func<T, IConvertible>> mapping)
        => chart with
        {
            Mappings = chart.Mappings.SetItem(id, new Mapping<T>(mapping))
        };

    public static Chart<T> WithGeom<T>(this Chart<T> chart, Func<GeometryBuilder<T>, Geometry<T>> geomFactory)
    {
        var geometry = geomFactory(GeometryBuilder.Create<T>());

        return chart with
        {
            Geometries = chart.Geometries.Add(geometry)
        };
    }
}

public record GeometryBuilder<T>;

public static class GeometryBuilder
{
    public static GeometryBuilder<T> Create<T>() => new();

    public static Geometry<T> Line<T>(this GeometryBuilder<T> builder, Action<Geometry<T>>? config = null) => Create(config);
    public static Geometry<T> Point<T>(this GeometryBuilder<T> builder, Action<Geometry<T>>? config = null) => Create(config);

    private static Geometry<T> Create<T>(Action<Geometry<T>>? config)
    {
        var geometry = new Geometry<T>(ImmutableDictionary<AestheticsId, Mapping<T>>.Empty);
        config?.Invoke(geometry);
        return geometry;
    }
}

public record GeometryConfig<T>;

public static class GeometryConfig
{
    public static Geometry<T> WithAesthetics<T>(this Geometry<T> geometry, AestheticsId id, Expression<Func<T, IConvertible>> mapping)
        => geometry with
        {
            Mappings = geometry.Mappings.SetItem(id, new Mapping<T>(mapping))
        };
}