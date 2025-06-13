
namespace Foundation.Graph.Query;

public interface IGraphQueryClient<TNodeId, TNode, TEdge, TGraph>
    : IGraphQueryClient<TNodeId, TNode, TEdge, TGraph, IGraphQuery<TNodeId, TNode, TEdge, TGraph>>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
}

public interface IGraphQueryClient<TNodeId, TNode, TEdge, TGraph, TQuery>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
    where TQuery : IGraphQuery<TNodeId, TNode, TEdge, TGraph>
{
    TQuery NewQuery();
}
