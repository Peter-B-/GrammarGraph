using System.Linq.Expressions;

namespace GrammarGraph.CSharp.Facets;

public record WrapFaced<T>(Expression<Func<T, IConvertible>> Map) : Facet<T>;
