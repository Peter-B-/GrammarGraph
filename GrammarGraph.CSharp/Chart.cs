using System.Collections.Immutable;
using System.Linq.Expressions;

namespace GrammarGraph.CSharp;

public record Chart<T>(
    IEnumerable<T> Data,
    ImmutableList<Layer> Layers,
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

public record Layer;

public static class Chart
{
    public static Chart<T> CreateChart<T>(this IEnumerable<T> data) =>
        new(data, ImmutableList<Layer>.Empty,
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
}