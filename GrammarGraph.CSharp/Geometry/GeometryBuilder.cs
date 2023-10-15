using System.Collections.Immutable;
using GrammarGraph.CSharp.Statistics;

namespace GrammarGraph.CSharp.Geometry;

public class GeometryBuilder<T>
{
    public Layer<T> Line(Func<Layer<T>, Layer<T>>? config = null,
        Statistic? stat = null)
    {
        return Create(config, new LineGeometry<T>(), stat ?? Stat.Identity());
    }

    public Layer<T> Step(Func<Layer<T>, Layer<T>>? config = null,
        Statistic? stat = null)
    {
        return Create(config, new LineGeometry<T>(LineType.Step), stat ?? Stat.Identity());
    }

    public Layer<T> Point(Func<Layer<T>, Layer<T>>? config = null,
        Statistic? stat = null)
    {
        return Create(config, new PointGeometry<T>(), stat ?? Stat.Identity());
    }

    private static Layer<T> Create(
        Func<Layer<T>, Layer<T>>? config,
        Geometry<T> geometry,
        Statistic stat
    )
    {
        var layer = new Layer<T>(
            geometry,
            stat,
            ImmutableDictionary<AestheticsId, Mapping<T>>.Empty);

        if (config != null)
            layer = config(layer);

        return layer;
    }
}

public static class GeometryBuilder
{
    public static GeometryBuilder<T> Create<T>()
    {
        return new GeometryBuilder<T>();
    }
}
