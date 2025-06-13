namespace Foundation.Graph.Query;

public interface IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    IGraphQueryExecute<TNodeId, TNode> Find();

    IGraphQueryExecute<TNodeId, TNode, TResult> Find<TResult>(Func<KeyValuePair<TNodeId, TNode>, TResult> selector);

    IGraphQueryExecute<TNodeId, TNode> FindPath();

    Func<TNode, bool>? NodePredicate { get; }

    Func<TNodeId, bool>? NodeIdPredicate { get; }

    IEnumerable<KeyValuePair<TNodeId, TNode>>? Nodes { get; }

    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> Out(Func<TNode, bool> predicate);

    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> Out(Func<TNodeId, bool> predicate);

    IEnumerable<KeyValuePair<TNodeId, TNode>>? Path { get; }

    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> Repeat(
        Func<IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>, IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>> query,
        Func<TNode, bool> predicate);

    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> Repeat(
        Func<IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>, IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>> query,
        Func<TNodeId, bool> predicate);
}
