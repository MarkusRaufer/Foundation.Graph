namespace Foundation.Graph;

public interface IPropertyEdge<TNode, TKey, TValue> : IEdge<TNode>
{
    IDictionary<TKey, TValue> Properties { get; }
}

public interface IPropertyEdge<TId, TNode, TKey, TValue>
    : IPropertyEdge<TNode, TKey, TValue>
    , IIdentifiable<TId>
    where TId : notnull
{
}
