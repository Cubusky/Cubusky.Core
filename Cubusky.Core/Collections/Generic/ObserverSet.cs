using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cubusky.Collections.Generic
{
    /// <summary>Represents a set of <see cref="IObserver{T}"/> and <see cref="IObservable{T}"/> that are all subscribed to each other.</summary>
    /// <typeparam name="T">The object that provides notification information.</typeparam>
    public class ObserverSet<T> : ISet<IObserver<T>>, ISet<IObservable<T>>
#if NET8_0_OR_GREATER
        , IReadOnlySet<IObserver<T>>
        , IReadOnlySet<IObservable<T>>
#else
        , IReadOnlyCollection<IObserver<T>>
        , IReadOnlyCollection<IObservable<T>>
#endif
    {
        private readonly HashSet<IObserver<T>> observers = new HashSet<IObserver<T>>();
        private readonly HashSet<IObservable<T>> observables = new HashSet<IObservable<T>>();
        private readonly Dictionary<(IObserver<T> observer, IObservable<T> observable), IDisposable> unsubscribers = new Dictionary<(IObserver<T> observer, IObservable<T> observable), IDisposable>();

        private static UnsubscriberNotFoundException unsubscriberNotFoundException => new UnsubscriberNotFoundException("During removal, no unsubscribed was found from the observer to the observable.");

        /// <summary>The exception that is thrown when the observer or observable specified for removal does not have an unsubscriber for any of its connections.</summary>
        public class UnsubscriberNotFoundException : Exception
        {
            /// <summary>Initializes a new instance of the <see cref="UnsubscriberNotFoundException"/> class.</summary>
            public UnsubscriberNotFoundException() { }

            /// <summary>Initializes a new instance of the <see cref="UnsubscriberNotFoundException"/> class with a specified error message.</summary>
            /// <param name="message">The error message that explains the reason for the exception.</param>
            public UnsubscriberNotFoundException(string message)
                : base(message) { }

            /// <summary>Initializes a new instance of the <see cref="UnsubscriberNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
            /// <param name="message">The error message that explains the reason for the exception.</param>
            /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
            public UnsubscriberNotFoundException(string message, Exception inner)
                : base(message, inner) { }
        }

        /// <summary>Add an observer to the current set and returns a value to indicate if the observer was successfully added.</summary>
        /// <param name="observer"></param>
        /// <returns><see langword="true"/> if the observer is added to the set; <see langword="false"/> if the observer is already in the set.</returns>
        public bool Add(IObserver<T> observer)
        {
            // Try to add the observer. If it already exists, return.
            if (!observers.Add(observer))
            {
                return false;
            }

            // Subscribe the observer to all observables.
            foreach (var observable in observables)
            {
                unsubscribers.Add((observer, observable), observable.Subscribe(observer));
            }

            return true;
        }

        /// <summary>Add an observable to the current set and returns a value to indicate if the observable was successfully added.</summary>
        /// <param name="observable"></param>
        /// <returns><see langword="true"/> if the observable is added to the set; <see langword="false"/> if the observable is already in the set.</returns>
        public bool Add(IObservable<T> observable)
        {
            // Try to add the observable. If it already exists, return.
            if (!observables.Add(observable))
            {
                return false;
            }

            // Subscribe the observable to all observers.
            foreach (var observer in observers)
            {
                unsubscribers.Add((observer, observable), observable.Subscribe(observer));
            }

            return true;
        }

        void ICollection<IObserver<T>>.Add(IObserver<T> observer) => Add(observer);
        void ICollection<IObservable<T>>.Add(IObservable<T> observable) => Add(observable);

        /// <summary>Unsubscribe every observer from every observable and clear the list of unsubscribers.</summary>
        private void ClearUnsubscribers()
        {
            foreach (var unsubscriber in unsubscribers.Values)
            {
                unsubscriber.Dispose();
            }
            unsubscribers.Clear();
        }

        /// <summary>Removes all observers and observables from a <see cref="ObserverSet{T}"/> object.</summary>
        public void Clear()
        {
            ClearUnsubscribers();
            observers.Clear();
            observables.Clear();
        }

        void ICollection<IObserver<T>>.Clear()
        {
            ClearUnsubscribers();
            observers.Clear();
        }

        void ICollection<IObservable<T>>.Clear()
        {
            ClearUnsubscribers();
            observables.Clear();
        }

        /// <summary>Determines wether a <see cref="ObserverSet{T}"/> object contains the specified observer.</summary>
        /// <param name="observer">The observer to locate in the <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object contains the specified observer; otherwise, <see langword="false"/>.</returns>
        public bool Contains(IObserver<T> observer) => observers.Contains(observer);

        /// <summary>Determines wether a <see cref="ObserverSet{T}"/> object contains the specified observable.</summary>
        /// <param name="observable">The observable to locate in the <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object contains the specified observable; otherwise, <see langword="false"/>.</returns>
        public bool Contains(IObservable<T> observable) => observables.Contains(observable);

        /// <summary>Copies the observers of a <see cref="ObserverSet{T}"/> object to an array.</summary>
        /// <param name="array">The destination array.</param>
        /// <exception cref="ArgumentNullException"/>
        public void CopyTo(IObserver<T>[] array) => observers.CopyTo(array);

        /// <inheritdoc cref="CopyTo(IObserver{T}[], int, int)"/>
        public void CopyTo(IObserver<T>[] array, int arrayIndex) => observers.CopyTo(array, arrayIndex);

        /// <summary>Copies the observers of a <see cref="ObserverSet{T}"/> object to an array, starting at the specified array index.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of observers to copy to the array.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public void CopyTo(IObserver<T>[] array, int arrayIndex, int count) => observers.CopyTo(array, arrayIndex, count);

        /// <summary>Copies the observables of a <see cref="ObserverSet{T}"/> object to an array.</summary>
        /// <param name="array">The destination array.</param>
        /// <exception cref="ArgumentNullException"/>
        public void CopyTo(IObservable<T>[] array) => observables.CopyTo(array);

        /// <inheritdoc cref="CopyTo(IObservable{T}[], int, int)"/>
        public void CopyTo(IObservable<T>[] array, int arrayIndex) => observables.CopyTo(array, arrayIndex);

        /// <summary>Copies the observables of a <see cref="ObserverSet{T}"/> object to an array, starting at the specified array index.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of observables to copy to the array.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public void CopyTo(IObservable<T>[] array, int arrayIndex, int count) => observables.CopyTo(array, arrayIndex, count);

        /// <summary>Removes the specified observer from a <see cref="ObserverSet{T}"/> object.</summary>
        /// <param name="observer">The observer to remove from the <see cref="ObserverSet{T}"/>.</param>
        /// <returns><see langword="true"/> if the observer is successfully found and removed; otherwise, <see langword="false"/>. This method returns <see langword="false"/> if <paramref name="observer"/> is not found in the <see cref="ObserverSet{T}"/> object.</returns>
        /// <exception cref="UnsubscriberNotFoundException"></exception>
        public bool Remove(IObserver<T> observer)
        {
            if (!observers.Remove(observer))
            {
                return false;
            }

            foreach (var observable in observables)
            {
                if (!unsubscribers.Remove((observer, observable), out var unsubscriber))
                {
                    throw unsubscriberNotFoundException;
                }

                unsubscriber.Dispose();
            }

            return true;
        }

        /// <summary>Removes the specified observable from a <see cref="ObserverSet{T}"/> object.</summary>
        /// <param name="observable">The observable to remove from the <see cref="ObserverSet{T}"/>.</param>
        /// <returns><see langword="true"/> if the observable is successfully found and removed; otherwise, <see langword="false"/>. This method returns <see langword="false"/> if <paramref name="observable"/> is not found in the <see cref="ObserverSet{T}"/> object.</returns>
        /// <exception cref="UnsubscriberNotFoundException"></exception>
        public bool Remove(IObservable<T> observable)
        {
            if (!observables.Remove(observable))
            {
                return false;
            }

            foreach (var observer in observers)
            {
                if (!unsubscribers.Remove((observer, observable), out var unsubscriber))
                {
                    throw unsubscriberNotFoundException;
                }

                unsubscriber.Dispose();
            }

            return true;
        }

        int ICollection<IObserver<T>>.Count => observers.Count;
        int ICollection<IObservable<T>>.Count => observables.Count;
        int IReadOnlyCollection<IObserver<T>>.Count => observers.Count;
        int IReadOnlyCollection<IObservable<T>>.Count => observers.Count;

        bool ICollection<IObserver<T>>.IsReadOnly => false;
        bool ICollection<IObservable<T>>.IsReadOnly => false;

        IEnumerator<IObserver<T>> IEnumerable<IObserver<T>>.GetEnumerator() => observers.GetEnumerator();
        IEnumerator<IObservable<T>> IEnumerable<IObservable<T>>.GetEnumerator() => observables.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => throw new InvalidOperationException($"Cannot enumerate the observers set without a type specification, as it is unclear if to enumerate {nameof(IObserver<T>)} or {nameof(IObservable<T>)}.");

        /// <summary>Removes all observers in the specified observers collection from the current <see cref="ObserverSet{T}"/> object.</summary>
        /// <param name="other">The collection of observers to remove from the <see cref="ObserverSet{T}"/> object.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnsubscriberNotFoundException"></exception>
        public void ExceptWith(IEnumerable<IObserver<T>> other)
        {
            foreach (var observer in other)
            {
                Remove(observer);
            }
        }

        /// <summary>Removes all observables in the specified observables collection from the current <see cref="ObserverSet{T}"/> object.</summary>
        /// <param name="other">The collection of observables to remove from the <see cref="ObserverSet{T}"/> object.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnsubscriberNotFoundException"></exception>
        public void ExceptWith(IEnumerable<IObservable<T>> other)
        {
            foreach (var observable in other)
            {
                Remove(observable);
            }
        }

        /// <summary>Modifies the current <see cref="ObserverSet{T}"/> object to contain only observers that are present in that object and in the specified observers collection.</summary>
        /// <param name="other">The collection of observers to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnsubscriberNotFoundException"></exception>
        public void IntersectWith(IEnumerable<IObserver<T>> other)
        {
            foreach (var observer in observers.ToArray())
            {
                if (!other.Contains(observer))
                {
                    Remove(observer);
                }
            }
        }

        /// <summary>Modifies the current <see cref="ObserverSet{T}"/> object to contain only observables that are present in that object and in the specified observables collection.</summary>
        /// <param name="other">The collection of observables to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnsubscriberNotFoundException"></exception>
        public void IntersectWith(IEnumerable<IObservable<T>> other)
        {
            foreach (var observable in observables.ToArray())
            {
                if (!other.Contains(observable))
                {
                    Remove(observable);
                }
            }
        }

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object is a proper subset of the specified observers collection.</summary>
        /// <param name="other">The collection of observers to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is a proper subset of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsProperSubsetOf(IEnumerable<IObserver<T>> other) => observers.IsProperSubsetOf(other);

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object is a proper subset of the specified observables collection.</summary>
        /// <param name="other">The collection of observables to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is a proper subset of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsProperSubsetOf(IEnumerable<IObservable<T>> other) => observables.IsProperSubsetOf(other);

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object is a proper superset of the specified observers collection.</summary>
        /// <param name="other">The collection of observers to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is a proper superset of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsProperSupersetOf(IEnumerable<IObserver<T>> other) => observers.IsProperSupersetOf(other);

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object is a proper superset of the specified observables collection.</summary>
        /// <param name="other">The collection of observables to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is a proper superset of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsProperSupersetOf(IEnumerable<IObservable<T>> other) => observables.IsProperSupersetOf(other);

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object is a subset of the specified observers collection.</summary>
        /// <param name="other">The collection of observers to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is a subset of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsSubsetOf(IEnumerable<IObserver<T>> other) => observers.IsSubsetOf(other);

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object is a subset of the specified observables collection.</summary>
        /// <param name="other">The collection of observables to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is a subset of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsSubsetOf(IEnumerable<IObservable<T>> other) => observables.IsSubsetOf(other);

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object is a superset of the specified observers collection.</summary>
        /// <param name="other">The collection of observers to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is a superset of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsSupersetOf(IEnumerable<IObserver<T>> other) => observers.IsSupersetOf(other);

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object is a superset of the specified observables collection.</summary>
        /// <param name="other">The collection of observables to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is a superset of <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsSupersetOf(IEnumerable<IObservable<T>> other) => observables.IsSupersetOf(other);

        /// <summary>Determines whether the current <see cref="ObserverSet{T}"/> object and a specified observers collection share common observers.</summary>
        /// <param name="other">The collection of observers to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object and <paramref name="other"/> share at least one common observer; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool Overlaps(IEnumerable<IObserver<T>> other) => observers.Overlaps(other);

        /// <summary>Determines whether the current <see cref="ObserverSet{T}"/> object and a specified observables collection share common observables.</summary>
        /// <param name="other">The collection of observables to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object and <paramref name="other"/> share at least one common observable; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool Overlaps(IEnumerable<IObservable<T>> other) => observables.Overlaps(other);

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object and the specified observers collection contain the same observers.</summary>
        /// <param name="other">The collection of observers to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is equal to <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool SetEquals(IEnumerable<IObserver<T>> other) => observers.SetEquals(other);

        /// <summary>Determines whether a <see cref="ObserverSet{T}"/> object and the specified observables collection contain the same observables.</summary>
        /// <param name="other">The collection of observables to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <returns><see langword="true"/> if the <see cref="ObserverSet{T}"/> object is equal to <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool SetEquals(IEnumerable<IObservable<T>> other) => observables.SetEquals(other);

        /// <summary>Modifies the current <see cref="ObserverSet{T}"/> object to contain only observers that are present either in that object or in the specified observers collection, but not both.</summary>
        /// <param name="other">The collection of observers to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnsubscriberNotFoundException"></exception>
        public void SymmetricExceptWith(IEnumerable<IObserver<T>> other)
        {
            foreach (var observer in other)
            {
                if (observers.Contains(observer))
                {
                    Remove(observer);
                }
                else
                {
                    Add(observer);
                }
            }
        }

        /// <summary>Modifies the current <see cref="ObserverSet{T}"/> object to contain only observables that are present either in that object or in the specified observables collection, but not both.</summary>
        /// <param name="other">The collection of observables to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnsubscriberNotFoundException"></exception>
        public void SymmetricExceptWith(IEnumerable<IObservable<T>> other)
        {
            foreach (var observable in other)
            {
                if (observables.Contains(observable))
                {
                    Remove(observable);
                }
                else
                {
                    Add(observable);
                }
            }
        }

        /// <summary>Modifies the current <see cref="ObserverSet{T}"/> object to contain all observers that are present in itself, the specified observers collection, or both.</summary>
        /// <param name="other">The collection of observers to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <exception cref="ArgumentNullException"/>
        public void UnionWith(IEnumerable<IObserver<T>> other)
        {
            foreach (var observer in other)
            {
                Add(observer);
            }
        }

        /// <summary>Modifies the current <see cref="ObserverSet{T}"/> object to contain all observables that are present in itself, the specified observables collection, or both.</summary>
        /// <param name="other">The collection of observables to compare to the current <see cref="ObserverSet{T}"/> object.</param>
        /// <exception cref="ArgumentNullException"/>
        public void UnionWith(IEnumerable<IObservable<T>> other)
        {
            foreach (var observable in other)
            {
                Add(observable);
            }
        }
    }
}
