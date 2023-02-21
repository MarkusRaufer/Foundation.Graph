namespace Foundation.Graph;
using System.Diagnostics.CodeAnalysis;

public static class ValueEdge
{
    public static ValueEdge<TNode> New<TNode>([DisallowNull] TNode source, [DisallowNull] TNode target)
    {
        return new ValueEdge<TNode>(source, target);
    }

    public static ValueEdge<TEdgeId, TNode> New<TEdgeId, TNode>([DisallowNull] TEdgeId id, [DisallowNull] TNode source, [DisallowNull] TNode target)
        where TEdgeId : notnull
    {
        return new ValueEdge<TEdgeId, TNode>(id, source, target);
    }
}

public record struct ValueEdge<TNode>(TNode Source, TNode Target) : IEdge<TNode>;

public record struct ValueEdge<TEdgeId, TNode>(TEdgeId Id, TNode Source, TNode Target) : IEdge<TEdgeId, TNode>
                     where TEdgeId : notnull;