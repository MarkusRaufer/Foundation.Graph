using FluentAssertions;
using Foundation.Graph.Tests;

namespace Foundation.Graph.Algorithm;

public class UndirectedSearchTests
{
    public class Bfs
    {
        [Fact]
        public void ConnectedEdges_Should_Return5Edges_When_StartNodeIs3()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var connections = UndirectedSearch.Bfs.ConnectedEdges(sut, 3).ToArray();

            connections.Length.Should().Be(5);

            var expected = new[]
            {
                UndirectedEdge.New(3, 2),
                UndirectedEdge.New(1, 2),
                UndirectedEdge.New(2, 4),
                UndirectedEdge.New(5, 4),
                UndirectedEdge.New(6, 5),
            };
            connections.Should().ContainInOrder(expected);
        }

        [Fact]
        public void ConnectedEdges_Should_Return5Edges_When_StartNodeIs4()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var connections = UndirectedSearch.Bfs.ConnectedEdges(sut, 4).ToArray();

            connections.Length.Should().Be(5);

            var expected = new[]
            {
                UndirectedEdge.New(2, 4),
                UndirectedEdge.New(5, 4),
                UndirectedEdge.New(1, 2),
                UndirectedEdge.New(3, 2),
                UndirectedEdge.New(6, 5),
            };
            connections.Should().ContainInOrder(expected);
        }

        [Fact]
        public void ConnectedEdges_Should_Return5Edges_When_StartNodeIs8()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var connections = UndirectedSearch.Bfs.ConnectedEdges(sut, 8).ToArray();

            connections.Length.Should().Be(5);

            var expected = new[]
            {
                UndirectedEdge.New(7, 8),
                UndirectedEdge.New(10, 8),
                UndirectedEdge.New(9, 8),
                UndirectedEdge.New(12, 8),
                UndirectedEdge.New(9, 11),
            };
            connections.Should().ContainInOrder(expected);
        }

        [Fact]
        public void ConnectedNodes_Should_Return5Edges_When_StartNodeIs4()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var nodes = UndirectedSearch.Bfs.ConnectedNodes(sut, 4).ToArray();

            nodes.Length.Should().Be(5);

            var expected = new[] { 2, 5, 1, 3, 6 };

            nodes.Should().ContainInOrder(expected);
        }

        [Fact]
        public void ConnectedNodes_Should_Return5Edges_When_StartNodeIs8()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var nodes = UndirectedSearch.Bfs.ConnectedNodes(sut, 8).ToArray();

            nodes.Length.Should().Be(5);

            var expected = new[] { 7, 10, 9, 12, 11 };

            nodes.Should().ContainInOrder(expected);
        }

        [Fact]
        public void ConnectedNodes_Should_Return5Edges_When_StartNodeIs11()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var nodes = UndirectedSearch.Bfs.ConnectedNodes(sut, 11).ToArray();

            nodes.Length.Should().Be(5);

            var expected = new[] { 9, 8, 7, 10, 12 };

            nodes.Should().ContainInOrder(expected);
        }
    }
}
