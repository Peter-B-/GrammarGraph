
#r "../GrammarGraph.Data/bin/Debug/net7.0/GrammarGraph.Data.dll"
open GrammarGraph.Data

DataSets.fuelEconomy()
|> Seq.groupBy (fun fe -> fe.Transmission)
|> Seq.map (fun (key,gr) -> $"{key}: {gr|> Seq.length}")
|> Seq.iter (printfn "%s")
