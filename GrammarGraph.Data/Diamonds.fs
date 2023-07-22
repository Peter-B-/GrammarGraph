module GrammarGraph.Data.Diamonds  

[<Measure>]
type mm

/// US Dollar $
[<Measure>]
type USD

type Cut =
    | Fair
    | Good
    | VeryGood
    | Premium
    | Ideal

type Clarity =
    /// worst
    | I1
    | SI2
    | SI1
    | VS2
    | VS1
    | VVS2
    | VVS1
    /// best
    | IF

type Color =
    /// worst
    | J
    | I
    | H
    | G
    | F
    | E
    /// best
    | D

module Cut =
    let parse =
        function
        | "Fair" -> Fair
        | "Good" -> Good
        | "Very Good" -> VeryGood
        | "Premium" -> Premium
        | "Ideal" -> Ideal
        | x -> failwith $"%s{x} is an unknown cut"

module Clarity =
    let parse =
        function
        | "I1" -> I1
        | "SI2" -> SI2
        | "SI1" -> SI1
        | "VS2" -> VS2
        | "VS1" -> VS1
        | "VVS2" -> VVS2
        | "VVS1" -> VVS1
        | "IF" -> IF
        | x -> failwith $"%s{x} is an unknown clarity"

module Color =
    let parse =
        function
        | "J" -> J
        | "I" -> I
        | "H" -> H
        | "G" -> G
        | "F" -> F
        | "E" -> E
        | "D" -> D
        | x -> failwith $"%s{x} is an unknown color"


type Diamond =
    {
        /// Weight of the diamond
        Carat: float

        /// Quality of the cut
        Cut: Cut

        /// diamond colour, from D (best) to J (worst)
        Color: Color

        /// a measurement of how clear the diamond is
        Clarity: Clarity

        /// total depth percentage = z / mean(x, y) = 2 * z / (x + y) (43--79)
        DepthRatio: float

        /// width of top of diamond relative to widest point (43--95)
        Table: float

        /// price in US dollars
        Price: float<USD>

        Length: float<mm>
        Width: float<mm>
        Depth: float<mm>
    }
    

let parse (parts: string array) =
    let mm x = x * 1.0<mm>
    let USD x = x * 1.0<USD>

    let parseFloat str =
        System.Double.Parse(str, System.Globalization.CultureInfo.InvariantCulture)

    { Carat = parts.[0] |> parseFloat
      Cut = parts.[1] |> Cut.parse
      Color = parts.[2] |> Color.parse
      Clarity = parts.[3] |> Clarity.parse
      DepthRatio = parts.[4] |> parseFloat
      Table = parts.[5] |> parseFloat
      Price = parts.[6] |> parseFloat |> USD
      Length = parts.[7] |> parseFloat |> mm
      Width = parts.[8] |> parseFloat |> mm
      Depth = parts.[9] |> parseFloat |> mm }

