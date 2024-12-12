using System;
using System.ComponentModel;

namespace Cubusky.Collections.Generic
{
    /// <summary>Creates instances of the <see cref="ItemCountPair{TItem}"/> struct.</summary>
    public static class ItemCountPair
    {
        /// <summary>Creates a new item/count pair instance using provided values.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="item">The item of the new <see cref="ItemCountPair{TItem}"/> to be created.</param>
        /// <param name="count">The count of the new <see cref="ItemCountPair{TItem}"/> to be created.</param>
        /// <returns>An item/count pair containing the provided arguments as values.</returns>
        public static ItemCountPair<TItem> Create<TItem>(TItem item, int count) =>
            new ItemCountPair<TItem>(item, count);

        /// <summary>Used by <see cref="ItemCountPair{TItem}.ToString"/> to reduce generic code</summary>
        internal static string PairToString(object? item, object? count) =>
#if NET8_0_OR_GREATER
            string.Create(null, stackalloc char[256], $"[{item}, {count}]");
#else
            $"[{item}, {count}]";
#endif
    }

    /// <summary>Defines an item/count pair that can be set or retrieved.</summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    [Serializable]
    public readonly struct ItemCountPair<TItem>
    {
        /// <summary>Gets the item in the item/count pair.</summary>
        /// <returns>A <typeparamref name="TItem"/> that is the item of the <see cref="ItemCountPair{TItem}"/>.</returns>
        public TItem Item { get; }

        /// <summary>Gets the count in the item/count pair.</summary>
        /// <returns>An <see langword="int"/> that is the count of the <see cref="ItemCountPair{TItem}"/>.</returns>
        public int Count { get; }

        /// <summary>Initializes a new instance of the <see cref="ItemCountPair{TItem}"/> structure with the specified item and count.</summary>
        /// <param name="item">The object defined in each item/count pair.</param>
        /// <param name="count">The count associated with the item.</param>
        public ItemCountPair(TItem item, int count)
        {
            Item = item;
            Count = count;
        }

        /// <summary>Returns a string representation of the <see cref="ItemCountPair{TItem}"/>, using the string representations of the item and count.</summary>
        /// <returns>A string representation of the <see cref="ItemCountPair{TItem}"/>, which includes the string representations of the item and count.</returns>
        public override string ToString() => ItemCountPair.PairToString(Item, Count);

        /// <summary>Deconstructs the current <see cref="ItemCountPair{TItem}"/>.</summary>
        /// <param name="item">The item of the current <see cref="ItemCountPair{TItem}"/>.</param>
        /// <param name="count">The count of the current <see cref="ItemCountPair{TItem}"/>.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out TItem item, out int count)
        {
            item = Item;
            count = Count;
        }
    }
}
