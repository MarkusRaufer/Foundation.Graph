namespace Foundation.Graph;

using System.Runtime.Serialization;

public class GraphException : Exception
{
    public GraphException()
    {
    }
    public GraphException(string message)
        : base(message)
    {
    }

    public GraphException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected GraphException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
