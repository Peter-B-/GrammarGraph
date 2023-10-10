using System.Reflection;

namespace GrammarGraph.CSharp.Data;

internal static class ResourceCsvReader
{
    public static IEnumerable<string> ReadCsv(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"GrammarGraph.CSharp.Data.DataSets.{name}.csv";

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new ResourceNotFoundException($"Embedded resource {resourceName} not found.");

        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line != null)
                yield return line;
        }
    }
}
