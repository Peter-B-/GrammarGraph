using GrammarGraph.Internal;

namespace GrammarGraph.Render;

public record LayerData<T>(
    Layer<T> Layer,
    DataFrame Data
);