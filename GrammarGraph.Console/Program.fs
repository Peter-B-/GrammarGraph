open GrammarGraph
open GrammarGraph.Data

let diamonds = DataSets.diamonds ()

for d in diamonds |> List.truncate 10 do
    printfn "%s" (d.ToString())
    d.Carat |> ignore
    d.Clarity |> ignore

let graph =
    diamonds
    |> Grammar.graph