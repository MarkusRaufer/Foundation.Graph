namespace Foundation.Graph;

using Foundation.Collections;
using NUnit.Framework;
using System.Collections.Specialized;
using System.Linq;

public class EdgeSetTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Ctor_ShouldBeEmpty_When_Allocated()
    {
        var sut = new EdgeSet<string, IEdge<string>>();
        
        Assert.AreEqual(0, sut.EdgeCount);
    }

    [Test]
    public void AddEdge_Should_HaveOneEdge_When_IfItWasEmptyBefore()
    {
        var sut = new EdgeSet<string, IEdge<string>>();
        
        sut.AddEdge(Edge.New("a", "b"));
        Assert.AreEqual(1, sut.EdgeCount);

        var edge = sut.Edges.Single();
        Assert.AreEqual("a", edge.Source);
        Assert.AreEqual("b", edge.Target);
    }

    [Test]
    public void CollectionChanged_Should_ThrowEvent_When_AddingAnEdge()
    {
        var sut = new EdgeSet<string, IEdge<string>>();

        var calledCollectionChanged = false;

        var expectedEdge = Edge.New("a", "b");

        void onCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            calledCollectionChanged = true;
            Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
            if (null == e.NewItems || 0 == e.NewItems.Count)
            {
                Assert.Fail("NewItems of NotifyCollectionChangedEventArgs was empty");
                return;
            }

            var newEdge = e.NewItems.CastTo<IEdge<string>>().Single();

            Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
            Assert.AreEqual(expectedEdge, newEdge);
        }

        sut.CollectionChanged += onCollectionChanged;

        sut.AddEdge(expectedEdge);

        Assert.IsTrue(calledCollectionChanged);
    }

    private void Sut_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    [Test]
    public void RemoveEdge_Should_BeEmpty_When_RemovedLastEdgeWithIdentifier()
    {
        var sut = new EdgeSet<string, int, IEdge<int, string>>();

        var edge = Edge.New(3, "a", "b");
        sut.AddEdge(edge);

        var removed = sut.RemoveEdge(3);
        Assert.IsTrue(removed);

        Assert.AreEqual(0, sut.EdgeCount);
    }

    [Test]
    public void RemoveEdge_Should_BeEmpty_When_RemovedLastEdgeWithoutIdentifier()
    {
        var sut = new EdgeSet<string, IEdge<string>>();

        var edge = Edge.New("a", "b");
        sut.AddEdge(edge);

        var removed = sut.RemoveEdge(edge);
        Assert.IsTrue(removed);

        Assert.AreEqual(0, sut.EdgeCount);
    }
}
