using GrammarGraph.Render;

namespace GrammarGraph.Geometry;

public abstract record Geometry<T>
{
    public abstract IGeometryLogic ConstructLogic();
}
