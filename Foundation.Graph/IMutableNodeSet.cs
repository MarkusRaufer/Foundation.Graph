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

public interface IMutableNodeSet<TNode>
{
    /// <summary>
    /// adds a node.
    /// </summary>
    /// <param name="node"></param>
    void AddNode(TNode node);

    /// <summary>
    /// adds a list of nodes.
    /// </summary>
    /// <param name="nodes"></param>
    void AddNodes(IEnumerable<TNode> nodes);

    /// <summary>
    /// clears the node list.
    /// </summary>
    void ClearNodes();

    /// <summary>
    /// removes a node.
    /// </summary>
    /// <param name="node"></param>
    bool RemoveNode(TNode node);

    /// <summary>
    /// removes a list of nodes.
    /// </summary>
    /// <param name="nodes"></param>
    void RemoveNodes(IEnumerable<TNode> nodes);
}

public interface IMutableNodeSet<TNodeId, TNode>
{
    /// <summary>
    /// adds a node.
    /// </summary>
    /// <param name="node"></param>
    void AddNode(TNodeId nodeId, TNode node);

    /// <summary>
    /// adds a list of nodes to the node set.
    /// </summary>
    /// <param name="nodes">list of nodes.</param>
    void AddNodes(IEnumerable<(TNodeId, TNode)> nodes);

    /// <summary>
    /// clears the node list.
    /// </summary>
    void ClearNodes();

    /// <summary>
    /// removes a node.
    /// </summary>
    /// <param name="node"></param>
    bool RemoveNode(TNodeId nodeId);

    /// <summary>
    /// removes a list of nodes.
    /// </summary>
    /// <param name="nodes"></param>
    void RemoveNodes(IEnumerable<TNodeId> nodeIds);
}
