using FluentAssertions;

namespace Foundation.Graph;


public class DirectedAdjacencyEdgeSetTests
{

    [Fact]
    public void IncomingNodes_Should_ReturnOnlyIncomingNodes_When_DefiningANode()
    {
        var sut = new DirectedAdjacencyEdgeSet<string, IEdge<string>>();

        sut.AddEdge(Edge.New("a", "b"));
        sut.AddEdge(Edge.New("a", "d"));
        sut.AddEdge(Edge.New("a", "e"));

        sut.AddEdge(Edge.New("b", "c"));
        sut.AddEdge(Edge.New("c", "d"));
        sut.AddEdge(Edge.New("d", "a"));

        var inOfA = sut.IncomingNodes("a").ToArray();

        inOfA.Length.Should().Be(1);
        inOfA.Contains("d");
    }

    [Fact]
    public void OutgoingNodes_Should_ReturnOnlyOutgoingNodes_When_DefiningANode()
    {
        var sut = new DirectedAdjacencyEdgeSet<string, IEdge<string>>();

        sut.AddEdge(Edge.New("a", "b"));
        sut.AddEdge(Edge.New("a", "d"));
        sut.AddEdge(Edge.New("a", "e"));

        sut.AddEdge(Edge.New("b", "c"));
        sut.AddEdge(Edge.New("c", "d"));
        sut.AddEdge(Edge.New("d", "a"));

        var outOfA = sut.OutgoingNodes("a").ToArray();

        outOfA.Length.Should().Be(3);
        outOfA.Contains("b");
        outOfA.Contains("d");
        outOfA.Contains("e");
    }
}
