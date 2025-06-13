using Foundation.Graph.Query;
using Foundation.Graph.Query.Tests;
using Shouldly;

namespace Foundation.Graph.Query.Json;

using Edge = Edge<Id>;
using G = Graph<Id, IdNode<Id, Dictionary<string, object?>>, Edge<Id>, NodeSet<Id, IdNode<Id, Dictionary<string, object?>>>, EdgeSet<Id, Edge<Id>>>;
using Node = IdNode<Id, Dictionary<string, object?>>;

public class JsonGraphQueryTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        var graph = GraphTestUtil.CreateGraph();
        GraphTestUtil.AddNodesAndEdges(graph);

        var client = GraphQueryClient.New<Id, Node, Edge, G>(graph);

        var nodes = client.NewQuery()
                          .V(x => x.Id == Id.New("I1"))
                          .Out((Node x) => true)
                          .Find()
                          .Execute()
                          .ToArray();
        var json =
            """
            {
            	"Method": "Find",
            	"V": {
            		"Id": "I1",
            		"Out": true	
            	}
            }
            """;

        // Act
        var result = client.ExecuteQuery(json, (node, dict) => node.Id == Id.New(dict["Id"].ThrowIfNull()))
                          .ToArray();

        // Assert
        result.Length.ShouldBe(nodes.Length);
        {
            var (id, node) = nodes[0];
            result[0].Value.ShouldBe(node);
        }
        {
            var (id, node) = nodes[1];
            result[1].Value.ShouldBe(node);
        }
    }
}
