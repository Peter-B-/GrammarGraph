using System.Collections.Immutable;
using GrammarGraph.Exceptions;
using GrammarGraph.Extensions;
using GrammarGraph.Internal;

namespace GrammarGraph.Statistics;

public abstract record PanelGroupStatistic : Statistic
{
    public override DataFrame Compute(DataFrame data)
    {
        var groupedData = data.Group();

        var transformed =
            groupedData
                .Select(ComputeOnGrouped)
                .ToList();

        return DataFrame.Merge(transformed);
    }

    protected abstract PanelGroupData ComputeOnGrouped(PanelGroupData data);
}

public record EcdfStatistic : PanelGroupStatistic
{
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

    protected override PanelGroupData ComputeOnGrouped(PanelGroupData data)
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

        var builder = ImmutableDictionary.CreateBuilder<AestheticsId, DataColumn>();
        builder.Add(valueAesthetics, new DoubleColumn(values));
        builder.Add(cumProbAesthetics, new DoubleColumn(cumProb));
        return new PanelGroupData(data.Panel, data.Group, builder.ToImmutable());
    }
}
