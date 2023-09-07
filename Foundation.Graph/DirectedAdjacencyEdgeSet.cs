namespace Foundation.Graph;

using Foundation.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

public class DirectedAdjacencyEdgeSet<TNode, TEdge> : IDirectedEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly MultiValueMap<TNode, TEdge> _edges;

    public DirectedAdjacencyEdgeSet()
    {
        _edges = new MultiValueMap<TNode, TEdge>();
    }

    public DirectedAdjacencyEdgeSet([DisallowNull] IEnumerable<TEdge> edges)
    {
        _edges = new MultiValueMap<TNode, TEdge>();
        foreach (var edge in edges)
            AddEdge(edge);
    }

    public bool AllowDuplicateEdges { get; set; }

    public int EdgeCount => _edges.Values.Distinct().Count();

    public IEnumerable<TEdge> Edges => _edges.Values.Distinct();

    public void AddEdge(TEdge edge)
    {
        if (!AllowDuplicateEdges && _edges.Contains(edge.Source, edge))
            throw new EdgeSetException($"duplicate edges not allowed. Edge: {edge}");

        _edges.Add(edge.Source, edge);
        _edges.Add(edge.Target, edge);
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edge));
    }

    public void AddEdges(IEnumerable<TEdge> edges)
    {
        foreach (var edge in edges)
        {
            _edges.Add(edge.Source, edge);
            _edges.Add(edge.Target, edge);
        }

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edges.ToArray()));
    }

    public void ClearEdges()
    {
        if (0 == _edges.Count) return;

        _edges.Clear();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool ExistsEdge(TEdge edge) => _edges.ContainsValue(edge);

    public bool ExistsEdge(TNode source, TNode target)
    {
        if (!_edges.TryGetValues(source, out ICollection<TEdge>? edges)) return false;

        return edges!.Any(x => x.Target.Equals(target));
    }

    public IEnumerable<TEdge> GetEdges(TNode node)
    {
        var inEdges = IncomingEdges(node);
        return inEdges.Concat(OutgoingEdges(node)).Distinct();
    }

    public IEnumerable<TEdge> IncomingEdges(TNode node)
    {
        return _edges.GetValues(node).Where(e => e.Target.Equals(node));
    }

    public IEnumerable<TNode> IncomingNodes(TNode node, Func<TEdge, bool>? predicate = null)
    {
        return (null == predicate)
            ? IncomingEdges(node).Select(e => e.Source)
            : IncomingEdges(node).Where(predicate).Select(e => e.Source);
    }

    public IEnumerable<TNode> OutgoingNodes(TNode node, Func<TEdge, bool>? predicate = null)
    {
        return (null == predicate)
            ? OutgoingEdges(node).Select(e => e.Target)
            : OutgoingEdges(node).Where(predicate).Select(e => e.Target);
    }

    public IEnumerable<TEdge> OutgoingEdges(TNode node)
    {
        return _edges.GetValues(node).Where(e => e.Source.Equals(node));
    }

    public bool RemoveEdge(TEdge edge)
    {
        var removed = Fused.Value(false).BlowIfChanged();
        removed.Value = _edges.Remove(edge.Source, edge);
        removed.Value = _edges.Remove(edge.Target, edge);

        if(removed.Value)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, edge));

        return removed.Value;
    }

    public void RemoveEdges(IEnumerable<TEdge> edges)
    {
        var removed = Fused.Value(false).BlowIfChanged();

        foreach (var edge in edges)
        {
            removed.Value = _edges.Remove(edge.Source, edge);
            removed.Value = _edges.Remove(edge.Target, edge);
        }

        if (removed.Value)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, edges.ToArray()));
    }
}

public class DirectedAdjacencyEdgeSet<TNode, TEdgeId, TEdge> : DirectedAdjacencyEdgeSet<TNode, TEdgeId, TEdge, EdgeSet<TNode, TEdgeId, TEdge>>
    where TEdge : IEdge<TEdgeId, TNode>
    where TEdgeId : notnull
    where TNode : notnull
{
    public DirectedAdjacencyEdgeSet() : this(new EdgeSet<TNode, TEdgeId, TEdge>())
    {
    }

    public DirectedAdjacencyEdgeSet(EdgeSet<TNode, TEdgeId, TEdge> edgeSet) : base(edgeSet)
    {
    }

    public DirectedAdjacencyEdgeSet([DisallowNull] IEnumerable<TEdge> edges) : this()
    {
        foreach (var edge in edges)
            AddEdge(edge);
    }

    ~DirectedAdjacencyEdgeSet()
    {
        Dispose(false);
    }
}

public class DirectedAdjacencyEdgeSet<TNode, TEdgeId, TEdge, TEdgeSet> 
    : IDirectedEdgeSet<TNode, TEdgeId, TEdge>
    , IDisposable
    where TEdge : IEdge<TEdgeId, TNode>
    where TEdgeId : notnull
    where TEdgeSet : IEdgeSet<TNode, TEdgeId, TEdge>
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly MultiValueMap<TNode, TEdge> _adjacent;
    private bool _disposed;

    public DirectedAdjacencyEdgeSet(TEdgeSet edgeSet)
    {
        EdgeSet = edgeSet.ThrowIfNull(nameof(edgeSet));
        _adjacent = new ();

        EdgeSet.CollectionChanged += EdgeSet_CollectionChanged;
    }

    ~DirectedAdjacencyEdgeSet()
    {
        Dispose(false);
    }

    public void AddEdge(TEdge edge)
    {
        if (EdgeSet.ExistsEdge(edge.Id))
            throw new EdgeSetException($"duplicate edges not allowed. Edge: {edge}");

        _adjacent.Add(edge.Source, edge);
        _adjacent.Add(edge.Target, edge);

        EdgeSet.AddEdge(edge);
    }

    public void AddEdges(IEnumerable<TEdge> edges)
    {
        foreach (var edge in edges)
        {
            _adjacent.Add(edge.Source, edge);
            _adjacent.Add(edge.Target, edge);
        }

        EdgeSet.AddEdges(edges);
    }

    public bool AllowDuplicateEdges { get; set; }

    public void ClearEdges()
    {
        if (0 == EdgeSet.EdgeCount) return;

        EdgeSet.ClearEdges();
        _adjacent.Clear();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            EdgeSet.CollectionChanged -= EdgeSet_CollectionChanged;
            if (EdgeSet is IDisposable disposable) disposable.Dispose();
            else EdgeSet.ClearEdges();

            _adjacent.Clear();

            _disposed = true;
        }
    }

    public int EdgeCount => EdgeSet.EdgeCount;

    public IEnumerable<TEdge> Edges => EdgeSet.Edges;

    protected TEdgeSet EdgeSet { get; }

    private void EdgeSet_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }
    
    public bool ExistsEdge(TEdge edge) => EdgeSet.ExistsEdge(edge.Id);

    public IEnumerable<TEdge> GetEdges(TNode node)
    {
        var inEdges = IncomingEdges(node);
        return inEdges.Concat(OutgoingEdges(node)).Distinct();
    }

    public IEnumerable<TEdge> IncomingEdges(TNode node)
    {
        return _adjacent.GetValues(node).Where(e => e.Target.Equals(node));
    }

    public IEnumerable<TNode> IncomingNodes(TNode node, Func<TEdge, bool>? predicate = null)
    {
        return (null == predicate)
            ? IncomingEdges(node).Select(e => e.Source)
            : IncomingEdges(node).Where(predicate).Select(e => e.Source);
    }

    public IEnumerable<TNode> OutgoingNodes(TNode node, Func<TEdge, bool>? predicate = null)
    {
        return (null == predicate)
            ? OutgoingEdges(node).Select(e => e.Target)
            : OutgoingEdges(node).Where(predicate).Select(e => e.Target);
    }

    public IEnumerable<TEdge> OutgoingEdges(TNode node)
    {
        return _adjacent.GetValues(node).Where(e => e.Source.Equals(node));
    }

    public bool RemoveEdge(TEdge edge)
    {
        _adjacent.Remove(edge.Source, edge);
        _adjacent.Remove(edge.Target, edge);

        return EdgeSet.RemoveEdge(edge);
    }

    public bool RemoveEdge([DisallowNull] TEdgeId edgeId)
    {
        if (null == CollectionChanged) return EdgeSet.RemoveEdge(edgeId);

        //event CollectionChanged needs an edge and not an edge id.
        var optionalEdge = EdgeSet.GetEdge(edgeId);
        if (optionalEdge.IsNone) return false;

        var edge = optionalEdge.OrThrow();
        _adjacent.Remove(edge.Source, edge);
        _adjacent.Remove(edge.Target, edge);

        return EdgeSet.RemoveEdge(edge);
    }

    public void RemoveEdges(IEnumerable<TEdge> edges)
    {
        foreach (var edge in edges)
        {
            _adjacent.Remove(edge.Source, edge);
            _adjacent.Remove(edge.Target, edge);
        }

        EdgeSet.RemoveEdges(edges);
    }

    public void RemoveEdges(IEnumerable<TEdgeId> edgeIds)
    {
        var edges = edgeIds.Select(id => EdgeSet.GetEdge(id))
                           .SelectSome();

        EdgeSet.RemoveEdges(edges.ToArray());
    }

    public bool ExistsEdge(TEdgeId edgeId) => EdgeSet.ExistsEdge(edgeId);


    public bool ExistsEdge(TNode source, TNode target) => EdgeSet.ExistsEdge(source, target);

    public Option<TEdge> GetEdge(TEdgeId edgeId) => EdgeSet.GetEdge(edgeId);
}
