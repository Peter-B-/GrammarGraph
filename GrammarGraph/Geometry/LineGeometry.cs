using GrammarGraph.Render;

namespace GrammarGraph.Geometry;

public record LineGeometry<T>(LineType LineType = LineType.Line) : Geometry<T>
{
    public override IGeometryLogic ConstructLogic()
    {
        return new LineGeometryLogic();
    }
}
