using System.Linq.Expressions;

namespace GrammarGraph.CSharp.Facets;

public record GridFaced<T>(Expression<Func<T, IConvertible>> RowMap, Expression<Func<T, IConvertible>> ColMap) : Facet<T>;
