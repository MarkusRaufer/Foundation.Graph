﻿namespace Foundation.Graph;

public static class EdgeSetExtensions
{
    public static bool ExistsEdge<TNode, TEdge>(this IReadOnlyEdgeSet<TNode, TEdge> edgeSet, TNode node)
        where TEdge : IEdge<TNode>
    {
        return edgeSet.Edges.Any(e => e.Source.EqualsNullable(node) || e.Target.EqualsNullable(node));
    }

    public static IEnumerable<TNode> GetNodes<TNode, TEdge>(this IReadOnlyEdgeSet<TNode, TEdge> edgeSet)
        where TEdge : IEdge<TNode>
    {
        return edgeSet.Edges.SelectMany(x => x.GetNodes()).Distinct();
    }
}

