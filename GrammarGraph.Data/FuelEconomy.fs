module GrammarGraph.Data.FuelEconomy

[<Measure>]
type l

[<Measure>]
type gallon

[<Measure>]
type mile

type Drive =
    | ``Front wheel drive``
    | ``Rear wheel drive``
    | ``Four wheel drive``

type Consumption =
    { City: float<mile / gallon>
      Highway: float<mile / gallon> }

module Drive =
    let parse =
        function
        | "f" -> ``Front wheel drive``
        | "r" -> ``Rear wheel drive``
        | "4" -> ``Four wheel drive``
        | x -> failwith $"%s{x} is an unknown drive"

type FuelEconomy =
    { Manufacturer: string
      Model: string
      Displacement: float<l>
      Year: int
      Cylinders: int
      Transmission: string
      Drive: Drive
      Consumption: Consumption
      FuelType: string
      Class: string }

let parse (parts: string array) =
    let l x = x * 1.0<l>
    let mpg x = x * 1.0<mile / gallon>

    let parseFloat (str:string) =
        System.Double.Parse(str, System.Globalization.CultureInfo.InvariantCulture)

    let parseInt (str:string) =
        System.Int32.Parse(str, System.Globalization.CultureInfo.InvariantCulture)

    let parseConsumption (cty, hwy) =
        { City = cty |> parseFloat |> mpg
          Highway = hwy |> parseFloat |> mpg }

    { Manufacturer = parts.[0]
      Model = parts.[1]
      Displacement = parts.[2] |> parseFloat |> l
      Year = parts.[3] |> parseInt
      Cylinders = parts.[4] |> parseInt
      Transmission = parts.[5]
      Drive = parts.[6] |> Drive.parse
      Consumption = (parts.[7], parts.[8]) |> parseConsumption
      FuelType = parts.[9]
      Class = parts.[10] }
