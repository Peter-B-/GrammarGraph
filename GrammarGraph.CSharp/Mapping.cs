﻿using System.Linq.Expressions;

namespace GrammarGraph.CSharp;

public record Mapping<T>(
    Expression<Func<T, object>> Expression,
    bool AsFactor
);
