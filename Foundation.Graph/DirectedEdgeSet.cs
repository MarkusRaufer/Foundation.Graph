// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿namespace Foundation.Graph;

using Foundation;
using Foundation.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

public class DirectedEdgeSet<TNode, TEdge> : DirectedEdgeSet<TNode, TEdge, EdgeSet<TNode, TEdge>>
    where TEdge : IEdge<TNode>
    where TNode : notnull
{
    public DirectedEdgeSet()
        : base(new EdgeSet<TNode, TEdge>(new HashSet<TEdge>()))
    {
    }

    public DirectedEdgeSet(EdgeSet<TNode, TEdge> edgeSet)
        : base(edgeSet)
    {
    }

    public DirectedEdgeSet(ICollection<TEdge> edges)
        : base(new EdgeSet<TNode, TEdge>(edges))
    {
    }

    public DirectedEdgeSet(IEnumerable<TEdge> edges)
        : base(new EdgeSet<TNode, TEdge>())
    {
        AddEdges(edges);
    }
}

public class DirectedEdgeSet<TNode, TEdge, TEdgeSet>
    : IDirectedEdgeSet<TNode, TEdge>
    , INotifyCollectionChanged
    , IDisposable
    where TEdge : notnull, IEdge<TNode>
    where TEdgeSet : IEdgeSet<TNode, TEdge>, INotifyCollectionChanged
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private bool _disposed;

    private readonly Lazy<IDictionary<TNode, HashSet<TEdge>>> _incomingEdges =
        new(() => new Dictionary<TNode, HashSet<TEdge>>());

    private readonly Lazy<IDictionary<TNode, HashSet<TEdge>>> _outgoingEdges =
        new(() => new Dictionary<TNode, HashSet<TEdge>>());

    public DirectedEdgeSet([DisallowNull] TEdgeSet edgeSet)
    {
        EdgeSet = edgeSet.ThrowIfNull(nameof(edgeSet));
        EdgeSet.CollectionChanged += EdgeSet_CollectionChanged;
    }

    ~DirectedEdgeSet()
    {
        Dispose(false);
    }

    
    public void AddEdge(TEdge edge)
    {
        if (!AllowDuplicateEdges && EdgeSet.ExistsEdge(edge))
            throw new ArgumentOutOfRangeException(nameof(edge), "edge exists");


        AddIncomingEdge(edge);
        AddOutgoingEdge(edge);

        //is set at the end because AddEdge calls CollectionChanged event.
        EdgeSet.AddEdge(edge);
    }

    public void AddEdges([DisallowNull] IEnumerable<TEdge> edges)
    {
        foreach (var edge in edges)
        {
            AddIncomingEdge(edge);
            AddOutgoingEdge(edge);
        }

        //is set at the end because AddEdges calls CollectionChanged event.
        EdgeSet.AddEdges(edges);
    }

    protected void AddIncomingEdge([DisallowNull] TEdge edge)
    {
        if (!_incomingEdges.Value.TryGetValue(edge.Target, out HashSet<TEdge>? edges))
        {
            edges = new HashSet<TEdge>();
            _incomingEdges.Value.Add(edge.Target, edges);
        }

        if (edges.Contains(edge)) return;
        edges.Add(edge);
    }

    protected void AddOutgoingEdge([DisallowNull] TEdge edge)
    {
        if (!_outgoingEdges.Value.TryGetValue(edge.Source, out HashSet<TEdge>? edges))
        {
            edges = new HashSet<TEdge>();
            _outgoingEdges.Value.Add(edge.Source, edges);
        }

        if (edges.Contains(edge)) return;
        edges.Add(edge);
    }

    /// <summary>
    /// If set to false, an exception will be thrown on adding an existing edge.
    /// </summary>
    public bool AllowDuplicateEdges { get; set; }

    public void ClearEdges()
    {
        if (0 == EdgeSet.EdgeCount) return;

        EdgeSet.ClearEdges();
        _incomingEdges.Value.Clear();
        _outgoingEdges.Value.Clear();
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

            _disposed = true;
        }
    }

    public int EdgeCount => EdgeSet.EdgeCount;

    public IEnumerable<TEdge> Edges => EdgeSet.Edges;

    protected TEdgeSet EdgeSet { get; private set; }

    private void EdgeSet_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CollectionChanged?.Invoke(this, e);
    }

    public bool ExistsEdge([DisallowNull] TEdge edge) => EdgeSet.ExistsEdge(edge);

    protected void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(this, args);
    }

    public IEnumerable<TEdge> GetEdges([DisallowNull] TNode node)
    {
        return IncomingEdges(node).Concat(OutgoingEdges(node))
                                  .Distinct();
    }

    public IEnumerable<TEdge> IncomingEdges([DisallowNull] TNode node)
    {
        if (_incomingEdges.Value.TryGetValue(node, out HashSet<TEdge>? edges))
            return edges;

        return Enumerable.Empty<TEdge>();
    }

    public IEnumerable<TNode> IncomingNodes([DisallowNull] TNode node, Func<TEdge, bool>? predicate = null)
    {
        return (null == predicate)
            ? IncomingEdges(node).Select(e => e.Source)
            : IncomingEdges(node).Where(predicate).Select(e => e.Source);
    }

    public IEnumerable<TNode> OutgoingNodes([DisallowNull] TNode node, Func<TEdge, bool>? predicate = null)
    {
        return (null == predicate)
            ? OutgoingEdges(node).Select(e => e.Target)
            : OutgoingEdges(node).Where(predicate).Select(e => e.Target);
    }

    public IEnumerable<TEdge> OutgoingEdges([DisallowNull] TNode node)
    {
        if (_outgoingEdges.Value.TryGetValue(node, out HashSet<TEdge>? edges))
            return edges;

        return Enumerable.Empty<TEdge>();
    }

    public bool RemoveEdge(TEdge? edge)
    {
        if (null == edge) return false;

        RemoveIncomingEdge(edge);
        RemoveOutgoingEdge(edge);

        return EdgeSet.RemoveEdge(edge);
    }

    public void RemoveEdges([DisallowNull] IEnumerable<TEdge> edges)
    {
        foreach (var edge in edges)
        {
            RemoveIncomingEdge(edge);
            RemoveOutgoingEdge(edge);
        }

        EdgeSet.RemoveEdges(edges);
    }

    protected bool RemoveIncomingEdge(TEdge? edge)
    {
        if (null == edge) return false;

        if (!_incomingEdges.Value.TryGetValue(edge.Target, out HashSet<TEdge>? edges))
            return false;

        var removed = edges.Remove(edge);
        if (0 == edges.Count)
            _incomingEdges.Value.Remove(edge.Target);

        return removed;
    }

    protected bool RemoveOutgoingEdge(TEdge? edge)
    {
        if (null == edge) return false;

        if (!_outgoingEdges.Value.TryGetValue(edge.Source, out HashSet<TEdge>? edges))
            return false;

        var removed = edges.Remove(edge);
        if (0 == edges.Count)
            _outgoingEdges.Value.Remove(edge.Source);

        return removed;
    }

    public bool ExistsEdge(TNode source, TNode target)
    {
        throw new NotImplementedException();
    }
}

public class DirectedEdgeSet<TNode, TEdgeId, TEdge, TEdgeSet>
    : IDirectedEdgeSet<TNode, TEdgeId, TEdge>
    , INotifyCollectionChanged
    where TEdge : IEdge<TEdgeId, TNode>
    where TEdgeId : notnull
    where TEdgeSet : IEdgeSet<TNode, TEdgeId, TEdge>, INotifyCollectionChanged
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private bool _disposed;

    private readonly Lazy<IDictionary<TNode, IDictionary<TEdgeId, TEdge>>> _incomingEdges =
        new(() => new Dictionary<TNode, IDictionary<TEdgeId, TEdge>>());

    private readonly Lazy<IDictionary<TNode, IDictionary<TEdgeId, TEdge>>> _outgoingEdges =
        new(() => new Dictionary<TNode, IDictionary<TEdgeId, TEdge>>());

    public DirectedEdgeSet([DisallowNull] TEdgeSet edgeSet)
    {
        EdgeSet = edgeSet.ThrowIfNull(nameof(edgeSet));
        EdgeSet.CollectionChanged += EdgeSet_CollectionChanged;
    }

    ~DirectedEdgeSet()
    {
        Dispose(false);
    }

    public void AddEdge([DisallowNull] TEdge edge)
    {
        ArgumentNullException.ThrowIfNull(edge, nameof(edge));

        AddIncomingEdge(edge);
        AddOutgoingEdge(edge);

        //is set at the end because AddEdge calls CollectionChanged event.
        EdgeSet.AddEdge(edge);
    }

    public void AddEdges(IEnumerable<TEdge> edges)
    {
        foreach (var edge in edges)
        {
            AddIncomingEdge(edge);
            AddOutgoingEdge(edge);
        }

        //is set at the end because AddEdges calls CollectionChanged event.
        EdgeSet.AddEdges(edges);
    }

    protected void AddIncomingEdge([DisallowNull] TEdge edge)
    {
        if (!_incomingEdges.Value.TryGetValue(edge.Target, out IDictionary<TEdgeId, TEdge>? edges))
        {
            edges = new Dictionary<TEdgeId, TEdge>();

            _incomingEdges.Value.Add(edge.Target, edges);
            edges.Add(edge.Id, edge);
            return;
        }

        if (!edges.ContainsKey(edge.Id)) edges.Add(edge.Id, edge);
    }

    protected void AddOutgoingEdge([DisallowNull] TEdge edge)
    {
        if (!_outgoingEdges.Value.TryGetValue(edge.Source, out IDictionary<TEdgeId, TEdge>? edges))
        {
            edges = new Dictionary<TEdgeId, TEdge>();
            _outgoingEdges.Value.Add(edge.Source, edges);
        }

        if (!edges.ContainsKey(edge.Id)) edges.Add(edge.Id, edge);
    }

    public bool AllowDuplicateEdges => false;

    public void ClearEdges()
    {
        if (0 == EdgeSet.EdgeCount) return;

        EdgeSet.ClearEdges();
        _incomingEdges.Value.Clear();
        _outgoingEdges.Value.Clear();
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

    public bool ExistsEdge(TEdgeId edgeId) => EdgeSet.ExistsEdge(edgeId);

    public bool ExistsEdge(TEdge edge) => EdgeSet.ExistsEdge(edge);

    public bool ExistsEdge(TNode source, TNode target)
    {
        return EdgeSet.ExistsEdge(source, target);
    }

    protected void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(this, args);
    }

    public Option<TEdge> GetEdge(TEdgeId edgeId) => EdgeSet.GetEdge(edgeId);

    public IEnumerable<TEdge> GetEdges(TNode node) => EdgeSet.GetEdges(node);

    public IEnumerable<TEdge> IncomingEdges(TNode node)
    {
        if (_incomingEdges.Value.TryGetValue(node, out IDictionary<TEdgeId, TEdge>? edges))
            return edges.Values;

        return Enumerable.Empty<TEdge>();
    }

    public IEnumerable<TNode> IncomingNodes(TNode node, Func<TEdge, bool>? predicate = null)
    {
        return (null == predicate)
            ? IncomingEdges(node).Select(e => e.Source)
            : IncomingEdges(node).Where(predicate).Select(e => e.Source);
    }

    public IEnumerable<TEdge> OutgoingEdges(TNode node)
    {
        if (_outgoingEdges.Value.TryGetValue(node, out IDictionary<TEdgeId, TEdge>? edges))
            return edges.Values;

        return Enumerable.Empty<TEdge>();
    }

    public IEnumerable<TNode> OutgoingNodes(TNode node, Func<TEdge, bool>? predicate = null)
    {
        return (null == predicate)
            ? OutgoingEdges(node).Select(e => e.Target)
            : OutgoingEdges(node).Where(predicate).Select(e => e.Target);
    }

    public bool RemoveEdge([DisallowNull] TEdgeId edgeId)
    {
        var edge = GetEdge(edgeId);
        if (edge.IsNone) return false;

        return RemoveEdge(edge.OrThrow());
    }

    public bool RemoveEdge(TEdge edge)
    {
        if (null == edge) return false;

        RemoveIncomingEdge(edge.Target, edge.Id);
        RemoveOutgoingEdge(edge.Source, edge.Id);

        return EdgeSet.RemoveEdge(edge);
    }

    public void RemoveEdges(IEnumerable<TEdgeId> edgeIds)
    {
        var edges = edgeIds.Select(id => EdgeSet.GetEdge(id))
                           .SelectSome();

        RemoveEdges(edges.ToArray());
    }

    public void RemoveEdges(IEnumerable<TEdge> edges)
    {
        foreach (var edge in edges)
        {
            RemoveIncomingEdge(edge.Target, edge.Id);
            RemoveOutgoingEdge(edge.Source, edge.Id);
        }

        EdgeSet.RemoveEdges(edges);
     }

    protected bool RemoveIncomingEdge(TNode target, TEdgeId edgeId)
    {
        if (!_incomingEdges.Value.TryGetValue(target, out IDictionary<TEdgeId, TEdge>? edges))
            return false;

        var removed = edges.Remove(edgeId);
        if (0 == edges.Count)
            _incomingEdges.Value.Remove(target);

        return removed;
    }

    protected bool RemoveOutgoingEdge(TNode source, TEdgeId edgeId)
    {
        if (!_outgoingEdges.Value.TryGetValue(source, out IDictionary<TEdgeId, TEdge>? edges))
            return false;

        var removed = edges.Remove(edgeId);
        if (0 == edges.Count)
            _outgoingEdges.Value.Remove(source);

        return removed;
    }
}