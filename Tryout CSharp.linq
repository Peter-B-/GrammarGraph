<Query Kind="Expression">
  <Reference Relative="GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.Data.dll">D:\Projekte\GrammarGraph\GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.Data.dll</Reference>
  <Reference Relative="GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.dll">D:\Projekte\GrammarGraph\GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.dll</Reference>
  <Namespace>GrammarGraph.CSharp.Data</Namespace>
  <Namespace>GrammarGraph.CSharp</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

DataSets.GetDiamonds()
	.CreateChart()
	.SetAesthetics(AestheticsId.X, d => d.Carat)
	.SetAesthetics(AestheticsId.Y, d => d.Price)
	.WithGeom(b => b.Point())
	.WithGeom(b => b.Line(
				  g => g.WithAesthetics(AestheticsId.Color, d => d.Color)
			  ))
	.InFacets(d => d.Cut)