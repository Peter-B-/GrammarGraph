using System.Globalization;

namespace GrammarGraph.CSharp.Data.Mpg;

public static class FuelEconomyParser
{
    private static readonly CultureInfo C = CultureInfo.InvariantCulture;

    public static FuelEconomy ParseFuelEconomy(string[] parts)
    {
        return new FuelEconomy(
            parts[0],
            parts[1],
            float.Parse(parts[2], C),
            int.Parse(parts[3], C),
            int.Parse(parts[4], C),
            parts[5],
            ParseDrive(parts[6]),
            new Consumption(
                float.Parse(parts[7], C),
                float.Parse(parts[8], C)
            ),
            parts[9],
            parts[10]
        );
    }

    private static Drive ParseDrive(string part)
    {
        return part switch
        {
            "f" => Drive.Front,
            "r" => Drive.Rear,
            "4" => Drive.Four,
            _ => throw new ArgumentOutOfRangeException(nameof(part), part, $"\"{part}\" is an unknown Drive")
        };
    }
}
