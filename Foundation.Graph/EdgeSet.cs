namespace Foundation.Graph;

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

public class EdgeSet<TNode, TEdge> : IEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly Lazy<ICollection<TEdge>> _edges;

    public EdgeSet()
    {
        _edges = new Lazy<ICollection<TEdge>>(() => new List<TEdge>());
    }
    public EdgeSet([DisallowNull] IEnumerable<TEdge> edges) : this(edges.ToList())
    {
    }

    public EdgeSet([DisallowNull] ICollection<TEdge> edges)
    {
        if (null == edges) throw new ArgumentNullException(nameof(edges));

        _edges = new Lazy<ICollection<TEdge>>(() => edges);
    }

    public void AddEdge([DisallowNull] TEdge edge)
    {
        ArgumentNullException.ThrowIfNull(edge, nameof(edge));

        if (!AllowDuplicateEdges && _edges.Value.Contains(edge))
            throw new EdgeSetException("edge exists");

        _edges.Value.Add(edge);
        
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edge));
    }

    public void AddEdges(IEnumerable<TEdge> edges)
    {
        ArgumentNullException.ThrowIfNull(edges, nameof(edges));

        foreach (var edge in edges)
        {
            if (!AllowDuplicateEdges && _edges.Value.Contains(edge))
                throw new EdgeSetException("edge exists");
            
            _edges.Value.Add(edge);
        }

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edges.ToArray()));
    }

    /// <summary>
    /// If set to false, an exception will be thrown on adding an existing edge.
    /// </summary>
    public bool AllowDuplicateEdges { get; set; }

    public void ClearEdges()
    {
        _edges.Value.Clear();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public int EdgeCount => _edges.Value.Count;

    public IEnumerable<TEdge> Edges => _edges.Value;

    public bool ExistsEdge(TEdge edge) => _edges.Value.Contains(edge);

    protected void FireCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(sender, args);
    }

    public IEnumerable<TEdge> GetEdges([DisallowNull] TNode node) => _edges.Value.Where(e => e.Source.Equals(node) || e.Target.Equals(node));

    public bool RemoveEdge(TEdge edge)
    {
        if (!_edges.Value.Remove(edge)) return false;
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, edge));
        return true;
    }

    public void RemoveEdges(IEnumerable<TEdge> edges)
    {
        var removedEdges = new List<TEdge>();
        foreach (var edge in edges)
        {
            if (_edges.Value.Remove(edge)) removedEdges.Add(edge);
        }

        if (0 == removedEdges.Count) return;
        
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedEdges.ToArray()));
    }
}

public class EdgeSet<TNode, TEdgeId, TEdge> : IEdgeSet<TNode, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
    where TEdgeId : notnull
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly Lazy<IDictionary<TEdgeId, TEdge>> _edges = new (() => new Dictionary<TEdgeId, TEdge>());

    /// <summary>
    /// Returns always false, because edges with same Id are not allowed.
    /// </summary>
    public bool AllowDuplicateEdges => false;

    public int EdgeCount => _edges.Value.Count;

    public IEnumerable<TEdge> Edges => _edges.Value.Values;


    public void AddEdge(TEdge edge)
    {
        ArgumentNullException.ThrowIfNull(edge, nameof(edge));

        _edges.Value.Add(edge.Id, edge);
        FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edge));
    }

    public void AddEdges(IEnumerable<TEdge> edges)
    {
        ArgumentNullException.ThrowIfNull(edges, nameof(edges));

        foreach (var edge in edges)
        {
            if (!AllowDuplicateEdges && _edges.Value.ContainsKey(edge.Id))
                throw new EdgeSetException("edge exists");

            _edges.Value.Add(edge.Id, edge);
        }

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edges.ToArray()));
    }

    public void ClearEdges()
    {
        if (0 == _edges.Value.Count) return;

        _edges.Value.Clear();
        FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool ExistsEdge(TEdgeId edgeId) => _edges.Value.ContainsKey(edgeId);

    public bool ExistsEdge(TEdge edge) => _edges.Value.Values.Any(e => e.Equals(edge));

    protected void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(this, args);
    }

    public Opt<TEdge> GetEdge(TEdgeId edgeId)
    {
        if (_edges.Value.TryGetValue(edgeId, out TEdge? edge)) return Opt.Some(edge);

        return Opt.None<TEdge>();
    }

    public IEnumerable<TEdge> GetEdges([DisallowNull] TNode node) => _edges.Value.Values.Where(e => e.Source.Equals(node) || e.Target.Equals(node));


    public bool RemoveEdge([DisallowNull] TEdge edge)
    {
        var removed = _edges.Value.Remove(edge.Id);
        if (removed) FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, edge));

        return removed;
    }

    public bool RemoveEdge([DisallowNull] TEdgeId edgeId)
    {
        if(null != CollectionChanged)
        {
            var optionalEdge = GetEdge(edgeId);
            if (optionalEdge.IsNone) return false;

            return RemoveEdge(optionalEdge.ValueOrThrow());
        }

        return _edges.Value.Remove(edgeId);
    }

    public void RemoveEdges(IEnumerable<TEdgeId> edgeIds)
    {
        foreach (var edgeId in edgeIds)
            RemoveEdge(edgeId);
    }

    public void RemoveEdges(IEnumerable<TEdge> edges)
    {
        var removedEdges = new List<TEdge>();
        foreach (var edge in edges)
        {
            if (_edges.Value.Remove(edge.Id)) removedEdges.Add(edge);
        }

        if (0 == removedEdges.Count) return;

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedEdges.ToArray()));
    }
}
