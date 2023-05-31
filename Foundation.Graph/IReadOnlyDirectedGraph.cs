namespace Foundation.Graph;

public interface IReadOnlyDirectedGraph<TNode, TEdge>
    : IReadOnlyGraph<TNode, TEdge>
    , IReadOnlyDirectedEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
}

public interface IReadOnlyDirectedGraph<TNodeId, TNode, TEdge>
    : IReadOnlyGraph<TNodeId, TNode, TEdge>
    , IReadOnlyDirectedEdgeSet<TNodeId, TEdge>
    where TEdge : IEdge<TNodeId>
    where TNodeId : notnull
{
}

public interface IReadOnlyDirectedGraph<TNodeId, TNode, TEdgeId, TEdge>
    : IReadOnlyGraph<TNodeId, TNode, TEdgeId, TEdge>
    , IReadOnlyDirectedEdgeSet<TNodeId, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNodeId>
    where TEdgeId : notnull
    where TNodeId : notnull
{
}
