namespace Foundation.Graph;

public interface IOutsideEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
    /// <summary>
    /// Checks if outside edge exists.
    /// </summary>
    /// <param name="edge"></param>
    /// <returns></returns>
    bool ExistsOutsideEdge(IOutsideEdge<TNode, TEdge> edge);

    /// <summary>
    /// Number of outside edges.
    /// </summary>
    int OutsideEdgeCount { get; }

    IEnumerable<IOutsideEdge<TNode, TEdge>> OutsideEdges { get; }
}
