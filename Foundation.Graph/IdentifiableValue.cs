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

/// <summary>
/// Identifiable decorator for a value. If you have a graph with node ids, you can use this decorator to add the value to the graph.
/// </summary>
/// <typeparam name="TValue">Typeo of the value.</typeparam>
/// <param name="Id"></param>
/// <param name="Value"></param>
public record IdentifiableValue<TValue>(Guid Id, TValue Value)
    : IdentifiableValue<Guid, TValue>(Id, Value);

/// <summary>
/// Identifiable decorator for a value. If you have a graph with node ids, you can use this decorator to add the value to the graph.
/// </summary>
/// <typeparam name="TId">Type of the id.</typeparam>
/// <typeparam name="TValue">Type of the value.</typeparam>
/// <param name="Id">This is the identifier.</param>
/// <param name="Value">This is a value.</param>
public record IdentifiableValue<TId, TValue>(TId Id, TValue Value)
    : IIdentifiable<TId>
    where TId : notnull;