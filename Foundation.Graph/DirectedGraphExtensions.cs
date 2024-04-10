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
﻿using Foundation.Collections.Generic;
using System.Xml.Linq;

namespace Foundation.Graph;

public static class DirectedGraphExtensions
{
    public static void AddSubGraph<TNodeId, TNode, TEdgeId, TEdge>(
        this IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge> graph,
        IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge> subGraph)
        where TEdge : IEdge<TEdgeId, TNodeId>
        where TNodeId : notnull
    {
        graph.ThrowIfNull();
        subGraph.ThrowIfNull();

        foreach (var edge in subGraph.Edges)
        {
            if (!graph.GetNode(edge.Source).TryGet(out var sourceNode))
            {
                if (subGraph.GetNode(edge.Source).TryGet(out sourceNode)) graph.AddNode(edge.Source, sourceNode);
            }

            if (!graph.GetNode(edge.Target).TryGet(out var targetNode))
            {
                if (subGraph.GetNode(edge.Target).TryGet(out targetNode)) graph.AddNode(edge.Target, targetNode);
            }

            if (!graph.OutgoingEdges(edge.Source).Any(x => x.Target.Equals(edge.Target))) graph.AddEdge(edge);
        }
    }

    public static void GetSubGraph<TNodeId, TNode, TEdgeId, TEdge, TGraph>(this TGraph graph, TGraph subGraph, TNodeId nodeId)
        where TGraph : IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge>
        where TEdge : IEdge<TEdgeId, TNodeId>
        where TEdgeId : notnull
        where TNodeId : notnull
    {
        subGraph.AddEdges(graph.OutgoingEdges(nodeId));

        var nodeTuples = subGraph.Edges.SelectMany(x => x.GetNodes())
                                       .Distinct()
                                       .FilterMap<TNodeId, (TNodeId nodeId, TNode node)>(nodeId =>
                                       {
                                           if (!graph.GetNode(nodeId).TryGet(out var node)) return Option.None<(TNodeId, TNode)>();

                                           return Option.Some((nodeId, node));
                                       });
                                      
        subGraph.AddNodes(nodeTuples);
    }

    public static void RemoveSubGraph<TNode, TEdge>(
        this IDirectedGraph<TNode, TEdge> graph,
        IDirectedGraph<TNode, TEdge> subGraph)
            where TEdge : IEdge<TNode>
            where TNode : notnull
    {
        graph.ThrowIfNull();
        subGraph.ThrowIfNull();

        foreach (var edge in subGraph.Edges)
        {
            graph.RemoveEdge(edge);
        }

        graph.RemoveNodes(subGraph.Nodes);
    }

    public static void RemoveSubGraph<TNodeId, TNode, TEdgeId, TEdge>(
        this IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge> graph,
        IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge> subGraph,
        Func<TNode, TNodeId> nodeIdSelector)
            where TEdge : IEdge<TEdgeId, TNodeId>
            where TEdgeId : notnull
            where TNodeId : notnull
    {
        graph.ThrowIfNull();
        subGraph.ThrowIfNull();
        nodeIdSelector.ThrowIfNull();

        foreach (var edge in subGraph.Edges)
        {
            graph.RemoveEdge(edge.Id);
        }

        var nodeIds = subGraph.Nodes.Select(nodeIdSelector);
        graph.RemoveNodes(nodeIds);
    }

    public static void ReplaceSubGraph<TNodeId, TNode, TEdgeId, TEdge>(
        this IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge> graph,
        IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge> subGraph,
        bool addNodeIfNotExists)
        where TEdge : IEdge<TEdgeId, TNodeId>
        where TNodeId : notnull
    {
        graph.ThrowIfNull();
        subGraph.ThrowIfNull();

        var subGraphEdges = subGraph.Edges.ToQueue();
        
        while (0 < subGraphEdges.Count)
        {
            var subGraphEdge = subGraphEdges.Dequeue();
            var sourceExists = graph.GetNode(subGraphEdge.Source).TryGet(out var sourceNode);
            var subGraphSourceExists = subGraph.GetNode(subGraphEdge.Source).TryGet(out var sourceReplacement);

            if (sourceExists)
            {
                if (subGraphSourceExists)
                {
                    graph.RemoveNode(subGraphEdge.Source);
                    graph.AddNode(subGraphEdge.Source, sourceReplacement!);
                }
            }
            else
            {
                if (subGraphSourceExists && addNodeIfNotExists) graph.AddNode(subGraphEdge.Source, sourceReplacement!);
            }

            var targetExists = graph.GetNode(subGraphEdge.Target).TryGet(out var targetNode);
            var subGraphTargetExists = subGraph.GetNode(subGraphEdge.Target).TryGet(out var targetReplacement);

            if (!targetExists)
            {
                if (subGraphTargetExists)
                {
                    graph.RemoveNode(subGraphEdge.Target);
                    graph.AddNode(subGraphEdge.Target, targetReplacement!);
                }
            }
            else
            {
                if (subGraphTargetExists && addNodeIfNotExists) graph.AddNode(subGraphEdge.Target, targetReplacement!);
            }

            var edges = graph.OutgoingEdges(subGraphEdge.Source).Where(x => x.Target.Equals(subGraphEdge.Target)).ToQueue();
            while(0 < edges.Count)
            {
                var edge = edges.Dequeue();
                graph.RemoveEdge(edge);
            }
            
            if (!graph.OutgoingEdges(subGraphEdge.Source).Any(x => x.Target.Equals(subGraphEdge.Target))) graph.AddEdge(subGraphEdge);
        }
    }
}
