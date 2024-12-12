using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Cubusky.Collections.Generic.Tests
{
    public class CounterTests
    {
        [Fact]
        public void Counter_InitializesWithDefaultConstructor()
        {
            var counter = new Counter<int>();
            Assert.Empty(counter);
        }

        [Fact]
        public void Counter_InitializesWithCapacity()
        {
            var counter = new Counter<int>(10);
            Assert.Empty(counter);
        }

        [Fact]
        public void Counter_InitializesWithCollection()
        {
            var collection = new List<ItemCountPair<int>> { new ItemCountPair<int>(1, 2), new ItemCountPair<int>(2, 3) };
            var counter = new Counter<int>(collection);
            Assert.Equal(2, counter.Count);
            Assert.Equal(2, counter[1]);
            Assert.Equal(3, counter[2]);
        }

        [Fact]
        public void Counter_InitializesWithComparer()
        {
            var counter = new Counter<string>(StringComparer.OrdinalIgnoreCase);
            counter.Add("test");
            Assert.True(counter.Contains("TEST"));
        }

        [Fact]
        public void Counter_AddItem()
        {
            var counter = new Counter<int>();
            counter.Add(1);
            Assert.Equal(1, counter[1]);
        }

        [Fact]
        public void Counter_AddItemWithCount()
        {
            var counter = new Counter<int>();
            counter.Add(1, 5);
            Assert.Equal(5, counter[1]);
        }

        [Fact]
        public void Counter_RemoveItem()
        {
            var counter = new Counter<int>();
            counter.Add(1, 5);
            counter.Remove(1);
            Assert.Equal(4, counter[1]);
        }

        [Fact]
        public void Counter_RemoveItemWithCount()
        {
            var counter = new Counter<int>();
            counter.Add(1, 5);
            counter.Remove(1, 3);
            Assert.Equal(2, counter[1]);
        }

        [Fact]
        public void Counter_ContainsItem()
        {
            var counter = new Counter<int>();
            counter.Add(1);
            Assert.True(counter.Contains(1));
        }

        [Fact]
        public void Counter_ContainsItemWithCount()
        {
            var counter = new Counter<int>();
            counter.Add(1, 5);
            Assert.True(counter.Contains(1, 5));
        }

        [Fact]
        public void Counter_Clear()
        {
            var counter = new Counter<int>();
            counter.Add(1, 5);
            counter.Clear();
            Assert.Empty(counter);
        }

        [Fact]
        public void Counter_EnsureCapacity()
        {
            var counter = new Counter<int>();
            counter.EnsureCapacity(10);
            Assert.Empty(counter);
        }

        [Fact]
        public void Counter_TrimExcess()
        {
            var counter = new Counter<int>();
            counter.Add(1, 5);
            counter.TrimExcess();
            Assert.Single(counter);
        }

        [Fact]
        public void Counter_IndexerGetSet()
        {
            var counter = new Counter<int>();
            counter[1] = 5;
            Assert.Equal(5, counter[1]);
            counter[1] = 0;
            Assert.False(counter.Contains(1));
        }

        [Fact]
        public void Counter_CopyTo()
        {
            var counter = new Counter<int>();
            counter.Add(1, 5);
            var array = new ItemCountPair<int>[1];
            ((ICollection<ItemCountPair<int>>)counter).CopyTo(array, 0);
            Assert.Equal(1, array[0].Item);
            Assert.Equal(5, array[0].Count);
        }

        [Fact]
        public void Counter_GetEnumerator()
        {
            var counter = new Counter<int>();
            counter.Add(1, 5);
            var enumerator = counter.GetEnumerator();
            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current.Item);
            Assert.Equal(5, enumerator.Current.Count);
        }

        public class EnumeratorTests
        {
            [Fact]
            public void Enumerator_InitializesWithCounter()
            {
                var counter = new Counter<int>();
                var enumerator = new Counter<int>.Enumerator(counter, Counter<int>.Enumerator.ReturnType.ItemCountPair);
                Assert.Equal(default, enumerator.Current);
            }

            [Fact]
            public void Enumerator_MoveNext()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var enumerator = new Counter<int>.Enumerator(counter, Counter<int>.Enumerator.ReturnType.ItemCountPair);
                Assert.True(enumerator.MoveNext());
                Assert.Equal(1, enumerator.Current.Item);
                Assert.Equal(5, enumerator.Current.Count);
            }

            [Fact]
            public void Enumerator_Current()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var enumerator = new Counter<int>.Enumerator(counter, Counter<int>.Enumerator.ReturnType.ItemCountPair);
                enumerator.MoveNext();
                var current = enumerator.Current;
                Assert.Equal(1, current.Item);
                Assert.Equal(5, current.Count);
            }

            [Fact]
            public void Enumerator_Dispose()
            {
                var counter = new Counter<int>();
                var enumerator = new Counter<int>.Enumerator(counter, Counter<int>.Enumerator.ReturnType.ItemCountPair);
                enumerator.Dispose();
                Assert.False(enumerator.MoveNext());
            }

            [Fact]
            public void Enumerator_ResetNotWorking()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var enumerator = new Counter<int>.Enumerator(counter, Counter<int>.Enumerator.ReturnType.ItemCountPair);
                enumerator.MoveNext();
                ((IEnumerator)enumerator).Reset();
                Assert.Equal(1, enumerator.Current.Item);
                Assert.Equal(5, enumerator.Current.Count);
                Assert.False(enumerator.MoveNext());
            }

            [Fact]
            public void Enumerator_CurrentAsObject()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var enumerator = new Counter<int>.Enumerator(counter, Counter<int>.Enumerator.ReturnType.ItemCountPair);
                enumerator.MoveNext();
                var current = ((IEnumerator)enumerator).Current;
                Assert.IsType<KeyValuePair<int, int>>(current);
                var pair = (KeyValuePair<int, int>)current;
                Assert.Equal(1, pair.Key);
                Assert.Equal(5, pair.Value);
            }

            [Fact]
            public void Enumerator_Entry()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var enumerator = new Counter<int>.Enumerator(counter, Counter<int>.Enumerator.ReturnType.CounterEntry);
                enumerator.MoveNext();
                var entry = ((ICounterEnumerator)enumerator).Entry;
                Assert.Equal(1, entry.Item);
                Assert.Equal(5, entry.Count);
            }

            [Fact]
            public void Enumerator_Item()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var enumerator = new Counter<int>.Enumerator(counter, Counter<int>.Enumerator.ReturnType.CounterEntry);
                enumerator.MoveNext();
                var item = ((ICounterEnumerator)enumerator).Item;
                Assert.Equal(1, item);
            }

            [Fact]
            public void Enumerator_Count()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var enumerator = new Counter<int>.Enumerator(counter, Counter<int>.Enumerator.ReturnType.CounterEntry);
                enumerator.MoveNext();
                var count = ((ICounterEnumerator)enumerator).Count;
                Assert.Equal(5, count);
            }
        }

        public class ItemCollectionTests
        {
            [Fact]
            public void ItemCollection_InitializesWithCounter()
            {
                var counter = new Counter<int>();
                var itemCollection = new Counter<int>.ItemCollection(counter);
                Assert.Empty(itemCollection);
            }

            [Fact]
            public void ItemCollection_ContainsItem()
            {
                var counter = new Counter<int>();
                counter.Add(1);
                var itemCollection = new Counter<int>.ItemCollection(counter);
                Assert.Contains(1, itemCollection);
            }

            [Fact]
            public void ItemCollection_CopyTo()
            {
                var counter = new Counter<int>();
                counter.Add(1);
                var itemCollection = new Counter<int>.ItemCollection(counter);
                var array = new int[1];
                itemCollection.CopyTo(array, 0);
                Assert.Equal(1, array[0]);
            }

            [Fact]
            public void ItemCollection_GetEnumerator()
            {
                var counter = new Counter<int>();
                counter.Add(1);
                var itemCollection = new Counter<int>.ItemCollection(counter);
                var enumerator = itemCollection.GetEnumerator();
                Assert.True(enumerator.MoveNext());
                Assert.Equal(1, enumerator.Current);
            }

            [Fact]
            public void ItemCollection_IsReadOnly()
            {
                var counter = new Counter<int>();
                var itemCollection = new Counter<int>.ItemCollection(counter);
                Assert.True(((ICollection<int>)itemCollection).IsReadOnly);
            }

            [Fact]
            public void ItemCollection_CopyToArray()
            {
                var counter = new Counter<int>();
                counter.Add(1);
                var itemCollection = new Counter<int>.ItemCollection(counter);
                var array = new int[1];
                ((ICollection)itemCollection).CopyTo(array, 0);
                Assert.Equal(1, array[0]);
            }

            [Fact]
            public void ItemCollection_IsSynchronized()
            {
                var counter = new Counter<int>();
                var itemCollection = new Counter<int>.ItemCollection(counter);
                Assert.False(((ICollection)itemCollection).IsSynchronized);
            }

            [Fact]
            public void ItemCollection_SyncRoot()
            {
                var counter = new Counter<int>();
                var itemCollection = new Counter<int>.ItemCollection(counter);
                Assert.NotNull(((ICollection)itemCollection).SyncRoot);
            }

        }

        public class CountCollectionTests
        {
            [Fact]
            public void CountCollection_InitializesWithCounter()
            {
                var counter = new Counter<int>();
                var countCollection = new Counter<int>.CountCollection(counter);
                Assert.Empty(countCollection);
            }

            [Fact]
            public void CountCollection_ContainsCount()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var countCollection = new Counter<int>.CountCollection(counter);
                Assert.Contains(5, countCollection);
            }

            [Fact]
            public void CountCollection_CopyTo()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var countCollection = new Counter<int>.CountCollection(counter);
                var array = new int[1];
                countCollection.CopyTo(array, 0);
                Assert.Equal(5, array[0]);
            }

            [Fact]
            public void CountCollection_GetEnumerator()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var countCollection = new Counter<int>.CountCollection(counter);
                var enumerator = countCollection.GetEnumerator();
                Assert.True(enumerator.MoveNext());
                Assert.Equal(5, enumerator.Current);
            }

            [Fact]
            public void CountCollection_IsReadOnly()
            {
                var counter = new Counter<int>();
                var countCollection = new Counter<int>.CountCollection(counter);
                Assert.True(((ICollection<int>)countCollection).IsReadOnly);
            }

            [Fact]
            public void CountCollection_CopyToArray()
            {
                var counter = new Counter<int>();
                counter.Add(1, 5);
                var countCollection = new Counter<int>.CountCollection(counter);
                var array = new int[1];
                ((ICollection)countCollection).CopyTo(array, 0);
                Assert.Equal(5, array[0]);
            }

            [Fact]
            public void CountCollection_IsSynchronized()
            {
                var counter = new Counter<int>();
                var countCollection = new Counter<int>.CountCollection(counter);
                Assert.False(((ICollection)countCollection).IsSynchronized);
            }

            [Fact]
            public void CountCollection_SyncRoot()
            {
                var counter = new Counter<int>();
                var countCollection = new Counter<int>.CountCollection(counter);
                Assert.NotNull(((ICollection)countCollection).SyncRoot);
            }
        }
    }
}
