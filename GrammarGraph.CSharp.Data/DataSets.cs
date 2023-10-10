using GrammarGraph.CSharp.Data.Diamonds;
using System.Globalization;

namespace GrammarGraph.CSharp.Data;

public static class DataSets
{
    private static readonly CultureInfo c = CultureInfo.InvariantCulture;

    public static IReadOnlyList<Diamond> GetDiamonds()
    {
        return ResourceCsvReader.ReadCsv("diamonds")
            .Skip(1)
            .Select(line => line.Split(','))
            .Select(DiamondParser.ParseDiamond)
            .ToList();


    }
}
