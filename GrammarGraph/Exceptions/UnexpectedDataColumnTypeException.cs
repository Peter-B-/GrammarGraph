using GrammarGraph.CSharp.Internal;

namespace GrammarGraph.CSharp.Exceptions;

public class UnexpectedDataColumnTypeException : Exception
{
    public UnexpectedDataColumnTypeException(Type expectedType, Type foundType, string? message = null, Exception? innerException = null) :
        base(message ?? $"Expected {nameof(DataColumn)} of type {expectedType.Name}, but found {foundType.Name}.", innerException)
    {
        ExpectedType = expectedType;
        FoundType = foundType;
    }

    public Type ExpectedType { get; }
    public Type FoundType { get; }
}
