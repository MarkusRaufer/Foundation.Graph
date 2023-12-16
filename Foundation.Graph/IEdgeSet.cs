namespace Foundation.Graph;

public interface IEdgeSet<in TNode, TEdge>
    : IReadOnlyEdgeSet<TNode, TEdge>
    , IMutableEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
}

public interface IEdgeSet<in TNode, TEdgeId, TEdge>
    : IReadOnlyEdgeSet<TNode, TEdgeId, TEdge>
    , IMutableEdgeSet<TNode, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
{
}
