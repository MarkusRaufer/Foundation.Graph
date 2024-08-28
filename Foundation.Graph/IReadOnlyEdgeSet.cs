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

/// <summary>
/// Contract for an immutable edge set;
/// </summary>
/// <typeparam name="TNode">Type of the nodes.</typeparam>
/// <typeparam name="TEdge">Type of the edges.</typeparam>
public interface IReadOnlyEdgeSet<in TNode, TEdge>
    where TEdge : IEdge<TNode>
{
    /// <summary>
    /// Number of edges.
    /// </summary>
    int EdgeCount { get; }

    /// <summary>
    /// List of edges.
    /// </summary>
    IEnumerable<TEdge> Edges { get; }

    /// <summary>
    /// Checks if edge exists.
    /// </summary>
    /// <param name="edge"></param>
    /// <returns></returns>
    bool ExistsEdge(TEdge edge);

    /// <summary>
    /// Checks if edge exists.
    /// </summary>
    /// <param name="source">source node</param>
    /// <param name="target">target node</param>
    /// <returns></returns>
    bool ExistsEdge(TNode source, TNode target);

    /// <summary>
    /// Returns all edges that have node as source or destination.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    IEnumerable<TEdge> GetEdges(TNode node);

    /// <summary>
    /// Returns all edges that have the specific source and target node.
    /// </summary>
    /// <param name="source">source node.</param>
    /// <param name="target">target node.</param>
    /// <returns></returns>
    IEnumerable<TEdge> GetEdges(TNode source, TNode target);

}

/// <summary>
/// Contract for an immutable edge set;
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="TEdgeId"></typeparam>
/// <typeparam name="TEdge"></typeparam>
public interface IReadOnlyEdgeSet<in TNode, TEdgeId, TEdge> : IReadOnlyEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
{
    bool ExistsEdge(TEdgeId edgeId);
    Option<TEdge> GetEdge(TEdgeId edgeId);
}
