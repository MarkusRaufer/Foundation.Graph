namespace Foundation.Graph;

public interface IEdgeWithProperty<TNode, TProperty> : IEdge<TNode>
{
    TProperty Property { get; }

}

public interface IEdgeWithProperty<TEdgeId, TNode, TProperty> 
    : IEdge<TNode>
    , IIdentifiable<TEdgeId>
{
    TProperty Property { get; }

}
