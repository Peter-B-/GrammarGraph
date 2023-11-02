using System.Collections.Immutable;

namespace GrammarGraph.CSharp.Extensions;

public static class ImmutableArrayFactory
{
    public static ImmutableArray<T> Repeat<T>(T value, int count)
    {
        var builder = ImmutableArray.CreateBuilder<T>(count);
        builder.AddRange(Enumerable.Repeat(value, count));
        return builder.MoveToImmutable();
    }
}
