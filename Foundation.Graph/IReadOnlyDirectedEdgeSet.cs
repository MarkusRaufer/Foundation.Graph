namespace Foundation.Graph;

public interface IReadOnlyDirectedEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
    /// <summary>
    /// Returns the incoming edges of a node.
    /// </summary>
    /// <param name="node">node with the incoming edges.</param>
    /// <returns></returns>
    IEnumerable<TEdge> IncomingEdges(TNode node);

    /// <summary>
    /// Returns the nodes from the incoming edges.
    /// </summary>
    /// <param name="node">node with the incoming edges.</param>
    /// <param name="predicate">Filter to reduce the result.</param>
    /// <returns></returns>
    IEnumerable<TNode> IncomingNodes(TNode node, Func<TEdge, bool>? predicate = null);

    /// <summary>
    /// returns all outgoing edges of a node.
    /// </summary>
    /// <param name="node">source node.</param>
    /// <param name="predicate">filter to reduce the returned set.</param>
    /// <returns></returns>
    IEnumerable<TEdge> OutgoingEdges(TNode node);

    /// <summary>
    /// returns a list of target nodes of the outgoing edges.
    /// </summary>
    /// <param name="node">source node.</param>
    /// <param name="predicate">filter to reduce the returned set.</param>
    /// <returns></returns>
    IEnumerable<TNode> OutgoingNodes(TNode node, Func<TEdge, bool>? predicate = null);
}

public interface IReadOnlyDirectedEdgeSet<TNode, TEdgeId, TEdge> : IReadOnlyDirectedEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
{
}
