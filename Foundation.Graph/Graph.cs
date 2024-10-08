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
using Foundation.ComponentModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Foundation.Graph
{
    public class Graph<TNode, TEdge>
        : Graph<TNode, TEdge, NodeSet<TNode>, EdgeSet<TNode, TEdge>>
        , IGraph<TNode, TEdge>
        where TEdge : IEdge<TNode>
        where TNode : notnull
    {
        public Graph(Func<TNode, TNode, TEdge>? edgeFactory = null) : base(new NodeSet<TNode>(), new EdgeSet<TNode, TEdge>(), edgeFactory)
        {
        }

        public Graph(NodeSet<TNode> nodeSet, EdgeSet<TNode, TEdge> edgeSet, Func<TNode, TNode, TEdge>? edgeFactory = null)
            : base(nodeSet, edgeSet, edgeFactory)
        {
        }

        ~Graph()
        {
            Dispose(false);
        }
    }

    public class Graph<TNode, TEdge, TNodeSet, TEdgeSet>
        : IGraph<TNode, TEdge>
        , IDisposable
        where TEdge : IEdge<TNode>
        where TNode : notnull
        where TNodeSet : INodeSet<TNode>, INotifyCollectionChanged
        where TEdgeSet : IEdgeSet<TNode, TEdge>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event NotifyCollectionChangedEventHandler? NodeSetChanged;
        public event NotifyCollectionChangedEventHandler? EdgeSetChanged;
        public event ObjectChangedEventHandler? ObjectChanged;

        public Graph(TNodeSet nodeSet, TEdgeSet edgeSet, Func<TNode, TNode, TEdge>? edgeFactory = null)
        {
            NodeSet = nodeSet.ThrowIfNull();
            EdgeSet = edgeSet.ThrowIfNull();

            EdgeFactory = edgeFactory;

            NodeSet.CollectionChanged += OnNodeSetChanged;
            EdgeSet.CollectionChanged += OnEdgeSetChanged;
        }

        ~Graph()
        {
            Dispose(false);
        }

        #region IEdgeSet members
        public virtual void AddEdge(TEdge edge)
        {
            EdgeSet.AddEdge(edge);
        }

        public virtual void AddEdges(IEnumerable<TEdge> edges)
        {
            EdgeSet.AddEdges(edges);
        }

        public void ClearEdges()
        {
            EdgeSet.ClearEdges();
        }

        public int EdgeCount
        {
            get { return EdgeSet.EdgeCount; }
        }

        protected Func<TNode, TNode, TEdge>? EdgeFactory { get; }


        public IEnumerable<TEdge> Edges
        {
            get { return EdgeSet.Edges; }
        }

        protected TEdgeSet EdgeSet { get; private set; }

        public bool ExistsEdge(TEdge edge)
        {
            return EdgeSet.ExistsEdge(edge);
        }

        public bool ExistsEdge(TNode source, TNode target)
        {
            return EdgeSet.ExistsEdge(source, target);
        }

        public IEnumerable<TEdge> GetEdges(TNode node) => EdgeSet.GetEdges(node);


        public IEnumerable<TEdge> GetEdges(TNode source, TNode target) => EdgeSet.GetEdges(source, target);

        public virtual bool RemoveEdge(TEdge edge)
        {
            return EdgeSet.RemoveEdge(edge);
        }

        public virtual void RemoveEdges(IEnumerable<TEdge> edges)
        {
            EdgeSet.RemoveEdges(edges);
        }
        #endregion IEdgeSet members

        #region INodeSet members
        public virtual void AddNode(TNode node) => NodeSet.AddNode(node);

        public virtual void AddNodes(IEnumerable<TNode> nodes)
        {
            NodeSet.AddNodes(nodes);
        }

        public bool ExistsNode(TNode node)
        {
            return NodeSet.ExistsNode(node);
        }

        public int NodeCount => NodeSet.NodeCount;

        public IEnumerable<TNode> Nodes => NodeSet.Nodes;

        public virtual bool RemoveNode(TNode node)
        {
            if (NodeSet.RemoveNode(node))
            {
                var edges = EdgeSet.GetEdges(node).ToArray();
                if (null != EdgeFactory)
                {
                    var incomingEdges = edges.Where(x => x.Target.Equals(node));
                    var outgoingEdges = edges.Where(x => x.Source.Equals(node));

                    foreach (var source in incomingEdges.Select(x => x.Source))
                    {
                        foreach (var target in outgoingEdges.Select(x => x.Target))
                        {
                            var newEdge = EdgeFactory(source, target);
                            if (newEdge is not null) AddEdge(newEdge);
                        }
                    }
                }

                RemoveEdges(edges);
                return true;
            }

            return false;
        }

        public virtual void RemoveNodes(IEnumerable<TNode> nodes)
        {
            foreach (var node in nodes.ToArray())
                RemoveNode(node);
        }
        #endregion IMutableNodeSet members

        public void Clear()
        {
            ClearEdges();
            ClearNodes();
        }

        public void ClearNodes()
        {
            NodeSet.ClearNodes();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                NodeSet.CollectionChanged -= OnNodeSetChanged;
                EdgeSet.CollectionChanged -= OnEdgeSetChanged;

                Clear();
            }
        }

        protected void FireCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(sender, args);
        }

        protected TNodeSet NodeSet { get; private set; }

        private void OnEdgeSetChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            EdgeSetChanged?.Invoke(this, e);
            FireCollectionChanged(this, e);
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(this, e));
        }

        private void OnNodeSetChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            NodeSetChanged?.Invoke(this, e);
            FireCollectionChanged(this, e);
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(this, e));
        }
    }

    public class Graph<TNodeId, TNode, TEdge, TNodeSet, TEdgeSet>
        : IGraph<TNodeId, TNode, TEdge>
        , IDisposable
        where TEdge : IEdge<TNodeId>
        where TEdgeSet : IEdgeSet<TNodeId, TEdge>, INotifyCollectionChanged
        where TNode : notnull
        where TNodeId : notnull
        where TNodeSet : INodeSet<TNodeId, TNode>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event NotifyCollectionChangedEventHandler? NodeSetChanged;
        public event NotifyCollectionChangedEventHandler? EdgeSetChanged;
        public event ObjectChangedEventHandler? ObjectChanged;

        public Graph(TNodeSet nodeSet, TEdgeSet edgeSet, Func<TNodeId, TNodeId, TEdge>? edgeFactory = null)
        {
            NodeSet = nodeSet.ThrowIfNull();
            EdgeSet = edgeSet.ThrowIfNull();
            EdgeFactory = edgeFactory;

            NodeSet.CollectionChanged += OnNodeSetChanged;
            EdgeSet.CollectionChanged += OnEdgeSetChanged;

        }

        protected Graph(SerializationInfo info, StreamingContext context)
        {
            if (info.GetValue(nameof(NodeSet), typeof(TNodeSet)) is TNodeSet nodeSet)
            {
                NodeSet = nodeSet;
                NodeSet.CollectionChanged += OnNodeSetChanged;
            }
            else throw new ArgumentException(nameof(NodeSet), "missing node set on serialization");


            if (info.GetValue(nameof(EdgeSet), typeof(TEdgeSet)) is TEdgeSet edgeSet)
            {
                EdgeSet = edgeSet;
                EdgeSet.CollectionChanged += OnEdgeSetChanged;
            }
            else throw new ArgumentException(nameof(EdgeSet), "missing edge set on serialization");
        }

        ~Graph()
        {
            Dispose(false);
        }

        public virtual void AddEdge(TEdge edge)
        {
            EdgeSet.AddEdge(edge);
        }

        public virtual void AddEdges(IEnumerable<TEdge> edges)
        {
            EdgeSet.AddEdges(edges);
        }

        public virtual void AddNode(TNodeId nodeId, [DisallowNull] TNode node)
        {
            NodeSet.AddNode(nodeId, node);
        }


        public virtual void AddNodes(IEnumerable<(TNodeId, TNode)> nodes)
        {
            NodeSet.AddNodes(nodes);
        }

        public void Clear()
        {
            ClearEdges();
            ClearNodes();
        }

        public void ClearEdges()
        {
            EdgeSet.ClearEdges();
        }

        public void ClearNodes()
        {
            NodeSet.ClearNodes();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                NodeSet.CollectionChanged -= OnNodeSetChanged;
                EdgeSet.CollectionChanged -= OnEdgeSetChanged;

                Clear();
            }
        }

        public int EdgeCount
        {
            get { return EdgeSet.EdgeCount; }
        }

        public IEnumerable<TEdge> Edges
        {
            get { return EdgeSet.Edges; }
        }

        protected Func<TNodeId, TNodeId, TEdge>? EdgeFactory { get; }

        protected TEdgeSet EdgeSet { get; private set; }

        public bool ExistsEdge(TEdge edge)
        {
            return EdgeSet.ExistsEdge(edge);
        }

        public bool ExistsEdge(TNodeId source, TNodeId target)
        {
            return EdgeSet.ExistsEdge(source, target);
        }

        public bool ExistsNode(TNodeId id)
        {
            return NodeSet.ExistsNode(id);
        }

        protected void FireCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(sender, args);
        }

        public IEnumerable<TEdge> GetEdges(TNodeId nodeId) => EdgeSet.GetEdges(nodeId);

        public IEnumerable<TEdge> GetEdges(TNodeId source, TNodeId target) => EdgeSet.GetEdges(source, target);

        public Option<TNode> GetNode(TNodeId nodeId) => NodeSet.GetNode(nodeId);

        public IEnumerable<TNode> GetNodes(IEnumerable<TNodeId> nodeIds) => NodeSet.GetNodes(nodeIds);

        public int NodeCount
        {
            get { return NodeSet.NodeCount; }
        }

        public IEnumerable<TNodeId> NodeIds
        {
            get { return NodeSet.NodeIds; }
        }

        public IEnumerable<TNode> Nodes
        {
            get { return NodeSet.Nodes; }
        }

        protected TNodeSet NodeSet { get; private set; }

        private void OnEdgeSetChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            EdgeSetChanged?.Invoke(this, e);
            FireCollectionChanged(this, e);
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(this, e));
        }

        private void OnNodeSetChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            NodeSetChanged?.Invoke(this, e);
            FireCollectionChanged(this, e);
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(this, e));
        }

        public virtual bool RemoveEdge(TEdge edge)
        {
            return EdgeSet.RemoveEdge(edge);
        }

        public virtual void RemoveEdges(IEnumerable<TEdge> edges)
        {
            EdgeSet.RemoveEdges(edges);
        }

        public virtual bool RemoveNode(TNodeId id)
        {
            if (NodeSet.RemoveNode(id))
            {
                var edges = EdgeSet.GetEdges(id).ToArray();
                if (null != EdgeFactory)
                {
                    var incomingEdges = edges.Where(x => x.Target.Equals(id));
                    var outgoingEdges = edges.Where(x => x.Source.Equals(id));

                    foreach (var source in incomingEdges.Select(x => x.Source))
                    {
                        foreach (var target in outgoingEdges.Select(x => x.Target))
                        {
                            var newEdge = EdgeFactory(source, target);
                            if (newEdge is not null) AddEdge(newEdge);
                        }
                    }
                }
                RemoveEdges(edges);
                return true;
            }
            
            return false;
        }

        public virtual void RemoveNodes(IEnumerable<TNodeId> nodes)
        {
            foreach (var node in nodes.ToList())
                RemoveNode(node);
        }

        public bool TryGetNode(TNodeId nodeId, [MaybeNullWhen(false)] out TNode node)
        {
            return NodeSet.TryGetNode(nodeId, out node);
        }
    }

    public abstract class Graph<TNodeId, TNode, TEdgeId, TEdge, TNodeSet, TEdgeSet>
        : IGraph<TNodeId, TNode, TEdgeId, TEdge>
        where TEdge : IEdge<TEdgeId, TNodeId>
        where TEdgeId : notnull
        where TNodeId : notnull
        where TNodeSet : INodeSet<TNodeId, TNode>, INotifyCollectionChanged
        where TEdgeSet : IEdgeSet<TNodeId, TEdgeId, TEdge>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event NotifyCollectionChangedEventHandler? NodeSetChanged;
        public event NotifyCollectionChangedEventHandler? EdgeSetChanged;
        public event ObjectChangedEventHandler? ObjectChanged;

        public Graph(TNodeSet nodeSet, TEdgeSet edgeSet, Func<TNodeId, TNodeId, TEdge>? edgeFactory = null)
        {
            NodeSet = nodeSet.ThrowIfNull();
            EdgeSet = edgeSet.ThrowIfNull();
            EdgeFactory = edgeFactory;

            NodeSet.CollectionChanged += OnNodeSetChanged;
            EdgeSet.CollectionChanged += OnEdgeSetChanged;

        }

        protected Graph(SerializationInfo info, StreamingContext context)
        {
            if (info.GetValue(nameof(NodeSet), typeof(TNodeSet)) is TNodeSet nodeSet)
            {
                NodeSet = nodeSet;
                NodeSet.CollectionChanged += OnNodeSetChanged;
            }
            else throw new ArgumentException(nameof(NodeSet), "missing node set on serialization");
                

            if (info.GetValue(nameof(EdgeSet), typeof(TEdgeSet)) is TEdgeSet edgeSet)
            {
                EdgeSet = edgeSet;
                EdgeSet.CollectionChanged += OnEdgeSetChanged;
            }
            else throw new ArgumentException(nameof(EdgeSet), "missing edge set on serialization");
        }

        ~Graph()
        {
            NodeSet.CollectionChanged -= OnNodeSetChanged;
            EdgeSet.CollectionChanged -= OnEdgeSetChanged;
        }

        public virtual void AddEdge(TEdge edge)
        {
            EdgeSet.AddEdge(edge);
        }

        public virtual void AddEdges(IEnumerable<TEdge> edges)
        {
            EdgeSet.AddEdges(edges);
        }

        public virtual void AddNode(TNodeId nodeId, TNode node)
        {
            NodeSet.AddNode(nodeId, node);
        }

        public virtual void AddNodes(IEnumerable<(TNodeId, TNode)> nodes)
        {
            NodeSet.AddNodes(nodes);
        }

        public void Clear()
        {
            ClearEdges();
            ClearNodes();
        }

        public void ClearEdges() => EdgeSet.ClearEdges();

        public void ClearNodes() => NodeSet.ClearNodes();

        public int EdgeCount => EdgeSet.EdgeCount;

        public IEnumerable<TEdge> Edges => EdgeSet.Edges;

        protected Func<TNodeId, TNodeId, TEdge>? EdgeFactory { get; }

        [NotNull]
        protected TEdgeSet EdgeSet { get; private set; }

        public bool ExistsEdge(TEdge edge) => EdgeSet.ExistsEdge(edge);

        public bool ExistsEdge(TEdgeId id) => EdgeSet.ExistsEdge(id);

        public bool ExistsEdge(TNodeId source, TNodeId target) => EdgeSet.ExistsEdge(source, target);

        public bool ExistsNode(TNodeId id) => NodeSet.ExistsNode(id);

        protected void FireCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(sender, args);
        }

        public Option<TEdge> GetEdge(TEdgeId id) => EdgeSet.GetEdge(id);

        public IEnumerable<TEdge> GetEdges(TNodeId node) => EdgeSet.GetEdges(node);

        public IEnumerable<TEdge> GetEdges(TNodeId source, TNodeId target) => EdgeSet.GetEdges(source, target);

        public Option<TNode> GetNode(TNodeId nodeId) => NodeSet.GetNode(nodeId);

        public IEnumerable<TNode> GetNodes(IEnumerable<TNodeId> nodeIds) => NodeSet.GetNodes(nodeIds);

        public int NodeCount => NodeSet.NodeCount;

        public IEnumerable<TNodeId> NodeIds =>  NodeSet.NodeIds;

        public IEnumerable<TNode> Nodes => NodeSet.Nodes;

        [NotNull]
        protected TNodeSet NodeSet { get; private set; }

        private void OnEdgeSetChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            EdgeSetChanged?.Invoke(this, e);
            FireCollectionChanged(this, e);
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(this, e));
        }

        private void OnNodeSetChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            NodeSetChanged?.Invoke(this, e);
            FireCollectionChanged(this, e);
            ObjectChanged?.Invoke(this, new ObjectChangedEventArgs(this, e));
        }

        public virtual bool RemoveEdge(TEdge edge)
        {
            return EdgeSet.RemoveEdge(edge);
        }
        public bool RemoveEdge(TEdgeId edgeId) => EdgeSet.RemoveEdge(edgeId);

        public virtual void RemoveEdges(IEnumerable<TEdge> edges)
        {
            EdgeSet.RemoveEdges(edges);
        }

        public void RemoveEdges(IEnumerable<TEdgeId> edgeIds) => EdgeSet.RemoveEdges(edgeIds);

        public abstract bool RemoveNode(TNodeId id);

        public virtual void RemoveNodes(IEnumerable<TNodeId> nodes)
        {
            foreach (var node in nodes.ToList())
                RemoveNode(node);
        }

        public bool TryGetNode(TNodeId nodeId, [NotNullWhen(true)] out TNode? node)
        {
            return NodeSet.TryGetNode(nodeId, out node);
        }
    }
}
