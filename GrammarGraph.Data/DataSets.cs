using GrammarGraph.Data.Diamonds;
using GrammarGraph.Data.Mpg;

namespace GrammarGraph.Data;

public static class DataSets
{
    public static IReadOnlyList<Diamond> GetDiamonds()
    {
        return ResourceCsvReader.ReadCsv("diamonds")
            .Skip(1)
            .Select(line => line.Split(','))
            .Select(DiamondParser.ParseDiamond)
            .ToList();
    }

    public static IReadOnlyList<FuelEconomy> GetFuelEconomies()
    {
        return ResourceCsvReader.ReadCsv("mpg")
            .Skip(1)
            .Select(line => line.Split(','))
            .Select(FuelEconomyParser.ParseFuelEconomy)
            .ToList();
    }
}
