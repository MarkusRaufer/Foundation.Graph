namespace Foundation.Graph;

using Shouldly;
using System.Linq;
using Xunit;

public class NodeSetTests
{
    private record Node(int Id, string Name);

    [Fact]
    public void GetNodes_Should_Return_ListOfNodes_When_NodesWithIdsExist()
    {
        var sut = new NodeSet<int, Node>();

        sut.AddNode(1, new Node(1, "one"));
        sut.AddNode(2, new Node(2, "two"));
        sut.AddNode(3, new Node(3, "three"));
        sut.AddNode(4, new Node(4, "four"));
        sut.AddNode(5, new Node(5, "five"));

        var nodes = sut.GetNodes([1, 3, 5]).ToArray();
        nodes.Length.ShouldBe(3);
        {
            var node = nodes[0];
            node.Id.ShouldBe(1);
            node.Name.ShouldBe("one");
        }
        {
            var node = nodes[1];
            node.Id.ShouldBe(3);
            node.Name.ShouldBe("three");
        }
        {
            var node = nodes[2];
            node.Id.ShouldBe(5);
            node.Name.ShouldBe("five");
        }
    }
}