using Foundation.Collections.Generic;

namespace Foundation.Graph;

public static class EdgeEnumerableExtensions
{
    public static MultiValueMap<TNode, TEdge> ToMultiValueMap<TNode, TEdge>(this IEnumerable<TEdge> edges)
        where TEdge : IEdge<TNode>
        where TNode : notnull
    {
        MultiValueMap<TNode, TEdge> map = [];

        foreach(var edge in edges)
        {
            map.Add(edge.Source, edge);
            map.Add(edge.Target, edge);
        }

        return map;
    }
}
