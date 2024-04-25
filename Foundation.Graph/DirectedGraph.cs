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
namespace Foundation.Graph;

using System.Collections.Specialized;

public class DirectedGraph<TNode, TEdge>
    : DirectedGraph<TNode, TEdge, NodeSet<TNode>, DirectedEdgeSet<TNode, TEdge>>
    , IDirectedGraph<TNode, TEdge>
    where TEdge : IEdge<TNode>
    where TNode : notnull
{
    public DirectedGraph(Func<TNode, TNode, TEdge>? edgeFactory = null)
        : base(new NodeSet<TNode>(), new DirectedEdgeSet<TNode, TEdge>(), edgeFactory)
    {
    }

    public DirectedGraph(IEnumerable<TNode> nodes, Func<TNode, TNode, TEdge>? edgeFactory = null)
        : base(new NodeSet<TNode>(nodes), new DirectedEdgeSet<TNode, TEdge>(), edgeFactory)
    {
    }

    public DirectedGraph(NodeSet<TNode> nodeSet, DirectedEdgeSet<TNode, TEdge> edgeSet, Func<TNode, TNode, TEdge>? edgeFactory = null)
        : base(nodeSet, edgeSet, edgeFactory)
    {
    }
}

public class DirectedGraph<TNode, TEdge, TNodeSet, TEdgeSet>
    : Graph<TNode, TEdge, TNodeSet, TEdgeSet>
    , IDirectedGraph<TNode, TEdge>
    where TEdge : IEdge<TNode>
    where TNode : notnull
    where TNodeSet : INodeSet<TNode>, INotifyCollectionChanged
    where TEdgeSet : IDirectedEdgeSet<TNode, TEdge>, INotifyCollectionChanged
{
    public DirectedGraph(TNodeSet nodes, TEdgeSet edges, Func<TNode, TNode, TEdge>? edgeFactory = null)
        : base(nodes, edges, edgeFactory)
    {
    }

    public IEnumerable<TEdge> IncomingEdges(TNode node)
    {
        return EdgeSet.IncomingEdges(node);
    }

    public IEnumerable<TNode> IncomingNodes(TNode node, Func<TEdge, bool>? predicate = null)
    {
        return EdgeSet.IncomingNodes(node, predicate);
    }

    public IEnumerable<TEdge> OutgoingEdges(TNode node)
    {
        return EdgeSet.OutgoingEdges(node);
    }

    public IEnumerable<TNode> OutgoingNodes(TNode node, Func<TEdge, bool>? predicate = null)
    {
        return EdgeSet.OutgoingNodes(node, predicate);
    }

    public override bool RemoveEdge(TEdge edge)
    {
        return EdgeSet.RemoveEdge(edge);
    }

    public override void RemoveEdges(IEnumerable<TEdge> edges)
    {
        EdgeSet.RemoveEdges(edges);
    }

    public override bool RemoveNode(TNode node)
    {
        if (NodeSet.RemoveNode(node))
        {
            var incomingEdges = EdgeSet.IncomingEdges(node).ToArray();
            var outgoingEdges = EdgeSet.OutgoingEdges(node).ToArray();

            if (null != EdgeFactory)
            {
                foreach (var incomingEdge in incomingEdges)
                {
                    foreach (var outgoingEdge in outgoingEdges)
                    {
                        var newEdge = EdgeFactory(incomingEdge.Source, outgoingEdge.Target);
                        if (newEdge is not null) AddEdge(newEdge);

                        RemoveEdge(outgoingEdge);
                    }

                    RemoveEdge(incomingEdge);
                }

                return true;
            }

            RemoveEdges(incomingEdges);
            RemoveEdges(outgoingEdges);

            return true;
        }

        return false;
    }

    public override void RemoveNodes(IEnumerable<TNode> nodes)
    {
        foreach (var node in nodes)
            RemoveNode(node);
    }
}


public class DirectedGraph<TNodeId, TNode, TEdge>
    : DirectedGraph<TNodeId, TNode, TEdge, NodeSet<TNodeId, TNode>, DirectedEdgeSet<TNodeId, TEdge>>
    , IDirectedGraph<TNodeId, TNode, TEdge>
    where TEdge : IEdge<TNodeId>
    where TNode : IIdentifiable<TNodeId>
    where TNodeId : notnull
{
    public DirectedGraph(Func<TNodeId, TNodeId, TEdge>? edgeFactory = null)
        : this(new NodeSet<TNodeId, TNode>(), new DirectedEdgeSet<TNodeId, TEdge>(), edgeFactory)
    {
    }

    public DirectedGraph(NodeSet<TNodeId, TNode> nodeSet, DirectedEdgeSet<TNodeId, TEdge> edgeSet, Func<TNodeId, TNodeId, TEdge>? edgeFactory = null)
        : base(nodeSet, edgeSet, edgeFactory)
    {
    }
}

public class DirectedGraph<TNodeId, TNode, TEdge, TNodeSet, TEdgeSet>
    : Graph<TNodeId, TNode, TEdge, TNodeSet, TEdgeSet>
    , IDirectedGraph<TNodeId, TNode, TEdge>
    where TEdge : IEdge<TNodeId>
    where TEdgeSet : IDirectedEdgeSet<TNodeId, TEdge>, INotifyCollectionChanged
    where TNode : notnull
    where TNodeId : notnull
    where TNodeSet : INodeSet<TNodeId, TNode>, INotifyCollectionChanged
{
    public DirectedGraph(TNodeSet nodeSet, TEdgeSet edgeSet, Func<TNodeId, TNodeId, TEdge>? edgeFactory = null)
        : base(nodeSet, edgeSet, edgeFactory)
    {
    }

    public IEnumerable<TEdge> IncomingEdges(TNodeId node)
    {
        return EdgeSet.IncomingEdges(node);
    }

    public IEnumerable<TNodeId> IncomingNodes(TNodeId node, Func<TEdge, bool>? predicate = null)
    {
        return EdgeSet.IncomingNodes(node, predicate);
    }

    public IEnumerable<TNodeId> OutgoingNodes(TNodeId node, Func<TEdge, bool>? predicate = null)
    {
        return EdgeSet.OutgoingNodes(node, predicate);
    }

    public IEnumerable<TEdge> OutgoingEdges(TNodeId node)
    {
        return EdgeSet.OutgoingEdges(node);
    }

    public override bool RemoveNode(TNodeId id)
    {
        if (NodeSet.RemoveNode(id))
        {
            var incomingEdges = EdgeSet.IncomingEdges(id).ToArray();
            var outgoingEdges = EdgeSet.OutgoingEdges(id).ToArray();

            if (null != EdgeFactory)
            {
                foreach (var incomingEdge in incomingEdges)
                {
                    foreach (var outgoingEdge in outgoingEdges)
                    {
                        var newEdge = EdgeFactory(incomingEdge.Source, outgoingEdge.Target);
                        if (newEdge is not null) AddEdge(newEdge);

                        RemoveEdge(outgoingEdge);
                    }

                    RemoveEdge(incomingEdge);
                }

                return true;
            }

            RemoveEdges(incomingEdges);
            RemoveEdges(outgoingEdges);

            return true;
        }

        return false;
    }
}

public abstract class DirectedGraph<TNodeId, TNode, TEdgeId, TEdge, TNodeSet, TEdgeSet>
    : Graph<TNodeId, TNode, TEdgeId, TEdge, TNodeSet, TEdgeSet>
    , IDirectedGraph<TNodeId, TNode, TEdgeId, TEdge>
    where TEdge : IEdge<TEdgeId, TNodeId>
    where TEdgeId : notnull
    where TEdgeSet : IDirectedEdgeSet<TNodeId, TEdgeId, TEdge>, INotifyCollectionChanged
    where TNodeId : notnull
    where TNodeSet : INodeSet<TNodeId, TNode>, INotifyCollectionChanged
{
    protected DirectedGraph(TNodeSet nodeSet, TEdgeSet edgeSet, Func<TNodeId, TNodeId, TEdge>? edgeFactory = null)
        : base(nodeSet, edgeSet, edgeFactory)
    {
    }

    public IEnumerable<TEdge> IncomingEdges(TNodeId node)
    {
        return EdgeSet.IncomingEdges(node);
    }

    public IEnumerable<TNodeId> IncomingNodes(TNodeId node, Func<TEdge, bool>? predicate = null)
    {
        return EdgeSet.IncomingNodes(node, predicate);
    }

    public IEnumerable<TNodeId> OutgoingNodes(TNodeId node, Func<TEdge, bool>? predicate = null)
    {
        return EdgeSet.OutgoingNodes(node, predicate);
    }

    public IEnumerable<TEdge> OutgoingEdges(TNodeId node)
    {
        return EdgeSet.OutgoingEdges(node);
    }

    public override bool RemoveNode(TNodeId id)
    {
        if (NodeSet.RemoveNode(id))
        {
            var incomingEdges = EdgeSet.IncomingEdges(id).ToArray();
            var outgoingEdges = EdgeSet.OutgoingEdges(id).ToArray();

            if (null != EdgeFactory)
            {
                foreach (var incomingEdge in incomingEdges)
                {
                    foreach (var outgoingEdge in outgoingEdges)
                    {
                        var newEdge = EdgeFactory(incomingEdge.Source, outgoingEdge.Target);
                        if (newEdge is not null) AddEdge(newEdge);

                        RemoveEdge(outgoingEdge);
                    }

                    RemoveEdge(incomingEdge);
                }

                return true;
            }

            RemoveEdges(incomingEdges);
            RemoveEdges(outgoingEdges);

            return true;
        }

        return false;
    }
}

