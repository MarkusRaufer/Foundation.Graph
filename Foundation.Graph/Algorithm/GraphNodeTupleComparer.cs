using System.Diagnostics.CodeAnalysis;

namespace Foundation.Graph.Algorithm;

public class GraphNodeTupleComparer<TNode, TEdge> : IEqualityComparer<(IGraph<TNode, TEdge>, TNode)>
    where TEdge : IEdge<TNode>
{
    public bool Equals((IGraph<TNode, TEdge>, TNode) lhs, (IGraph<TNode, TEdge>, TNode ) rhs)
    {
        var (lgraph, lnode) = lhs;
        var (rgraph, rnode) = rhs;

        if (!ReferenceEquals(lgraph, rgraph))
            return false;

        if (null == lnode) return null == rnode;

        return null != rnode && lnode.Equals(rnode);
    }

    public int GetHashCode([DisallowNull] (IGraph<TNode, TEdge>, TNode) tuple)
    {
        var (graph, node) = tuple;
        return System.HashCode.Combine(graph, node);
    }
}

