namespace Foundation.Graph;

public interface IOutsideEdge<TNode, TEdge> : IEdge<TNode>
    where TEdge : IEdge<TNode>
{
    IGraph<TNode, TEdge> SourceGraph { get; }
    IGraph<TNode, TEdge> TargetGraph { get; }
}
