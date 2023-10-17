using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection.Emit;
using GrammarGraph.CSharp.Internal;
using Plotly.NET;

namespace GrammarGraph.CSharp.Render;

public class PlotlyRenderEngine
{
    public static Type GetObjectType<T>(Expression<Func<T, object>> expr)
    {
        if (expr.Body.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked)
        {
            if (expr.Body is UnaryExpression unary)
                return unary.Operand.Type;
        }

        return expr.Body.Type;
    }

    public GenericChart.GenericChart Render<T>(GgChart<T> chart)
    {
        var combinedLayers = chart.Layers
            .Select(layer =>
                        layer with {Aesthetics = CombineAesthetics(chart, layer)}
            )
            .ToImmutableArray();


        var rawLayerData = combinedLayers
            .Select(layer => GetRawData(layer, chart.Data))
            .ToImmutableArray();

        var layerData = rawLayerData
            .Select(ApplyStatistics)
            .ToImmutableArray();

        var firstLayer = layerData.First();

        return Chart2D.Chart.Point<double, double, string>(
            firstLayer.Data.GetDoubleColumn(AestheticsId.X).Values,
            firstLayer.Data.GetDoubleColumn(AestheticsId.Y).Values
        );
    }

    private LayerData<T> ApplyStatistics<T>(LayerData<T> layerData)
    {
        var dataFrame = layerData.Layer.Stat.Compute(layerData.Data);
        return layerData with {Data = dataFrame};
    }

    private ImmutableDictionary<AestheticsId, Mapping<T>> CombineAesthetics<T>(GgChart<T> chart, Layer<T> layer)
    {
        return chart.Aesthetics.SetItems(layer.Aesthetics);
    }

    private static DataColumn CreateDataColumn<T>(IEnumerable<T> data, Mapping<T> mapping)
    {
        var objectType = GetObjectType(mapping.Expression);
        var accessor = mapping.Expression.Compile();

        if (objectType == typeof(double) || objectType == typeof(float) || objectType == typeof(int))
        {
            Func<T, double> extract = objectType switch
            {
                _ when objectType == typeof(double) => d => (double)accessor(d),
                _ when objectType == typeof(float) => d => (double)(float)accessor(d),
                _ when objectType == typeof(int) => d => (double)(int)accessor(d),
            };

            var values = data
                .Select(d => extract(d))
                .ToImmutableArray();
            return new DoubleColumn(values);
        }

        if (objectType == typeof(string))
        {
            var values = data
                .Select(d => (string) accessor(d))
                .ToImmutableArray();
            return new FactorColumn(values);
        }

        {
            var values = data
                .Select(d => accessor(d).ToString() ?? "na.")
                .ToImmutableArray();
            return new FactorColumn(values);
        }
    }

    private static LayerData<T> GetRawData<T>(Layer<T> layer, IEnumerable<T> data)
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
        return new LayerData<T>(layer, new DataFrame(columns));
    }
}

public record LayerData<T>(
    Layer<T> Layer,
    DataFrame Data
);
