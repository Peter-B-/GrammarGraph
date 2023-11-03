namespace GrammarGraph.Exceptions;

public class GraphicsLogicException : Exception
{
    public GraphicsLogicException(string? message) : base(message)
    {
    }

    public GraphicsLogicException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
