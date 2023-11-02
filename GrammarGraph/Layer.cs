using System.Collections.Immutable;
using GrammarGraph.CSharp.Geometry;
using GrammarGraph.CSharp.Statistics;

namespace GrammarGraph.CSharp;

public record Layer<T>(
    Geometry<T> Geometry,
    Statistic Stat,
    ImmutableDictionary<AestheticsId, Mapping<T>> Aesthetics
);
