namespace Foundation.Graph;

public interface IMutableNodeSet<TNode>
{
    /// <summary>
    /// adds a node.
    /// </summary>
    /// <param name="node"></param>
    void AddNode(TNode node);

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

public interface IMutableNodeSet<TNodeId, TNode>
{
    /// <summary>
    /// adds a node.
    /// </summary>
    /// <param name="node"></param>
    void AddNode(TNodeId nodeId, TNode node);

    /// <summary>
    /// adds a list of nodes to the node set.
    /// </summary>
    /// <param name="nodes">list of nodes.</param>
    void AddNodes(IEnumerable<(TNodeId, TNode)> nodes);

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
