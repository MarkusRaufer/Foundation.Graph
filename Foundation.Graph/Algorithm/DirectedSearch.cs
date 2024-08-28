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
﻿using Foundation;
using Foundation.Collections.Generic;

namespace Foundation.Graph.Algorithm;

public static class DirectedSearch
{
    /// <summary>
    /// Breadth first search.
    /// </summary>
    public static class Bfs
    {
        private record NodeDepth<TNode>(TNode Node, int SearchDepth);

        public static IEnumerable<TEdge> IncomingEdges<TNode, TEdge>(
          IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
          TNode node,
          int searchDepth = int.MaxValue,
          Func<TEdge, bool>? predicate = null,
          Func<TNode, bool>? stopPredicate = null)
          where TEdge : IEdge<TNode>
        {
            var nodes = new Queue<NodeDepth<TNode>>();
            nodes.Enqueue(new NodeDepth<TNode>(node, 1));

            var visitedEdges = new HashSet<TEdge>();
            var visitedNodes = new HashSet<TNode>();
            while (0 < nodes.Count)
            {
                var n = nodes.Dequeue();

                if (n.SearchDepth == searchDepth) yield break;

                if (null != stopPredicate && stopPredicate(n.Node))
                    yield break;

                if (visitedNodes.Contains(n.Node))
                    continue;

                visitedNodes.Add(n.Node);
                var inEdges = (null == predicate)
                    ? edgeSet.IncomingEdges(n.Node).Except(visitedEdges)
                    : edgeSet.IncomingEdges(n.Node).Where(predicate).Except(visitedEdges);

                foreach (var inEdge in inEdges)
                {
                    yield return inEdge;
                    visitedEdges.Add(inEdge);
                    nodes.Enqueue(new NodeDepth<TNode>(inEdge.Source, n.SearchDepth + 1));
                }
            }
        }

        public static IEnumerable<EdgeWithDepthLevel<TEdge, TNode>> IncomingEdgesWithDepthLevel<TNode, TEdge>(
          IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
          TNode node,
          int searchDepth = int.MaxValue,
          Func<TEdge, bool>? predicate = null,
          Func<TNode, bool>? stopPredicate = null)
          where TEdge : IEdge<TNode>
        {
            var nodes = new Queue<NodeDepth<TNode>>();
            nodes.Enqueue(new NodeDepth<TNode>(node, 1));

            var visitedEdges = new HashSet<TEdge>();
            var visitedNodes = new HashSet<TNode>();
            while (0 < nodes.Count)
            {
                var n = nodes.Dequeue();

                if (n.SearchDepth == searchDepth) yield break;

                if (null != stopPredicate && stopPredicate(n.Node))
                    yield break;

                if (visitedNodes.Contains(n.Node))
                    continue;

                visitedNodes.Add(n.Node);
                var inEdges = (null == predicate)
                    ? edgeSet.IncomingEdges(n.Node).Except(visitedEdges)
                    : edgeSet.IncomingEdges(n.Node).Where(predicate).Except(visitedEdges);

                foreach (var inEdge in inEdges)
                {
                    yield return new EdgeWithDepthLevel<TEdge, TNode>(inEdge, n.SearchDepth);
                    visitedEdges.Add(inEdge);
                    nodes.Enqueue(new NodeDepth<TNode>(inEdge.Source, n.SearchDepth + 1));
                }
            }
        }

        public static IEnumerable<TNode> IncomingNodes<TNode, TEdge>(
            IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            int searchDepth = int.MaxValue,
            Func<TEdge, bool>? predicate = null,
            Func<TNode, bool>? stopPredicate = null)
            where TEdge : IEdge<TNode>
        {
            return IncomingEdges(edgeSet, node, searchDepth, predicate, stopPredicate).Select(e => e.Source);
        }

        public static IEnumerable<NodeWithDepthLevel<TNode>> IncomingNodesWithDepthLevel<TNode, TEdge>(
            IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            int searchDepth = int.MaxValue,
            Func<TEdge, bool>? predicate = null,
            Func<TNode, bool>? stopPredicate = null)
            where TEdge : IEdge<TNode>
        {
            return IncomingEdgesWithDepthLevel(edgeSet, node, searchDepth, predicate, stopPredicate).Select(e => new NodeWithDepthLevel<TNode>(e.Edge.Source, e.Depth));
        }

        public static IEnumerable<TEdge> OutgoingEdges<TNode, TEdge>(
            IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            int searchDepth = int.MaxValue,
            Func<TEdge, bool>? predicate = null,
            Func<TNode, bool>? stopPredicate = null)
            where TEdge : IEdge<TNode>
        {
            var nodes = new Queue<NodeDepth<TNode>>();
            nodes.Enqueue(new NodeDepth<TNode>(node, 0));

            var visitedEdges = new HashSet<TEdge>();
            var visitedNodes = new HashSet<TNode>();

            while (0 < nodes.Count)
            {
                var n = nodes.Dequeue();

                if (n.SearchDepth == searchDepth) yield break;

                if (null != stopPredicate && stopPredicate(n.Node)) yield break;

                if (visitedNodes.Contains(n.Node))
                    continue;

                visitedNodes.Add(n.Node);

                var outEdges = (null == predicate)
                    ? edgeSet.OutgoingEdges(n.Node).Except(visitedEdges)
                    : edgeSet.OutgoingEdges(n.Node).Where(predicate).Except(visitedEdges);

                foreach (var outEdge in outEdges)
                {
                    yield return outEdge;
                    visitedEdges.Add(outEdge);
                    nodes.Enqueue(new NodeDepth<TNode>(outEdge.Target, n.SearchDepth + 1));
                }
            }
        }

        public static IEnumerable<EdgeWithDepthLevel<TEdge, TNode>> OutgoingEdgesWithDepthLevel<TNode, TEdge>(
            IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            int searchDepth = int.MaxValue,
            Func<TEdge, bool>? predicate = null,
            Func<TNode, bool>? stopPredicate = null)
            where TEdge : IEdge<TNode>
        {
            var nodes = new Queue<NodeDepth<TNode>>();
            nodes.Enqueue(new NodeDepth<TNode>(node, 1));

            var visitedEdges = new HashSet<TEdge>();
            var visitedNodes = new HashSet<TNode>();

            while (0 < nodes.Count)
            {
                var n = nodes.Dequeue();

                if (n.SearchDepth == searchDepth) yield break;

                if (null != stopPredicate && stopPredicate(n.Node)) yield break;

                if (visitedNodes.Contains(n.Node))
                    continue;

                visitedNodes.Add(n.Node);

                var outEdges = (null == predicate)
                    ? edgeSet.OutgoingEdges(n.Node).Except(visitedEdges)
                    : edgeSet.OutgoingEdges(n.Node).Where(predicate).Except(visitedEdges);

                foreach (var outEdge in outEdges)
                {
                    yield return new EdgeWithDepthLevel<TEdge, TNode>(outEdge, n.SearchDepth);
                    visitedEdges.Add(outEdge);
                    nodes.Enqueue(new NodeDepth<TNode>(outEdge.Target, n.SearchDepth + 1));
                }
            }
        }

        public static IEnumerable<TNode> OutgoingNodes<TNode, TEdge>(
            IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            int searchDepth = int.MaxValue,
            Func<TEdge, bool>? predicate = null,
            Func<TNode, bool>? stopPredicate = null)
            where TEdge : IEdge<TNode>
        {
            // TODO: int.MaxValue
            return OutgoingEdges(edgeSet, node, searchDepth, predicate, stopPredicate).Select(e => e.Target);
        }

        public static IEnumerable<NodeWithDepthLevel<TNode>> OutgoingNodesWithDepthLevel<TNode, TEdge>(
            IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            int searchDepth = int.MaxValue,
            Func<TEdge, bool>? predicate = null,
            Func<TNode, bool>? stopPredicate = null)
            where TEdge : IEdge<TNode>
        {
            // TODO: int.MaxValue
            return OutgoingEdgesWithDepthLevel(edgeSet, node, searchDepth, predicate, stopPredicate).Select(e => new NodeWithDepthLevel<TNode>(e.Edge.Target, e.Depth));
        }

        public static Option<TNode> CommonParent<TNode, TEdge>(IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet, IEnumerable<TNode> nodes)
            where TEdge : IEdge<TNode>
        {
            edgeSet.ThrowIfNull(nameof(edgeSet));
            nodes.ThrowIfNull(nameof(nodes));

            if (!nodes.FirstAsOption().TryGet(out var first)) return Option.None<TNode>();

#if NETSTANDARD2_0
            var incomingNodesOfFirst = new HashSet<TNode>(IncomingNodes(edgeSet!, first));
#else
            var incomingNodesOfFirst = IncomingNodes(edgeSet!, first).ToHashSet();
#endif

            var incomingNodes = nodes.Ignore(first)
                                     .Select(x => IncomingNodes(edgeSet!, x))
                                     .ToArray();

            var enumerators = incomingNodes.Select(x => x.GetEnumerator()).ToArray();
            var queuedEnumerators = enumerators.ToQueue();

            var parent = Option.None<TNode>();

            while (queuedEnumerators.Count > 0)
            {
                var it = queuedEnumerators.Dequeue();
                var foundParent = findSameParent(it!);
                if (foundParent.IsNone) return Option.None<TNode>();

                if (parent.IsNone)
                {
                    parent = foundParent;
                    continue;
                }

                if (foundParent == parent) continue;

                parent = foundParent;

                queuedEnumerators = enumerators.Ignore(it).ToQueue();
            }

            return parent;

            Option<TNode> findSameParent(IEnumerator<TNode> enumerator)
            {
                while (enumerator.MoveNext())
                {
                    if (incomingNodesOfFirst.Contains(enumerator.Current)) return Option.Some(enumerator.Current);
                }

                return Option.None<TNode>();
            }
        }

        public static IEnumerable<TNode> Neighbors<TNode, TEdge>(IDirectedGraph<TNode, TEdge> graph, TNode node)
            where TEdge : IEdge<TNode>
        {
            return Bfs.IncomingNodes(graph, node)
                      .Concat(graph.OutgoingNodes(node))
                      .Ignore(node)
                      .Distinct();
        }

        public static Option<TParent> Parent<TNode, TEdge, TParent>(IDirectedGraph<TNode, TEdge> graph, TNode node)
                    where TEdge : IEdge<TNode>
        {
            return Bfs.IncomingNodes(graph, node)
                      .OfType<TParent>()
                      .FirstAsOption();
        }

        public static Option<TParent> Parent<TNode, TEdge, TParent>(
                    IDirectedGraph<TNode, TEdge> graph,
                    TNode node,
                    Func<TNode, bool> stopPredicate,
                    int searchDepth = int.MaxValue)
                    where TEdge : IEdge<TNode>
        {
            return Bfs.IncomingNodes(graph, node, searchDepth, null, stopPredicate)
                      .OfType<TParent>()
                      .FirstAsOption();
        }
    }

    public static IEnumerable<TNode> RootNodes<TNode, TEdge>(
        IDirectedGraph<TNode, TEdge> graph)
        where TEdge : IEdge<TNode>
    {
        return graph.Nodes.Except(graph.Edges.Select(e => e.Target));
    }

    public static IEnumerable<TNode> RootNodes<TNodeId, TNode, TEdge>(
        IDirectedGraph<TNodeId, TNode, TEdge> graph, Func<TNode, TNodeId> selector)
        where TEdge : IEdge<TNodeId>
        where TNodeId : notnull
    {
        var rootNodeIds = graph.Nodes.Select(selector).Except(graph.Edges.Select(e => e.Target));
        return rootNodeIds.Select(graph.GetNode).SelectSome();
    }

    public static IEnumerable<TNode> RootNodes<TNodeId, TNode, TEdgeId, TEdge>(
        IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge> graph, Func<TNode, TNodeId> selector)
        where TEdge : IEdge<TEdgeId, TNodeId>
        where TEdgeId : notnull
        where TNodeId : notnull
    {
        var rootNodeIds = graph.Nodes.Select(selector).Except(graph.Edges.Select(e => e.Target));
        return rootNodeIds.Select(graph.GetNode).SelectSome();
    }

    public static IEnumerable<TNode> RootNodes<TNode, TEdge>(IDirectedGraph<TNode, TEdge> graph, Func<TEdge, bool> predicate)
        where TEdge : IEdge<TNode>
    {
        var sourceNodes = graph.Edges.Where(predicate).Select(e => e.Source).Distinct();
        return sourceNodes.Except(graph.Edges.Select(e => e.Target));
    }

    public static IEnumerable<TNode> SameTreeLevel<TNode, TEdge>(IDirectedGraph<TNode, TEdge> graph, TNode node)
        where TEdge : IEdge<TNode>
    {
        return Bfs.IncomingNodes(graph, node)
                  .SelectMany(i => graph.OutgoingNodes(i))
                  .IfEmpty(RootNodes(graph))
                  .Ignore(node)
                  .Distinct();
    }

    public static IEnumerable<TNode> TerminalNodes<TNode, TEdge>(IDirectedGraph<TNode, TEdge> graph, Func<TEdge, bool>? edgePredicate = null)
        where TEdge : IEdge<TNode>
    {
        return null == edgePredicate
            ? graph.Nodes.Except(graph.Edges.Select(e => e.Source))
            : graph.Nodes.Except(graph.Edges.Where(edgePredicate).Select(e => e.Source));
    }

    public static IEnumerable<TNode> TerminalNodes<TNodeId, TNode, TEdge>(
        IDirectedGraph<TNodeId, TNode, TEdge> graph,
        Func<TNode, TNodeId> nodeIdSelector,
        Func<TEdge, bool>? edgePredicate = null)
        where TEdge : IEdge<TNodeId>
        where TNodeId : notnull
    {
        return null == edgePredicate
            ? graph.Nodes.ExceptBy(graph.Edges.Select(e => e.Source), node => nodeIdSelector(node), nodeId => nodeId, node => node)
            : graph.Nodes.ExceptBy(graph.Edges.Where(edgePredicate).Select(e => e.Source), node => nodeIdSelector(node), nodeId => nodeId, node => node);
    }
}

