using System.Collections.Immutable;
using GrammarGraph.Extensions;
using GrammarGraph.Internal;
using Plotly.NET;

namespace GrammarGraph.Render;

public record PlotDescription(
    PanelCollection Panels,
    ImmutableArray<Layer> Layers
);

public record Layer(
    IGeometryLogic Geometry,
    ImmutableArray<Group> Groups,
    DataFrame Data
);

public record PanelCollection(
    int Rows, int Columns,
    ImmutableArray<Panel> Panels
);

public abstract record Panel;

public record GridPanel(
    string RowLabel,
    string ColLabel,
    int RowIdx,
    int ColIdx
) : Panel;

public record WrapPanel(
    string Label,
    int RowIdx,
    int ColIdx
) : Panel;

public record SinglePanel : Panel;

public record Factor(
    int Index,
    ImmutableArray<string> Levels
)
{
    public string Value => Levels[Index];

    public override string ToString()
    {
        var factors = Levels
            .Take(6)
            .JoinStrings(", ");
        if (Levels.Length > 6)
            factors += ", ...";
        return $"{Value} / [{factors}]";
    }
}

public interface IGeometryLogic
{
    GenericChart.GenericChart CreateChart(PanelGroupData data);
}

public class LineGeometryLogic : IGeometryLogic
{
    public GenericChart.GenericChart CreateChart(PanelGroupData data)
    {
        var xType = data[AestheticsId.X].Type;
        var yType = data[AestheticsId.Y].Type;

        var resultChart = (xType, yType) switch
        {
            (DataColumnType.Double, DataColumnType.Double) =>
                Chart2D.Chart.Line<double, double, string>(
                    data.GetDoubleColumn(AestheticsId.X).Values,
                    data.GetDoubleColumn(AestheticsId.Y).Values
                ),
            (DataColumnType.Factor, DataColumnType.Double) =>
                Chart2D.Chart.Line<string, double, string>(
                    data.GetFactorColumn(AestheticsId.X).Values,
                    data.GetDoubleColumn(AestheticsId.Y).Values
                ),
            (DataColumnType.Double, DataColumnType.Factor) =>
                Chart2D.Chart.Line<double, string, string>(
                    data.GetDoubleColumn(AestheticsId.X).Values,
                    data.GetFactorColumn(AestheticsId.Y).Values
                ),
            (DataColumnType.Factor, DataColumnType.Factor) =>
                Chart2D.Chart.Line<string, string, string>(
                    data.GetFactorColumn(AestheticsId.X).Values,
                    data.GetFactorColumn(AestheticsId.Y).Values
                )
        };
        return resultChart;
    }
}

public class PointGeometryLogic : IGeometryLogic
{
    public GenericChart.GenericChart CreateChart(PanelGroupData data)
    {
        var xType = data[AestheticsId.X].Type;
        var yType = data[AestheticsId.Y].Type;


        var resultChart = (xType, yType) switch
        {
            (DataColumnType.Double, DataColumnType.Double) =>
                Chart2D.Chart.Point<double, double, string>(
                    data.GetDoubleColumn(AestheticsId.X).Values,
                    data.GetDoubleColumn(AestheticsId.Y).Values
                ),
            (DataColumnType.Factor, DataColumnType.Double) =>
                Chart2D.Chart.Point<string, double, string>(
                    data.GetFactorColumn(AestheticsId.X).Values,
                    data.GetDoubleColumn(AestheticsId.Y).Values
                ),
            (DataColumnType.Double, DataColumnType.Factor) =>
                Chart2D.Chart.Point<double, string, string>(
                    data.GetDoubleColumn(AestheticsId.X).Values,
                    data.GetFactorColumn(AestheticsId.Y).Values
                ),
            (DataColumnType.Factor, DataColumnType.Factor) =>
                Chart2D.Chart.Point<string, string, string>(
                    data.GetFactorColumn(AestheticsId.X).Values,
                    data.GetFactorColumn(AestheticsId.Y).Values
                )
        };
        return resultChart;
    }
}
