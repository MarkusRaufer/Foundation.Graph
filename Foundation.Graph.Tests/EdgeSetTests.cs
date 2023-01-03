namespace Foundation.Graph;

using Foundation.Collections;
using System.Collections.Specialized;
using System.Linq;
using Xunit;

public class EdgeSetTests
{
    [Fact]
    public void Ctor_ShouldBeEmpty_When_Allocated()
    {
        var sut = new EdgeSet<string, IEdge<string>>();
        
        Assert.Equal(0, sut.EdgeCount);
    }

    [Fact]
    public void AddEdge_Should_HaveOneEdge_When_IfItWasEmptyBefore()
    {
        var sut = new EdgeSet<string, IEdge<string>>();
        
        sut.AddEdge(Edge.New("a", "b"));
        Assert.Equal(1, sut.EdgeCount);

        var edge = sut.Edges.Single();
        Assert.Equal("a", edge.Source);
        Assert.Equal("b", edge.Target);
    }

    [Fact]
    public void CollectionChanged_Should_ThrowEvent_When_AddingAnEdge()
    {
        var sut = new EdgeSet<string, IEdge<string>>();

        var calledCollectionChanged = false;

        var expectedEdge = Edge.New("a", "b");

        void onCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            calledCollectionChanged = true;
            Assert.Equal(NotifyCollectionChangedAction.Add, e.Action);
            if (null == e.NewItems || 0 == e.NewItems.Count)
            {
                Assert.Fail("NewItems of NotifyCollectionChangedEventArgs was empty");
                return;
            }

            var newEdge = e.NewItems.CastTo<IEdge<string>>().Single();

            Assert.Equal(NotifyCollectionChangedAction.Add, e.Action);
            Assert.Equal(expectedEdge, newEdge);
        }

        sut.CollectionChanged += onCollectionChanged;

        sut.AddEdge(expectedEdge);

        Assert.True(calledCollectionChanged);
    }

    private void Sut_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    [Fact]
    public void RemoveEdge_Should_BeEmpty_When_RemovedLastEdgeWithIdentifier()
    {
        var sut = new EdgeSet<string, int, IEdge<int, string>>();

        var edge = Edge.New(3, "a", "b");
        sut.AddEdge(edge);

        var removed = sut.RemoveEdge(3);
        Assert.True(removed);

        Assert.Equal(0, sut.EdgeCount);
    }

    [Fact]
    public void RemoveEdge_Should_BeEmpty_When_RemovedLastEdgeWithoutIdentifier()
    {
        var sut = new EdgeSet<string, IEdge<string>>();

        var edge = Edge.New("a", "b");
        sut.AddEdge(edge);

        var removed = sut.RemoveEdge(edge);
        Assert.True(removed);

        Assert.Equal(0, sut.EdgeCount);
    }
}
