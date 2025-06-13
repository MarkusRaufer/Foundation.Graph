using Foundation;
using Foundation.Graph;

namespace Foundation.Graph.Query;

public static class GraphQueryClient
{
    public static GraphQueryClient<TNodeId, TNode, TEdge, TGraph> New<TNodeId, TNode, TEdge, TGraph>(TGraph graph)
        where TEdge : IEdge<TNodeId>
        where TNodeId : notnull
        where TGraph : IGraph<TNodeId, TNode, TEdge>
        => new(graph);
}

public partial class GraphQueryClient<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TNodeId : notnull
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    private readonly TGraph _graph;

    public GraphQueryClient(TGraph graph)
    {
        _graph = graph.ThrowIfNull();
    }

    public IGraphQuery<TNodeId, TNode, TEdge, TGraph> NewQuery() => new GraphQuery<TNodeId, TNode, TEdge, TGraph>(_graph);
}
