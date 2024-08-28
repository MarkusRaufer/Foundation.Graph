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
﻿using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Graph;

public class UndirectedGraph<TNode, TEdge> 
    : IGraph<TNode, TEdge>
    , IDisposable
    where TEdge : IEdge<TNode>
    where TNode : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly UndirectedEdgeSet<TNode, TEdge> _edgeSet;
    private readonly NodeSet<TNode> _nodeSet;

    public UndirectedGraph()
    {
        _edgeSet = new UndirectedEdgeSet<TNode, TEdge>();
        _nodeSet = new NodeSet<TNode>();

        _edgeSet.CollectionChanged += CollectionChanged;
        _nodeSet.CollectionChanged += CollectionChanged;
    }

    ~UndirectedGraph()
    {
        Dispose(false);
    }

    public bool AllowDuplicateEdges => false;



    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (!disposing) return;

        _edgeSet.CollectionChanged -= CollectionChanged;
        _nodeSet.CollectionChanged -= CollectionChanged;

        _edgeSet.ClearEdges();
        _nodeSet.ClearNodes();
    }

    public void AddEdge(TEdge edge) => _edgeSet.AddEdge(edge);

    public void AddEdges(IEnumerable<TEdge> edges) => _edgeSet.AddEdges(edges);

    public void AddNode([DisallowNull] TNode node) => _nodeSet.AddNode(node);

    public void AddNodes(IEnumerable<TNode> nodes) => _nodeSet.AddNodes(nodes);

    public void ClearEdges() => _edgeSet.ClearEdges();

    public int EdgeCount => _edgeSet.EdgeCount;

    public IEnumerable<TEdge> Edges => _edgeSet.Edges;

    public void ClearNodes() => _nodeSet.ClearNodes();

    public bool ExistsEdge(TEdge edge) => _edgeSet.ExistsEdge(edge);

    public bool ExistsEdge(TNode source, TNode target) => _edgeSet.ExistsEdge(source, target);

    public bool ExistsNode([DisallowNull] TNode node) => _nodeSet.ExistsNode(node);

    public IEnumerable<TEdge> GetEdges(TNode node) => _edgeSet.GetEdges(node);

    public IEnumerable<TEdge> GetEdges(TNode source, TNode target) => GetEdges(source, target);

    public int NodeCount => _nodeSet.NodeCount;

    public IEnumerable<TNode> Nodes => _nodeSet.Nodes;

    public bool RemoveEdge(TEdge edge) => _edgeSet.RemoveEdge(edge);

    public void RemoveEdges(IEnumerable<TEdge> edges) => _edgeSet.RemoveEdges(edges);

    public bool RemoveNode(TNode node) => _nodeSet.RemoveNode(node);

    public void RemoveNodes(IEnumerable<TNode> nodes) => _nodeSet.RemoveNodes(nodes);
}
