using System.Collections.Immutable;
using GrammarGraph.Geometry;
using GrammarGraph.Statistics;

namespace GrammarGraph;

public record Layer<T>(
    Geometry<T> Geometry,
    Statistic Stat,
    ImmutableDictionary<AestheticsId, Mapping<T>> Aesthetics
);
