using System.Collections.Immutable;
using GrammarGraph.CSharp.Exceptions;
using GrammarGraph.CSharp.Internal;

namespace GrammarGraph.CSharp.Statistics;

public record EcdfStatistic : Statistic
{
    public override DataFrame Compute(DataFrame data)
    {
        var (valueAesthetics, cumProbAesthetics) = (data.Contains(AestheticsId.X), data.Contains(AestheticsId.Y)) switch
        {
            (true, true) => throw new GraphicsConfigurationException($"Ecdf statistics expects either {AestheticsId.X} or {AestheticsId.Y} to be configured."),
            (false, false) => throw new GraphicsConfigurationException($"Ecdf statistics expects either {AestheticsId.X} or {AestheticsId.Y} to be configured."),
            (true, false) => (AestheticsId.X, AestheticsId.Y),
            (false, true) => (AestheticsId.Y, AestheticsId.X)
        };

        var input = data.GetDoubleColumn(valueAesthetics);

        var (values, cumProb) = ComputeEcdf(input.Values);

        var newColumns = new KeyValuePair<AestheticsId, DataColumn>[]
        {
            new(valueAesthetics, new DoubleColumn(values)),
            new(cumProbAesthetics, new DoubleColumn(cumProb))
        };

        //var newData = new DataFrame(data.Columns.SetItems(newColumns));

        //return newData;
        return data;
    }

    public static (ImmutableArray<double> values, ImmutableArray<double> cumulativeProbability) ComputeEcdf(ImmutableArray<double> data)
    {
        // Todo: Write implementation that removes duplicate values

        var sortedData = data.OrderBy(x => x).ToArray();
        var ecdf = new double[sortedData.Length];
        double n = ecdf.Length;

        for (var i = 0; i < ecdf.Length; i++)
            ecdf[i] = (i + 1) / n;

        return (ImmutableArray.Create(sortedData), ImmutableArray.Create(ecdf));
    }
}
