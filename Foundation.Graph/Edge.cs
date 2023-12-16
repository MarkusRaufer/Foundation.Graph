namespace Foundation.Graph;
using System.Diagnostics.CodeAnalysis;

public static class Edge
{
    public static Edge<TNode> New<TNode>([DisallowNull] TNode source, [DisallowNull] TNode target)
        where TNode : notnull
    {
        return new Edge<TNode>(source, target);
    }

    public static Edge<TEdgeId, TNode> New<TEdgeId, TNode>([DisallowNull] TEdgeId id, [DisallowNull] TNode source, [DisallowNull] TNode target)
        where TEdgeId : notnull
        where TNode : notnull
    {
        return new Edge<TEdgeId, TNode>(id, source, target);
    }
}

public record Edge<TNode>(TNode Source, TNode Target)
    : IEdge<TNode>
    where TNode : notnull;

public record Edge<TEdgeId, TNode>(TEdgeId Id, TNode Source, TNode Target) 
    : Edge<TNode>(Source, Target)
    , IEdge<TEdgeId, TNode>
    where TEdgeId : notnull
    where TNode : notnull;
