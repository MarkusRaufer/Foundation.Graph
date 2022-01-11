namespace Foundation.Graph;

using System.Runtime.Serialization;

[Serializable]
public class NodeSetException : Exception
{
    public NodeSetException()
    {
    }

    public NodeSetException(string message)
        : base(message)
    {
    }

    public NodeSetException(string message, Exception inner)
        : base(message, inner)
    {
    }

    protected NodeSetException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
