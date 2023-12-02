// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.HighPerformance.Enumerables;

/// <summary>
/// A <see langword="ref"/> <see langword="struct"/> that tokenizes a given <see cref="Span{T}"/> instance.
/// </summary>
/// <typeparam name="T">The type of items to enumerate.</typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public ref struct SpanTokenizer3<T>
    where T : IEquatable<T>
{
    /// <summary>
    /// The source <see cref="Span{T}"/> instance.
    /// </summary>
    private readonly Span<T> span;

    /// <summary>
    /// One of separators item to use.
    /// </summary>
    private readonly T separator0;

    /// <summary>
    /// One of separators item to use.
    /// </summary>
    private readonly T separator1;

    /// <summary>
    /// One of separators item to use.
    /// </summary>
    private readonly T separator2;

    /// <summary>
    /// The current initial offset.
    /// </summary>
    private int start;

    /// <summary>
    /// The current final offset.
    /// </summary>
    private int end;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpanTokenizer3{T}"/> struct.
    /// </summary>
    /// <param name="span">The source <see cref="Span{T}"/> instance.</param>
    /// <param name="separator0">One of separators item to use.</param>
    /// <param name="separator1">One of separators item to use.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SpanTokenizer3(Span<T> span, T separator0, T separator1, T separator2)
    {
        this.span = span;
        this.separator0 = separator0;
        this.separator1 = separator1;
        this.separator2 = separator2;
        this.start = 0;
        this.end = -1;
    }

    /// <summary>
    /// Implements the duck-typed <see cref="IEnumerable{T}.GetEnumerator"/> method.
    /// </summary>
    /// <returns>An <see cref="SpanTokenizer3{T}"/> instance targeting the current <see cref="Span{T}"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly SpanTokenizer3<T> GetEnumerator() => this;

    /// <summary>
    /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext"/> method.
    /// </summary>
    /// <returns><see langword="true"/> whether a new element is available, <see langword="false"/> otherwise</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool MoveNext()
    {
        int newEnd = this.end + 1;
        int length = this.span.Length;

        // Additional check if the separator is not the last character
        if (newEnd <= length)
        {
            this.start = newEnd;

            int index = this.span.Slice(newEnd).IndexOfAny(this.separator0, this.separator1, this.separator2);

            // Extract the current subsequence
            if (index >= 0)
            {
                this.end = newEnd + index;

                return true;
            }

            this.end = length;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the duck-typed <see cref="IEnumerator{T}.Current"/> property.
    /// </summary>
    public readonly Span<T> Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => this.span.Slice(this.start, this.end - this.start);
    }
}