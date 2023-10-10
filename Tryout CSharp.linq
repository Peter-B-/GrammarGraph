<Query Kind="Statements">
  <Reference Relative="GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.dll">D:\Projekte\GrammarGraph\GrammarGraph.CSharp\bin\Debug\net8.0\GrammarGraph.CSharp.dll</Reference>
  <Namespace>GrammarGraph.CSharp</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

new List<Item>()
	.CreateChart()
	.SetAesthetics(AestheticsId.X, d => d.TimeStamp)
	.SetAesthetics(AestheticsId.Y, d => d.Count)
	.WithGeom(b => b.Point())
	.WithGeom(b => b.Line(
				  g => g.WithAesthetics(AestheticsId.Color, d => d.Server)
			  ))
	.InFacets(d => d.User)
	.Dump();



public record Item(
	DateTime TimeStamp,
	int Count,
	string Server,
	string User,
	TimeSpan RunTime,
	TimeSpan QueryTime
);