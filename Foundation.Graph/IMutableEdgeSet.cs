namespace Foundation.Graph;

/// <summary>
/// Contract for a mutable edge set.
/// </summary>
/// <typeparam name="TNode">Typ der Knoten.</typeparam>
/// <typeparam name="TEdge">Typ der Kanten.</typeparam>
public interface IMutableEdgeSet<in TNode, TEdge>
    where TEdge : IEdge<TNode>
{
    /// <summary>
    /// adds an edge.
    /// </summary>
    /// <param name="edge"></param>
    void AddEdge(TEdge edge);

    /// <summary>
    /// adds a list of edges.
    /// </summary>
    /// <param name="edges"></param>
    void AddEdges(IEnumerable<TEdge> edges);

    /// <summary>
    /// clears the edge list.
    /// </summary>
    void ClearEdges();

    /// <summary>
    /// removes an edge.
    /// </summary>
    /// <param name="edge"></param>
    bool RemoveEdge(TEdge edge);

    /// <summary>
    /// removes a list of edges.
    /// </summary>
    /// <param name="edges"></param>
    void RemoveEdges(IEnumerable<TEdge> edges);
}

/// <summary>
/// Contract for a mutable edge set with identifiable edges.
/// </summary>
/// <typeparam name="TNode">Type of node.</typeparam>
/// <typeparam name="TEdgeId">Type of edge id.</typeparam>
/// <typeparam name="TEdge">Type of edge.</typeparam>
public interface IMutableEdgeSet<in TNode, TEdgeId, TEdge> : IMutableEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
{
    bool RemoveEdge(TEdgeId edgeId);
    void RemoveEdges(IEnumerable<TEdgeId> edgeIds);
}
