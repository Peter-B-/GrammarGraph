<Query Kind="FSharpProgram">
  <Reference Relative="GrammarGraph.Data\bin\Debug\net7.0\GrammarGraph.Data.dll">D:\Projekte\GrammarGraph\GrammarGraph.Data\bin\Debug\net7.0\GrammarGraph.Data.dll</Reference>
  <Reference Relative="GrammarGraph\bin\Debug\net7.0\GrammarGraph.dll">D:\Projekte\GrammarGraph\GrammarGraph\bin\Debug\net7.0\GrammarGraph.dll</Reference>
</Query>

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
