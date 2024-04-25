using FluentAssertions;
using Foundation.Graph.Linq;
using Foundation.Linq.Expressions;
using System.Linq.Expressions;

namespace Foundation.Graph.Tests.Linq;

public class BinaryExpressionGraphFactoryTests
{
    private record Person(string Name, DateTime Birthday);

    [Fact]
    public void CreateGraph_Should_ReturnGraph_When_LambdaHas2ParametersAndBinaryExpressionsAreConnectedWithAndAndOr()
    {
        LambdaExpression lambda = (string str, int x) => str == "A" && x == 3 || x > 10;
        var sut = new BinaryExpressionGraphFactory();

        var graph = sut.CreateGraph(lambda);

        var nodes = graph.Nodes.ToArray();
        nodes.Length.Should().Be(11);
        graph.NodeCount.Should().Be(11);

        var edges = graph.Edges.ToArray();
        edges.Length.Should().Be(10);
        graph.EdgeCount.Should().Be(10);
    }

    [Fact]
    public void CreateGraph_WithParameter_Should_ReturnGraph_When_LambdaHas2ParametersAndBinaryExpressionsAreConnectedWithAndAndOr()
    {
        LambdaExpression lambda = (string str, int x) => str == "A" && x == 3 || x > 10;
        var sut = new BinaryExpressionGraphFactory();

        var p = Expression.Parameter(typeof(int), "x");
        var graph = sut.CreateGraph(lambda, p);

        var nodes = graph.Nodes.ToArray();
        nodes.Length.Should().Be(8);
        graph.NodeCount.Should().Be(8);

        var edges = graph.Edges.ToArray();
        edges.Length.Should().Be(6);
        graph.EdgeCount.Should().Be(6);
    }

    [Fact]
    public void CreateGraph_Should_ReturnGraph_When_LambdaHas2ParametersAndAllBinaryExpressionsAreConnectedWithOr()
    {
        LambdaExpression lambda = (string str, int x) => str == "A" && x == 3 || str == "B" && x == 5;
        var sut = new BinaryExpressionGraphFactory();

        var graph = sut.CreateGraph(lambda);

        var nodes = graph.Nodes.ToArray();
        nodes.Length.Should().Be(15);
        graph.NodeCount.Should().Be(15);

        var edges = graph.Edges.ToArray();
        edges.Length.Should().Be(14);
        graph.EdgeCount.Should().Be(14);
    }

    [Fact]
    public void CreateGraph_Should_ReturnGraph_When_LambdaHas1Parameter()
    {
        LambdaExpression lambda = (DateTime x) => x == new DateTime(2010,1, 10) || x == new DateTime(2020, 1, 12) || x == new DateTime(2020, 1, 14);
        var sut = new BinaryExpressionGraphFactory();

        var graph = sut.CreateGraph(lambda);

        var nodes = graph.Nodes.ToArray();
        nodes.Length.Should().Be(11);
        graph.NodeCount.Should().Be(11);
        
        var edges = graph.Edges.ToArray();
        edges.Length.Should().Be(10);
        graph.EdgeCount.Should().Be(10);
    }

    [Fact]
    public void CreateGraph_Should_ReturnGraph_When_LambdaHas2ParametersAndBinaryExpressionsAreConnectedWithOrAndAnd()
    {
        LambdaExpression lambda = (Person p, DateTime x) => x == new DateTime(2010, 1, 10) || x == new DateTime(2020, 1, 12) || (p.Name == "Bob" && (x.Year < DateTime.Now.Year - 18));
        var sut = new BinaryExpressionGraphFactory();

        var graph = sut.CreateGraph(lambda);

        var nodes = graph.Nodes.ToArray();
        nodes.Length.Should().Be(17);
        graph.NodeCount.Should().Be(17);

        var edges = graph.Edges.ToArray();
        edges.Length.Should().Be(16);
        graph.EdgeCount.Should().Be(16);
    }
}
