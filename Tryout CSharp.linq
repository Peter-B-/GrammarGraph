<Query Kind="Expression">
  <Reference Relative="GrammarGraph.Data\bin\Debug\net8.0\GrammarGraph.Data.dll">D:\Projekte\GrammarGraph\GrammarGraph.Data\bin\Debug\net8.0\GrammarGraph.Data.dll</Reference>
  <Reference Relative="GrammarGraph\bin\Debug\net8.0\GrammarGraph.dll">D:\Projekte\GrammarGraph\GrammarGraph\bin\Debug\net8.0\GrammarGraph.dll</Reference>
  <Namespace>GrammarGraph.CSharp</Namespace>
  <Namespace>GrammarGraph.CSharp.Data</Namespace>
  <Namespace>GrammarGraph.CSharp.Geometry</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

DataSets.GetDiamonds()
	.Take(10)
	.CreateChart()
	.SetAesthetics(AestheticsId.X, d => d.Carat)
	.SetAesthetics(AestheticsId.Y, d => d.Price)
	.Add(b => b.Point(b => b.SetAesthetics(AestheticsId.Color, d => d.Cut)))
	.Add(b => b.Line())
	.InFacets(d => d.Cut)
	
