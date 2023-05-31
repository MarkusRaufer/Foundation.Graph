namespace Foundation.Graph;

public interface IReadOnlyGraph<TNode, TEdge>
    : IReadOnlyNodeSet<TNode>
    , IReadOnlyEdgeSet<TNode, TEdge>
     where TEdge : IEdge<TNode>
{
}

public interface IReadOnlyGraph<TNodeId, TNode, TEdge> 
    : IReadOnlyNodeSet<TNodeId, TNode>
    , IReadOnlyEdgeSet<TNodeId, TEdge>
     where TEdge : IEdge<TNodeId> 
    where TNodeId : notnull
{
}

public interface IReadOnlyGraph<TNodeId, TNode, TEdgeId, TEdge>
    : IReadOnlyNodeSet<TNodeId, TNode>
    , IReadOnlyEdgeSet<TNodeId, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNodeId>
    where TEdgeId : notnull
    where TNodeId : notnull
{
}
