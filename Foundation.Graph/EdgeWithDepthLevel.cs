namespace Foundation.Graph;

public record EdgeWithDepthLevel<TEdge, TNode>(TEdge Edge, int Depth) where TEdge : IEdge<TNode>;