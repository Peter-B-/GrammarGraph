<Query Kind="FSharpProgram">
  <Reference Relative="GrammarGraph.Data\bin\Debug\net7.0\GrammarGraph.Data.dll">D:\Projekte\GrammarGraph\GrammarGraph.Data\bin\Debug\net7.0\GrammarGraph.Data.dll</Reference>
  <Reference Relative="GrammarGraph\bin\Debug\net7.0\GrammarGraph.dll">D:\Projekte\GrammarGraph\GrammarGraph\bin\Debug\net7.0\GrammarGraph.dll</Reference>
</Query>

open GrammarGraph
open GrammarGraph.Data

DataSets.diamonds ()
|> Graph.create
|> Aes.x (fun x -> x.Price)
|> Aes.y (fun x -> x.Carat)
|> Geom.point
|> Graph.plot
|> Dump
