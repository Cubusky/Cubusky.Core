using System;
using System.Collections;
using System.Collections.Generic;

namespace Cubusky.Collections.Generic
{
    /// <summary>Represents a generic collection of item/count pairs.</summary>
    /// <typeparam name="TItem">The type of items in the counter.</typeparam>
    public interface ICounter<TItem> : ICollection<ItemCountPair<TItem>>, IEnumerable<ItemCountPair<TItem>>, IEnumerable
    {
        /// <summary>Gets or sets the count of the specified item.</summary>
        /// <param name="item">The item of the count to get or set.</param>
        /// <returns>The count of the specified item or zero if absent.</returns>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">value is negative.</exception>
        /// <exception cref="NotSupportedException">The property is set and the <see cref="ICounter{TItem}"/> object is read-only.</exception>
        int this[TItem item] { get; set; }

        /// <summary>Gets an <see cref="ICollection{T}"/> containing the items of the <see cref="ICounter{TItem}"/>.</summary>
        /// <returns>An <see cref="ICollection{T}"/> containing the items of the object that implements <see cref="ICounter{TItem}"/>.</returns>
        ICollection<TItem> Items { get; }

        /// <summary>Gets an <see cref="ICollection{Int32}"/> containing the counts in the <see cref="ICounter{TItem}"/>.</summary>
        /// <returns>An <see cref="ICollection{Int32}"/> containing the counts in the object that implements <see cref="ICounter{TItem}"/>.</returns>
        ICollection<int> Counts { get; }

        /// <summary>Gets or creates the element with the provided item in the <see cref="ICounter{TItem}"/> and increments the count of that element.</summary>
        /// <param name="item">The <see cref="object"/> to use as the item of the element to get or add.</param>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="ICounter{TItem}"/> is read-only.</exception>
        void Add(TItem item);

        /// <summary>Gets or creates the element with the provided item in the <see cref="ICounter{TItem}"/> and adds the provided count to that element.</summary>
        /// <param name="item">The <see cref="object"/> to use as the item of the element to get or add.</param>
        /// <param name="count">The <see cref="int"/> to add to the count of the element.</param>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">count is negative or zero.</exception>
        /// <exception cref="NotSupportedException">The <see cref="ICounter{TItem}"/> is read-only.</exception>
        void Add(TItem item, int count);

        /// <summary>Determines whether the <see cref="ICounter{TItem}"/> contains an element with the specified item.</summary>
        /// <param name="item">The item to locate in the <see cref="ICounter{TItem}"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="ICounter{TItem}"/> contains an element with the item; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        bool Contains(TItem item);

        /// <summary>Determines whether the <see cref="ICounter{TItem}"/> contains an element with the specified item and count.</summary>
        /// <param name="item">The item to locate in the <see cref="ICounter{TItem}"/>.</param>
        /// <param name="count">The count to locate in the item.</param>
        /// <returns><see langword="true"/> if the <see cref="ICounter{TItem}"/> contains an element with the item and count; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">count is negative or zero.</exception>
        bool Contains(TItem item, int count);

        /// <summary>Gets the element with the provided item in the <see cref="ICounter{TItem}"/>, decrements the count of that element, and removes the element from the <see cref="ICounter{TItem}"/> if the remainder is zero.</summary>
        /// <param name="item">The item of the element to get or remove.</param>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="NotSupportedException">The <see cref="ICounter{TItem}"/> is read-only.</exception>
        bool Remove(TItem item);

        /// <summary>Gets the element with the provided item in the <see cref="ICounter{TItem}"/>, subtracts from the count of that element, and removes the element from the <see cref="ICounter{TItem}"/> if the remainder is negative to zero.</summary>
        /// <param name="item">The item of the element to get or remove.</param>
        /// <param name="count">The <see cref="int"/> to substract from the count of the element.</param>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">count is negative or zero.</exception>
        /// <exception cref="NotSupportedException">The <see cref="ICounter{TItem}"/> is read-only.</exception>
        bool Remove(TItem item, int count);

        void ICollection<ItemCountPair<TItem>>.Add(ItemCountPair<TItem> item) => Add(item.Item, item.Count);
        bool ICollection<ItemCountPair<TItem>>.Contains(ItemCountPair<TItem> item) => Contains(item.Item, item.Count);
        bool ICollection<ItemCountPair<TItem>>.Remove(ItemCountPair<TItem> item) => Remove(item.Item, item.Count);
    }

    /// <summary>Represents a generic read-only collection of item/count pairs.</summary>
    /// <typeparam name="TItem">The type of items in the read-only counter.</typeparam>
    public interface IReadOnlyCounter<TItem> : IReadOnlyCollection<ItemCountPair<TItem>>, IEnumerable<ItemCountPair<TItem>>, IEnumerable
    {
        /// <summary>Gets the count of the specified item in the read-only counter.</summary>
        /// <param name="item">The item of the count to get.</param>
        /// <returns>The count of the specified item in the read-only counter or zero if absent.</returns>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        int this[TItem item] { get; }

        /// <summary>Gets an enumerable collection that contains the items in the read-only counter.</summary>
        /// <returns>An enumerable collection that contains the items in the read-only counter.</returns>
        IEnumerable<TItem> Items { get; }

        /// <summary>Gets an enumerable collection that contains the counts in the read-only counter.</summary>
        /// <returns>An enumerable collection that contains the counts in the read-only counter.</returns>
        IEnumerable<int> Counts { get; }

        /// <summary>Determines whether the read-only counter contains an element that has the specified item.</summary>
        /// <param name="item">The item to locate.</param>
        /// <returns><see langword="true"/> if the read-only counter contains an element that has the specified item; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        bool Contains(TItem item);

        /// <summary>Determines whether the read-only counter contains an element that has the specified item and count.</summary>
        /// <param name="item">The item to locate.</param>
        /// <param name="count">The count to locate in the item.</param>
        /// <returns><see langword="true"/> if the read-only counter contains an element that has the specified item and count; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">item is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">count is negative or zero.</exception>
        bool Contains(TItem item, int count);
    }
}
