namespace Foundation.Graph.Query;

public interface IVertices<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    IEnumerable<KeyValuePair<TNodeId, TNode>> Find();

    IEnumerable<TResult> Find<TResult>(Func<KeyValuePair<TNodeId, TNode>, TResult> selector);

    IEnumerable<KeyValuePair<TNodeId, TNode>> FindPath();

    Func<TNode, bool>? NodePredicate { get; }

    Func<TNodeId, bool>? NodeIdPredicate { get; }

    IEnumerable<KeyValuePair<TNodeId, TNode>>? Nodes { get; }

    IVertices<TNodeId, TNode, TEdge, TGraph> Out(Func<TNode, bool> predicate);

    IVertices<TNodeId, TNode, TEdge, TGraph> Out(Func<TNodeId, bool> predicate);

    IEnumerable<KeyValuePair<TNodeId, TNode>>? Path { get; }

    IVertices<TNodeId, TNode, TEdge, TGraph> Repeat(
        Func<IVertices<TNodeId, TNode, TEdge, TGraph>, IVertices<TNodeId, TNode, TEdge, TGraph>> query,
        Func<TNode, bool> predicate);
}
