namespace Foundation.Graph;

public interface IDirectedGraph<TNode, TEdge>
    : IGraph<TNode, TEdge>
    , IDirectedEdgeSet<TNode, TEdge>
    , IReadOnlyDirectedGraph<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
}

public interface IDirectedGraph<TNodeId, TNode, TEdge>
    : IGraph<TNodeId, TNode, TEdge>
    , IDirectedEdgeSet<TNodeId, TEdge>
    , IReadOnlyDirectedGraph<TNodeId, TNode, TEdge>
    where TEdge : IEdge<TNodeId>
    where TNodeId : notnull
{
}

public interface IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge>
    : IGraph<TNodeId, TNode, TEdgeId, TEdge>
    , IDirectedEdgeSet<TNodeId, TEdgeId, TEdge>
    , IReadOnlyDirectedGraph<TNodeId, TNode, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNodeId>
    where TEdgeId : notnull
    where TNodeId : notnull
{
}
