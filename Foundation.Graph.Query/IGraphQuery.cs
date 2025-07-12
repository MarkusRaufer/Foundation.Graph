namespace Foundation.Graph.Query;

public interface IGraphQuery<TNodeId, TNode, TEdge, TGraph>
    : IGraphQuery<TNodeId, TNode, TEdge, TGraph, IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
}

public interface IGraphQuery<TNodeId, TNode, TEdge, TGraph, TQueryElement>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
    where TQueryElement : IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>
{
    /// <summary>
    /// Traverse outgoing nodes.
    /// </summary>
    /// <param name="predicate">Node predicate.</param>
    /// <returns></returns>
    TQueryElement V(Func<TNode, bool> predicate);

    /// <summary>
    /// Traverse outgoing nodes with specific id.
    /// </summary>
    /// <param name="predicate">Node id predicate.</param>
    /// <returns></returns>
    TQueryElement V(Func<TNodeId, bool> predicate);
}
