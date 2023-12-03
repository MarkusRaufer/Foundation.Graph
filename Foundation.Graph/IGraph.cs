namespace Foundation.Graph;

public interface IGraph<TNode, TEdge>
    : IReadOnlyGraph<TNode, TEdge>
    , INodeSet<TNode>
    , IEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
}

public interface IGraph<TNodeId, TNode, TEdge>
    : IReadOnlyGraph<TNodeId, TNode, TEdge>
    , INodeSet<TNodeId, TNode>
    , IEdgeSet<TNodeId, TEdge>
    where TEdge : IEdge<TNodeId>
{
}

public interface IGraph<TNodeId, TNode, TEdgeId, TEdge>
    : IReadOnlyGraph<TNodeId, TNode, TEdgeId, TEdge>
    , INodeSet<TNodeId, TNode>
    , IEdgeSet<TNodeId, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNodeId>
{
}
