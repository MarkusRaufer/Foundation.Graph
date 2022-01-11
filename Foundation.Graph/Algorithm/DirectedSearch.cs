using Foundation.Collections.Generic;

namespace Foundation.Graph.Algorithm;

public static class DirectedSearch
{
    /// <summary>
    /// Breadth first search.
    /// </summary>
    public static class Bfs
    {
        public static IEnumerable<TEdge> IncomingEdges<TNode, TEdge>(
          IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
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
                var inEdges = (null == predicate)
                    ? edgeSet.IncomingEdges(n).Except(visitedEdges)
                    : edgeSet.IncomingEdges(n).Where(predicate).Except(visitedEdges);

                foreach (var inEdge in inEdges)
                {
                    yield return inEdge;
                    visitedEdges.Add(inEdge);
                    nodes.Enqueue(inEdge.Source);
                }
            }
        }

        public static IEnumerable<TNode> IncomingNodes<TNode, TEdge>(
            IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            Func<TEdge, bool>? predicate = null,
            Func<TNode, bool>? stopPredicate = null)
            where TEdge : IEdge<TNode>
        {
            return IncomingEdges(edgeSet, node, predicate, stopPredicate).Select(e => e.Source);
        }

        public static IEnumerable<TEdge> OutgoingEdges<TNode, TEdge>(
            IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
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
                var outEdges = (null == predicate)
                    ? edgeSet.OutgoingEdges(n).Except(visitedEdges)
                    : edgeSet.OutgoingEdges(n).Where(predicate).Except(visitedEdges);

                foreach (var outEdge in outEdges)
                {
                    yield return outEdge;
                    visitedEdges.Add(outEdge);
                    nodes.Enqueue(outEdge.Target);
                }
            }
        }

        public static IEnumerable<TNode> OutgoingNodes<TNode, TEdge>(
            IReadOnlyDirectedEdgeSet<TNode, TEdge> edgeSet,
            TNode node,
            Func<TEdge, bool>? predicate = null,
            Func<TNode, bool>? stopPredicate = null)
            where TEdge : IEdge<TNode>
        {
            return OutgoingEdges(edgeSet, node, predicate, stopPredicate).Select(e => e.Target);
        }
    }

    public static IEnumerable<TNode> Neighbors<TNode, TEdge>(IDirectedGraph<TNode, TEdge> graph, TNode node)
                where TEdge : IEdge<TNode>
    {
        return Bfs.IncomingNodes(graph, node)
                  .Concat(graph.OutgoingNodes(node))
                  .Except(node)
                  .Distinct();
    }

    public static Opt<TParent> Parent<TNode, TEdge, TParent>(IDirectedGraph<TNode, TEdge> graph, TNode node)
                where TEdge : IEdge<TNode>
    {
        return Bfs.IncomingNodes(graph, node)
                  .OfType<TParent>()
                  .FirstAsOpt();
    }

    public static Opt<TParent> Parent<TNode, TEdge, TParent>(
                IDirectedGraph<TNode, TEdge> graph,
                TNode node,
                Func<TNode, bool> stopPredicate)
                where TEdge : IEdge<TNode>
    {
        return Bfs.IncomingNodes(graph, node, null, stopPredicate)
                  .OfType<TParent>()
                  .FirstAsOpt();
    }

    public static IEnumerable<TNode> RootNodes<TNode, TEdge>(
        IDirectedGraph<TNode, TEdge> graph)
        where TEdge : IEdge<TNode>
    {
        return graph.Nodes.Except(graph.Edges.Select(e => e.Target));
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
                  .Except(node)
                  .Distinct();
    }

    public static IEnumerable<TNode> TerminalNodes<TNode, TEdge>(IDirectedGraph<TNode, TEdge> graph, Func<TEdge, bool>? edgePredicate = null)
        where TEdge : IEdge<TNode>
    {
        return null == edgePredicate
            ? graph.Nodes.Except(graph.Edges.Select(e => e.Source))
            : graph.Nodes.Except(graph.Edges.Where(edgePredicate).Select(e => e.Source));
    }
}

