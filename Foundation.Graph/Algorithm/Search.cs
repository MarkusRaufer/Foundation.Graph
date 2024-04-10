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

namespace Foundation.Graph.Algorithm
{
    public static class Search
    {
        public static IEnumerable<TEdge> IncomingEdges<TNode, TEdge>(
            IReadOnlyEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            Func<TEdge, bool>? predicate = null)
            where TEdge : IEdge<TNode>
        {
            return (null == predicate)
                ? edgeSet.Edges.Where(e => e.IsIncoming(node))
                : edgeSet.Edges.Where(e => e.IsIncoming(node) && predicate(e));
        }

        public static IEnumerable<TNode> IncomingNodes<TNode, TEdge>(
            IReadOnlyEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            Func<TEdge, bool>? predicate = null)
            where TEdge : IEdge<TNode>
        {
            return IncomingEdges(edgeSet, node, predicate).Select(e => e.Source);
        }

        public static IEnumerable<TEdge> OutgoingEdges<TNode, TEdge>(
            IReadOnlyEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            Func<TEdge, bool>? predicate = null)
            where TEdge : IEdge<TNode>
        {
            return(null == predicate)
                ? edgeSet.Edges.Where(e => e.IsOutgoing(node))
                : edgeSet.Edges.Where(e => e.IsOutgoing(node) && predicate(e));
        }

        public static IEnumerable<TNode> OutgoingNodes<TNode, TEdge>(
            IReadOnlyEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            Func<TEdge, bool>? predicate = null)
            where TEdge : IEdge<TNode>
        {
            return OutgoingEdges(edgeSet, node, predicate).Select(e => e.Target);
        }

        /// <summary>
        /// Breadth-first search.
        /// </summary>
        public static class Bfs
        {
            public static IEnumerable<TEdge> IncomingEdges<TNode, TEdge>(
                IReadOnlyEdgeSet<TNode, TEdge> edgeSet,
                TNode node,
                Func<TEdge, bool>? predicate = null,
                Func<TNode, bool>? stopPredicate = null)
                where TEdge : IEdge<TNode>
            {
                var nodes = new Queue<TNode>();
                nodes.Enqueue(node);

                var visitedEdges = new HashSet<TEdge>();
                var visitedNodes = new HashSet<TNode>();
                while (0 < nodes.Count)
                {
                    var n = nodes.Dequeue();
                    if (null != stopPredicate && stopPredicate(n))
                        yield break;

                    if (visitedNodes.Contains(n))
                        continue;

                    visitedNodes.Add(n);
                    var inEdges = Search.IncomingEdges(edgeSet, n, predicate).Except(visitedEdges);
                    foreach (var inEdge in inEdges)
                    {
                        yield return inEdge;
                        visitedEdges.Add(inEdge);
                        nodes.Enqueue(inEdge.Source);
                    }
                }
            }

            public static IEnumerable<TNode> IncomingNodes<TNode, TEdge>(
                IReadOnlyEdgeSet<TNode, TEdge> edgeSet,
                TNode node,
                Func<TEdge, bool>? predicate = null,
                Func<TNode, bool>? stopPredicate = null)
                where TEdge : IEdge<TNode>
            {
                var edges = IncomingEdges(edgeSet, node, predicate, stopPredicate);
                return edges.SelectMany(e => e.GetNodesTargetSource<TNode, TEdge>()).Distinct();
            }

            /// <summary>
            /// Returns all outgoing edges from a specific node.
            /// </summary>
            /// <typeparam name="TNode">The type of the nodes.</typeparam>
            /// <typeparam name="TEdge">The type of the edges.</typeparam>
            /// <param name="edgeSet"></param>
            /// <param name="node">The node, where the search starts.</param>
            /// <param name="predicate">A filter for the edges.</param>
            /// <param name="stopPredicate">Stops searching if predicate is true. The node of the predicate is included as target node.</param>
            /// <returns></returns>
            public static IEnumerable<TEdge> OutgoingEdges<TNode, TEdge>(
                IReadOnlyEdgeSet<TNode, TEdge> edgeSet,
                TNode node,
                Func<TEdge, bool>? predicate = null,
                Func<TNode, bool>? stopPredicate = null)
                where TEdge : IEdge<TNode>
            {
                var nodes = new Queue<TNode>();
                nodes.Enqueue(node);

                var visitedEdges = new HashSet<TEdge>();
                var visitedNodes = new HashSet<TNode>();
                while (0 < nodes.Count)
                {
                    var n = nodes.Dequeue();
                    if (null != stopPredicate && stopPredicate(n))
                        yield break;

                    if (visitedNodes.Contains(n))
                        continue;

                    visitedNodes.Add(n);
                    var outEdges = Search.OutgoingEdges(edgeSet, n, predicate).Except(visitedEdges);
                    foreach (var outEdge in outEdges)
                    {
                        yield return outEdge;
                        visitedEdges.Add(outEdge);
                        nodes.Enqueue(outEdge.Target);
                    }
                }
            }

            /// <summary>
            /// Returns all outgoing nodes from a specific node.
            /// </summary>
            /// <typeparam name="TNode">The type of the nodes.</typeparam>
            /// <typeparam name="TEdge">The type of the edges.</typeparam>
            /// <param name="edgeSet"></param>
            /// <param name="node">The node, where the search starts.</param>
            /// <param name="predicate">A filter for the edges.</param>
            /// <param name="stopPredicate">Stops searching if predicate is true. The node of the predicate is included as target node.</param>
            /// <returns></returns>
            public static IEnumerable<TNode> OutgoingNodes<TNode, TEdge>(
                IReadOnlyEdgeSet<TNode, TEdge> edgeSet,
                TNode node,
                Func<TEdge, bool>? predicate = null,
                Func<TNode, bool>? stopPredicate = null)
                where TEdge : IEdge<TNode>
            {
                var edges = OutgoingEdges(edgeSet, node, predicate, stopPredicate);
                return edges.SelectMany(e => e.GetNodes<TNode, TEdge>()).Distinct();
            }
        }
    }
}
