using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Cubusky.Collections.Generic
{
    /// <summary>Represents a collection of items and counts.</summary>
    /// <typeparam name="TItem">The type of the items in the counter.</typeparam>
    public class Counter<TItem> : ICounter<TItem>, IReadOnlyCounter<TItem>, ICounter, IDeserializationCallback
#if !NET8_0_OR_GREATER
        , ISerializable
#endif
        where TItem : notnull
    {
        private Dictionary<TItem, int> CountByItem { get; }
        private ItemCollection? items;
        private CountCollection? counts;

        /// <summary>Gets the <see cref="IEqualityComparer{T}"/> that is used to determine equality of the items in the counter.</summary>
        /// <returns>The <see cref="IEqualityComparer{T}"/> generic interface implementation that is used to determine equality of items for the current <see cref="Counter{TItem}"/> and to provide hash values for the items.</returns>
        public IEqualityComparer<TItem> Comparer => CountByItem.Comparer;

        /// <summary>Gets the number of item/count pairs contained in the <see cref="Counter{TItem}"/>.</summary>
        /// <returns>The number of item/count pairs contained in the <see cref="Counter{TItem}"/>.</returns>
        public int Count => CountByItem.Count;

        /// <summary>Gets a collection containing the items in the <see cref="Counter{TItem}"/>.</summary>
        /// <returns>An <see cref="Counter{TItem}"/>.<see cref="Counter{TItem}.ItemCollection"/> containing the items in the <see cref="Counter{TItem}"/>.</returns>
        public ItemCollection Items => items ??= new ItemCollection(this);
        ICollection<TItem> ICounter<TItem>.Items => Items;
        IEnumerable<TItem> IReadOnlyCounter<TItem>.Items => Items;

        /// <summary>Gets a collection containing the counts in the <see cref="Counter{TItem}"/>.</summary>
        /// <returns>An <see cref="Counter{TItem}"/>.<see cref="Counter{TItem}.CountCollection"/> containing the counts in the <see cref="Counter{TItem}"/>.</returns>
        public CountCollection Counts => counts ??= new CountCollection(this);
        ICollection<int> ICounter<TItem>.Counts => Counts;
        IEnumerable<int> IReadOnlyCounter<TItem>.Counts => Counts;

        bool ICollection<ItemCountPair<TItem>>.IsReadOnly => ((ICollection<KeyValuePair<TItem, int>>)CountByItem).IsReadOnly;

        private static KeyValuePair<TItem, int> ItemCountToKeyValuePair(ItemCountPair<TItem> itemCount) => new KeyValuePair<TItem, int>(itemCount.Item, itemCount.Count);
        private static ItemCountPair<TItem> KeyValuePairToItemCount(KeyValuePair<TItem, int> keyValuePair) => new ItemCountPair<TItem>(keyValuePair.Key, keyValuePair.Value);

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that is empty, has the default initial capacity, and uses the default equality comparer for the item type.</summary>
        public Counter() => CountByItem = new Dictionary<TItem, int>();

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that is empty, has the specified initial capacity, and uses the default equality comparer for the item type.</summary>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than 0.</exception>
        /// <inheritdoc cref="doc_Counter"/>
        public Counter(int capacity) => CountByItem = new Dictionary<TItem, int>(capacity);

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that contains the elements copied from the specified <see cref="IEnumerable{T}"/>.</summary>
        /// <exception cref="ArgumentNullException">collection is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="doc_Counter"/>
        public Counter(IEnumerable<ItemCountPair<TItem>> collection) => CountByItem = new Dictionary<TItem, int>(collection.Select(ItemCountToKeyValuePair));

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that is empty, has the specified initial capacity, and uses the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <inheritdoc cref="doc_Counter"/>
        public Counter(IEqualityComparer<TItem>? comparer) => CountByItem = new Dictionary<TItem, int>(comparer);

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that contains the elements copied from the specified <see cref="ICounter{TItem}"/> and uses the default equality comparer for the item type.</summary>
        /// <exception cref="ArgumentNullException">counter is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="doc_Counter"/>
        public Counter(ICounter<TItem> counter) => CountByItem = new Dictionary<TItem, int>(counter.Select(ItemCountToKeyValuePair));

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that is empty, has the specified initial capacity, and uses the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <exception cref="ArgumentOutOfRangeException">capacity is less than 0.</exception>
        /// <inheritdoc cref="doc_Counter"/>
        public Counter(int capacity, IEqualityComparer<TItem>? comparer) => CountByItem = new Dictionary<TItem, int>(capacity, comparer);

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that contains the elements copied from the specified <see cref="IEnumerable{T}"/> and uses the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <exception cref="ArgumentNullException">collection is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="doc_Counter"/>
        public Counter(IEnumerable<ItemCountPair<TItem>> collection, IEqualityComparer<TItem>? comparer) => CountByItem = new Dictionary<TItem, int>(collection.Select(ItemCountToKeyValuePair), comparer);

        /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/> class that contains the elements copied from the specified <see cref="ICounter{TItem}"/> and uses the specified <see cref="IEqualityComparer{T}"/>.</summary>
        /// <exception cref="ArgumentNullException">counter is null. -or- item is null.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <inheritdoc cref="doc_Counter"/>
        public Counter(ICounter<TItem> counter, IEqualityComparer<TItem>? comparer) => CountByItem = new Dictionary<TItem, int>(counter.Select(ItemCountToKeyValuePair), comparer);

        /// <param name="capacity">The initial number of elements that the <see cref="Counter{TItem}"/> can contain.</param>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to the new <see cref="Counter{TItem}"/>.</param>
        /// <param name="counter">The <see cref="ICounter{TItem}"/> whose elements are copied to the new <see cref="Counter{TItem}"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing items, or null to use the default <see cref="EqualityComparer{T}"/> for the type of the item.</param>
        internal static void doc_Counter(int capacity, IEnumerable<ItemCountPair<TItem>> collection, ICounter<TItem> counter, IEqualityComparer<TItem>? comparer) { }

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

        void ICollection<ItemCountPair<TItem>>.CopyTo(ItemCountPair<TItem>[] array, int index)
        {
            Throw.IfArgumentNull(array, nameof(array));
            ThrowIfArrayLengthInsufficient(array, index, Count);

            foreach (var pair in this)
            {
                array[index++] = pair;
            }
        }

        /// <summary>Returns an enumerator that iterates through the <see cref="Counter{TItem}"/>.</summary>
        /// <returns>A <see cref="Counter{TItem}"/>.<see cref="Counter{TItem}.Enumerator"/> structure for the <see cref="Counter{TItem}"/>.</returns>
        public Enumerator GetEnumerator() => new Enumerator(this, Enumerator.ReturnType.ItemCountPair);

        IEnumerator<ItemCountPair<TItem>> IEnumerable<ItemCountPair<TItem>>.GetEnumerator() => Count == 0
            ? Enumerable.Empty<ItemCountPair<TItem>>().GetEnumerator()
            : GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<ItemCountPair<TItem>>)this).GetEnumerator();

        /// <inheritdoc/>
        public virtual void OnDeserialization(object? sender) => ((IDeserializationCallback)CountByItem).OnDeserialization(sender);

#if !NET8_0_OR_GREATER
        /// <inheritdoc/>
        public void GetObjectData(SerializationInfo info, StreamingContext context) => ((ISerializable)CountByItem).GetObjectData(info, context);
#endif

        #region nongeneric ICounter
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

            if (array is ItemCountPair<TItem>[] pairs)
            {
                foreach (var pair in this)
                {
                    pairs[index++] = pair;
                }
            }
            else if (array is CounterEntry[] counterEntryArray)
            {
                foreach (var pair in this)
                {
                    counterEntryArray[index++] = new CounterEntry(pair.Item, pair.Count);
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

        ICounterEnumerator ICounter.GetEnumerator() => new Enumerator(this, Enumerator.ReturnType.CounterEntry);
        #endregion

        #region Collections
        /// <summary>Enumerates the elements of a <see cref="Counter{TItem}"/>.</summary>
        public struct Enumerator : IEnumerator<ItemCountPair<TItem>>, ICounterEnumerator
        {
            private Dictionary<TItem, int>.Enumerator inner;

            internal enum ReturnType
            {
                ItemCountPair = 0,
                CounterEntry = 1,
            }

            internal Enumerator(Counter<TItem> counter, ReturnType returnType)
            {
                inner = returnType switch
                {
                    ReturnType.CounterEntry => (Dictionary<TItem, int>.Enumerator)((IDictionary)counter.CountByItem).GetEnumerator(),
                    _ => counter.CountByItem.GetEnumerator()
                };
            }

            /// <summary>Gets the element at the current position of the enumerator.</summary>
            /// <returns>The element in the <see cref="Counter{TItem}"/> at the current position of the enumerator.</returns>
            public ItemCountPair<TItem> Current => KeyValuePairToItemCount(inner.Current);

            /// <summary>Releases all resources used by the <see cref="Counter{TItem}"/>.<see cref="Enumerator"/>.</summary>
            public void Dispose() => inner.Dispose();

            /// <summary>Advances the enumerator to the next element of the <see cref="Counter{TItem}"/>.</summary>
            /// <inheritdoc/>
            public bool MoveNext() => inner.MoveNext();

#pragma warning disable IDE0251 // Make member 'readonly'
            object? IEnumerator.Current => ((IEnumerator)inner).Current;
            void IEnumerator.Reset() => ((IEnumerator)inner).Reset();

            CounterEntry ICounterEnumerator.Entry
            {
                get
                {
                    var entry = ((IDictionaryEnumerator)inner).Entry;
                    return new CounterEntry(entry.Key, (int)entry.Value!);
                }
            }
            object ICounterEnumerator.Item => ((IDictionaryEnumerator)inner).Key;
            int ICounterEnumerator.Count => (int)((IDictionaryEnumerator)inner).Value!;
#pragma warning restore IDE0251 // Make member 'readonly'
        }

        /// <summary>Represents the collection of items in a <see cref="Counter{TItem}"/>. This class cannot be inherited.</summary>
        [DebuggerDisplay("Count = {Count}")]
        public sealed class ItemCollection : ICollection<TItem>, ICollection, IReadOnlyCollection<TItem>
        {
            private readonly Dictionary<TItem, int>.KeyCollection inner;

            /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/> class that reflects the items in the specified <see cref="Counter{TItem}"/>.</summary>
            /// <param name="counter">The <see cref="Counter{TItem}"/> whose items are reflected in the new <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/>.</param>
            /// <exception cref="ArgumentNullException">counter is null.</exception>
            public ItemCollection(Counter<TItem> counter)
            {
                Throw.IfArgumentNull(counter, nameof(counter));
                inner = new Dictionary<TItem, int>.KeyCollection(counter.CountByItem);
            }

            /// <summary>Gets the number of elements contained in the <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/>.</summary>
            /// <returns>The number of elements contained in the <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/>. Retrieving the value of this property is an O(1) operation.</returns>
            public int Count => inner.Count;

            /// <summary>Returns an enumerator that iterates through the <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/>.</summary>
            /// <returns>A <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/>.<see cref="Enumerator"/> for the <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/>.</returns>
            public Enumerator GetEnumerator() => new Enumerator(inner);

            /// <inheritdoc/>
            public bool Contains(TItem item) => inner.Contains(item);

            /// <summary>Copies the <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/> elements to an existing one-dimensional <see cref="Array"/>, starting at the specified array index.</summary>
            /// <inheritdoc cref="Dictionary{TKey, TValue}.KeyCollection.CopyTo(TKey[], int)" />
            public void CopyTo(TItem[] array, int index) => inner.CopyTo(array, index);

            bool ICollection<TItem>.IsReadOnly => ((ICollection<TItem>)inner).IsReadOnly;
            void ICollection<TItem>.Add(TItem item) => ((ICollection<TItem>)inner).Add(item);
            void ICollection<TItem>.Clear() => ((ICollection<TItem>)inner).Clear();
            bool ICollection<TItem>.Remove(TItem item) => ((ICollection<TItem>)inner).Remove(item);
            IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => ((IEnumerable<TItem>)inner).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)inner).GetEnumerator();

            void ICollection.CopyTo(Array array, int index) => ((ICollection)inner).CopyTo(array, index);
            bool ICollection.IsSynchronized => ((ICollection)inner).IsSynchronized;
            object ICollection.SyncRoot => ((ICollection)inner).SyncRoot;

            /// <summary>Enumerates the elements of a <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/>.</summary>
            public struct Enumerator : IEnumerator<TItem>, IEnumerator
            {
                private Dictionary<TItem, int>.KeyCollection.Enumerator inner;

                internal Enumerator(Dictionary<TItem, int>.KeyCollection keyCollection)
                {
                    inner = keyCollection.GetEnumerator();
                }

                /// <summary>Gets the element at the current position of the enumerator.</summary>
                /// <returns>The element in the <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/> at the current position of the enumerator.</returns>
                public TItem Current => inner.Current;

                /// <summary>Releases all resources used by the <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/>.<see cref="Enumerator"/>.</summary>
                public void Dispose() => inner.Dispose();

                /// <summary>Advances the enumerator to the next element of the <see cref="Counter{TItem}"/>.<see cref="ItemCollection"/>.</summary>
                /// <inheritdoc/>
                public bool MoveNext() => inner.MoveNext();

#pragma warning disable IDE0251 // Make member 'readonly'
                object? IEnumerator.Current => ((IEnumerator)inner).Current;
                void IEnumerator.Reset() => ((IEnumerator)inner).Reset();
#pragma warning restore IDE0251 // Make member 'readonly'
            }
        }

        /// <summary>Represents the collection of counts in a <see cref="Counter{TItem}"/>. This class cannot be inherited.</summary>
        [DebuggerDisplay("Count = {Count}")]
        public sealed class CountCollection : ICollection<int>, ICollection, IReadOnlyCollection<int>
        {
            private readonly Dictionary<TItem, int>.ValueCollection inner;

            /// <summary>Initializes a new instance of the <see cref="Counter{TItem}"/>.<see cref="CountCollection"/> class that reflects the items in the specified <see cref="Counter{TItem}"/>.</summary>
            /// <param name="counter">The <see cref="Counter{TItem}"/> whose items are reflected in the new <see cref="Counter{TItem}"/>.<see cref="CountCollection"/>.</param>
            /// <exception cref="ArgumentNullException">counter is null.</exception>
            public CountCollection(Counter<TItem> counter)
            {
                Throw.IfArgumentNull(counter, nameof(counter));
                inner = new Dictionary<TItem, int>.ValueCollection(counter.CountByItem);
            }

            /// <summary>Gets the number of elements contained in the <see cref="Counter{TItem}"/>.<see cref="CountCollection"/>.</summary>
            /// <returns>The number of elements contained in the <see cref="Counter{TItem}"/>.<see cref="CountCollection"/>. Retrieving the value of this property is an O(1) operation.</returns>
            public int Count => inner.Count;

            /// <summary>Returns an enumerator that iterates through the <see cref="Counter{TItem}"/>.<see cref="CountCollection"/>.</summary>
            /// <returns>A <see cref="Counter{TItem}"/>.<see cref="CountCollection"/>.<see cref="Enumerator"/> for the <see cref="Counter{TItem}"/>.<see cref="CountCollection"/>.</returns>
            public Enumerator GetEnumerator() => new Enumerator(inner);

            /// <inheritdoc/>
            /// <exception cref="ArgumentOutOfRangeException">count is negative or zero.</exception>
            bool ICollection<int>.Contains(int count)
            {
                Throw.IfArgumentNegativeOrZero(count, nameof(count));
                return ((ICollection<int>)inner).Contains(count);
            }

            /// <summary>Copies the <see cref="Counter{TItem}"/>.<see cref="CountCollection"/> elements to an existing one-dimensional <see cref="Array"/>, starting at the specified array index.</summary>
            /// <inheritdoc cref="Dictionary{TKey, TValue}.ValueCollection.CopyTo(TValue[], int)" />
            public void CopyTo(int[] array, int index) => inner.CopyTo(array, index);

            bool ICollection<int>.IsReadOnly => ((ICollection<int>)inner).IsReadOnly;
            void ICollection<int>.Add(int count) => ((ICollection<int>)inner).Add(count);
            void ICollection<int>.Clear() => ((ICollection<int>)inner).Clear();
            bool ICollection<int>.Remove(int count) => ((ICollection<int>)inner).Remove(count);
            IEnumerator<int> IEnumerable<int>.GetEnumerator() => ((IEnumerable<int>)inner).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)inner).GetEnumerator();

            void ICollection.CopyTo(Array array, int index) => ((ICollection)inner).CopyTo(array, index);
            bool ICollection.IsSynchronized => ((ICollection)inner).IsSynchronized;
            object ICollection.SyncRoot => ((ICollection)inner).SyncRoot;

            /// <summary>Enumerates the elements of a <see cref="Counter{TItem}"/>.<see cref="CountCollection"/>.</summary>
            public struct Enumerator : IEnumerator<int>, IEnumerator
            {
                private Dictionary<TItem, int>.ValueCollection.Enumerator inner;

                internal Enumerator(Dictionary<TItem, int>.ValueCollection valueCollection)
                {
                    inner = valueCollection.GetEnumerator();
                }

                /// <summary>Gets the element at the current position of the enumerator.</summary>
                /// <returns>The element in the <see cref="Counter{TItem}"/>.<see cref="CountCollection"/> at the current position of the enumerator.</returns>
                public int Current => inner.Current;

                /// <summary>Releases all resources used by the <see cref="Counter{TItem}"/>.<see cref="CountCollection"/>.<see cref="Enumerator"/>.</summary>
                public void Dispose() => inner.Dispose();

                /// <summary>Advances the enumerator to the next element of the <see cref="Counter{TItem}"/>.<see cref="CountCollection"/>.</summary>
                /// <inheritdoc/>
                public bool MoveNext() => inner.MoveNext();

#pragma warning disable IDE0251 // Make member 'readonly'
                object? IEnumerator.Current => ((IEnumerator)inner).Current;
                void IEnumerator.Reset() => ((IEnumerator)inner).Reset();
#pragma warning restore IDE0251 // Make member 'readonly'
            }
        }
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
