namespace Foundation.Graph;

using System.Runtime.Serialization;

[Serializable]
public class EdgeSetException : Exception
{
    public EdgeSetException() { }
    public EdgeSetException(string message) : base(message) { }
    public EdgeSetException(string message, Exception inner) : base(message, inner) { }
    protected EdgeSetException(
        SerializationInfo info,
        StreamingContext context) : base(info, context) { }
}

