namespace GrammarGraph.Exceptions;

public class GraphicsConfigurationException : Exception
{
    public GraphicsConfigurationException()
    {
    }

    public GraphicsConfigurationException(string? message) : base(message)
    {
    }

    public GraphicsConfigurationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
