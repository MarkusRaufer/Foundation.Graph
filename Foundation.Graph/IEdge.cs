namespace Foundation.Graph;

/// <summary>
/// Interface of a graph edge.
/// </summary>
/// <typeparam name="TNode">Der Knotentyp.</typeparam>
public interface IEdge<out TNode>
{
    TNode Source { get; }
    TNode Target { get; }
}

/// <summary>
/// Interface of a identifiable graph edge.
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TNode"></typeparam>
public interface IEdge<TId, out TNode>
    : IEdge<TNode>
    , IIdentifiable<TId>
{
}
