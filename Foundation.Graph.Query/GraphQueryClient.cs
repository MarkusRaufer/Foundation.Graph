namespace Foundation.Graph.Query;

public static class GraphQueryClient
{
    public static GraphQueryClient<TNodeId, TNode, TEdge, TGraph> New<TNodeId, TNode, TEdge, TGraph>(TGraph graph)
        where TEdge : IEdge<TNodeId>
        where TGraph : IGraph<TNodeId, TNode, TEdge>
        where TNode : notnull
        where TNodeId : notnull
        => new(graph);
}

public partial class GraphQueryClient<TNodeId, TNode, TEdge, TGraph>
    : GraphQueryClient<TNodeId, TNode, TEdge, TGraph, IGraphQuery<TNodeId, TNode, TEdge, TGraph>>
    , IGraphQueryClient<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
    where TNode : notnull
    where TNodeId : notnull
{
    public GraphQueryClient(TGraph graph) : base(graph)
    {
    }

    /// <inheritdoc/>
    public override IGraphQuery<TNodeId, TNode, TEdge, TGraph> NewQuery() => new GraphQuery<TNodeId, TNode, TEdge, TGraph>(Graph);
}

public abstract class GraphQueryClient<TNodeId, TNode, TEdge, TGraph, TQuery>
    : IGraphQueryClient<TNodeId, TNode, TEdge, TGraph, TQuery>
    where TEdge : IEdge<TNodeId>
    where TGraph : IGraph<TNodeId, TNode, TEdge>
    where TNode : notnull
    where TNodeId : notnull
    where TQuery : IGraphQuery<TNodeId, TNode, TEdge, TGraph>
{
    public GraphQueryClient(TGraph graph)
    {
        Graph = graph.ThrowIfNull();
    }

    protected TGraph Graph { get; }

    /// <inheritdoc/>
    public abstract TQuery NewQuery();
}
