namespace GrammarGraph.Extensions;

public static class StringExtensions
{
    public static string JoinStrings(this IEnumerable<string> parts, string separator)
    {
        return string.Join(separator, parts);
    }
}
