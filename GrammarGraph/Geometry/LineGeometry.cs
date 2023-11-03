namespace GrammarGraph.Geometry;

public record LineGeometry<T>(LineType LineType = LineType.Line) : Geometry<T>;
