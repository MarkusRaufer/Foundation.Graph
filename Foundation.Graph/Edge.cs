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
using System.Diagnostics.CodeAnalysis;

public static class Edge
{
    public static Edge<TNode> New<TNode>([DisallowNull] TNode source, [DisallowNull] TNode target)
        where TNode : notnull
    {
        return new Edge<TNode>(source, target);
    }

    public static Edge<TEdgeId, TNode> New<TEdgeId, TNode>([DisallowNull] TEdgeId id, [DisallowNull] TNode source, [DisallowNull] TNode target)
        where TEdgeId : notnull
        where TNode : notnull
    {
        return new Edge<TEdgeId, TNode>(id, source, target);
    }
}

public record Edge<TNode>(TNode Source, TNode Target)
    : IEdge<TNode>
    where TNode : notnull;

public record Edge<TEdgeId, TNode>(TEdgeId Id, TNode Source, TNode Target) 
    : Edge<TNode>(Source, Target)
    , IEdge<TEdgeId, TNode>
    where TEdgeId : notnull
    where TNode : notnull;
