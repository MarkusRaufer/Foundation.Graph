namespace Foundation.Graph;

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Contract for a mutable edge set.
/// </summary>
/// <typeparam name="TNode">Typ der Knoten.</typeparam>
/// <typeparam name="TEdge">Typ der Kanten.</typeparam>
public interface IEdgeSet<in TNode, TEdge>
    : IReadOnlyEdgeSet<TNode, TEdge>
    , INotifyCollectionChanged
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
    /// If set to false, an exception will be thrown on adding an existing edge.
    /// </summary>
    bool AllowDuplicateEdges { get; }

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

public interface IEdgeSet<in TNode, TEdgeId, TEdge> 
    : IEdgeSet<TNode, TEdge>
    , IReadOnlyEdgeSet<TNode, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
{
    bool RemoveEdge([DisallowNull] TEdgeId edgeId);
    void RemoveEdges(IEnumerable<TEdgeId> edgeIds);
}
