namespace Foundation.Graph;

using NUnit.Framework;
using System.Linq;

public class DirectedAdjacencyEdgeSetTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
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
        Assert.AreEqual(1, inOfA.Length);
        Assert.AreEqual("d", inOfA[0]);
    }

    [Test]
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
        Assert.AreEqual(3, outOfA.Length);
        CollectionAssert.Contains(outOfA, "b");
        CollectionAssert.Contains(outOfA, "d");
        CollectionAssert.Contains(outOfA, "e");
    }
}
