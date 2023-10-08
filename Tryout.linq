<Query Kind="FSharpProgram">
  <Reference Relative="GrammarGraph.Data\bin\Debug\net7.0\GrammarGraph.Data.dll">D:\Projekte\GrammarGraph\GrammarGraph.Data\bin\Debug\net7.0\GrammarGraph.Data.dll</Reference>
  <Reference Relative="GrammarGraph\bin\Debug\net7.0\GrammarGraph.dll">D:\Projekte\GrammarGraph\GrammarGraph\bin\Debug\net7.0\GrammarGraph.dll</Reference>
</Query>

open GrammarGraph
open GrammarGraph.Data


DataSets.diamonds ()
//|> Dump
|> Graph.create
|> Aes.x( fun x -> x.Depth )
|> Aes.y( fun x -> x.DepthRatio )
|> Geom.point
|> Graph.plot
|> Dump
