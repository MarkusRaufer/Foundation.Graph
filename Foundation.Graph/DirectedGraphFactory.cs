namespace Foundation.Graph;

using Foundation.Collections.Generic;
using Foundation.Graph.Algorithm;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public static class DirectedGraphFactory
{
    public static TGraph CreateSubGraphFromOutgoingEdges<TGraph, TNode, TEdge>(
        this TGraph graph,
        Func<TGraph> factory,
        [DisallowNull] TNode node,
        Func<TEdge, bool>? edgePredicate = null)
        where TEdge : IEdge<TNode>
        where TGraph : IDirectedGraph<TNode, TEdge>
    {
        var subGraph = factory();

        subGraph.AddNode(node);
        var outEdges = DirectedSearch.Bfs.OutgoingEdges(graph, node, edgePredicate);
        var outNodes = outEdges.SelectMany(edge => edge.GetNodes()).Except(node);
        subGraph.AddNodes(outNodes);
        subGraph.AddEdges(outEdges);

        return subGraph;
    }

    public static TGraph CreateSubGraphFromOutgoingEdges<TGraph, TNode, TEdge>(
        this TGraph graph,
        Func<TGraph> factory,
        Func<TNode, bool>? nodePredicate = null,
        Func<TEdge, bool>? edgePredicate = null)
        where TEdge : IEdge<TNode>
        where TGraph : IDirectedGraph<TNode, TEdge>
    {
        var nodes = null == nodePredicate ? graph.Nodes : graph.Nodes.Where(nodePredicate).ToList();

        return CreateSubGraphFromOutgoingEdges(graph, factory, nodes, edgePredicate);
    }

    public static TGraph CreateSubGraphFromOutgoingEdges<TGraph, TNode, TEdge>(
        this TGraph graph,
        Func<TGraph> factory,
        IEnumerable<TNode> nodes,
        Func<TEdge, bool>? edgePredicate = null)
        where TEdge : IEdge<TNode>
        where TGraph : IDirectedGraph<TNode, TEdge>
    {
        var subGraph = factory();

        foreach (var node in nodes)
        {
            if (null == node) continue;

            subGraph.AddNode(node);
            var outEdges = DirectedSearch.Bfs.OutgoingEdges(graph, node, edgePredicate);
            var outNodes = outEdges.SelectMany(edge => edge.GetNodes()).Except(nodes);
            subGraph.AddNodes(outNodes);
            subGraph.AddEdges(outEdges);
        }

        return subGraph;
    }
}

