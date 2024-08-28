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
namespace Foundation.Graph;

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public class EdgeSet<TNode, TEdge>
    : IEdgeSet<TNode, TEdge>
    , INotifyCollectionChanged
    where TEdge : IEdge<TNode>
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly ICollection<TEdge> _edges;

    public EdgeSet() : this([])
    {
    }

    public EdgeSet([DisallowNull] IEnumerable<TEdge> edges) : this(edges.ToList())
    {
    }

    public EdgeSet([DisallowNull] ICollection<TEdge> edges)
    {
        _edges = edges.ThrowIfNull();
    }

    public void AddEdge([DisallowNull] TEdge edge)
    {
        edge.ThrowIfNull();

        var count = _edges.Count;
        _edges.Add(edge);
        
        if (_edges.Count > count)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edge));
    }

    public void AddEdges(IEnumerable<TEdge> edges)
    {
        edges.ThrowIfNull();

        var count = _edges.Count;
        foreach (var edge in edges)
        {
            _edges.Add(edge);
        }

        if (_edges.Count > count)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edges.ToArray()));
    }

    public void ClearEdges()
    {
        if (_edges.Count == 0) return;

        _edges.Clear();
        
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public int EdgeCount => _edges.Count;

    public IEnumerable<TEdge> Edges => _edges;

    public bool ExistsEdge(TEdge edge) => _edges.Contains(edge);

    public bool ExistsEdge(TNode source, TNode target)
    {
        return _edges.Any(x => x.Source.Equals(source) && x.Target.Equals(target));
    }

    protected void FireCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(sender, args);
    }

    public IEnumerable<TEdge> GetEdges([DisallowNull] TNode node) => _edges.Where(e => e.Source.Equals(node) || e.Target.Equals(node));

    public IEnumerable<TEdge> GetEdges(TNode source, TNode target) => _edges.Where(e => e.Source.Equals(source) && e.Target.Equals(target));

    public bool RemoveEdge(TEdge edge)
    {
        if (!_edges.Remove(edge)) return false;

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, edge));
        return true;
    }

    public void RemoveEdges(IEnumerable<TEdge> edges)
    {
        var removedEdges = new List<TEdge>();
        foreach (var edge in edges)
        {
            if (_edges.Remove(edge)) removedEdges.Add(edge);
        }

        if (0 == removedEdges.Count) return;
        
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedEdges.ToArray()));
    }
}

public class EdgeSet<TNode, TEdgeId, TEdge>
    : IEdgeSet<TNode, TEdgeId, TEdge>
    , INotifyCollectionChanged
    where TEdge : IEdge<TEdgeId, TNode>
    where TEdgeId : notnull
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly Dictionary<TEdgeId, TEdge> _edges = [];

    public int EdgeCount => _edges.Count;

    public IEnumerable<TEdge> Edges => _edges.Values;


    public void AddEdge(TEdge edge)
    {
        edge.ThrowIfNull();

        var count = _edges.Count;
        _edges.Add(edge.Id, edge);

        if (_edges.Count > count)
            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edge));
    }

    public void AddEdges(IEnumerable<TEdge> edges)
    {
        edges.ThrowIfNull();

        var count = _edges.Count;

        foreach (var edge in edges)
        {
            _edges.Add(edge.Id, edge);
        }

        if (_edges.Count > count)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edges.ToArray()));
    }

    public void ClearEdges()
    {
        if (0 == _edges.Count) return;

        _edges.Clear();
        FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool ExistsEdge(TEdgeId edgeId) => _edges.ContainsKey(edgeId);

    public bool ExistsEdge(TEdge edge) => _edges.Values.Any(e => e.Equals(edge));

    public bool ExistsEdge(TNode source, TNode target)
    {
        return _edges.Values.Any(x => x.Source.Equals(source) && x.Target.Equals(target));
    }

    protected void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(this, args);
    }

    public Option<TEdge> GetEdge(TEdgeId edgeId)
    {
        if (_edges.TryGetValue(edgeId, out TEdge? edge)) return Option.Some(edge);

        return Option.None<TEdge>();
    }

    public IEnumerable<TEdge> GetEdges([DisallowNull] TNode node) => _edges.Values.Where(e => e.Source.Equals(node) || e.Target.Equals(node));

    public IEnumerable<TEdge> GetEdges(TNode source, TNode target) => _edges.Values.Where(e => e.Source.Equals(source) && e.Target.Equals(target));

    public bool RemoveEdge([DisallowNull] TEdge edge)
    {
        var removed = _edges.Remove(edge.Id);
        if (removed) FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, edge));

        return removed;
    }

    public bool RemoveEdge([DisallowNull] TEdgeId edgeId)
    {
        if(null != CollectionChanged)
        {
            var optionalEdge = GetEdge(edgeId);
            if (optionalEdge.IsNone) return false;

            return RemoveEdge(optionalEdge.OrThrow());
        }

        return _edges.Remove(edgeId);
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
            if (_edges.Remove(edge.Id)) removedEdges.Add(edge);
        }

        if (0 == removedEdges.Count) return;

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedEdges.ToArray()));
    }
}
