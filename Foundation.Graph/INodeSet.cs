namespace Foundation.Graph;

using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

public interface INodeSet<TNode>
    : IReadOnlyNodeSet<TNode>
    , IMutableNodeSet<TNode>
{
}

public interface INodeSet<TNodeId, TNode>
    : IReadOnlyNodeSet<TNodeId, TNode>
    , IMutableNodeSet<TNodeId, TNode>
{
}
