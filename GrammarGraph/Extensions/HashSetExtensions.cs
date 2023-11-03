namespace GrammarGraph.Extensions;

public static class HashSetExtensions
{
    public static T GetOrAdd<T>(this HashSet<T> hashSet, T item)
    {
        if (hashSet.TryGetValue(item, out var actualItem))
            return actualItem;
        hashSet.Add(item);
        return item;
    }
}
