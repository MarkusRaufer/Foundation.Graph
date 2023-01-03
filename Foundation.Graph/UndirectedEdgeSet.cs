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

    private readonly IMultiValueMap<TNode, TEdge> _edges;

    public UndirectedEdgeSet()
    {
        _edges = new MultiValueMap<TNode, TEdge>();
    }

    public UndirectedEdgeSet(IEnumerable<TEdge> edges) : this()
    {
        AddEdges(edges);
    }

    public bool AllowDuplicateEdges => false;

    public int EdgeCount => _edges.Count;

    public IEnumerable<TEdge> Edges => _edges.Values.Distinct();

    public void AddEdge(TEdge edge)
    {
        if (_edges.TryGetValues(edge.Source, out IEnumerable<TEdge>? edges))
        {
            if (edges.Any(x => x.Target.Equals(edge.Target))) return;
        }

        addEdge(edge);

        void addEdge(TEdge edge)
        {
            _edges.Add(edge.Source, edge);
            _edges.Add(edge.Target, edge);

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, edge));
        }
    }

    public void AddEdges(IEnumerable<TEdge> edges)
    {
        foreach(var edge in edges)
            AddEdge(edge);
    }

    public void ClearEdges()
    {
        _edges.Clear();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool ExistsEdge(TEdge edge) => ExistsEdge(edge.Source, edge.Target);

    public bool ExistsEdge(TNode source, TNode target)
    {
        return _edges.TryGetValues(source, out IEnumerable<TEdge> edges)
            && edges.Any(x => x.Target.Equals(target));
    }

    public IEnumerable<TEdge> GetEdges(TNode node) => _edges.GetValues(node);

    public bool RemoveEdge(TEdge edge)
    {
        var removed = _edges.Remove(edge.Source, edge)
                  && _edges.Remove(edge.Target, edge);

        if (!removed) return false;

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, edge));
   
        return true;
    }

    public void RemoveEdges(IEnumerable<TEdge> edges)
    {
        foreach(var edge in edges)
            RemoveEdge(edge);
    }
}
