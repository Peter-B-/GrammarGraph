<Query Kind="Expression">
  <Reference Relative="GrammarGraph.Data\bin\Debug\net8.0\GrammarGraph.Data.dll">D:\Projekte\GrammarGraph\GrammarGraph.Data\bin\Debug\net8.0\GrammarGraph.Data.dll</Reference>
  <Reference Relative="GrammarGraph\bin\Debug\net8.0\GrammarGraph.dll">D:\Projekte\GrammarGraph\GrammarGraph\bin\Debug\net8.0\GrammarGraph.dll</Reference>
  <Namespace>GrammarGraph</Namespace>
  <Namespace>GrammarGraph.Data</Namespace>
  <Namespace>GrammarGraph.Geometry</Namespace>
  <Namespace>GrammarGraph.Statistics</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

DataSets.GetDiamonds()
	.Take(100000)
	.CreateChart()
	.SetAesthetics(AestheticsId.X, d => d.Price)
	.SetAesthetics(AestheticsId.Color, d => d.Cut)
	.Add(b => b.Line(stat: new EcdfStatistic()))
	.InFacets(d => d.Color)
	
