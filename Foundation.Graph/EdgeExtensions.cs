namespace Foundation.Graph;

public static class EdgeExtensions
{
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

