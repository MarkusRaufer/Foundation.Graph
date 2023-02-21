using Foundation.Collections.Generic;
using System.Collections.Specialized;

namespace Foundation.Graph;

/// <summary>
/// This is an undirected edge set.
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="TEdge"></typeparam>
public class UndirectedEdgeSet<TNode, TEdge> : IEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly MultiValueMap<TNode, TEdge> _node2Edges;
    private readonly MultiValueMap<TEdge, TNode> _edge2Nodes;

    public UndirectedEdgeSet()
    {
        _node2Edges = new MultiValueMap<TNode, TEdge>();
        _edge2Nodes = new MultiValueMap<TEdge, TNode>();
    }

    public UndirectedEdgeSet(IEnumerable<TEdge> edges) : this()
    {
        AddEdges(edges);
    }

    public bool AllowDuplicateEdges => false;

    public int EdgeCount => _edge2Nodes.Keys.Count;

    public IEnumerable<TEdge> Edges => _edge2Nodes.Keys;

    public void AddEdge(TEdge edge)
    {
        if (_edge2Nodes.ContainsKey(edge)) return;

        _edge2Nodes.Add(edge, edge.Source);
        _edge2Nodes.Add(edge, edge.Target);

        _node2Edges.Add(edge.Source, edge);
        _node2Edges.Add(edge.Target, edge);

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edge));
    }

    public void AddEdges(IEnumerable<TEdge> edges)
    {
        foreach(var edge in edges)
            AddEdge(edge);
    }

    public void ClearEdges()
    {
        _edge2Nodes.Clear();
        _node2Edges.Clear();

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool ExistsEdge(TEdge edge) => _edge2Nodes.ContainsKey(edge);

    public bool ExistsEdge(TNode source, TNode target)
    {
        if (_node2Edges.TryGetValue(source, out TEdge? sourceEdge) && sourceEdge.EqualsUndirected(source, target)) return true;

        return _node2Edges.TryGetValue(target, out TEdge? targetEdge) && targetEdge.EqualsUndirected(source, target);
    }

    public IEnumerable<TEdge> GetEdges(TNode node)
    {
        return _node2Edges.GetValues(new [] { node });
    }

    public bool RemoveEdge(TEdge edge)
    {
        var removed = _edge2Nodes.Remove(edge);
        if(removed)
        {
            _node2Edges.Remove(edge.Source);
            _node2Edges.Remove(edge.Target);
        }

        if (removed)
             CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, edge));
   
        return removed;
    }

    public void RemoveEdges(IEnumerable<TEdge> edges)
    {
        foreach(var edge in edges)
            RemoveEdge(edge);
    }
}
