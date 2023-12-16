using FluentAssertions;
using Foundation.Graph.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Graph.Tests.Algorithm;

public class DirectedSearchTests
{
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
