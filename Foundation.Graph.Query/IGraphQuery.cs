using System.Linq.Expressions;

namespace Foundation.Graph.Query;

public interface IGraphQuery<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    IVertices<TNodeId, TNode, TEdge, TGraph> V(Func<TNode, bool> predicate);
    IVertices<TNodeId, TNode, TEdge, TGraph> V(Func<TNodeId, bool> predicate);
}
