open GrammarGraph.Data



let diamonds = 
    DataSets.diamonds
    |> Seq.truncate 10

for d in diamonds do
    printfn "%f" d
