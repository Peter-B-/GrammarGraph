open GrammarGraph.Data

let diamonds = 
    DataSets.diamonds ()
    |> List.truncate 10

for d in diamonds do
    printfn "%s" (d.ToString())
    d.Carat |> ignore
    d.Clarity |> ignore


