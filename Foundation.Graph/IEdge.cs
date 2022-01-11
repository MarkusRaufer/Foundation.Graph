namespace Foundation.Graph;

/// <summary>
/// Interface für eine Kante.
/// </summary>
/// <typeparam name="TNode">Der Knotentyp.</typeparam>
public interface IEdge<out TNode>
{
    TNode Source { get; }
    TNode Target { get; }
}

public interface IEdge<TId, out TNode>
    : IEdge<TNode>
    , IIdentifiable<TId>
    where TId : notnull
{
}
