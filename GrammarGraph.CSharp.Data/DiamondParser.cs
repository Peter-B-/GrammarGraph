using GrammarGraph.CSharp.Data.Diamonds;
using System.Globalization;

namespace GrammarGraph.CSharp.Data;

internal static class DiamondParser
{
    private static readonly CultureInfo c = CultureInfo.InvariantCulture;

    private static Clarity ParseClarity(string v) => v.Trim('"') switch
    {
        "I1" => Clarity.I1,
        "SI2" => Clarity.SI2,
        "SI1" => Clarity.SI1,
        "VS2" => Clarity.VS2,
        "VS1" => Clarity.VS1,
        "VVS2" => Clarity.VVS2,
        "VVS1" => Clarity.VVS1,
        "IF" => Clarity.IF,
        _ => throw new ArgumentException($"\"{v}\" is an unknown Clarity"),
    };

    private static Color ParseColor(string v) => v.Trim('"') switch
    {
        "J" => Color.J,
        "I" => Color.I,
        "H" => Color.H,
        "G" => Color.G,
        "F" => Color.F,
        "E" => Color.E,
        "D" => Color.D,
        _ => throw new ArgumentException($"\"{v}\" is an unknown Color"),
    };

    private static Cut ParseCut(string v) => v.Trim('"') switch
    {
        "Fair" => Cut.Fair,
        "Good" => Cut.Good,
        "Very Good" => Cut.VeryGood,
        "Premium" => Cut.Premium,
        "Ideal" => Cut.Ideal,
        _ => throw new ArgumentException($"\"{v}\" is an unknown Cut"),
    };

    public static Diamond ParseDiamond(string[] parts) =>
        new Diamond(
            double.Parse(parts[0], c),
            ParseCut(parts[1]),
            ParseColor(parts[2]),
            ParseClarity(parts[3]),
            double.Parse(parts[4], c),
            double.Parse(parts[5], c),
            double.Parse(parts[6], c),
            double.Parse(parts[7], c),
            double.Parse(parts[8], c),
            double.Parse(parts[9], c)
            );
}
