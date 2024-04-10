using FluentAssertions;
using Foundation.Graph.Algorithm;
using Foundation.Graph.Linq;
using Foundation.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Graph.Tests.Linq;

public class ExpressionGraphFactoryTests
{
    [Fact]
    public void Test()
    {
        var sut = new BinaryExpressionGraphFactory();

        LambdaExpression lambda = (DateTime dt, string x, string y) => x == "A" && dt > new DateTime(2010,1, 10) || y == "B" && dt < new DateTime(2020, 1, 20);

        var dtParameter = lambda.Parameters.First();
        var terminalExpressions = lambda.GetTerminalBinaryExpressions()
                                        .Where(x => x.GetParameters().Any(x => x.Name == dtParameter.Name))
                                        .ToArray();

        var graph = sut.CreateGraph(lambda);

        graph.NodeCount.Should().Be(7);
        graph.EdgeCount.Should().Be(6);

        var nodes = graph.Nodes.ToArray();
        {
            //BinaryExpression be = (DateTime dt, string x) => x == "A" && dt > new DateTime(2010, 1, 10);
            var expression = nodes[0];
            nodes[0].EqualsToExpression(lambda);
        }
        
    }
}
