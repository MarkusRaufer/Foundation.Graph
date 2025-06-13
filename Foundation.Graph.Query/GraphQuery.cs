namespace Foundation.Graph.Query;

public static class GraphQuery
{
    public static GraphQuery<TNodeId, TNode, TEdge, TGraph> New<TNodeId, TNode, TEdge, TGraph>(TGraph graph)
        where TEdge : IEdge<TNodeId>
        where TNode : notnull
        where TNodeId : notnull
        where TGraph : IGraph<TNodeId, TNode, TEdge>
        => new(graph);
}

public class GraphQuery<TNodeId, TNode, TEdge, TGraph> 
    : GraphQuery<TNodeId, TNode, TEdge, TGraph, IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>>
    , IGraphQuery<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TNode : notnull
    where TNodeId : notnull
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    public GraphQuery(TGraph graph) : base(graph)
    {
    }

    public override IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> V(Func<TNode, bool> predicate)
    {
        return new GraphQueryElement<TNodeId, TNode, TEdge, TGraph>(Graph, predicate, Graph.NodeTuples, null);
    }

    public override IGraphQueryElement<TNodeId, TNode, TEdge, TGraph> V(Func<TNodeId, bool> predicate)
    {
        return new GraphQueryElement<TNodeId, TNode, TEdge, TGraph>(Graph, predicate, Graph.NodeTuples, null);
    }
}

public abstract class GraphQuery<TNodeId, TNode, TEdge, TGraph, TQueryElement> 
    : IGraphQuery<TNodeId, TNode, TEdge, TGraph, TQueryElement>
    where TEdge : IEdge<TNodeId>
    where TNode : notnull
    where TNodeId : notnull
    where TGraph : IGraph<TNodeId, TNode, TEdge>
    where TQueryElement : IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>
{
    public GraphQuery(TGraph graph)
    {
        Graph = graph.ThrowIfNull();
    }

    protected TGraph Graph { get; }

    public abstract TQueryElement V(Func<TNode, bool> predicate);

    public abstract TQueryElement V(Func<TNodeId, bool> predicate);
}
