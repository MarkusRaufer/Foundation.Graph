using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Graph;

public class UndirectedGraph<TNode, TEdge> 
    : IGraph<TNode, TEdge>
    , IDisposable
    where TEdge : IEdge<TNode>
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly UndirectedEdgeSet<TNode, TEdge> _edgeSet;
    private readonly NodeSet<TNode> _nodeSet;

    public UndirectedGraph()
    {
        _edgeSet = new UndirectedEdgeSet<TNode, TEdge>();
        _nodeSet = new NodeSet<TNode>();

        _edgeSet.CollectionChanged += CollectionChanged;
        _nodeSet.CollectionChanged += CollectionChanged;
    }

    ~UndirectedGraph()
    {
        Dispose(false);
    }

    public bool AllowDuplicateEdges => false;



    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (!disposing) return;

        _edgeSet.CollectionChanged -= CollectionChanged;
        _nodeSet.CollectionChanged -= CollectionChanged;

        _edgeSet.ClearEdges();
        _nodeSet.ClearNodes();
    }

    public void AddEdge(TEdge edge) => _edgeSet.AddEdge(edge);

    public void AddEdges(IEnumerable<TEdge> edges) => _edgeSet.AddEdges(edges);

    public void AddNode([DisallowNull] TNode node) => _nodeSet.AddNode(node);

    public void AddNodes(IEnumerable<TNode> nodes) => _nodeSet.AddNodes(nodes);

    public void ClearEdges() => _edgeSet.ClearEdges();

    public int EdgeCount => _edgeSet.EdgeCount;

    public IEnumerable<TEdge> Edges => _edgeSet.Edges;

    public void ClearNodes() => _nodeSet.ClearNodes();

    public bool ExistsEdge(TEdge edge) => _edgeSet.ExistsEdge(edge);

    public bool ExistsEdge(TNode source, TNode target) => _edgeSet.ExistsEdge(source, target);

    public bool ExistsNode([DisallowNull] TNode node) => _nodeSet.ExistsNode(node);

    public IEnumerable<TEdge> GetEdges(TNode node) => _edgeSet.GetEdges(node);

    public int NodeCount => _nodeSet.NodeCount;

    public IEnumerable<TNode> Nodes => _nodeSet.Nodes;

    public bool RemoveEdge(TEdge edge) => _edgeSet.RemoveEdge(edge);

    public void RemoveEdges(IEnumerable<TEdge> edges) => _edgeSet.RemoveEdges(edges);

    public bool RemoveNode(TNode node) => _nodeSet.RemoveNode(node);

    public void RemoveNodes(IEnumerable<TNode> nodes) => _nodeSet.RemoveNodes(nodes);
}
