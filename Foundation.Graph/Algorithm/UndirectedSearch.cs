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
    }
}
