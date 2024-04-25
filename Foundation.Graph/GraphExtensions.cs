using Foundation;
using Foundation.Collections.Generic;

namespace Foundation.Graph;

public static class GraphExtensions
{
    /// <summary>
    /// Copies nodes and edges from source to target.
    /// </summary>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    /// <typeparam name="TEdge">Type of edges.</typeparam>
    /// <param name="source">Source graph.</param>
    /// <param name="target">Target graph.</param>
    public static void CopyTo<TNode, TEdge>(this IReadOnlyGraph<TNode, TEdge> source, IMutableGraph<TNode, TEdge> target)
        where TEdge : IEdge<TNode>
    {
        target.AddNodes(source.Nodes);
        target.AddEdges(source.Edges);
    }

    /// <summary>
    /// Copies nodes and edges from source to target.
    /// </summary>
    /// <typeparam name="TNodeId">Identifier of the node.</typeparam>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    /// <typeparam name="TEdge">Type of edges.</typeparam>
    /// <param name="source">Source graph.</param>
    /// <param name="target">Target graph.</param>
    public static void CopyTo<TNodeId, TNode, TEdge>(this IReadOnlyGraph<TNodeId, TNode, TEdge> source, IMutableGraph<TNodeId, TNode, TEdge> target)
        where TEdge : IEdge<TNodeId>
    {
        var nodes = source.NodeIds.FilterMap(x =>
        {
            if (source.TryGetNode(x, out var node)) return Option.Some((id: x, node!));

            return Option.None<(TNodeId, TNode)>();
        });
        target.AddNodes(nodes);
        target.AddEdges(source.Edges);
    }

    /// <summary>
    /// Copies nodes and edges from source to target.
    /// </summary>
    /// <typeparam name="TNodeId">Identifier of the node.</typeparam>
    /// <typeparam name="TNode">Type of nodes.</typeparam>
    /// <typeparam name="TEdgeId">Identifier of the edge.</typeparam>
    /// <typeparam name="TEdge">Type of edges.</typeparam>
    /// <param name="source">Source graph.</param>
    /// <param name="target">Target graph.</param>
    public static void CopyTo<TNodeId, TNode, TEdgeId, TEdge>(this IReadOnlyGraph<TNodeId, TNode, TEdgeId, TEdge> source, IMutableGraph<TNodeId, TNode, TEdgeId, TEdge> target)
        where TEdge : IEdge<TEdgeId, TNodeId>
    {
        var nodes = source.NodeIds.FilterMap(x =>
        {
            if (source.TryGetNode(x, out var node)) return Option.Some((id: x, node!));

            return Option.None<(TNodeId, TNode)>();
        });
        target.AddNodes(nodes);
        target.AddEdges(source.Edges);
    }

    /// <summary>
    /// Replaces source node with target node. Edges are considered.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="graph"></param>
    /// <param name="source"></param>
    /// <param name="target"></param>
    public static void ReplaceNode<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode source, TNode target, Func<TNode, TNode, TEdge> edgeFactory)
        where TEdge : IEdge<TNode>
    {
        graph.ThrowIfNull();
        source.ThrowIfNull();
        target.ThrowIfNull();
        edgeFactory.ThrowIfNull();

        if (!graph.ExistsNode(target)) graph.AddNode(target);

        var incomingEdges = graph.Edges.Where(x => x.Target!.Equals(source));

        foreach (var incomingEdge in incomingEdges)
        {
            var replaceEdge = edgeFactory(incomingEdge.Source, target);

            graph.RemoveEdge(incomingEdge);
            graph.AddEdge(replaceEdge);
        }

        var outgoingEdges = graph.Edges.Where(x => x.Source!.Equals(source));

        foreach (var outgoingEdge in outgoingEdges)
        {
            var replaceEdge = edgeFactory(target, outgoingEdge.Target);

            graph.RemoveEdge(outgoingEdge);
            graph.AddEdge(replaceEdge);
        }

        graph.RemoveNode(source);
    }
}

