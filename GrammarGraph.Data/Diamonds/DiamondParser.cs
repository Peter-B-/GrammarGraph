using System.Globalization;

namespace GrammarGraph.CSharp.Data.Diamonds;

internal static class DiamondParser
{
    private static readonly CultureInfo C = CultureInfo.InvariantCulture;

    private static Clarity ParseClarity(string v)
    {
        return v.Trim('"') switch
        {
            "I1" => Clarity.I1,
            "SI2" => Clarity.SI2,
            "SI1" => Clarity.SI1,
            "VS2" => Clarity.VS2,
            "VS1" => Clarity.VS1,
            "VVS2" => Clarity.VVS2,
            "VVS1" => Clarity.VVS1,
            "IF" => Clarity.IF,
            _ => throw new ArgumentOutOfRangeException(nameof(v), v, $"\"{v}\" is an unknown Clarity")
        };
    }

    private static Color ParseColor(string v)
    {
        return v.Trim('"') switch
        {
            "J" => Color.J,
            "I" => Color.I,
            "H" => Color.H,
            "G" => Color.G,
            "F" => Color.F,
            "E" => Color.E,
            "D" => Color.D,
            _ => throw new ArgumentOutOfRangeException(nameof(v), v, $"\"{v}\" is an unknown Color")
        };
    }

    private static Cut ParseCut(string v)
    {
        return v.Trim('"') switch
        {
            "Fair" => Cut.Fair,
            "Good" => Cut.Good,
            "Very Good" => Cut.VeryGood,
            "Premium" => Cut.Premium,
            "Ideal" => Cut.Ideal,
            _ => throw new ArgumentOutOfRangeException(nameof(v), v, $"\"{v}\" is an unknown Cut")
        };
    }

    public static Diamond ParseDiamond(string[] parts)
    {
        return new Diamond(
            double.Parse(parts[0], C),
            ParseCut(parts[1]),
            ParseColor(parts[2]),
            ParseClarity(parts[3]),
            double.Parse(parts[4], C),
            double.Parse(parts[5], C),
            double.Parse(parts[6], C),
            double.Parse(parts[7], C),
            double.Parse(parts[8], C),
            double.Parse(parts[9], C)
        );
    }
}
