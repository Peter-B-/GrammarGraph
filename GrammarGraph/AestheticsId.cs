namespace GrammarGraph;

public record AestheticsId(string Id)
{
    public static AestheticsId Color => new(Known.Color);
    public static AestheticsId Fill => new(Known.Fill);
    public static AestheticsId Shape => new(Known.Shape);
    public static AestheticsId Size => new(Known.Size);
    public static AestheticsId X => new(Known.X);
    public static AestheticsId Y => new(Known.Y);

    public static implicit operator AestheticsId(string id)
    {
        return new AestheticsId(id);
    }

    public override string ToString() => Id;
    private string ToDump() => Id;


    internal static class Known
    {
        public static string Color => "color";
        public static string Fill => "fill";
        public static string Shape => "shape";
        public static string Size => "size";
        public static string X => "x";
        public static string Y => "y";
    }
}
