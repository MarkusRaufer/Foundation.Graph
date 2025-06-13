using Foundation.Graph.Query.Json;

namespace Foundation.Graph.Query;

public partial class GraphQueryClient<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TNodeId : notnull
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    public Func<IEnumerable<KeyValuePair<TNodeId, TNode>>> NewQuery(string json, Func<TNode, IDictionary<string, object?>, bool> nodePredicate)
    {
        return () => ExecuteQuery(json, nodePredicate);
    }

    public IEnumerable<KeyValuePair<TNodeId, TNode>> ExecuteQuery(string json, Func<TNode, IDictionary<string, object?>, bool> nodePredicate)
    {
        json.ThrowIfNullOrWhiteSpace();

        var query = NewQuery();

        var jsonQuery = JsonGraphQuery.New<TNodeId, TNode, TEdge, TGraph>();
        return jsonQuery.Execute(query, json, nodePredicate);
    }
}
