﻿open GrammarGraph
open GrammarGraph.Data

let diamonds = DataSets.diamonds ()

for d in diamonds |> List.truncate 10 do
    printfn "%s" (d.ToString())
    d.Carat |> ignore
    d.Clarity |> ignore

let graph =
    diamonds
    |> Graph.create
    |> Aes.x (fun x -> x.Price)
    |> Aes.y (fun x -> x.Carat)
    |> Geom.point
    |> Graph.plot