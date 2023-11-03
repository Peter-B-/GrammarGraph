using System.Collections.Immutable;

namespace GrammarGraph.Extensions;

public static class ImmutableArrayExtensions
{
    public static ImmutableArray<T> Subset<T>(this ImmutableArray<T> source, ImmutableArray<int> indices)
    {
        var builder = ImmutableArray.CreateBuilder<T>(indices.Length);

        foreach (var index in indices)
            builder.Add(source[index]);

        return builder.MoveToImmutable();
    }
}
