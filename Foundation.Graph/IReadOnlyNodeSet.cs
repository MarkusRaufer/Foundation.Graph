namespace Foundation.Graph;

using System.Diagnostics.CodeAnalysis;

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
    bool ExistsNode([DisallowNull] TNode node);

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
    where TNodeId : notnull
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
    Opt<TNode> GetNode(TNodeId nodeId);

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

    bool TryGetNode(TNodeId nodeId, [MaybeNullWhen(false)] out TNode? node);
}
