using FluentAssertions;
using Foundation.Graph.Algorithm;
using Foundation.Graph.Linq;
using System.Linq.Expressions;

namespace Foundation.Graph.Tests.Algorithm;

public class DirectedSearchTests
{
    [Fact]
    public void CommonParent_Should_ReturnNone_When_UsingHeight3()
    {
        var edgeSet = new DirectedEdgeSet<int, Edge<int>>();

        var edges = CreateEdges(1, 2, 4).ToArray();
        edgeSet.AddEdges(edges);

        var result = DirectedSearch.Bfs.CommonParent(edgeSet, [111, 211]);

        result.IsNone.Should().BeTrue();
    }

    [Fact]
    public void CommonParent_Should_ReturnNone_When_UsingHeight3x()
    {
        LambdaExpression lambda = (string str, int x) => str == "A" && x == 3 || x > 10;

        var factory = new BinaryExpressionGraphFactory();
        var graph = factory.CreateGraph(lambda);

        var parameters = graph.Nodes.OfType<ParameterExpression>().Where(x => x.NodeType == ExpressionType.Parameter && x.Name == "x").ToArray();

        var eq = parameters[0].Equals(parameters[1]);
        var result = DirectedSearch.Bfs.CommonParent(graph, parameters);

        result.IsNone.Should().BeTrue();
    }

    [Fact]
    public void CommonParent_Should_ReturnNone_When_UsingHeight4()
    {
        var edgeSet = new DirectedEdgeSet<int, Edge<int>>();

        //                             1                                               2
        //                             |                                               |
        //                +------------+--------------+                                +
        //                |                           |                                |
        //               11                          12                               21
        //                |                           |                                |
        //       +--------+-------+            +------+-----+         +----------------+----------------+    
        //       |                |            |            |         |                |                |
        //      111              112          121          122       211              212              213
        //       |                |                                   |                |                | 
        //   +---+----+       +---+----+                          +---+----+       +---+----+       +---+----+
        //   |        |       |        |                          |        |       |        |       |        |
        // 1111     1112    1121     1122                       2111     2112    2121     2122    2131     2132


        edgeSet.AddEdge(newEdge(1, 11));
        edgeSet.AddEdge(newEdge(1, 12));
        edgeSet.AddEdge(newEdge(2, 21));

        edgeSet.AddEdge(newEdge(11, 111));
        edgeSet.AddEdge(newEdge(11, 112));
        edgeSet.AddEdge(newEdge(12, 121));
        edgeSet.AddEdge(newEdge(12, 122));
        edgeSet.AddEdge(newEdge(21, 211));
        edgeSet.AddEdge(newEdge(21, 212));
        edgeSet.AddEdge(newEdge(21, 213));

        edgeSet.AddEdge(newEdge(111, 1111));
        edgeSet.AddEdge(newEdge(111, 1112));
        edgeSet.AddEdge(newEdge(112, 1121));
        edgeSet.AddEdge(newEdge(112, 1122));
        edgeSet.AddEdge(newEdge(211, 2111));
        edgeSet.AddEdge(newEdge(211, 2112));
        edgeSet.AddEdge(newEdge(212, 2121));
        edgeSet.AddEdge(newEdge(212, 2122));
        edgeSet.AddEdge(newEdge(213, 2131));
        edgeSet.AddEdge(newEdge(213, 2132));

        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [11, 21]);

            result.TryGet(out var parent).Should().BeFalse();
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [112, 212]);

            result.TryGet(out var parent).Should().BeFalse();
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1121, 2131]);

            result.TryGet(out var parent).Should().BeFalse();
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [11, 2122]);

            result.TryGet(out var parent).Should().BeFalse();
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1111, 121, 2122]);

            result.TryGet(out var parent).Should().BeFalse();
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [112, 12, 2111]);

            result.TryGet(out var parent).Should().BeFalse();
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1111, 1121, 121, 2111, 2132]);

            result.TryGet(out var parent).Should().BeFalse();
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [2131, 212, 2111, 112, 1112]);

            result.TryGet(out var parent).Should().BeFalse();
        }
        static Edge<int> newEdge(int source, int target) => new(source, target);
    }

    [Fact]
    public void CommonParent_Should_ReturnParent_When_UsingHeight3()
    {
        var edgeSet = new DirectedEdgeSet<int, Edge<int>>();

        var edges = CreateEdges(1, 2, 4).ToArray();
        edgeSet.AddEdges(edges);

        var result = DirectedSearch.Bfs.CommonParent(edgeSet, [111, 112]);

        result.TryGet(out var parent).Should().BeTrue();
        parent.Should().Be(11);
    }

    [Fact]
    public void CommonParent_Should_ReturnParent_When_UsingHeight4()
    {
        var edgeSet = new DirectedEdgeSet<int, Edge<int>>();

        //                                            1
        //                                            |
        //                +---------------------------+--------------------------------+
        //                |                           |                                |
        //               11                          12                               13
        //                |                           |                                |
        //       +--------+-------+            +------+-----+         +----------------+----------------+    
        //       |                |            |            |         |                |                |
        //      111              112          121          122       131              132              133
        //       |                |                                   |                |                | 
        //   +---+----+       +---+----+                          +---+----+       +---+----+       +---+----+
        //   |        |       |        |                          |        |       |        |       |        |
        // 1111     1112    1121     1122                       1311     1312    1321     1322    1331     1332


        edgeSet.AddEdge(newEdge(1, 11));
        edgeSet.AddEdge(newEdge(1, 12));
        edgeSet.AddEdge(newEdge(1, 13));

        edgeSet.AddEdge(newEdge(11, 111));
        edgeSet.AddEdge(newEdge(11, 112));
        edgeSet.AddEdge(newEdge(12, 121));
        edgeSet.AddEdge(newEdge(12, 122));
        edgeSet.AddEdge(newEdge(13, 131));
        edgeSet.AddEdge(newEdge(13, 132));
        edgeSet.AddEdge(newEdge(13, 133));

        edgeSet.AddEdge(newEdge(111, 1111));
        edgeSet.AddEdge(newEdge(111, 1112));
        edgeSet.AddEdge(newEdge(112, 1121));
        edgeSet.AddEdge(newEdge(112, 1122));
        edgeSet.AddEdge(newEdge(131, 1311));
        edgeSet.AddEdge(newEdge(131, 1312));
        edgeSet.AddEdge(newEdge(132, 1321));
        edgeSet.AddEdge(newEdge(132, 1322));
        edgeSet.AddEdge(newEdge(133, 1331));
        edgeSet.AddEdge(newEdge(133, 1332));

        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1121, 1122]);

            result.TryGet(out var parent).Should().BeTrue();
            parent.Should().Be(112);
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1111, 112]);

            result.TryGet(out var parent).Should().BeTrue();
            parent.Should().Be(11);
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1311, 1322]);

            result.TryGet(out var parent).Should().BeTrue();
            parent.Should().Be(13);
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1111, 1322]);

            result.TryGet(out var parent).Should().BeTrue();
            parent.Should().Be(1);
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1311, 1322, 1331]);

            result.TryGet(out var parent).Should().BeTrue();
            parent.Should().Be(13);
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1311, 132, 1331]);

            result.TryGet(out var parent).Should().BeTrue();
            parent.Should().Be(13);
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1322, 131, 13]);

            result.TryGet(out var parent).Should().BeTrue();
            parent.Should().Be(1);
        }
        {
            var result = DirectedSearch.Bfs.CommonParent(edgeSet, [1111, 112, 1311, 132, 1331]);

            result.TryGet(out var parent).Should().BeTrue();
            parent.Should().Be(1);
        }

        static Edge<int> newEdge(int source, int target) => new(source, target);
    }

    [Fact]
    public void IncommingEdgesWithDepthLevel_Should_ReturnNone_When_UsingHeight3()
    {
        var edges = CreateEdges(1, 2, 4).ToArray();
        var edgeSet = new DirectedEdgeSet<int, Edge<int>>();
        edgeSet.AddEdges(edges);

        var incomingEdges = DirectedSearch.Bfs.IncomingEdgesWithDepthLevel(edgeSet, 1122).ToArray();

        incomingEdges.Length.Should().Be(3);
        {
            var incomingEdge = incomingEdges[0];
            incomingEdge.Depth.Should().Be(1);
        }
        {
            var incomingEdge = incomingEdges[1];
            incomingEdge.Depth.Should().Be(2);
        }
        {
            var incomingEdge = incomingEdges[2];
            incomingEdge.Depth.Should().Be(3);
        }
    }

    [Fact]
    public void IncommingNodesWithDepthLevel_Should_ReturnNone_When_UsingHeight3()
    {
        var graph = new DirectedGraph<int, Edge<int>>();
        foreach (var edge in CreateEdges(1, 2, 4))
        {
            if (!graph.ExistsNode(edge.Source)) graph.AddNode(edge.Source);
            if (!graph.ExistsNode(edge.Target)) graph.AddNode(edge.Target);

            if (!graph.ExistsEdge(edge)) graph.AddEdge(edge);
        }
       
        var incomingNodes = DirectedSearch.Bfs.IncomingNodesWithDepthLevel(graph, 1122).ToArray();

        incomingNodes.Length.Should().Be(3);
        {
            var incomingNode = incomingNodes[0];
            incomingNode.Depth.Should().Be(1);
        }
        {
            var incomingNode = incomingNodes[1];
            incomingNode.Depth.Should().Be(2);
        }
        {
            var incomingNode = incomingNodes[2];
            incomingNode.Depth.Should().Be(3);
        }
    }

    [Fact]
    public void OutgoingEdges()
    {
        var edgeSet = new DirectedEdgeSet<int, Edge<int>>();

        var edges = CreateEdges(1, 2, 4).ToArray();
        edgeSet.AddEdges(edges);

        var outgoingEdges = DirectedSearch.Bfs.OutgoingEdges(edgeSet, 1, 2).ToArray();
        
        outgoingEdges.Length.Should().Be(6);
        outgoingEdges[0].Should().Be(newEdge(1, 11));
        outgoingEdges[1].Should().Be(newEdge(1, 12));
        outgoingEdges[2].Should().Be(newEdge(11, 111));
        outgoingEdges[3].Should().Be(newEdge(11, 112));
        outgoingEdges[4].Should().Be(newEdge(12, 121));
        outgoingEdges[5].Should().Be(newEdge(12, 122));

        static Edge<int> newEdge(int source, int target) => new (source, target);
    }

    [Fact]
    public void OutgoingEdgesWithDepthLevel_Should_ReturnNone_When_UsingHeight3()
    {
        var edges = CreateEdges(1, 2, 4).ToArray();
        var edgeSet = new DirectedEdgeSet<int, Edge<int>>();
        edgeSet.AddEdges(edges);

        var outgoingEdges = DirectedSearch.Bfs.OutgoingEdgesWithDepthLevel(edgeSet, 1).ToArray();

        outgoingEdges.Length.Should().Be(14);

        // depth level 1
        {
            var outgoingEdge = outgoingEdges[0];
            outgoingEdge.Depth.Should().Be(1);
        }
        {
            var outgoingEdge = outgoingEdges[1];
            outgoingEdge.Depth.Should().Be(1);
        }

        // depth level 2
        {
            var outgoingEdge = outgoingEdges[2];
            outgoingEdge.Depth.Should().Be(2);
        }
        {
            var outgoingEdge = outgoingEdges[3];
            outgoingEdge.Depth.Should().Be(2);
        }
        {
            var outgoingEdge = outgoingEdges[4];
            outgoingEdge.Depth.Should().Be(2);
        }
        {
            var outgoingEdge = outgoingEdges[5];
            outgoingEdge.Depth.Should().Be(2);
        }

        // depth level 3
        {
            var outgoingEdge = outgoingEdges[6];
            outgoingEdge.Depth.Should().Be(3);
        }
        {
            var outgoingEdge = outgoingEdges[7];
            outgoingEdge.Depth.Should().Be(3);
        }
        {
            var outgoingEdge = outgoingEdges[8];
            outgoingEdge.Depth.Should().Be(3);
        }
        {
            var outgoingEdge = outgoingEdges[9];
            outgoingEdge.Depth.Should().Be(3);
        }
        {
            var outgoingEdge = outgoingEdges[10];
            outgoingEdge.Depth.Should().Be(3);
        }
        {
            var outgoingEdge = outgoingEdges[11];
            outgoingEdge.Depth.Should().Be(3);
        }
        {
            var outgoingEdge = outgoingEdges[12];
            outgoingEdge.Depth.Should().Be(3);
        }
        {
            var outgoingEdge = outgoingEdges[13];
            outgoingEdge.Depth.Should().Be(3);
        }
    }

    private static IEnumerable<Edge<int>> CreateEdges(int numberOfRootNodes, int numberOfChildNodes, int hierarchyLevel)
    {
        var currentLevel = 0;
        for (int i = 1; i <= numberOfRootNodes; i++)
        {
            var childEdges = CreateChildEdges(i, currentLevel, numberOfChildNodes, hierarchyLevel);
            foreach (var edge in childEdges)
                yield return edge;
        }
    }

    private static IEnumerable<Edge<int>> CreateChildEdges(int parentNodeId, int currentLevel, int numberOfChildNodes, int hierarchyLevel)
    {
        ++currentLevel;
        if (currentLevel == hierarchyLevel) yield break;

        for (var i = 1; i <= numberOfChildNodes; i++)
        {
            var nodeId = CreateNodeId(parentNodeId, i);
            yield return new Edge<int>(parentNodeId, nodeId);

            var edges = CreateChildEdges(nodeId, currentLevel, numberOfChildNodes, hierarchyLevel);
            foreach (var edge in edges)
                yield return edge;
        }
    }

    private static int CreateNodeId(int parentNodeId, int childNodeId) => (parentNodeId * 10) + childNodeId;
}
