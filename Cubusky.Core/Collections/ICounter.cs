using System;
using System.Collections;

namespace Cubusky.Collections
{
    /// <summary>Represents a nongeneric collection of item/count pairs.</summary>
    public interface ICounter : ICollection, IEnumerable
    {
        /// <summary>Gets or sets the count of the specified item.</summary>
        /// <param name="item">The item of the count to get or set.</param>
        /// <returns>The count of the specified item or zero if absent.</returns>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="ArgumentException">item is of the wrong type.</exception>
        /// <exception cref="NotSupportedException">The property is set and the <see cref="ICounter"/> object is read-only. -or- The property is set, item does not exist in the collection, and the <see cref="ICounter"/> has a fixed size.</exception>
        int this[object item] { get; set; }

        /// <summary>Gets a value indicating whether the <see cref="ICounter"/> object has a fixed size.</summary>
        /// <returns><see langword="true"/> if the <see cref="ICounter"/> object has a fixed size; otherwise, <see langword="false"/>.</returns>
        bool IsFixedSize { get; }

        /// <summary>Gets a value indicating whether the <see cref="ICounter"/> object is read-only.</summary>
        /// <returns><see langword="true"/> if the <see cref="ICounter"/> object is read-only; otherwise, <see langword="false"/>.</returns>
        bool IsReadOnly { get; }

        /// <summary>Gets an <see cref="ICollection"/> object containing the items in the <see cref="ICounter"/> object.</summary>
        /// <returns>An <see cref="ICollection"/> object containing the items in the <see cref="ICounter"/> object.</returns>
        ICollection Items { get; }

        /// <summary>Gets an <see cref="ICollection"/> object containing the counts in the <see cref="ICounter"/> object.</summary>
        /// <returns>An <see cref="ICollection"/> object containing the counts in the <see cref="ICounter"/> object.</returns>
        ICollection Counts { get; }

        /// <summary>Gets or creates the element with the provided item in the <see cref="ICounter"/> object and increments the count of that element.</summary>
        /// <param name="item">The <see cref="object"/> to use as the item of the element to get or add.</param>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="ICounter"/> is read-only. -or- The <see cref="ICounter"/> has a fixed size.</exception>
        void Add(object item);

        /// <summary>Gets or creates the element with the provided item in the <see cref="ICounter"/> object and adds the provided count to that element.</summary>
        /// <param name="item">The <see cref="object"/> to use as the item of the element to get or add.</param>
        /// <param name="count">The <see cref="int"/> to add to the count of the element.</param>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">count is negative or zero.</exception>
        /// <exception cref="NotSupportedException">The <see cref="ICounter"/> is read-only. -or- The <see cref="ICounter"/> has a fixed size.</exception>
        void Add(object item, int count);

        /// <summary>Removes all elements from the <see cref="ICounter"/> object.</summary>
        /// <exception cref="NotSupportedException">The <see cref="ICounter"/> object is read-only.</exception>
        void Clear();

        /// <summary>Determines whether the <see cref="ICounter"/> object contains an element with the specified item.</summary>
        /// <param name="item">The item to locate in the <see cref="ICounter"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ICounter"/> contains an element with the item; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        bool Contains(object item);

        /// <summary>Determines whether the <see cref="ICounter"/> object contains an element with the specified item and count.</summary>
        /// <param name="item">The item to locate in the <see cref="ICounter"/> object.</param>
        /// <param name="count">The count to locate in the item.</param>
        /// <returns><see langword="true"/> if the <see cref="ICounter"/> contains an element with the item and count; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">count is negative.</exception>
        bool Contains(object item, int count);

        /// <summary>Returns <see cref="IDictionaryEnumerator"/> object for the <see cref="ICounter"/> object.</summary>
        /// <returns>An <see cref="IDictionaryEnumerator"/> object for the <see cref="ICounter"/> object.</returns>
        new IDictionaryEnumerator GetEnumerator();

        /// <summary>Gets the element with the provided item in the <see cref="ICounter"/> object, decrements the count of that element, and removes the element from the <see cref="ICounter"/> object if the remainder is zero.</summary>
        /// <param name="item">The <see cref="object"/> to use as the item of the element to get or remove.</param>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="ICounter"/> is read-only. -or- The <see cref="ICounter"/> has a fixed size.</exception>
        void Remove(object item);

        /// <summary>Gets the element with the provided item in the <see cref="ICounter"/> object, subtracts the count of that element, and removes the element from the <see cref="ICounter"/> object if the remainder is negative to zero.</summary>
        /// <param name="item">The <see cref="object"/> to use as the item of the element to get or remove.</param>
        /// <param name="count">The <see cref="int"/> to substract from the count of the element.</param>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">count is negative or zero.</exception>
        /// <exception cref="NotSupportedException">The <see cref="ICounter"/> is read-only. -or- The <see cref="ICounter"/> has a fixed size.</exception>
        void Remove(object item, int count);
    }
}
