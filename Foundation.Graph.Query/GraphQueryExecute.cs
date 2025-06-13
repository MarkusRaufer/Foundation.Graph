namespace Foundation.Graph.Query;

public class GraphQueryExecute<TNodeId, TNode> : IGraphQueryExecute<TNodeId, TNode>
    where TNodeId : notnull
    where TNode : notnull
{
    private readonly Func<IEnumerable<KeyValuePair<TNodeId, TNode>>> _query;

    public GraphQueryExecute(Func<IEnumerable<KeyValuePair<TNodeId, TNode>>> query)
    {
        _query = query.ThrowIfNull();
    }
    public IEnumerable<KeyValuePair<TNodeId, TNode>> Execute() => _query();
}

public class GraphQueryExecute<TNodeId, TNode, TResult> : IGraphQueryExecute<TNodeId, TNode, TResult>
    where TNodeId : notnull
    where TNode : notnull
{
    private readonly Func<IEnumerable<TResult>> _query;

    public GraphQueryExecute(Func<IEnumerable<TResult>> query)
    {
        _query = query.ThrowIfNull();
    }
    public IEnumerable<TResult> Execute() => _query();
}