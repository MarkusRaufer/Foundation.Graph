namespace Foundation.Graph;

public static class EdgeExtensions
{
    public static bool EqualsUndirected<TNode, TEdge>(this TEdge lhs, TEdge rhs)
        where TEdge : IEdge<TNode>
    {
        return lhs.Source.EqualsNullable(rhs.Source) && lhs.Target.EqualsNullable(rhs.Target)
            || lhs.Source.EqualsNullable(rhs.Target) && lhs.Target.EqualsNullable(rhs.Source);
    }

    public static IEnumerable<TNode> GetNodes<TNode>(this IEdge<TNode> edge)
    {
        yield return edge.Source;
        yield return edge.Target;
    }

    public static IEnumerable<TNode> GetNodes<TNode, TEdge>(this TEdge edge)
        where TEdge : IEdge<TNode>
    {
        yield return edge.Source;
        yield return edge.Target;
    }

    public static IEnumerable<TNode> GetNodesNotInEdge<TNode>(this IEdge<TNode> edge, TNode notNode)
    {
        if (!edge.Source.EqualsNullable(notNode)) yield return edge.Source;
        if (!edge.Target.EqualsNullable(notNode)) yield return edge.Target;
    }


    public static IEnumerable<TNode> GetNodesTargetSource<TNode>(this IEdge<TNode> edge)
    {
        yield return edge.Target;
        yield return edge.Source;
    }

    public static IEnumerable<TNode> GetNodesTargetSource<TNode, TEdge>(this TEdge edge)
        where TEdge : IEdge<TNode>
    {
        yield return edge.Target;
        yield return edge.Source;
    }

    /// <summary>
    /// returns the opposite node of the edge. If node is Source, Target is returned otherwise Source.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="edge"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static TNode GetOtherNode<TNode, TEdge>(this TEdge edge, TNode node)
        where TEdge : IEdge<TNode>
    {
        if (edge.Source.EqualsNullable(node)) return edge.Target;
        if (edge.Target.EqualsNullable(node)) return edge.Source;

        throw new ArgumentOutOfRangeException($"node {node} not part of edge", nameof(node));
    }

    public static IEnumerable<TNode> GetUniqueNodes<TNode>(this IEdge<TNode> edge)
    {
        yield return edge.Source;

        if (!edge.Source.EqualsNullable(edge.Target))
            yield return edge.Target;
    }

    public static IEnumerable<TNode> GetUniqueNodes<TNode, TEdge>(this TEdge edge)
        where TEdge : IEdge<TNode>
    {
        yield return edge.Source;

        if (!edge.Source.EqualsNullable(edge.Target))
            yield return edge.Target;
    }

    public static bool HasNode<TNode, TEdge>(this TEdge edge, TNode node)
        where TEdge : IEdge<TNode>
    {
        if (edge.Source.EqualsNullable(node)) return true;
        if (edge.Target.EqualsNullable(node)) return true;
        return false;
    }

    public static bool IsIncoming<TNode, TEdge>(this TEdge edge, TNode node)
        where TEdge : IEdge<TNode>
    {
        return edge.Target.EqualsNullable(node);
    }

    public static bool IsOutgoing<TNode, TEdge>(this TEdge edge, TNode node)
        where TEdge : IEdge<TNode>
    {
        return edge.Source.EqualsNullable(node);
    }
}

