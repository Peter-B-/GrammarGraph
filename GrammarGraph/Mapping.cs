using System.Linq.Expressions;

namespace GrammarGraph;

public record Mapping<T>(
    Expression<Func<T, object>> Expression,
    bool AsFactor
);
