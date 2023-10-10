namespace GrammarGraph.CSharp.Data.Diamonds;

/// <summary>
/// 
/// </summary>
/// <param name="Carat">Weight of the diamond in [ct]</param>
/// <param name="Cut">Quality of the cut</param>
/// <param name="Color">Diamond colour, from D (best) to J (worst)</param>
/// <param name="Clarity">A measurement of how clear the diamond is</param>
/// <param name="DepthRatio">Total depth percentage = z / mean(x, y) = 2 * z / (x + y) (43--79)</param>
/// <param name="Table">Width of top of diamond relative to widest point (43--95)</param>
/// <param name="Price">Price in US dollars [$]</param>
/// <param name="Length">Length in [mm]</param>
/// <param name="Width">Width in [mm]</param>
/// <param name="Depth">Depth in [mm]</param>
public record Diamond(
    double Carat,
    Cut Cut,
    Color Color,
    Clarity Clarity,
    double DepthRatio,
    double Table,
    double Price,
    double Length,
    double Width,
    double Depth
    );
