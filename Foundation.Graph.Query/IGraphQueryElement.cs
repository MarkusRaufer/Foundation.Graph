namespace Foundation.Graph.Query;

public interface IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    /// <summary>
    /// Returns what is found at the end of the graph search path.
    /// </summary>
    /// <returns></returns>
    IGraphQueryExecute<TNodeId, TNode> Find();

    /// <summary>
    /// Returns what is found at the end of the graph search path.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="selector"></param>
    /// <returns></returns>
    IGraphQueryExecute<TNodeId, TNode, TResult> Find<TResult>(Func<KeyValuePair<TNodeId, TNode>, TResult> selector);

    /// <summary>
    /// Returns all elements which are found on the graph search path.
    /// </summary>
    /// <returns></returns>
    IGraphQueryExecute<TNodeId, TNode> FindPath();

    /// <summary>
    /// Returns all incoming nodes that match the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Filter of the incoming nodes.</param>
    /// <returns></returns>
    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> InV(Func<TNode, bool> predicate);

    /// <summary>
    /// Returns all incoming nodes that match the <paramref name="predicate"/>
    /// </summary>
    /// <param name="predicate">Node id filter of the incoming nodes.</param>
    /// <returns></returns>
    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> InV(Func<TNodeId, bool> predicate);

    /// <summary>
    /// The predicate of nodes.
    /// </summary>
    Func<TNode, bool>? NodePredicate { get; }

    /// <summary>
    /// The predicate of node ids.
    /// </summary>
    Func<TNodeId, bool>? NodeIdPredicate { get; }

    IEnumerable<KeyValuePair<TNodeId, TNode>>? Nodes { get; }

    /// <summary>
    /// Returns all outgoing nodes that match the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Filter of the outgoing nodes.</param>
    /// <returns></returns>
    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> OutV(Func<TNode, bool> predicate);

    /// <summary>
    /// Returns all outgoing nodes that match the <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">Node id filter of the outgoing nodes.</param>
    /// <returns></returns>
    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> OutV(Func<TNodeId, bool> predicate);

    /// <summary>
    /// Returns all nodes on the path.
    /// </summary>
    IEnumerable<KeyValuePair<TNodeId, TNode>>? Path { get; }

    /// <summary>
    /// Searches until satisfied.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> Repeat(
        Func<IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>, IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>> query,
        Func<TNode, bool> predicate);

    /// <summary>
    /// Searches until satisfied.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> Repeat(
        Func<IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>, IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>> query,
        Func<TNodeId, bool> predicate);
}
