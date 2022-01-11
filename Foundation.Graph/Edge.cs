﻿namespace Foundation.Graph;
using System.Diagnostics.CodeAnalysis;

public static class Edge
{
    public static Edge<TNode> New<TNode>([DisallowNull] TNode source, [DisallowNull] TNode target)
    {
        return new Edge<TNode>(source, target);
    }

    public static Edge<TEdgeId, TNode> New<TEdgeId, TNode>([DisallowNull] TEdgeId id, [DisallowNull] TNode source, [DisallowNull] TNode target)
        where TEdgeId : notnull
    {
        return new Edge<TEdgeId, TNode>(id, source, target);
    }
}

public record struct Edge<TNode>(TNode Source, TNode Target) : IEdge<TNode>;

public record struct Edge<TEdgeId, TNode>(TEdgeId Id, TNode Source, TNode Target) : IEdge<TEdgeId, TNode>
                     where TEdgeId : notnull;