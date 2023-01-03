namespace Foundation.Graph;

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

public class NodeSet<TNode> : INodeSet<TNode>
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly HashSet<TNode> _nodes;

    public NodeSet()
    {
        _nodes = new HashSet<TNode>();
    }

    public NodeSet([DisallowNull] IEnumerable<TNode> nodes)
    {
        if (null == nodes) throw new ArgumentNullException(nameof(nodes));
        _nodes = new HashSet<TNode>(nodes);
    }

    public void AddNode([DisallowNull] TNode node)
    {
        if(_nodes.Add(node))
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, node));
    }

    public void AddNodes([DisallowNull] IEnumerable<TNode> nodes)
    {
        foreach (var node in nodes)
        {
            if (null == node) continue;
            AddNode(node);
        }
    }

    public void ClearNodes()
    {
        _nodes.Clear();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool ExistsNode([DisallowNull] TNode node) => _nodes.Contains(node);

    protected void FireCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(sender, args);
    }

    public int NodeCount => _nodes.Count;

    public IEnumerable<TNode> Nodes => _nodes;

    public bool RemoveNode(TNode? node)
    {
        if (null == node || !_nodes.Remove(node)) return false;

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, node));
        return true;
    }

    public void RemoveNodes([DisallowNull] IEnumerable<TNode> nodes)
    {
        foreach (var node in nodes)
            RemoveNode(node);
    }
}

public class NodeSet<TNodeId, TNode> : INodeSet<TNodeId, TNode>
    where TNodeId : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly Lazy<IDictionary<TNodeId, TNode>> _nodes;

    public NodeSet()
        : this(new Lazy<IDictionary<TNodeId, TNode>>(() => new Dictionary<TNodeId, TNode>()))
    {
    }

    public NodeSet([DisallowNull] IDictionary<TNodeId, TNode> nodes)
        : this(new Lazy<IDictionary<TNodeId, TNode>>(() => nodes))
    {
    }

    public NodeSet([DisallowNull] Lazy<IDictionary<TNodeId, TNode>> nodes)
    {
        _nodes = nodes.ThrowIfNull(nameof(nodes));
    }

    public void AddNode(TNodeId nodeId, [DisallowNull] TNode node)
    {
        if (ExistsNode(nodeId))
            throw new NodeSetException($"node {node} exists");

        _nodes.Value.Add(nodeId, node);
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, node));
    }

    public void ClearNodes()
    {
        _nodes.Value.Clear();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool ExistsNode(TNodeId id)
    {
        return _nodes.Value.ContainsKey(id);
    }

    protected void FireCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(sender, args);
    }

    public Option<TNode> GetNode(TNodeId nodeId) => TryGetNode(nodeId, out TNode? node) ? Option.Maybe(node) : Option.None<TNode>();

    public int NodeCount => _nodes.Value.Count;

    public IEnumerable<TNodeId> NodeIds => _nodes.Value.Keys;

    public IEnumerable<TNode> Nodes => _nodes.Value.Values;

    public bool RemoveNode(TNodeId id)
    {
        if (!_nodes.Value.Remove(id)) return false;
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, id));
        return true;
    }

    public void RemoveNodes([DisallowNull] IEnumerable<TNodeId> nodes)
    {
        foreach (var node in nodes)
            RemoveNode(node);
    }

    public bool TryGetNode(TNodeId nodeId, out TNode? node)
    {
        return _nodes.Value.TryGetValue(nodeId, out node);
    }
}
