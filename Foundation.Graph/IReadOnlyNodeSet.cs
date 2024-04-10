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
/// Interface for a set of nodes.
/// </summary>
/// <typeparam name="TNode"></typeparam>
public interface IReadOnlyNodeSet<TNode>
{
    /// <summary>
    /// Checks, if node exists.
    /// </summary>
    /// <param name="node">Node which is searched.</param>
    /// <returns></returns>
    bool ExistsNode(TNode node);

    /// <summary>
    /// Number of nodes.
    /// </summary>
    int NodeCount { get; }

    /// <summary>
    /// List of nodes.
    /// </summary>
    IEnumerable<TNode> Nodes { get; }
}

/// <summary>
/// Interface for a set of nodes.
/// </summary>
/// <typeparam name="TNode"></typeparam>
public interface IReadOnlyNodeSet<TNodeId, TNode>
{
    /// <summary>
    /// Returns true, if node exists.
    /// </summary>
    /// <param name="node">Node which is searched.</param>
    /// <returns></returns>
    bool ExistsNode(TNodeId id);

    /// <summary>
    /// Returns Some if node exists.
    /// </summary>
    /// <param name="nodeId"></param>
    /// <returns></returns>
    Option<TNode> GetNode(TNodeId nodeId);

    /// <summary>
    /// Number of nodes.
    /// </summary>
    int NodeCount { get; }

    /// <summary>
    /// List of node ids.
    /// </summary>
    IEnumerable<TNodeId> NodeIds { get; }

    /// <summary>
    /// List of nodes.
    /// </summary>
    IEnumerable<TNode> Nodes { get; }

    bool TryGetNode(TNodeId nodeId, out TNode? node);
}
