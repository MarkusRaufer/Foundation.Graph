using Foundation.Collections.Generic;

namespace Foundation.Graph.Algorithm;

public static class UndirectedSearch
{
    public static class Bfs
    {
        /// <summary>
        /// Returns all edges which are connected with a specific node.
        /// </summary>
        /// <typeparam name="TNode">Type of nodes.</typeparam>
        /// <typeparam name="TEdge">Type of edges.</typeparam>
        /// <param name="edgeSet">The edge set including the edges.</param>
        /// <param name="node">The node whose edges are to be found.</param>
        /// <returns>A list of edges.</returns>
        public static IEnumerable<TEdge> ConnectedEdges<TNode, TEdge>(IReadOnlyEdgeSet<TNode, TEdge> edgeSet, TNode node)
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
                    if (visitedEdges.Contains(edge)) continue;

                    yield return edge;
                    visitedEdges.Add(edge);

                    var otherNode = edge.GetOtherNode(n);
                    nodes.Enqueue(otherNode);
                }
            }
        }

        public static IEnumerable<TEdge> ConnectedEdges<TNode, TEdge>(IReadOnlyEdgeSet<TNode, TEdge> edgeSet, TEdge edge)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            var edges = new Queue<TEdge>();
            edges.Enqueue(edge);

            var visitedEdges = new HashSet<TEdge>();

            while (0 < edges.Count)
            {
                var e = edges.Dequeue();

                if (visitedEdges.Contains(e))
                    continue;

                visitedEdges.Add(e);

                yield return e;

                var connectedEdges = edgeSet.GetEdges(e.Source).Concat(edgeSet.GetEdges(e.Target)).Ignore(e);
                foreach (var connectedEdge in connectedEdges)
                {
                    if (visitedEdges.Contains(connectedEdge)) continue;

                    edges.Enqueue(connectedEdge);
                }
            }
        }

        /// <summary>
        /// Returns all nodes which are connected with a specific node.
        /// </summary>
        /// <typeparam name="TNode">Type of nodes.</typeparam>
        /// <typeparam name="TEdge">Type of edges.</typeparam>
        /// <param name="edgeSet">The edge set including the edges.</param>
        /// <param name="node">The node whose connected nodes are to be found.</param>
        /// <returns>A list of nodes.</returns>
        public static IEnumerable<TNode> ConnectedNodes<TNode, TEdge>(IReadOnlyEdgeSet<TNode, TEdge> edgeSet, TNode node)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            return ConnectedEdges(edgeSet, node).SelectMany(edge => edge.GetNodesWithout(node)).Distinct();
        }

        public static IEnumerable<IEnumerable<TNode>> FindConnectedNodes<TNode, TEdge>(IReadOnlyEdgeSet<TNode, TEdge> edgeSet)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            foreach (var path in FindConnectedPaths(edgeSet))
            {
                yield return path.SelectMany(x => x.GetNodes<TNode, TEdge>())
                                 .Distinct();
            }
        }

        /// <summary>
        /// Returns a list of connected edge paths.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="edgeSet"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TEdge>> FindConnectedPaths<TNode, TEdge>(IReadOnlyEdgeSet<TNode, TEdge> edgeSet)
                where TNode : notnull
                where TEdge : IEdge<TNode>
        {
            var edges = new Queue<TEdge>();
            if (0 == edgeSet.EdgeCount) yield break;

            var visitedEdges = new HashSet<TEdge>();

            while (edgeSet.EdgeCount != visitedEdges.Count)
            {
                var edge = edgeSet.Edges.Except(visitedEdges).FirstOrDefault();
                if (null == edge) break;

                visitedEdges.Add(edge);
                var path = ConnectedEdges(edgeSet, edge);

                path.ForEach(x => visitedEdges.Add(x));

                yield return path;
            }
        }

        /// <summary>
        /// returns all nodes with a single connection. The sequence is random.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="edgeSet"></param>
        /// <returns></returns>
        public static IEnumerable<TNode> NodesWithSingleConnection<TNode, TEdge>(IReadOnlyEdgeSet<TNode, TEdge> edgeSet)
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

        /// <summary>
        /// Returns the neighbor nodes of node.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="edgeSet"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static IEnumerable<TNode> Neighbors<TNode, TEdge>(EdgeSet<TNode, TEdge> edgeSet, TNode node)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            return edgeSet.GetEdges(node).Select(x => x.GetOtherNode(node)).Ignore(node).Distinct();
        }

        /// <summary>
        /// returns tuples of nodes with their number of connections.
        /// </summary>
        /// <typeparam name="TNode"></typeparam>
        /// <typeparam name="TEdge"></typeparam>
        /// <param name="edgeSet"></param>
        /// <returns></returns>
        public static IEnumerable<(TNode node, int numberOfConnections)> NodesWithNumberOfConnections<TNode, TEdge>(IReadOnlyEdgeSet<TNode, TEdge> edgeSet)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            var nodes = new HashSet<TNode>(edgeSet.Edges.SelectMany(x => x.GetNodes()));

            foreach(var node in nodes)
            {
                var edges = edgeSet.GetEdges(node);
                yield return (node, numberOfConnections: edges.Count());
            }
        }

        /// <summary>
        /// Returns all nodes which are not connected by a edge.
        /// </summary>
        /// <typeparam name="TNode">Type of nodes</typeparam>
        /// <typeparam name="TEdge">Type of edges.</typeparam>
        /// <param name="graph">Graph including the nodes and the edges.</param>
        /// <returns>A list of nodes.</returns>
        public static IEnumerable<TNode> UnconnectedNodes<TNode, TEdge>(IGraph<TNode, TEdge> graph)
            where TNode : notnull
            where TEdge : IEdge<TNode>
        {
            foreach (var node in graph.Nodes)
            {
                if (ConnectedEdges(graph, node).Any()) continue;

                yield return node;
            }
        }
    }
}
