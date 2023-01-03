using System.Diagnostics.CodeAnalysis;

namespace Foundation.Graph;

public static class UndirectedEdge
{
    public static UndirectedEdge<TNode> New<TNode>([DisallowNull] TNode source, [DisallowNull] TNode target)
        where TNode : notnull
    {
        return new UndirectedEdge<TNode>(source, target);
    }
}

public struct UndirectedEdge<TNode> 
    : IEdge<TNode>
    , IEquatable<UndirectedEdge<TNode>>
    where TNode : notnull
{
    private int _hashCode;

    public UndirectedEdge(TNode source, TNode target)
    {
        Source = source;
        Target = target;

        _hashCode = HashCode.FromOrderedObject(Source, Target);
    }

    public static bool operator ==(UndirectedEdge<TNode> left, UndirectedEdge<TNode> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UndirectedEdge<TNode> left, UndirectedEdge<TNode> right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is UndirectedEdge<TNode> other && Equals(other);

    public bool Equals(UndirectedEdge<TNode> other)
    {
        return Source.Equals(other.Source) && Target.Equals(other.Target)
            || Source.Equals(other.Target) && Target.Equals(other.Source);
    }

    public override int GetHashCode() => _hashCode;

    public TNode Source { get; }
    public TNode Target { get; }

    public override string ToString() => $"{Source}-{Target}";
}
