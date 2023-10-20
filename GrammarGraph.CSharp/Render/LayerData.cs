using GrammarGraph.CSharp.Internal;

namespace GrammarGraph.CSharp.Render;

public record LayerData<T>(
    Layer<T> Layer,
    DataFrame Data
);