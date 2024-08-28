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
﻿using System.Diagnostics.CodeAnalysis;

namespace Foundation.Graph.Algorithm;

public class GraphNodeTupleComparer<TNode, TEdge> : IEqualityComparer<(IGraph<TNode, TEdge>, TNode)>
    where TEdge : IEdge<TNode>
{
    public bool Equals((IGraph<TNode, TEdge>, TNode) lhs, (IGraph<TNode, TEdge>, TNode ) rhs)
    {
        var (lgraph, lnode) = lhs;
        var (rgraph, rnode) = rhs;

        if (!ReferenceEquals(lgraph, rgraph))
            return false;

        if (null == lnode) return null == rnode;

        return null != rnode && lnode.Equals(rnode);
    }

    public int GetHashCode([DisallowNull] (IGraph<TNode, TEdge>, TNode) tuple)
    {
        var (graph, node) = tuple;
#if NETSTANDARD2_0
        return Foundation.HashCode.FromObject(graph, node);
#else
        return System.HashCode.Combine(graph, node);
#endif
    }
}

