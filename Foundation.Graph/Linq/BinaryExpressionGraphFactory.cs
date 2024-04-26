//// The MIT License (MIT)
////
//// Copyright (c) 2020 Markus Raufer
////
//// All rights reserved.
////
//// Permission is hereby granted, free of charge, to any person obtaining a copy
//// of this software and associated documentation files (the "Software"), to deal
//// in the Software without restriction, including without limitation the rights
//// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//// copies of the Software, and to permit persons to whom the Software is
//// furnished to do so, subject to the following conditions:
////
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
////
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//// SOFTWARE.
//using Foundation.Linq.Expressions;
//using System.Data.Common;
//using System.Linq.Expressions;
//using System.Reflection.Metadata;
//using System.Xml.Linq;

//namespace Foundation.Graph.Linq;

//public class BinaryExpressionGraphFactory : ExpressionVisitor
//{
//    private readonly DirectedGraph<Expression, IEdge<Expression>> _graph = new (new NodeSet<Expression>(new List<Expression>()),
//                                                                                new DirectedEdgeSet<Expression, IEdge<Expression>>(),
//                                                                                (source, target) => Edge.New(source, target));

//    private ParameterExpression? _parameter;
//    private Queue<BinaryExpression> _nodes = new();

//    private ConstantExpression CreateBooleanConstant(bool value) => Expression.Constant(value);
    
//    public IDirectedGraph<Expression, IEdge<Expression>> CreateGraph(Expression expression)
//    {
//        expression.ThrowIfNull();

//        _graph.Clear();
//        base.Visit(expression);

//        return _graph;
//    }

//    public IDirectedGraph<Expression, IEdge<Expression>> CreateGraph(Expression expression, ParameterExpression parameter)
//    {
//        expression.ThrowIfNull();
//        _parameter = parameter.ThrowIfNull();

//        _graph.Clear();
//        if (!expression.GetParameters().Any(x => x.EqualsToExpression(parameter))) return _graph;

//        base.Visit(expression);

//        return _graph;
//    }

//    public IDirectedGraph<Expression, IEdge<Expression>> CreateGraph(LambdaExpression expression, ParameterExpression parameter)
//    {
//        expression.ThrowIfNull();
//        _parameter = parameter.ThrowIfNull();

//        _graph.Clear();
//        if (!expression.Parameters.Any(x => x.EqualsToExpression(parameter))) return _graph;

//        base.Visit(expression);

//        return _graph;
//    }

//    protected override Expression VisitBinary(BinaryExpression node)
//    {
//        var binary = base.VisitBinary(node);

//        if(node.IsTerminalBinary())
//        {
//            return binary;
//        }

//        //if (!_graph.ExistsNode(node)) _graph.AddNode(node);

//        //Expression? leftAdded = null;
//        //Expression? rightAdded = null;

//        //if (null == _parameter)
//        //{
//        //    if (!_graph.ExistsNode(node.Left))
//        //    {
//        //        leftAdded = NormalizeExpression(node.Left);

//        //        _graph.AddNode(leftAdded);
//        //    }

//        //    if (leftAdded is not null && !_graph.ExistsEdge(node, leftAdded)) _graph.AddEdge(Edge.New(node, leftAdded));

//        //    if (!_graph.ExistsNode(node.Right))
//        //    {
//        //        rightAdded = NormalizeExpression(node.Right);
//        //        _graph.AddNode(rightAdded);
//        //    }

//        //    if (rightAdded is not null && !_graph.ExistsEdge(node, rightAdded)) _graph.AddEdge(Edge.New(node, rightAdded));

//        //    return base.VisitBinary(node);
//        //}

//        //if (!node.Left.HasParameter(_parameter))
//        //{
//        //    if (!node.IsTerminalBinary()) _nodes.Enqueue(node);

//        //    return base.VisitBinary(node);
//        //}

//        //leftAdded = NormalizeExpression(node.Left);
//        //if (!_graph.ExistsEdge(node, node.Left)) _graph.AddEdge(Edge.New(node, leftAdded));


//        //if (!node.Right.HasParameter(_parameter))
//        //{
//        //    if (!node.IsTerminalBinary()) _nodes.Enqueue(node);

//        //    if (node.Right is rightParameter )

//        //    return base.VisitBinary(node);
//        //}

//        //rightAdded = NormalizeExpression(node.Right);
//        //if (!_graph.ExistsEdge(node, node.Right)) _graph.AddEdge(Edge.New(node, rightAdded));


//        //return base.VisitBinary(node);
//    }

//    private static Expression NormalizeExpression(Expression expression)
//    {
//        return expression is ParameterExpression parameter ? IdExpression.New(parameter) : expression;
//    }
//}
