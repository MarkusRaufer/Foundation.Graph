using System.Linq.Expressions;

namespace Foundation.Graph.Query;

public interface IGraphQuery<TNodeId, TNode, TEdge, TGraph>
    : IGraphQuery<TNodeId, TNode, TEdge, TGraph, IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
}

public interface IGraphQuery<TNodeId, TNode, TEdge, TGraph, TQueryElement>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
    where TQueryElement : IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>
{
    TQueryElement V(Func<TNode, bool> predicate);
    TQueryElement V(Func<TNodeId, bool> predicate);
}
