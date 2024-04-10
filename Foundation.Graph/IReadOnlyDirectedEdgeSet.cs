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

public interface IReadOnlyDirectedEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TNode>
{
    /// <summary>
    /// Returns the incoming edges of a node.
    /// </summary>
    /// <param name="node">node with the incoming edges.</param>
    /// <returns></returns>
    IEnumerable<TEdge> IncomingEdges(TNode node);

    /// <summary>
    /// Returns the nodes from the incoming edges.
    /// </summary>
    /// <param name="node">node with the incoming edges.</param>
    /// <param name="predicate">Filter to reduce the result.</param>
    /// <returns></returns>
    IEnumerable<TNode> IncomingNodes(TNode node, Func<TEdge, bool>? predicate = null);

    /// <summary>
    /// returns all outgoing edges of a node.
    /// </summary>
    /// <param name="node">source node.</param>
    /// <param name="predicate">filter to reduce the returned set.</param>
    /// <returns></returns>
    IEnumerable<TEdge> OutgoingEdges(TNode node);

    /// <summary>
    /// returns a list of target nodes of the outgoing edges.
    /// </summary>
    /// <param name="node">source node.</param>
    /// <param name="predicate">filter to reduce the returned set.</param>
    /// <returns></returns>
    IEnumerable<TNode> OutgoingNodes(TNode node, Func<TEdge, bool>? predicate = null);
}

public interface IReadOnlyDirectedEdgeSet<TNode, TEdgeId, TEdge> : IReadOnlyDirectedEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
{
}
