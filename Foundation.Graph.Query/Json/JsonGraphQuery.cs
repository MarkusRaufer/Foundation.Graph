using Foundation.Collections.Generic;
using Foundation.Text.Json.Serialization;
using System.Text.Json;

namespace Foundation.Graph.Query.Json;

public static class JsonGraphQuery
{
    public static JsonGraphQuery<TNodeId, TNode, TEdge, TGraph> New<TNodeId, TNode, TEdge, TGraph>()
        where TEdge : IEdge<TNodeId>
        where TNodeId : notnull
        where TGraph : IGraph<TNodeId, TNode, TEdge>
        => new();
}

public class JsonGraphQuery<TNodeId, TNode, TEdge, TGraph> : JsonQueryVisitor
    where TEdge : IEdge<TNodeId>
    where TNodeId : notnull
    where TGraph : IGraph<TNodeId, TNode, TEdge>
{
    private IGraphQuery<TNodeId, TNode, TEdge, TGraph>? _graphQuery;
    private IGraphQueryElement<TNodeId, TNode, TEdge, TGraph>? _queryElements;
    private Func<TNode, IDictionary<string, object?>, bool>? _nodePredicate;
    private Func<TNodeId, IDictionary<string, object?>, bool>? _nodeIdPredicate;

    public IEnumerable<KeyValuePair<TNodeId, TNode>> Execute(
        IGraphQuery<TNodeId, TNode, TEdge, TGraph> query,
        string json,
        Func<TNode, IDictionary<string, object?>, bool> nodePredicate)
    {
        _graphQuery = query.ThrowIfNull();
        _nodePredicate = nodePredicate.ThrowIfNull();

        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new ObjectJsonConverter()
            }
        };
        var jsonQuery = JsonSerializer.Deserialize<JsonQuery>(json, options)
                      ?? throw new ArgumentException($"invalid JSON format", nameof(json));

        Visit(jsonQuery);

        return jsonQuery.Method switch
        {
            QueryMethod.Find => If.Value(_queryElements)
                                  .NotNull(x => x.Find().Execute())
                                  .Else(() => []),
            QueryMethod.FindPath => If.Value(_queryElements)
                                      .NotNull(x => x.FindPath().Execute())
                                      .Else(() => []),
            _ => []
        };
    }

    public IEnumerable<KeyValuePair<TNodeId, TNode>> Execute(
        IGraphQuery<TNodeId, TNode, TEdge, TGraph> query,
        string json,
        Func<TNodeId, IDictionary<string, object?>, bool> nodeIdPredicate)
    {
        _graphQuery = query.ThrowIfNull();
        _nodeIdPredicate = nodeIdPredicate.ThrowIfNull();

        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new ObjectJsonConverter()
            }
        };
        var jsonQuery = JsonSerializer.Deserialize<JsonQuery>(json, options)
                      ?? throw new ArgumentException($"invalid JSON format", nameof(json));

        Visit(jsonQuery);

        return jsonQuery.Method switch
        {
            QueryMethod.Find => If.Value(_queryElements)
                                  .NotNull(x => x.Find().Execute())
                                  .Else(() => []),
            QueryMethod.FindPath => If.Value(_queryElements)
                                      .NotNull(x => x.FindPath().Execute())
                                      .Else(() => []),
            _ => []
        };
    }

    public Func<IEnumerable<KeyValuePair<TNodeId, TNode>>> CreateQuery(
        IGraphQuery<TNodeId, TNode, TEdge, TGraph> query,
        string json,
        Func<TNode, IDictionary<string, object?>, bool> nodePredicate)
    {
        return () => Execute(query, json, nodePredicate).EmptyIfNull();
    }

    public Func<IEnumerable<KeyValuePair<TNodeId, TNode>>> CreateQuery(
        IGraphQuery<TNodeId, TNode, TEdge, TGraph> query,
        string json,
        Func<TNodeId, IDictionary<string, object?>, bool> nodeIdPredicate)
    {
        return () => Execute(query, json, nodeIdPredicate).EmptyIfNull();
    }

    public override void VisitVwithoutOut(Dictionary<string, object?> v)
    {
        if (_graphQuery is not null)
        {
            if (_nodePredicate is not null)
            {
                _queryElements = _graphQuery.V(x => _nodePredicate(x, v));
            }
            else if (_nodeIdPredicate is not null)
            {
                _queryElements = _graphQuery.V(x => _nodeIdPredicate(x, v));
            }
        }
        

        base.VisitVwithoutOut(v);
    }

    public override void VisitOut(Dictionary<string, object?> @out)
    {
        if (_queryElements is not null)
        {
            if (_nodePredicate is not null)
            {
                _queryElements = _queryElements.Out(x => _nodePredicate(x, @out));
            }
            else if (_nodeIdPredicate is not null)
            {
                _queryElements = _queryElements.Out(x => _nodeIdPredicate(x, @out));
            }
        }
        
        base.VisitOut(@out);
    }

    public override void VisitOut(object? @out)
    {
        if (_queryElements is not null)
        {
            if (@out is bool all)
            {
                if (_nodePredicate is not null)
                {
                   _queryElements = _queryElements.Out((TNode x) => all);
                }
                else if (_nodeIdPredicate is not null)
                {
                    _queryElements = _queryElements.Out((TNodeId x) => all);
                }
            }
        }

        base.VisitOut(@out);
    }
}
