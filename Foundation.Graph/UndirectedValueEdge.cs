using System.Diagnostics.CodeAnalysis;

namespace Foundation.Graph;

public static class UndirectedValueEdge
{
    public static UndirectedValueEdge<TNode> New<TNode>([DisallowNull] TNode source, [DisallowNull] TNode target)
        where TNode : notnull
    {
        return new UndirectedValueEdge<TNode>(source, target);
    }
}

public struct UndirectedValueEdge<TNode> 
    : IEdge<TNode>
    , IEquatable<UndirectedValueEdge<TNode>>
    where TNode : notnull
{
    private readonly int _hashCode;

    public UndirectedValueEdge(TNode source, TNode target)
    {
        Source = source;
        Target = target;

        _hashCode = HashCode.FromOrderedObject(Source, Target);
    }

    public static bool operator ==(UndirectedValueEdge<TNode> left, UndirectedValueEdge<TNode> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UndirectedValueEdge<TNode> left, UndirectedValueEdge<TNode> right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is UndirectedValueEdge<TNode> other && Equals(other);

    public bool Equals(UndirectedValueEdge<TNode> other) => this.EqualsUndirected<TNode, UndirectedValueEdge<TNode>>(other);

    public override int GetHashCode() => _hashCode;

    public TNode Source { get; }
    public TNode Target { get; }

    public override string ToString() => $"{Source}-{Target}";
}

public struct UndirectedValueEdge<TId, TNode>
    : IEdge<TId, TNode>
    , IEquatable<UndirectedValueEdge<TId, TNode>>
    where TId : notnull
    where TNode : notnull
{
    private readonly int _hashCode;

    public UndirectedValueEdge(TId id, TNode source, TNode target)
    {
        Id = id;
        Source = source;
        Target = target;

        _hashCode = HashCode.FromOrderedObject<object>(Id, Source, Target);
    }

    public static bool operator ==(UndirectedValueEdge<TId, TNode> left, UndirectedValueEdge<TId, TNode> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UndirectedValueEdge<TId, TNode> left, UndirectedValueEdge<TId, TNode> right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is UndirectedEdge<TId, TNode> other && Equals(other);

    public bool Equals(UndirectedValueEdge<TId, TNode> other) => this.EqualsUndirected<TNode, UndirectedValueEdge<TId, TNode>>(other);

    public override int GetHashCode() => _hashCode;

    public TId Id { get; }
    public TNode Source { get; }
    public TNode Target { get; }

    public override string ToString() => $"Id: {Id}, {Source}-{Target}";
}
