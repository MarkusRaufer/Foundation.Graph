﻿namespace Foundation.Graph;

/// <summary>
/// Contract for an immutable edge set;
/// </summary>
/// <typeparam name="TNode">Type of the nodes.</typeparam>
/// <typeparam name="TEdge">Type of the edges.</typeparam>
public interface IReadOnlyEdgeSet<in TNode, TEdge>
    where TEdge : IEdge<TNode>
{
    /// <summary>
    /// Number of edges.
    /// </summary>
    int EdgeCount { get; }

    /// <summary>
    /// List of edges.
    /// </summary>
    IEnumerable<TEdge> Edges { get; }

    /// <summary>
    /// Checks if edge exists.
    /// </summary>
    /// <param name="edge"></param>
    /// <returns></returns>
    bool ExistsEdge(TEdge edge);

    /// <summary>
    /// Checks if edge exists.
    /// </summary>
    /// <param name="source">source node</param>
    /// <param name="target">target node</param>
    /// <returns></returns>
    bool ExistsEdge(TNode source, TNode target);

    /// <summary>
    /// Returns all edges that have node as source or destination.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    IEnumerable<TEdge> GetEdges(TNode node);
}

/// <summary>
/// Contract for an immutable edge set;
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="TEdgeId"></typeparam>
/// <typeparam name="TEdge"></typeparam>
public interface IReadOnlyEdgeSet<in TNode, TEdgeId, TEdge> : IReadOnlyEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
{
    bool ExistsEdge(TEdgeId edgeId);
    Option<TEdge> GetEdge(TEdgeId edgeId);
}
