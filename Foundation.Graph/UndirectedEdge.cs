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
﻿using System.Diagnostics.CodeAnalysis;

namespace Foundation.Graph;

public static class UndirectedEdge
{
    public static UndirectedEdge<TNode> New<TNode>([DisallowNull] TNode source, [DisallowNull] TNode target)
        where TNode : notnull
    {
        return new UndirectedEdge<TNode>(source, target);
    }
}

public class UndirectedEdge<TNode> 
    : IEdge<TNode>
    , IEquatable<UndirectedEdge<TNode>>
{
    private readonly int _hashCode;

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

    public override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as UndirectedEdge<TNode>);

    public bool Equals(UndirectedEdge<TNode>? other) => this.EqualsUndirected<TNode, UndirectedEdge<TNode>>(other);

    public override int GetHashCode() => _hashCode;

    public TNode Source { get; }
    public TNode Target { get; }

    public override string ToString() => $"{Source}-{Target}";
}

public struct UndirectedEdge<TId, TNode>
    : IEdge<TId, TNode>
    , IEquatable<UndirectedEdge<TId, TNode>>
    where TId : notnull
    where TNode : notnull
{
    private readonly int _hashCode;

    public UndirectedEdge(TId id, TNode source, TNode target)
    {
        Id = id;
        Source = source;
        Target = target;

        _hashCode = HashCode.FromOrderedObject<object>(Id, Source, Target);
    }

    public static bool operator ==(UndirectedEdge<TId, TNode> left, UndirectedEdge<TId, TNode> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UndirectedEdge<TId, TNode> left, UndirectedEdge<TId, TNode> right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is UndirectedEdge<TId, TNode> other && Equals(other);

    public bool Equals(UndirectedEdge<TId, TNode> other) => this.EqualsUndirected<TNode, UndirectedEdge<TId, TNode>>(other);

    public override int GetHashCode() => _hashCode;

    public TId Id { get; }
    public TNode Source { get; }
    public TNode Target { get; }

    public override string ToString() => $"Id: {Id}, {Source}-{Target}";
}
