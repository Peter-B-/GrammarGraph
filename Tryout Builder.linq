<Query Kind="Statements">
  <Reference Relative="GrammarGraph.Data\bin\Debug\net8.0\GrammarGraph.Data.dll">D:\Projekte\GrammarGraph\GrammarGraph.Data\bin\Debug\net8.0\GrammarGraph.Data.dll</Reference>
  <Reference Relative="GrammarGraph\bin\Debug\net8.0\GrammarGraph.dll">D:\Projekte\GrammarGraph\GrammarGraph\bin\Debug\net8.0\GrammarGraph.dll</Reference>
  <Namespace>GrammarGraph</Namespace>
  <Namespace>GrammarGraph.Data</Namespace>
  <Namespace>GrammarGraph.Geometry</Namespace>
  <Namespace>GrammarGraph.Render</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

var chart = 
	DataSets.GetDiamonds()
		.Take(100)
		.CreateChart()
		.SetAesthetics(AestheticsId.X, d => d.Carat)
		.SetAesthetics(AestheticsId.Y, d => d.Price)
		.Add(b => b.Point(b => b.SetAesthetics(AestheticsId.Color, d => d.Cut)))
		.Add(b => b.Line())
		.InFacets(d => d.Cut, d => d.Clarity)
		;
	
var plot = new PlotBuilder().BuildPlot(chart);	
plot.Dump();


