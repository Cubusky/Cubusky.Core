using System;
using System.Collections.Generic;

namespace Cubusky.Numerics
{
    /// <summary>Exposes the indexer, which supports indexing of a specified type.</summary>
    /// <typeparam name="T">The type of objects to index.</typeparam>
    public interface IIndexable<T> : IEnumerable<T>
    {
#if NET8_0_OR_GREATER
        /// <summary>Get the immutable count for the type.</summary>
        static abstract int Count { get; }
#endif

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="IIndexable{T}"/></exception>
        T this[int index] { get; set; }
    }
}
