using Foundation.Collections.Generic;
using System.Xml.Linq;

namespace Foundation.Graph.Algorithm;

public static class UndirectedSearch
{
    public static class Bfs
    {
        public static IEnumerable<TEdge> ConnectedEdges<TNode, TEdge>(IEdgeSet<TNode, TEdge> edgeSet, TNode node)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            var nodes = new Queue<TNode>();
            nodes.Enqueue(node);

            var visitedEdges = new HashSet<TEdge>();
            var visitedNodes = new HashSet<TNode>();

            while (0 < nodes.Count)
            {
                var n = nodes.Dequeue();

                if (visitedNodes.Contains(n))
                    continue;

                visitedNodes.Add(n);

                var edges = edgeSet.GetEdges(n);
                foreach (var edge in edges)
                {
                    if(visitedEdges.Contains(edge)) continue;

                    yield return edge;
                    visitedEdges.Add(edge);

                    var otherNode = edge.Source.Equals(n) ? edge.Target : edge.Source;
                    nodes.Enqueue(otherNode);
                }
            }
        }

        public static IEnumerable<TNode> ConnectedNodes<TNode, TEdge>(IEdgeSet<TNode, TEdge> edgeSet, TNode node)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            return ConnectedEdges(edgeSet, node).SelectMany(x => x.GetNodesNotInEdge(node)).Distinct();
        }

        /// <summary>
        /// returns all nodes with a single connection. The sequence is random.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="edgeSet"></param>
        /// <returns></returns>
        public static IEnumerable<TNode> NodesWithSingleConnection<TNode, TEdge>(IEdgeSet<TNode, TEdge> edgeSet)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            var sources = edgeSet.Edges.GroupBy(x => x.Source);
            var targets = edgeSet.Edges.GroupBy(x => x.Target);

            var ignore = new HashSet<TNode>();
            foreach(var source in sources)
            {
                if (targets.Any(x => x.Key.Equals(source.Key)))
                {
                    ignore.Add(source.Key);
                    continue;
                }

                if (1 == source.Count()) yield return source.Key;
            }
            foreach (var target in targets)
            {
                if (ignore.Contains(target.Key)) continue;

                if (1 == target.Count()) yield return target.Key;
            }
        }
        public static IEnumerable<IEnumerable<TNode>> FindConnectedNodes<TNode, TEdge>(IEdgeSet<TNode, TEdge> edgeSet)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            foreach (var path in FindConnectedPaths(edgeSet))
            {
                yield return path.SelectMany(x => x.GetNodes<TNode, TEdge>())
                                 .Distinct()
                                 .ToArray();
            }
        }

        /// <summary>
        /// Finds all connected edges.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="edgeSet"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TEdge>> FindConnectedPaths<TNode, TEdge>(IEdgeSet<TNode, TEdge> edgeSet)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            if (0 == edgeSet.EdgeCount) yield break;

            var nodes = new Queue<TNode>();
            var path = new List<TEdge>();

            nodes.Enqueue(edgeSet.Edges.First().Source);

            var visitedEdges = new HashSet<TEdge>();
            var visitedNodes = new HashSet<TNode>();

            while (0 < nodes.Count)
            {
                var node = nodes.Dequeue();

                if (visitedNodes.Contains(node))
                    continue;

                visitedNodes.Add(node);

                var connectionEdges = edgeSet.GetEdges(node);
                foreach (var connectionEdge in connectionEdges)
                {
                    if (visitedEdges.Contains(connectionEdge)) continue;

                    var otherNode = connectionEdge.GetOtherNode(node);
                    nodes.Enqueue(otherNode);

                    path.Add(connectionEdge);
                    visitedEdges.Add(connectionEdge);
                }

                if(0 == nodes.Count)
                {
                    yield return path;

                    path = new List<TEdge>();

                    var notTraversedEdge = edgeSet.Edges.FirstAsOption(x => !visitedEdges.Contains(x));
                    if (notTraversedEdge.IsSome) nodes.Enqueue(notTraversedEdge.OrThrow().Source);
                }
            }
        }
    }
}
