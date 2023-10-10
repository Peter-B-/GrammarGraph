namespace GrammarGraph.CSharp.Data.Mpg;

/// <param name="City">Fuel consumption in city in [miles/gallon]</param>
/// <param name="Highway">Fuel consumption on highway in [miles/gallon]</param>
public record Consumption(
    float City,
    float Highway
);
