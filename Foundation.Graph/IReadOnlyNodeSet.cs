namespace Foundation.Graph;

/// <summary>
/// Interface for a set of nodes.
/// </summary>
/// <typeparam name="TNode"></typeparam>
public interface IReadOnlyNodeSet<TNode>
{
    /// <summary>
    /// Checks, if node exists.
    /// </summary>
    /// <param name="node">Node which is searched.</param>
    /// <returns></returns>
    bool ExistsNode(TNode node);

    /// <summary>
    /// Number of nodes.
    /// </summary>
    int NodeCount { get; }

    /// <summary>
    /// List of nodes.
    /// </summary>
    IEnumerable<TNode> Nodes { get; }
}

/// <summary>
/// Interface for a set of nodes.
/// </summary>
/// <typeparam name="TNode"></typeparam>
public interface IReadOnlyNodeSet<TNodeId, TNode>
{
    /// <summary>
    /// Returns true, if node exists.
    /// </summary>
    /// <param name="node">Node which is searched.</param>
    /// <returns></returns>
    bool ExistsNode(TNodeId id);

    /// <summary>
    /// Returns Some if node exists.
    /// </summary>
    /// <param name="nodeId"></param>
    /// <returns></returns>
    Option<TNode> GetNode(TNodeId nodeId);

    /// <summary>
    /// Number of nodes.
    /// </summary>
    int NodeCount { get; }

    /// <summary>
    /// List of node ids.
    /// </summary>
    IEnumerable<TNodeId> NodeIds { get; }

    /// <summary>
    /// List of nodes.
    /// </summary>
    IEnumerable<TNode> Nodes { get; }

    bool TryGetNode(TNodeId nodeId, out TNode? node);
}
