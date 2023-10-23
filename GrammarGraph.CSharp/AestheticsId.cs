namespace GrammarGraph.CSharp;

public record AestheticsId(string Id)
{
    public static AestheticsId Color => new(Known.Color);
    public static AestheticsId Fill => new(Known.Fill);
    public static AestheticsId Size => new(Known.Size);
    public static AestheticsId Shape => new(Known.Shape);
    public static AestheticsId X => new(Known.X);
    public static AestheticsId Y => new(Known.Y);

    public static implicit operator AestheticsId(string id)
    {
        return new AestheticsId(id);
    }

    private string ToDump()
    {
        return Id;
    }

    internal static class Intern
    {
        public static AestheticsId FacetRow => new(Known.FacetRow);
        public static AestheticsId FacetColumn => new(Known.FacetColumn);
        public static AestheticsId Facet => new(Known.Facet);
    }

    internal static class Known
    {
        public static string Color => "color";
        public static string Fill => "fill";
        public static string Size => "size";
        public static string Shape => "shape";
        public static string X => "x";
        public static string Y => "y";
        public static string FacetRow => "FacetRow";
        public static string FacetColumn => "FacetColumn";
        public static string Facet => "Facet";
    }
}
