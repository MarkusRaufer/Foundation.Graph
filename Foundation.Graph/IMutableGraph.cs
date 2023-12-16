namespace Foundation.Graph;

public interface IMutableGraph<TNode, TEdge>
    : IMutableNodeSet<TNode>
    , IMutableEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
}

public interface IMutableGraph<TNodeId, TNode, TEdge>
    : IMutableNodeSet<TNodeId, TNode>
    , IMutableEdgeSet<TNodeId, TEdge>
    where TEdge : IEdge<TNodeId>
{
}

public interface IMutableGraph<TNodeId, TNode, TEdgeId, TEdge>
    : IMutableNodeSet<TNodeId, TNode>
    , IMutableEdgeSet<TNodeId, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNodeId>
{
}
