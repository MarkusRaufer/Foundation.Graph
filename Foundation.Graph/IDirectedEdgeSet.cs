namespace Foundation.Graph;

public interface IDirectedEdgeSet<TNode, TEdge> 
    : IEdgeSet<TNode, TEdge>
    , IReadOnlyDirectedEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
}

public interface IDirectedEdgeSet<TNode, TEdgeId, TEdge> 
    : IDirectedEdgeSet<TNode, TEdge>
    , IEdgeSet<TNode, TEdgeId, TEdge>
    , IReadOnlyDirectedEdgeSet<TNode, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
    where TEdgeId : notnull
{
}
