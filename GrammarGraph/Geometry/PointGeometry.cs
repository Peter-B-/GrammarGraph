using GrammarGraph.Render;

namespace GrammarGraph.Geometry;

public record PointGeometry<T> : Geometry<T>
{
    public override IGeometryLogic ConstructLogic()
    {
        return new PointGeometryLogic();
    }
}
