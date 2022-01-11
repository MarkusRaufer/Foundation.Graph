namespace Foundation.Graph;

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

public interface INodeSet<TNode>
    : IReadOnlyNodeSet<TNode>
    , INotifyCollectionChanged
{
    /// <summary>
    /// adds a node.
    /// </summary>
    /// <param name="node"></param>
    void AddNode([DisallowNull] TNode node);

    /// <summary>
    /// adds a list of nodes.
    /// </summary>
    /// <param name="nodes"></param>
    void AddNodes(IEnumerable<TNode> nodes);

    /// <summary>
    /// clears the node list.
    /// </summary>
    void ClearNodes();

    /// <summary>
    /// removes a node.
    /// </summary>
    /// <param name="node"></param>
    bool RemoveNode(TNode node);

    /// <summary>
    /// removes a list of nodes.
    /// </summary>
    /// <param name="nodes"></param>
    void RemoveNodes(IEnumerable<TNode> nodes);
}

public interface INodeSet<TNodeId, TNode>
    : IReadOnlyNodeSet<TNodeId, TNode>
    , INotifyCollectionChanged
    where TNodeId : notnull
{
    /// <summary>
    /// adds a node.
    /// </summary>
    /// <param name="node"></param>
    void AddNode([DisallowNull] TNodeId nodeId, [DisallowNull] TNode node);

    /// <summary>
    /// clears the node list.
    /// </summary>
    void ClearNodes();

    /// <summary>
    /// removes a node.
    /// </summary>
    /// <param name="node"></param>
    bool RemoveNode(TNodeId nodeId);

    /// <summary>
    /// removes a list of nodes.
    /// </summary>
    /// <param name="nodes"></param>
    void RemoveNodes(IEnumerable<TNodeId> nodeIds);
}
