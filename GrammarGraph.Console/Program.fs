open System
open System.Linq.Expressions
open GrammarGraph
open GrammarGraph.Data
open GrammarGraph.Data.Diamonds

let diamonds = DataSets.diamonds ()

for d in diamonds |> List.truncate 10 do
    printfn "%s" (d.ToString())
    d.Carat |> ignore
    d.Clarity |> ignore

let graph =
    diamonds
    |> Graph.create
    |> Aes.x (fun x -> x.Table)
    |> Aes.y (fun x -> x.Carat)
    |> Geom.point
    |> Graph.plot


let fuel = DataSets.fuelEconomy ()

let graph2 =
    fuel
    |> Graph.create
    |> Aes.x (fun x -> x.Cylinders)
    |> Aes.y (fun x -> x.Year)
    |> Geom.point
    |> Graph.plot

