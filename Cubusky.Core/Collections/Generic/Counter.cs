using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Cubusky.Collections.Generic
{
    /// <summary>Represents a collection of item/count pairs.</summary>
    /// <typeparam name="TItem">The type of the items in the counter.</typeparam>
    public class Counter<TItem> : ICounter<TItem>, IReadOnlyCounter<TItem>, ICounter, IDeserializationCallback
#if !NET8_0_OR_GREATER
        , ISerializable
#endif
        where TItem : notnull
    {
        private Dictionary<TItem, int> CountByItem { get; }

        /// <summary>Gets the <see cref="IEqualityComparer{T}"/> that is used to determine equality of the items in the counter.</summary>
        /// <returns>The <see cref="IEqualityComparer{T}"/> generic interface implementation that is used to determine equality of items for the current <see cref="Counter{TItem}"/> and to provide hash values for the items.</returns>
        public IEqualityComparer<TItem> Comparer => CountByItem.Comparer;

        /// <summary>Gets the number of item/count pairs contained in the <see cref="Counter{TItem}"/>.</summary>
        /// <returns>The number of item/count pairs contained in the <see cref="Counter{TItem}"/>.</returns>
        public int Count => CountByItem.Count;

        /// <summary>Gets a collection containing the items in the <see cref="Counter{TItem}"/>.</summary>
        /// <returns>A <see cref="Dictionary{TItem, Int32}.KeyCollection"/> containing the items in the <see cref="Counter{TItem}"/>.</returns>
        public Dictionary<TItem, int>.KeyCollection Items => CountByItem.Keys;
        ICollection<TItem> ICounter<TItem>.Items => Items;
        IEnumerable<TItem> IReadOnlyCounter<TItem>.Items => Items;

        /// <summary>Gets a collection containing the counts in the <see cref="Counter{TItem}"/>.</summary>
        /// <returns>An <see cref="Dictionary{TItem, Int32}.Values"/> containing the counts in the <see cref="Counter{TItem}"/>.</returns>
        public Dictionary<TItem, int>.ValueCollection Counts => CountByItem.Values;
        ICollection<int> ICounter<TItem>.Counts => Counts;
        IEnumerable<int> IReadOnlyCounter<TItem>.Counts => Counts;

        bool ICollection<ItemCount<TItem>>.IsReadOnly => ((ICollection<KeyValuePair<TItem, int>>)CountByItem).IsReadOnly;

        #region Conversion
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ItemCount<TItem> ToItemCount(KeyValuePair<TItem, int> pair) => Unsafe.As<KeyValuePair<TItem, int>, ItemCount<TItem>>(ref pair);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static KeyValuePair<TItem, int> ToKeyValuePair(ItemCount<TItem> pair) => Unsafe.As<ItemCount<TItem>, KeyValuePair<TItem, int>>(ref pair);
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that is empty, has the default initial capacity, and uses the default equality comparer for the item type.</summary>
        public Counter() => CountByItem = new Dictionary<TItem, int>();

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that is empty, has the specified initial capacity, and uses the default equality comparer for the item type.</summary>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than 0.</exception>
        /// <inheritdoc cref="Counter_doc"/>
        public Counter(int capacity) => CountByItem = new Dictionary<TItem, int>(capacity);

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that contains the elements copied from the specified <see cref="IEnumerable{T}"/>.</summary>
        /// <exception cref="ArgumentNullException">collection is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="Counter_doc"/>
        public Counter(IEnumerable<ItemCount<TItem>> collection) => CountByItem = new Dictionary<TItem, int>(collection.Select(ToKeyValuePair));

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that is empty, has the specified initial capacity, and uses the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="Counter_doc"/>
        public Counter(IEqualityComparer<TItem>? comparer) => CountByItem = new Dictionary<TItem, int>(comparer);

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that contains the elements copied from the specified <see cref="ICounter{TItem}"/> and uses the default equality comparer for the item type.</summary>
        /// <exception cref="ArgumentNullException">counter is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="Counter_doc"/>
        public Counter(ICounter<TItem> counter) => CountByItem = new Dictionary<TItem, int>(counter.Select(ToKeyValuePair));

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that is empty, has the specified initial capacity, and uses the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than 0.</exception>
        /// <inheritdoc cref="Counter_doc"/>
        public Counter(int capacity, IEqualityComparer<TItem>? comparer) => CountByItem = new Dictionary<TItem, int>(capacity, comparer);

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that contains the elements copied from the specified <see cref="IEnumerable{T}"/> and uses the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <exception cref="ArgumentNullException">collection is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="Counter_doc"/>
        public Counter(IEnumerable<ItemCount<TItem>> collection, IEqualityComparer<TItem>? comparer) => CountByItem = new Dictionary<TItem, int>(collection.Select(ToKeyValuePair), comparer);

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that contains the elements copied from the specified <see cref="ICounter{TItem}"/> and uses the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <exception cref="ArgumentNullException">counter is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="Counter_doc"/>
        public Counter(ICounter<TItem> counter, IEqualityComparer<TItem>? comparer) => CountByItem = new Dictionary<TItem, int>(counter.Select(ToKeyValuePair), comparer);

        /// <param name="capacity">The initial number of elements that the <see cref="Counter{TItem}"/> can contain.</param>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to the new <see cref="Counter{TItem}"/>.</param>
        /// <param name="counter">The <see cref="ICounter{TItem}"/> whose elements are copied to the new <see cref="Counter{TItem}"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing items, or null to use the default <see cref="EqualityComparer{T}"/> for the type of the item.</param>
        private static void Counter_doc(int capacity, IEnumerable<ItemCount<TItem>> collection, ICounter<TItem> counter, IEqualityComparer<TItem>? comparer) { }
        #endregion

        #region Generic Methods
        /// <inheritdoc/>
        public int this[TItem item]
        {
            get => CountByItem.GetValueOrDefault(item);
            set
            {
                if (value == 0)
                {
                    CountByItem.Remove(item);
                }
                else
                {
                    Throw.IfArgumentNegative(value, nameof(value));
                    CountByItem[item] = value;
                }
            }
        }

        /// <inheritdoc/>
        public void Add(TItem item) => AddInternal(item, 1);

        /// <inheritdoc/>
        public void Add(TItem item, int count)
        {
            Throw.IfArgumentNegativeOrZero(count, nameof(count));
            AddInternal(item, count);
        }

        internal void AddInternal(TItem item, int count)
        {
            if (!CountByItem.TryAdd(item, count))
            {
                CountByItem[item] += count;
            }
        }

        /// <inheritdoc/>
        public bool Contains(TItem item) => CountByItem.ContainsKey(item);

        /// <inheritdoc/>
        public bool Contains(TItem item, int count)
        {
            Throw.IfArgumentNegativeOrZero(count, nameof(count));
            return CountByItem.TryGetValue(item, out var value) && value == count;
        }

        /// <inheritdoc/>
        public bool Remove(TItem item) => RemoveInternal(item, 1);

        /// <inheritdoc/>
        public bool Remove(TItem item, int count)
        {
            Throw.IfArgumentNegativeOrZero(count, nameof(count));
            return RemoveInternal(item, count);
        }

        internal bool RemoveInternal(TItem item, int count) => CountByItem.ContainsKey(item)
            && ((CountByItem[item] -= count) > 0 || CountByItem.Remove(item));

        /// <summary>Removes all items and counts from the <see cref="Counter{TItem}"/>.</summary>
        public void Clear() => CountByItem.Clear();

        /// <summary>Ensures that the counter can hold up to 'capacity' entries without any further expansion of its backing storage.</summary>
        /// <param name="capacity">The number of entries.</param>
        /// <returns>The current capacity of the <see cref="Counter{TItem}"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">capacity is negative.</exception>
        public int EnsureCapacity(int capacity) => CountByItem.EnsureCapacity(capacity);

        /// <summary>Sets the capacity of this counter to what it would be if it had been originally initialized with all its entries.</summary>
        /// <remarks>
        /// This method can be used to minimize the memory overhead once it is known that no new elements will be added.<br/>
        /// To allocate minimum size storage array, execute the following statements:<br/>
        /// counter.Clear();<br/>
        /// counter.TrimExcess();<br/>
        /// </remarks>
        public void TrimExcess() => CountByItem.TrimExcess();

        /// <summary>Sets the capacity of this counter to hold up 'capacity' entries without any further expansion of its backing storage.</summary>
        /// <param name="capacity">The new capacity.</param>
        /// <remarks>This method can be used to minimize the memory overheadonce it is known that no new elements will be added.</remarks>
        /// <exception cref="ArgumentOutOfRangeException">capacity is negative.</exception>
        public void TrimExcess(int capacity) => CountByItem.TrimExcess(capacity);

        void ICollection<ItemCount<TItem>>.CopyTo(ItemCount<TItem>[] array, int index)
        {
            Throw.IfArgumentNull(array, nameof(array));
            ThrowIfArrayLengthInsufficient(array, index, Count);

            foreach (var pair in this)
            {
                array[index++] = pair;
            }
        }

        /// <summary>Returns an enumerator that iterates through the <see cref="Counter{TItem}"/>.</summary>
        /// <returns>A <see cref="Dictionary{TItem, Int32}.Enumerator"/> structure for the <see cref="Counter{TItem}"/>.</returns>
        public IEnumerator<ItemCount<TItem>> GetEnumerator() => CountByItem.Select(ToItemCount).GetEnumerator();

        IEnumerator<ItemCount<TItem>> IEnumerable<ItemCount<TItem>>.GetEnumerator() => Count == 0
            ? Enumerable.Empty<ItemCount<TItem>>().GetEnumerator()
            : GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<ItemCount<TItem>>)this).GetEnumerator();
        #endregion

        #region nongeneric Methods
        private static TItem CastOrThrow(object item)
        {
            try
            {
                return (TItem)item;
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("The argument is of the wrong type.", nameof(item));
            }
        }

        private static bool IsTItem(object item)
        {
            Throw.IfArgumentNull(item, nameof(item));
            return item is TItem;
        }

        ICollection ICounter.Items => Items;
        ICollection ICounter.Counts => Counts;
        bool ICollection.IsSynchronized => ((ICollection)CountByItem).IsSynchronized;
        object ICollection.SyncRoot => ((ICollection)CountByItem).SyncRoot;

        bool ICounter.IsFixedSize => ((IDictionary)CountByItem).IsFixedSize;
        bool ICounter.IsReadOnly => ((IDictionary)CountByItem).IsReadOnly;

        int ICounter.this[object item]
        {
            get => IsTItem(item) ? this[(TItem)item] : 0;
            set => this[CastOrThrow(item)] = value;
        }

        void ICounter.Add(object item) => Add(CastOrThrow(item));

        void ICounter.Add(object item, int count) => Add(CastOrThrow(item), count);

        bool ICounter.Contains(object item) => IsTItem(item) && Contains((TItem)item);

        bool ICounter.Contains(object item, int count)
        {
            Throw.IfArgumentNegativeOrZero(count, nameof(count));
            return IsTItem(item) && Contains((TItem)item, count);
        }

        void ICounter.Remove(object item)
        {
            if (IsTItem(item))
            {
                Remove((TItem)item);
            }
        }

        void ICounter.Remove(object item, int count)
        {
            if (IsTItem(item))
            {
                Remove((TItem)item, count);
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            Throw.IfArgumentNull(array, nameof(array));
            ThrowIfArrayNotSupported(array);
            ThrowIfArrayLengthInsufficient(array, index, Count);

            if (array is ItemCount<TItem>[] pairs)
            {
                foreach (var pair in this)
                {
                    pairs[index++] = pair;
                }
            }
            else if (array is DictionaryEntry[] dictionaryEntryArray)
            {
                foreach (var pair in this)
                {
                    dictionaryEntryArray[index++] = new DictionaryEntry(pair.Item, pair.Count);
                }
            }
            else
            {
                var objects = array as object[] ?? throw IncompatibleArrayType();

                try
                {
                    foreach (var pair in this)
                    {
                        objects[index++] = pair;
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw IncompatibleArrayType();
                }

                static ArgumentException IncompatibleArrayType() => new ArgumentException("Incompatible array type");
            }
        }

        IDictionaryEnumerator ICounter.GetEnumerator() => CountByItem.GetEnumerator();
        #endregion

        #region Serialization / Deserialization
        /// <inheritdoc/>
        public virtual void OnDeserialization(object? sender) => ((IDeserializationCallback)CountByItem).OnDeserialization(sender);

#if !NET8_0_OR_GREATER
        /// <inheritdoc/>
        public void GetObjectData(SerializationInfo info, StreamingContext context) => ((ISerializable)CountByItem).GetObjectData(info, context);
#endif
        #endregion

        #region Throw Helpers
        private static void ThrowIfArrayLengthInsufficient(Array array, int index, int count)
        {
            Throw.IfArgumentGreaterThan((uint)index, (uint)array.Length);
            if (array.Length - index < count)
            {
                throw new ArgumentException("Array length plus index is too small to copy to.");
            }
        }

        private static void ThrowIfArrayNotSupported(Array array)
        {
            if (array.Rank != 1)
            {
                throw new ArgumentException("Multidimensional array not supported.");
            }
            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("Array has nonzero lower bound.");
            }
        }
        #endregion
    }
}
