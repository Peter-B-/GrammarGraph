<Query Kind="Expression">
  <Reference Relative="GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.Data.dll">D:\Projekte\GrammarGraph\GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.Data.dll</Reference>
  <Reference Relative="GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.dll">D:\Projekte\GrammarGraph\GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.dll</Reference>
  <Namespace>GrammarGraph.CSharp.Data</Namespace>
  <Namespace>GrammarGraph.CSharp</Namespace>
  <Namespace>GrammarGraph.CSharp.Geometry</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

DataSets.GetDiamonds()
	.Take(1000)
	.CreateChart()
	.SetAesthetics(AestheticsId.X, d => (float)d.Price)
	.SetAesthetics(AestheticsId.Y, d => (int)d.Carat)
	.Add(b => b.Point(b => b.SetAesthetics(AestheticsId.Color, d => d.Color)))
	.Add(b => b.Line())
	.InFacets(d => d.Cut)
	
