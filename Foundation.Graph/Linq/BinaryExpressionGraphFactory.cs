// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using Foundation.Linq.Expressions;
using System.Linq.Expressions;

namespace Foundation.Graph.Linq;

public class BinaryExpressionGraphFactory : ExpressionVisitor
{
    private readonly DirectedGraph<Expression, IEdge<Expression>> _graph = new (new NodeSet<Expression>(new List<Expression>()), new DirectedEdgeSet<Expression, IEdge<Expression>>());
    private ParameterExpression? _parameter;

    public IDirectedGraph<Expression, IEdge<Expression>> CreateGraph(Expression expression)
    {
        expression.ThrowIfNull();

        _graph.Clear();

        base.Visit(expression);

        return _graph;
    }

    public IDirectedGraph<Expression, IEdge<Expression>> CreateGraph(Expression expression, ParameterExpression parameter)
    {
        expression.ThrowIfNull();
        _parameter = parameter.ThrowIfNull();

        _graph.Clear();

        base.Visit(expression);

        return _graph;
    }


    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (_parameter is not null && !node.GetParameters().Any(x => x.EqualsToExpression(_parameter)))
        {
            return base.VisitBinary(node);
        }

        if (!_graph.ExistsNode(node)) _graph.AddNode(node);

        Expression? leftAdded = null;
        if (node.Left is BinaryExpression binaryLeft)
        {
            if (_parameter is not null && !binaryLeft.GetParameters().Any(x => x.EqualsToExpression(_parameter)))
            {
                return base.VisitBinary(node);
            }
            
            if (!_graph.ExistsNode(binaryLeft))
            {
                leftAdded = node.Left;
                _graph.AddNode(leftAdded);
            }
        }
        else
        {
            if (node.Left is ParameterExpression paramLeft)
            {
                leftAdded = IdExpression.New(Guid.NewGuid(), paramLeft);
                _graph.AddNode(leftAdded);
            }
            else
            {
                leftAdded = node.Left;
                _graph.AddNode(leftAdded);
            }
        }

        if (leftAdded is not null && !_graph.ExistsEdge(node, leftAdded)) _graph.AddEdge(Edge.New(node, leftAdded));


        Expression? rightAdded = null;
        if (node.Right is BinaryExpression binaryRight)
        {
            if (_parameter is not null && !binaryRight.GetParameters().Any(x => x.EqualsToExpression(_parameter)))
            {
                return base.VisitBinary(node);
            }

            if (!_graph.ExistsNode(binaryRight))
            {
                rightAdded = node.Right;
                _graph.AddNode(rightAdded);
            }
        }
        else
        {
            if (node.Right is ParameterExpression paramRight)
            {
                rightAdded = IdExpression.New(Guid.NewGuid(), paramRight);
                _graph.AddNode(rightAdded);
            }
            else
            {
                rightAdded = node.Right;
                _graph.AddNode(rightAdded);
            }
        }

        if (rightAdded is not null && !_graph.ExistsEdge(node, rightAdded)) _graph.AddEdge(Edge.New(node, rightAdded));

        return base.VisitBinary(node);
    }
}
