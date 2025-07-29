using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Cubusky.Collections.Generic
{
    /// <summary>Creates instances of the <see cref="ItemCount{TItem}"/> struct.</summary>
    public static class ItemCount
    {
        /// <summary>Creates a new item/count pair instance using provided values.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="item">The item of the new <see cref="ItemCount{TItem}"/> to be created.</param>
        /// <param name="count">The count of the new <see cref="ItemCount{TItem}"/> to be created.</param>
        /// <returns>An item/count pair containing the provided arguments as values.</returns>
        public static ItemCount<TItem> Create<TItem>(TItem item, int count) =>
            new ItemCount<TItem>(item, count);

        /// <summary>Used by KeyValuePair.ToString to reduce generic code</summary>
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
    public readonly struct ItemCount<TItem>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TItem item; // Do not rename (binary serialization)

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int count; // Do not rename (binary serialization)

        /// <summary>Initializes a new instance of the <see cref="ItemCount{TItem}"/> structure with the specified item and count.</summary>
        /// <param name="item">The item defined in each item/count pair.</param>
        /// <param name="count">The count associated with the item.</param>
        public ItemCount(TItem item, int count = default)
        {
            this.item = item;
            this.count = count;
        }

        /// <summary>Gets the item of the item/count pair.</summary>
        /// <returns>A <typeparamref name="TItem"/> that is the item of the <see cref="ItemCount{TItem}"/>.</returns>
        public TItem Item => item;

        /// <summary>Gets the count of the item/count pair.</summary>
        /// <returns>A <see cref="int"/> that is the count of the <see cref="ItemCount{TItem}"/>.</returns>
        public int Count => count;

        /// <inheritdoc />
        public override string ToString() => ItemCount.PairToString(Item, Count);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out TItem item, out int count)
        {
            item = Item;
            count = Count;
        }

        /// <inheritdoc />
        public static implicit operator ItemCount<TItem>(TItem item) => new ItemCount<TItem>(item, 1);

        /// <inheritdoc />
        public static implicit operator ItemCount<TItem>(KeyValuePair<TItem, int> tuple) => new ItemCount<TItem>(tuple.Key, tuple.Value);

        /// <inheritdoc />
        public static implicit operator ItemCount<TItem>(ValueTuple<TItem, int> tuple) => new ItemCount<TItem>(tuple.Item1, tuple.Item2);

        /// <inheritdoc />
        public static implicit operator KeyValuePair<TItem, int>(ItemCount<TItem> itemCount) => new KeyValuePair<TItem, int>(itemCount.Item, itemCount.Count);

        /// <inheritdoc />
        public static implicit operator ValueTuple<TItem, int>(ItemCount<TItem> itemCount) => (itemCount.Item, itemCount.Count);
    }
}
