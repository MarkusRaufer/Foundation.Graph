using System.Linq.Expressions;

namespace Foundation.Graph.Query;

public static class GraphQuery
{
    public static GraphQuery<TNodeId, TNode, TEdge, TGraph> New<TNodeId, TNode, TEdge, TGraph>(TGraph graph)
        where TEdge : IEdge<TNodeId>
        where TNodeId : notnull
        where TGraph : IGraph<TNodeId, TNode, TEdge>
        => new(graph);
}

public class GraphQuery<TNodeId, TNode, TEdge, TGraph> : IGraphQuery<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TNodeId : notnull
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    private readonly TGraph _graph;

    public GraphQuery(TGraph graph)
    {
        _graph = graph.ThrowIfNull();
    }

    public IVertices<TNodeId, TNode, TEdge, TGraph> V(Func<TNode, bool> predicate)
    {
        return new Vertices<TNodeId, TNode, TEdge, TGraph>(_graph, predicate, _graph.NodeTuples, null);
    }

    public IVertices<TNodeId, TNode, TEdge, TGraph> V(Func<TNodeId, bool> predicate)
    {
        return new Vertices<TNodeId, TNode, TEdge, TGraph>(_graph, predicate, _graph.NodeTuples, null);
    }
}
