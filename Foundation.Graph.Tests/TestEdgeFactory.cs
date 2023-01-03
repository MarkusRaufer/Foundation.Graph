namespace Foundation.Graph.Tests;

public static class TestEdgeFactory
{
    /// <summary>
    /// [1]
    ///    \ 
    ///    [2]-[4]-[5]-[6]
    ///    /
    /// [3]
    ///
    /// [7]  [9]-[11]
    ///    \ /
    ///    [8]-[12]
    ///    /
    /// [10]
    ///
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Edge<int>> GetEdges()
    {
        return new[]
        {
            Edge.New(1, 2),
            Edge.New(3, 2),
            Edge.New(2, 4),
            Edge.New(5, 4),
            Edge.New(6, 5),
            Edge.New(7, 8),
            Edge.New(10, 8),
            Edge.New(9, 8),
            Edge.New(9, 11),
            Edge.New(12, 8),
        };
    }

    ////// <summary>
    /// [1]
    ///    \ 
    ///    [2]-[4]-[5]-[6]
    ///    /
    /// [3]
    ///
    /// [7]  [9]-[11]
    ///    \ /
    ///    [8]-[12]
    ///    /
    /// [10]
    ///
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<UndirectedEdge<int>> GetUndirectedEdges()
    {
        return new[]
        {
            UndirectedEdge.New(1, 2),
            UndirectedEdge.New(3, 2),
            UndirectedEdge.New(2, 4),
            UndirectedEdge.New(5, 4),
            UndirectedEdge.New(6, 5),
            UndirectedEdge.New(7, 8),
            UndirectedEdge.New(10, 8),
            UndirectedEdge.New(9, 8),
            UndirectedEdge.New(9, 11),
            UndirectedEdge.New(12, 8),
        };
    }
}
