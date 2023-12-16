namespace Foundation.Graph;
using System.Diagnostics.CodeAnalysis;

public static class ValueEdge
{
    public static ValueEdge<TNode> New<TNode>(TNode source, TNode target)
        where TNode : notnull
    {
        return new ValueEdge<TNode>(source, target);
    }

    public static ValueEdge<TEdgeId, TNode> New<TEdgeId, TNode>(TEdgeId id, TNode source, TNode target)
        where TEdgeId : notnull
        where TNode : notnull
    {
        return new ValueEdge<TEdgeId, TNode>(id, source, target);
    }
}

public record struct ValueEdge<TNode>(TNode Source, TNode Target) : IEdge<TNode>;

public record struct ValueEdge<TEdgeId, TNode>(TEdgeId Id, TNode Source, TNode Target) : IEdge<TEdgeId, TNode>
    where TEdgeId : notnull
    where TNode : notnull;