namespace GrammarGraph.CSharp.Data.Mpg;

/// <summary>
///     An item from the fuel economy dataset shipped with ggplot.
///     <para>
///         See <see href="https://ggplot2.tidyverse.org/reference/mpg.html" />.
///     </para>
/// </summary>
public record FuelEconomy(
    string Manufacturer,
    string Model,
    float Displacement,
    int Year,
    int Cylinders,
    string Transmission,
    Drive Drive,
    Consumption Consumption,
    string FuelType,
    string Class
);
