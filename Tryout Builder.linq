<Query Kind="Statements">
  <Reference Relative="GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.Data.dll">D:\Projekte\GrammarGraph\GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.Data.dll</Reference>
  <Reference Relative="GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.dll">D:\Projekte\GrammarGraph\GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.dll</Reference>
  <Namespace>GrammarGraph.CSharp.Data</Namespace>
  <Namespace>GrammarGraph.CSharp</Namespace>
  <Namespace>GrammarGraph.CSharp.Geometry</Namespace>
  <Namespace>GrammarGraph.CSharp.Render</Namespace>
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


