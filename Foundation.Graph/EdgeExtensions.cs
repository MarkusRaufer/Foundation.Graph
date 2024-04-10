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

public static class EdgeExtensions
{
    public static bool EqualsUndirected<TNode, TEdge>(this TEdge? lhs, TEdge? rhs)
        where TEdge : IEdge<TNode>
    {
        if (null == lhs) return null == rhs;
        if (null == rhs) return false;
        return lhs.EqualsUndirected(rhs.Source, rhs.Target);
    }

    public static bool EqualsUndirected<TNode, TEdge>(this TEdge? lhs, TNode source, TNode target)
        where TEdge : IEdge<TNode>
    {
        if (null == lhs) return false;
        return lhs.Source.EqualsNullable(source) && lhs.Target.EqualsNullable(target)
            || lhs.Source.EqualsNullable(target) && lhs.Target.EqualsNullable(source);
    }

    public static IEnumerable<TNode> GetNodes<TNode>(this IEdge<TNode> edge)
    {
        yield return edge.Source;
        yield return edge.Target;
    }

    public static IEnumerable<TNode> GetNodes<TNode, TEdge>(this TEdge edge)
        where TEdge : IEdge<TNode>
    {
        yield return edge.Source;
        yield return edge.Target;
    }

    public static IEnumerable<TNode> GetNodesTargetSource<TNode>(this IEdge<TNode> edge)
    {
        yield return edge.Target;
        yield return edge.Source;
    }

    public static IEnumerable<TNode> GetNodesTargetSource<TNode, TEdge>(this TEdge edge)
        where TEdge : IEdge<TNode>
    {
        yield return edge.Target;
        yield return edge.Source;
    }

    public static IEnumerable<TNode> GetNodesWithout<TNode>(this IEdge<TNode> edge, TNode node)
    {
        if (!edge.Source.EqualsNullable(node)) yield return edge.Source;
        if (!edge.Target.EqualsNullable(node)) yield return edge.Target;
    }


    /// <summary>
    /// returns the opposite node of the edge. If node is Source, Target is returned otherwise Source.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <param name="edge"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static TNode GetOtherNode<TNode, TEdge>(this TEdge edge, TNode node)
        where TEdge : IEdge<TNode>
    {
        if (edge.Source.EqualsNullable(node)) return edge.Target;
        if (edge.Target.EqualsNullable(node)) return edge.Source;

        throw new ArgumentOutOfRangeException($"node {node} not part of the edge", nameof(node));
    }

    public static IEnumerable<TNode> GetUniqueNodes<TNode>(this IEdge<TNode> edge)
    {
        yield return edge.Source;

        if (!edge.Source.EqualsNullable(edge.Target))
            yield return edge.Target;
    }

    public static IEnumerable<TNode> GetUniqueNodes<TNode, TEdge>(this TEdge edge)
        where TEdge : IEdge<TNode>
    {
        yield return edge.Source;

        if (!edge.Source.EqualsNullable(edge.Target))
            yield return edge.Target;
    }

    public static bool HasNode<TNode, TEdge>(this TEdge edge, TNode node)
        where TEdge : IEdge<TNode>
    {
        if (edge.Source.EqualsNullable(node)) return true;
        if (edge.Target.EqualsNullable(node)) return true;
        return false;
    }

    public static bool IsIncoming<TNode, TEdge>(this TEdge edge, TNode node)
        where TEdge : IEdge<TNode>
    {
        return edge.Target.EqualsNullable(node);
    }

    public static bool IsOutgoing<TNode, TEdge>(this TEdge edge, TNode node)
        where TEdge : IEdge<TNode>
    {
        return edge.Source.EqualsNullable(node);
    }
}

