using FluentAssertions;
using Foundation.Graph.Tests;

namespace Foundation.Graph.Algorithm;

public class UndirectedSearchTests
{
    public class Bfs
    {
        [Fact]
        public void ConnectedEdges_Should_Return5Edges_When_StartEdge_SourceIs4_And_TargetIs5()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var connections = UndirectedSearch.Bfs.ConnectedEdges(sut, UndirectedEdge.New(4, 5)).ToArray();

            connections.Length.Should().Be(5);

            var expected = new[]
            {
                UndirectedEdge.New(3, 2),
                UndirectedEdge.New(1, 2),
                UndirectedEdge.New(2, 4),
                UndirectedEdge.New(5, 4),
                UndirectedEdge.New(6, 5),
            };
            connections.Should().Contain(expected);
        }

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

        [Fact]
        public void EdgeCount_Should_Return10_When_10EdgesAdded()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            sut.EdgeCount.Should().Be(10);
        }

        [Fact]
        public void FindConnectedPaths_Should_Return2EdgePaths_When_2EdgePathsExist()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var paths = UndirectedSearch.Bfs.FindConnectedPaths(sut).ToArray();

            paths.Length.Should().Be(2);
            {
                var path = paths[0];

                var expected = new[]
                {
                    UndirectedEdge.New(1, 2),
                    UndirectedEdge.New(3, 2),
                    UndirectedEdge.New(2, 4),
                    UndirectedEdge.New(5, 4),
                    UndirectedEdge.New(6, 5),
                };

                path.Should().Contain(expected);
            }
            {
                var path = paths[1];

                var expected = new[]
                {
                    UndirectedEdge.New(7, 8),
                    UndirectedEdge.New(10, 8),
                    UndirectedEdge.New(9, 8),
                    UndirectedEdge.New(12, 8),
                    UndirectedEdge.New(9, 11),
                };

                path.Should().Contain(expected);
            }
        }

        [Fact]
        public void ConnectedNodes_Should_Return2EdgePaths_When_2EdgePathsExist()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var paths = UndirectedSearch.Bfs.FindConnectedNodes(sut).ToArray();

            paths.Length.Should().Be(2);
            {
                var path = paths[0];

                var expected = new[] { 1, 2, 3, 4, 5, 6 };

                path.Should().Contain(expected);
            }
            {
                var path = paths[1];

                var expected = new[] { 7, 8, 9, 10, 11, 12 };

                path.Should().Contain(expected);
            }
        }

        [Fact]
        public void NodesWithSingleConnection_Should_Return5Edges_When_StartNodeIs11()
        {
            var sut = new UndirectedEdgeSet<int, UndirectedEdge<int>>();

            var edges = TestEdgeFactory.GetUndirectedEdges().ToArray();

            foreach (var edge in edges)
            {
                sut.AddEdge(edge);
            }

            var nodes = UndirectedSearch.Bfs.NodesWithSingleConnection(sut).ToArray();

            nodes.Length.Should().Be(7);

            var expected = new[] { 1, 3, 6, 7, 10, 11, 12 };

            nodes.Should().Contain(expected);
        }
    }
}
