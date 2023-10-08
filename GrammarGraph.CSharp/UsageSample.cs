namespace GrammarGraph.CSharp;

public static class UsageSample
{
    public static void Run()
    {
        var data = new List<Item>();

        var chart =
                data
                    .CreateChart()
                    .WithAesthetics(AestheticsId.X, d => d.TimeStamp)
                    .WithAesthetics(AestheticsId.Y, d => d.Count)
                    .WithGeom(b => b.Point())
                    .WithGeom(b => b.Line(
                                  g => g.WithAesthetics(AestheticsId.Color, d => d.Server)
                              ))
            ;
    }
}

public record Item(
    DateTime TimeStamp,
    int Count,
    string Server,
    string User,
    TimeSpan RunTime,
    TimeSpan QueryTime
);