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

public class IdNode
{
    public static IdNode<Guid, TNode> New<TNode>(TNode node) => New(Guid.NewGuid(), node);

    public static IdNode<Guid, TNode> New<TNode>(Guid id, TNode node) => new(id, node);

    public static IdNode<TId, TNode> New<TId, TNode>(TId id, TNode node) where TId : IComparable<TId>, IEquatable<TId> => new (id, node);
}

public readonly struct IdNode<TId, TState> 
    : IIdentifiable<TId>
    , IComparable<IdNode<TId, TState>>
    , IEquatable<IdNode<TId, TState>>
    where TId : IComparable<TId>, IEquatable<TId>
{
    public IdNode(TId id, TState node)
    {
        Id = id.ThrowIfNull();
        State = node.ThrowIfNull();
    }

    public static bool operator ==(IdNode<TId, TState> lhs, IdNode<TId, TState> rhs) => lhs.Equals(rhs);

    public static bool operator !=(IdNode<TId, TState> lhs, IdNode<TId, TState> rhs) => !(lhs == rhs);

    public static bool operator >(IdNode<TId, TState> lhs, IdNode<TId, TState> rhs) => lhs.CompareTo(rhs) == 1;

    public static bool operator >=(IdNode<TId, TState> lhs, IdNode<TId, TState> rhs) => lhs.CompareTo(rhs) is >= 0;

    public static bool operator <(IdNode<TId, TState> lhs, IdNode<TId, TState> rhs) => lhs.CompareTo(rhs) == -1;

    public static bool operator <=(IdNode<TId, TState> lhs, IdNode<TId, TState> rhs) => lhs.CompareTo(rhs) is <= 0;

    public TId Id { get; }

    public int CompareTo(IdNode<TId, TState> other) => Id.CompareTo(other.Id);

    public bool Equals(IdNode<TId, TState> other) => Id.Equals(other.Id);

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is IdNode<TId, TState> other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();

    public TState State { get; }

    public override string ToString() => $"Id={Id}";
}
