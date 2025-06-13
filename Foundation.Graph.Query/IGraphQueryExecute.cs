namespace Foundation.Graph.Query;

public interface IGraphQueryExecute<TNodeId, TNode>
{
    IEnumerable<KeyValuePair<TNodeId, TNode>> Execute();
}

public interface IGraphQueryExecute<TNodeId, TNode, TResult>
{
    IEnumerable<TResult> Execute();
}
