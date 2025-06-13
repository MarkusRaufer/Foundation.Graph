
using Foundation.Collections.Generic;

namespace Foundation.Graph.Query;

public class Vertices<TNodeId, TNode, TEdge, TGraph> : IVertices<TNodeId, TNode, TEdge, TGraph>
    where TEdge : IEdge<TNodeId>
    where TNodeId : notnull
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    private readonly TGraph _graph;
    private readonly Func<TNode, bool>? _nodePredicate;
    private readonly Func<TNodeId, bool>? _nodeIdPredicate;

    private readonly IEnumerable<KeyValuePair<TNodeId, TNode>> _nodes;
    private readonly Func<TNodeId, IEnumerable<TEdge>> _outEdges;
    private readonly IEnumerable<KeyValuePair<TNodeId, TNode>>? _path;

    public Vertices(
        TGraph graph,
        Func<TNode, bool> nodePredicate,
        IEnumerable<KeyValuePair<TNodeId, TNode>> nodes,
        IEnumerable<KeyValuePair<TNodeId, TNode>>? path
        )
    {
        _graph = graph.ThrowIfNull();
        _nodePredicate = nodePredicate.ThrowIfNull();

        _outEdges = _graph is IDirectedGraph<TNodeId, TEdge> directedGraph
            ? directedGraph.OutgoingEdges
            : OutGoingEdges;

        var (match, noMatch) = nodes.Partition(x => nodePredicate(x.Value));
        _nodes = match;

        _path = path is null ? _nodes : path.Ignore(noMatch);
    }

    private IEnumerable<TEdge> OutGoingEdges(TNodeId nodeId)
    {
        return _graph.Edges.Where(e => e.Source.Equals(nodeId));
    }

    public Vertices(
        TGraph graph,
        Func<TNodeId, bool> nodeIdPredicate,
        IEnumerable<KeyValuePair<TNodeId, TNode>> nodes,
        IEnumerable<KeyValuePair<TNodeId, TNode>>? path
        )
    {
        _graph = graph.ThrowIfNull();
        _nodeIdPredicate = nodeIdPredicate.ThrowIfNull();
        _nodes = nodes.ThrowIfNull().Where(x => nodeIdPredicate(x.Key));

        _path = path is null ? _nodes : path;
    }

    private IVertices<TNodeId, TNode, TEdge, TGraph>? CreateFromPredicate(
        IVertices<TNodeId, TNode, TEdge, TGraph>? query)
    {
        if (query is null || query.Nodes is null) return null;

        if (query.NodePredicate is not null) return CreateFromPredicate(query, query.NodePredicate);
        if (query.NodeIdPredicate is not null) return CreateFromPredicate(query, query.NodeIdPredicate);

        return null;
    }

    private IVertices<TNodeId, TNode, TEdge, TGraph>? CreateFromPredicate(
        IVertices<TNodeId, TNode, TEdge, TGraph>? query, Func<TNode, bool> predicate)
    {
        if (query is null || query.Nodes is null) return null;

            return new Vertices<TNodeId, TNode, TEdge, TGraph>(
                                    _graph,
                                    predicate,
                                    query.Nodes,
                                    query.Path);
    }

    private IVertices<TNodeId, TNode, TEdge, TGraph> CreateFromPredicate(
        IVertices<TNodeId, TNode, TEdge, TGraph>? query, Func<TNodeId, bool> predicate)
    {
        if (query is null || query.Nodes is null) return null;

            return new Vertices<TNodeId, TNode, TEdge, TGraph>(
                                    _graph,
                                    predicate,
                                    query.Nodes,
                                    query.Path);
    }

    public IEnumerable<KeyValuePair<TNodeId, TNode>> Find() => _nodes;

    public IEnumerable<TResult> Find<TResult>(Func<KeyValuePair<TNodeId, TNode>, TResult> selector)
        => _nodes.Select(selector);
    

    public IEnumerable<KeyValuePair<TNodeId, TNode>> FindPath() => _path.EmptyIfNull();

    public Func<TNode, bool>? NodePredicate => _nodePredicate;

    public Func<TNodeId, bool>? NodeIdPredicate => _nodeIdPredicate;

    public IEnumerable<KeyValuePair<TNodeId, TNode>>? Nodes => _nodes;

    public IVertices<TNodeId, TNode, TEdge, TGraph> Out(Func<TNode, bool> predicate)
    {
        var outEdges = _nodes.SelectMany(kv => _outEdges(kv.Key));
        var outNodes = _graph.GetNodeTuples(outEdges.Select(x => x.Target));

        var path = _path is null ? outNodes : _path.UnionBy(outNodes, x => x.Key);

        return new Vertices<TNodeId, TNode, TEdge, TGraph>(_graph, predicate, outNodes, path);
    }

    public IVertices<TNodeId, TNode, TEdge, TGraph> Out(Func<TNodeId, bool> predicate)
    {
        var outEdges = _nodes.SelectMany(kv => _outEdges(kv.Key));
        var outNodes = _graph.GetNodeTuples(outEdges.Select(x => x.Target));
        
        var path = _path is null ? outNodes : _path.UnionBy(outNodes, x => x.Key);

        return new Vertices<TNodeId, TNode, TEdge, TGraph>(_graph, predicate, outNodes, path);
    }

    public IEnumerable<KeyValuePair<TNodeId, TNode>>? Path => _path;

    public IVertices<TNodeId, TNode, TEdge, TGraph> Repeat(
        Func<IVertices<TNodeId, TNode, TEdge, TGraph>, IVertices<TNodeId, TNode, TEdge, TGraph>> query,
        Func<TNode, bool> predicate)
    {
        IVertices<TNodeId, TNode, TEdge, TGraph> querySource = this;
        while (true)
        {
            var queryResult = query(querySource);
            if (queryResult is null || queryResult.Nodes is null) break;

            if (queryResult.Nodes.Any(x => predicate(x.Value)))
            {
                var endResult = CreateFromPredicate(queryResult, predicate);
                if (endResult is null) return this;
                return endResult;
            }

            var result = CreateFromPredicate(queryResult);
            if (result is null || result.Nodes is null) break;

            querySource = result;
        }

        return this;
    }
}
