using System.Collections.Immutable;
using GrammarGraph.Extensions;
using GrammarGraph.Facets;
using GrammarGraph.Internal;
using GrammarGraph.Statistics;

namespace GrammarGraph.Render;

public class PlotBuilder
{
    public PlotDescription BuildPlot<T>(GgChart<T> chart)
    {
        var combinedLayers = chart.Layers
            .Select(layer =>
                        layer with {Aesthetics = CombineAesthetics(chart, layer)}
            )
            .ToImmutableArray();

        var chartData = chart.Data as IReadOnlyList<T> ?? chart.Data.ToList();

        var facet = chart.Facet ?? new SingleFacet<T>();
        var panels = facet.GetPanels(chartData);

        var rawLayerData = combinedLayers
            .Select(layer => new
            {
                Layer = ConstructLayers(layer, chartData, facet, panels.Panels),
                LayerDefinition = layer,
            })
            .ToImmutableArray();

        var layerData = rawLayerData
            .Select(item =>
            {
                var layerDefinition = item.LayerDefinition;
                return ApplyStatistics(item.Layer, layerDefinition.Stat);
            })
            .ToImmutableArray();

        var plot = new PlotDescription(
            panels,
            layerData
        );
        return plot;
    }


    public static DataColumn CreateDataColumn<T>(IEnumerable<T> data, Mapping<T> mapping)
    {
        var objectType = PlotlyRenderEngine.GetObjectType(mapping.Expression);
        var accessor = mapping.Expression.Compile();

        if (!mapping.AsFactor)
            if (objectType == typeof(double) || objectType == typeof(float) || objectType == typeof(int))
            {
                Func<T, double> extract = objectType switch
                {
                    _ when objectType == typeof(double) => d => (double) accessor(d),
                    _ when objectType == typeof(float) => d => (float) accessor(d),
                    _ when objectType == typeof(int) => d => (int) accessor(d)
                };

                var values = data
                    .Select(d => extract(d))
                    .ToImmutableArray();
                return new DoubleColumn(values);
            }

        if (objectType == typeof(string))
        {
            var values = data
                .Select(d => (string) accessor(d));
            return FactorColumn.FromStrings(values);
        }

        {
            var values = data
                .Select(d => accessor(d).ToString() ?? "na.")
                .ToImmutableArray();
            return FactorColumn.FromStrings(values);
        }
    }


    private Layer ApplyStatistics(Layer layer, Statistic statistic)
    {
        var dataFrame = statistic.Compute(layer.Data);
        return layer with {Data = dataFrame};
    }

    private ImmutableDictionary<AestheticsId, Mapping<T>> CombineAesthetics<T>(GgChart<T> chart, Layer<T> layer) =>
        chart.Aesthetics
            .SetItems(layer.Aesthetics);


    private static Layer ConstructLayers<T>(
        Layer<T> layer, IReadOnlyList<T> data,
        Facet<T> facet, ImmutableArray<Panel> panels
    )
    {
        var columns =
            layer.Aesthetics
                .Select(kvp =>
                {
                    var (aestheticsId, mapping) = kvp;

                    var dataColumn = CreateDataColumn(data, mapping);
                    return (Key: aestheticsId, Data: dataColumn);
                })
                .ToImmutableDictionary(m => m.Key, m => m.Data);

        var panelMap = facet.AssignToPanels(data, panels);

        // Find all categorical columns (factors) and their aesthetics
        var factorAesthetics =
            columns
                .Where(kvp => kvp.Value is FactorColumn)
                .Select(kvp => new AestheticsFactorColumn(kvp.Key, (kvp.Value as FactorColumn)!))
                .ToList();

        // Create groups from factor columns
        var (groups, groupMap) = CreateGroupsFrom(factorAesthetics, data.Count);

        // Remove factor columns from data set
        var nonCategoricalColumns = columns
            .RemoveRange(factorAesthetics.Select(fcc => fcc.Aesthetics));


        var geometryLogic = layer.Geometry.ConstructLogic();
        return new Layer(geometryLogic, groups, new DataFrame(nonCategoricalColumns, panelMap, groupMap));
    }

    private static ImmutableArray<Group> CreateGroups(IReadOnlyList<AestheticsFactorColumn> factorColumns)
    {
        var array = new AestheticsFactor[factorColumns.Count];

        var count = 1;
        foreach (var column in factorColumns)
            count *= column.FactorColumn.Levels.Length;

        var builder = ImmutableArray.CreateBuilder<Group>(count);

        FillArray(0);

        return builder.MoveToImmutable();

        void FillArray(int aesIdx)
        {
            foreach (var levelFactor in factorColumns[aesIdx].FactorColumn.GetLevelFactors())
            {
                array[aesIdx] = new AestheticsFactor(factorColumns[aesIdx].Aesthetics, levelFactor);
                if (aesIdx == factorColumns.Count-1)
                    builder.Add(new Group(ImmutableArray.Create(array)));
                else
                    FillArray(aesIdx + 1);
            }
        }
    }

    private static (ImmutableArray<Group> groups, ImmutableArray<Group> groupMap) CreateGroupsFrom(
        IReadOnlyList<AestheticsFactorColumn> factorColumns, int dataCount)
    {
        // There is no grouping by categorical variables. Use default group.
        if (!factorColumns.Any())
        {
            var group = Group.Default;
            var maps = ImmutableArrayFactory.Repeat(group, dataCount);

            return (ImmutableArray.Create(group), maps);
        }

        var groups = CreateGroups(factorColumns);

        // Assign values to groups.
        // There has to be a better way than this.
        var itemCount = factorColumns[0].FactorColumn.Indices.Length;
        var indices = new int[factorColumns.Count];
        var mapBuilder = ImmutableArray.CreateBuilder<Group>(itemCount);
        for (var dataIdx = 0; dataIdx < itemCount; dataIdx++)
        {
            for (var i = 0; i < factorColumns.Count; i++)
                indices[i] = factorColumns[i].FactorColumn.Indices[dataIdx];

            var group = groups.First(gr => gr.MatchesIndices(indices));
            mapBuilder.Add(group);
        }

        var groupMaps = mapBuilder.MoveToImmutable();

        return (groups, groupMaps);
    }
}

public static class FactorColumnExtensions
{
    public static IEnumerable<Factor> GetLevelFactors(this FactorColumn column) =>
        column.Levels
            .Select((_, idx) => new Factor(idx, column.Levels));
}
