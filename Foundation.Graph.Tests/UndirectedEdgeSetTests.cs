using Foundation.Collections.Generic;
using Foundation.Graph.Tests;
using Shouldly;

namespace Foundation.Graph;

public class UndirectedEdgeSetTests
{
    [Fact]
    public void GetEdges_Should_Return2Edges_When_NodeIsConnectedWith2Nodes()
    {
        var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

        var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

        foreach(var edge in edges)
        {
            sut.AddEdge(edge);
        }

        var connections = sut.GetEdges(4).ToArray();

        connections.Length.ShouldBe(2);

        var expected = new[]
        {
            UndirectedEdge.New(2, 4),
            UndirectedEdge.New(4, 5),
        };

        connections.Contains(expected).ShouldBeTrue();
    }

    [Fact]
    public void GetEdges_Should_Return3Edges_When_NodeIsConnectedWith3Nodes()
    {
        var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

        var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

        foreach (var edge in edges)
        {
            sut.AddEdge(edge);
        }

        var connections = sut.GetEdges(8).ToArray();
        connections.Length.ShouldBe(4);

        var expected = new[]
        {
            UndirectedEdge.New(7, 8),
            UndirectedEdge.New(10, 8),
            UndirectedEdge.New(9, 8),
            UndirectedEdge.New(12, 8),
        };

        connections.Contains(expected).ShouldBeTrue();
    }
}
