using GrammarGraph.CSharp.Data.Diamonds;
using GrammarGraph.CSharp.Data.Mpg;

namespace GrammarGraph.CSharp.Data;

public static class DataSets
{
    public static IReadOnlyList<Diamond> GetDiamonds() =>
        ResourceCsvReader.ReadCsv("diamonds")
            .Skip(1)
            .Select(line => line.Split(','))
            .Select(DiamondParser.ParseDiamond)
            .ToList();

    public static IReadOnlyList<FuelEconomy> GetFuelEconomies() =>
        ResourceCsvReader.ReadCsv("mpg")
            .Skip(1)
            .Select(line => line.Split(','))
            .Select(FuelEconomyParser.ParseFuelEconomy)
            .ToList();
}

