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
﻿namespace Foundation.Graph;

using Foundation.Collections.Generic;
using Foundation.Graph.Algorithm;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public static class DirectedGraphFactory
{
    public static TGraph CreateSubGraphFromOutgoingEdges<TGraph, TNode, TEdge>(
        this TGraph graph,
        Func<TGraph> factory,
        [DisallowNull] TNode node,
        Func<TEdge, bool>? edgePredicate = null)
        where TEdge : IEdge<TNode>
        where TGraph : IDirectedGraph<TNode, TEdge>
    {
        var subGraph = factory();

        subGraph.AddNode(node);

        //TODO: int.MaxValue
        var outEdges = DirectedSearch.Bfs.OutgoingEdges(graph, node, int.MaxValue, edgePredicate);
        var outNodes = outEdges.SelectMany(edge => edge.GetNodes()).Ignore(node);
        subGraph.AddNodes(outNodes);
        subGraph.AddEdges(outEdges);

        return subGraph;
    }

    public static TGraph CreateSubGraphFromOutgoingEdges<TGraph, TNode, TEdge>(
        this TGraph graph,
        Func<TGraph> factory,
        Func<TNode, bool>? nodePredicate = null,
        Func<TEdge, bool>? edgePredicate = null)
        where TEdge : IEdge<TNode>
        where TGraph : IDirectedGraph<TNode, TEdge>
    {
        var nodes = null == nodePredicate ? graph.Nodes : graph.Nodes.Where(nodePredicate).ToList();

        return CreateSubGraphFromOutgoingEdges(graph, factory, nodes, edgePredicate);
    }

    public static TGraph CreateSubGraphFromOutgoingEdges<TGraph, TNode, TEdge>(
        this TGraph graph,
        Func<TGraph> factory,
        IEnumerable<TNode> nodes,
        Func<TEdge, bool>? edgePredicate = null)
        where TEdge : IEdge<TNode>
        where TGraph : IDirectedGraph<TNode, TEdge>
    {
        var subGraph = factory();

        foreach (var node in nodes)
        {
            if (null == node) continue;

            subGraph.AddNode(node);
            var outEdges = DirectedSearch.Bfs.OutgoingEdges(graph, node, int.MaxValue, edgePredicate);
            var outNodes = outEdges.SelectMany(edge => edge.GetNodes()).Except(nodes);
            subGraph.AddNodes(outNodes);
            subGraph.AddEdges(outEdges);
        }

        return subGraph;
    }
}

