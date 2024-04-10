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
/// Contract for a mutable edge set.
/// </summary>
/// <typeparam name="TNode">Typ der Knoten.</typeparam>
/// <typeparam name="TEdge">Typ der Kanten.</typeparam>
public interface IMutableEdgeSet<in TNode, TEdge>
    where TEdge : IEdge<TNode>
{
    /// <summary>
    /// adds an edge.
    /// </summary>
    /// <param name="edge"></param>
    void AddEdge(TEdge edge);

    /// <summary>
    /// adds a list of edges.
    /// </summary>
    /// <param name="edges"></param>
    void AddEdges(IEnumerable<TEdge> edges);

    /// <summary>
    /// clears the edge list.
    /// </summary>
    void ClearEdges();

    /// <summary>
    /// removes an edge.
    /// </summary>
    /// <param name="edge"></param>
    bool RemoveEdge(TEdge edge);

    /// <summary>
    /// removes a list of edges.
    /// </summary>
    /// <param name="edges"></param>
    void RemoveEdges(IEnumerable<TEdge> edges);
}

/// <summary>
/// Contract for a mutable edge set with identifiable edges.
/// </summary>
/// <typeparam name="TNode">Type of node.</typeparam>
/// <typeparam name="TEdgeId">Type of edge id.</typeparam>
/// <typeparam name="TEdge">Type of edge.</typeparam>
public interface IMutableEdgeSet<in TNode, TEdgeId, TEdge> : IMutableEdgeSet<TNode, TEdge>
    where TEdge : IEdge<TEdgeId, TNode>
{
    bool RemoveEdge(TEdgeId edgeId);
    void RemoveEdges(IEnumerable<TEdgeId> edgeIds);
}
