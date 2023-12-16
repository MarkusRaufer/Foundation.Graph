namespace Foundation.Graph;

public interface IGraph<TNode, TEdge>
    : IMutableGraph<TNode, TEdge>
    , IReadOnlyGraph<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
}

public interface IGraph<TNodeId, TNode, TEdge>
    : IMutableGraph<TNodeId, TNode, TEdge>
    , IReadOnlyGraph<TNodeId, TNode, TEdge>
    where TEdge : IEdge<TNodeId>
{
}

public interface IGraph<TNodeId, TNode, TEdgeId, TEdge>
    : IMutableGraph<TNodeId, TNode, TEdgeId, TEdge>
    , IReadOnlyGraph<TNodeId, TNode, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNodeId>
{
}
