using System;
using System.Collections.Generic;
using Xunit;

namespace Cubusky.Collections.Generic.Tests
{
    public class ObserverSetTests
    {
        public class NumberObservable : IObservable<float>
        {
            private readonly List<IObserver<float>> observers = new List<IObserver<float>>();

            private class Unsubscriber : IDisposable
            {
                private readonly List<IObserver<float>> _observers;
                private readonly IObserver<float> _observer;

                public Unsubscriber(List<IObserver<float>> observers, IObserver<float> observer)
                {
                    _observers = observers;
                    _observer = observer;
                }

                public void Dispose()
                {
                    if (_observer != null)
                    {
                        _observers.Remove(_observer);
                    }
                }
            }

            public IDisposable Subscribe(IObserver<float> observer)
            {
                if (!observers.Contains(observer))
                {
                    observers.Add(observer);
                }

                return new Unsubscriber(observers, observer);
            }

            public void SetNumber(float number)
            {
                foreach (var observer in observers)
                {
                    observer.OnNext(number);
                }
            }

            public void ThrowError()
            {
                foreach (var observer in observers)
                {
                    observer.OnError(new NotFiniteNumberException());
                }
            }

            public void Clear()
            {
                foreach (var observer in observers)
                {
                    observer.OnCompleted();
                }
                observers.Clear();
            }
        }

        public class NumberObserver : IObserver<float>
        {
            public float? Number { get; private set; }

            public virtual void OnCompleted() => Number = null;
            public virtual void OnError(Exception error) => Number = float.NaN;
            public virtual void OnNext(float value) => Number = value;
        }

        public class NumberSet : IDisposable
        {
            public NumberObserver PrimaryObserver = new NumberObserver();
            public NumberObserver SecondaryObserver = new NumberObserver();
            public NumberObserver TertiaryObserver = new NumberObserver();
            public NumberObservable PrimaryObservable = new NumberObservable();
            public NumberObservable SecondaryObservable = new NumberObservable();
            public NumberObservable TertiaryObservable = new NumberObservable();

            public ObserverSet<float> Set = new ObserverSet<float>();

            public ISet<IObserver<float>> Observers => Set;
            public ISet<IObservable<float>> Observables => Set;

            public IEnumerable<IObserver<float>> OtherObservers => new[] { PrimaryObserver, TertiaryObserver };
            public IEnumerable<IObservable<float>> OtherObservables => new[] { PrimaryObservable, TertiaryObservable };

            public NumberSet()
            {
                Set.Add(PrimaryObserver);
                Set.Add(SecondaryObserver);
                Set.Add(PrimaryObservable);
                Set.Add(SecondaryObservable);
            }

            public void Dispose()
            {
                Set.Clear();
                GC.SuppressFinalize(this);
            }
        }

        [Fact]
        public void ObserverSet_OnNext()
        {
            using var numberSet = new NumberSet();

            numberSet.PrimaryObservable.SetNumber(14);
            Assert.Equal(14, numberSet.PrimaryObserver.Number);
            Assert.Equal(14, numberSet.SecondaryObserver.Number);

            numberSet.SecondaryObservable.SetNumber(15);
            Assert.Equal(15, numberSet.PrimaryObserver.Number);
            Assert.Equal(15, numberSet.SecondaryObserver.Number);
        }

        [Fact]
        public void ObserverSet_OnError()
        {
            using var numberSet = new NumberSet();

            numberSet.PrimaryObservable.ThrowError();
            Assert.Equal(float.NaN, numberSet.PrimaryObserver.Number);
            Assert.Equal(float.NaN, numberSet.SecondaryObserver.Number);
        }

        [Fact]
        public void ObserverSet_OnCompleted()
        {
            using var numberSet = new NumberSet();

            numberSet.PrimaryObservable.Clear();
            Assert.Null(numberSet.PrimaryObserver.Number);
            Assert.Null(numberSet.SecondaryObserver.Number);
        }

        [Fact]
        public void ObserverSet_Add()
        {
            var numberObservable = new NumberObservable();
            var numberObserver = new NumberObserver();

            var numberSet = new ObserverSet<float>()
            {
                numberObservable,
                numberObserver
            };

            numberObservable.SetNumber(5);
            Assert.Equal(5, numberObserver.Number);
        }

        [Fact]
        public void ObserverSet_Remove()
        {
            using var numberSet = new NumberSet();

            numberSet.Set.Remove(numberSet.PrimaryObservable);
            numberSet.Set.Remove(numberSet.SecondaryObserver);

            numberSet.PrimaryObservable.SetNumber(1);
            Assert.NotEqual(1, numberSet.PrimaryObserver.Number);

            numberSet.SecondaryObservable.SetNumber(2);
            Assert.NotEqual(2, numberSet.SecondaryObserver.Number);
        }

        [Fact]
        public void ObserverSet_Count()
        {
            using var numberSet = new NumberSet();
            Assert.Equal(2, numberSet.Observables.Count);
            Assert.Equal(0, numberSet.Observers.Count);

            numberSet.Set.Remove(numberSet.PrimaryObservable);
            numberSet.Set.Add(numberSet.TertiaryObserver);
            Assert.Equal(1, numberSet.Observables.Count);
            Assert.Equal(3, numberSet.Observers.Count);

            numberSet.Set.Clear();
            Assert.Equal(0, numberSet.Observables.Count);
            Assert.Equal(0, numberSet.Observers.Count);
        }

        [Fact]
        public void ObserverSet_ExceptWith()
        {
            using var numberSet = new NumberSet();

            numberSet.Set.ExceptWith(numberSet.OtherObservables);
            Assert.DoesNotContain(numberSet.PrimaryObservable, numberSet.Observables);
            Assert.Contains(numberSet.SecondaryObservable, numberSet.Observables);

            numberSet.Set.ExceptWith(numberSet.OtherObservers);
            Assert.DoesNotContain(numberSet.PrimaryObserver, numberSet.Observers);
            Assert.Contains(numberSet.SecondaryObserver, numberSet.Observers);
        }

        [Fact]
        public void ObserverSet_IntersectWith()
        {
            using var numberSet = new NumberSet();

            numberSet.Set.IntersectWith(numberSet.OtherObservables);
            Assert.Contains(numberSet.PrimaryObservable, numberSet.Observables);
            Assert.DoesNotContain(numberSet.SecondaryObservable, numberSet.Observables);

            numberSet.Set.IntersectWith(numberSet.OtherObservers);
            Assert.Contains(numberSet.PrimaryObserver, numberSet.Observers);
            Assert.DoesNotContain(numberSet.SecondaryObserver, numberSet.Observers);
        }

        [Fact]
        public void ObserverSet_SymmetricExceptWith()
        {
            using var numberSet = new NumberSet();

            numberSet.Set.SymmetricExceptWith(numberSet.OtherObservables);
            Assert.DoesNotContain(numberSet.PrimaryObservable, numberSet.Observables);
            Assert.Contains(numberSet.SecondaryObservable, numberSet.Observables);
            Assert.Contains(numberSet.TertiaryObservable, numberSet.Observables);

            numberSet.Set.SymmetricExceptWith(numberSet.OtherObservers);
            Assert.DoesNotContain(numberSet.PrimaryObserver, numberSet.Observers);
            Assert.Contains(numberSet.SecondaryObserver, numberSet.Observers);
            Assert.Contains(numberSet.TertiaryObserver, numberSet.Observers);
        }

        [Fact]
        public void ObserverSet_UnionWith()
        {
            using var numberSet = new NumberSet();

            numberSet.Set.UnionWith(numberSet.OtherObservables);
            Assert.Contains(numberSet.PrimaryObservable, numberSet.Observables);
            Assert.Contains(numberSet.SecondaryObservable, numberSet.Observables);
            Assert.Contains(numberSet.TertiaryObservable, numberSet.Observables);

            numberSet.Set.UnionWith(numberSet.OtherObservers);
            Assert.Contains(numberSet.PrimaryObserver, numberSet.Observers);
            Assert.Contains(numberSet.SecondaryObserver, numberSet.Observers);
            Assert.Contains(numberSet.TertiaryObserver, numberSet.Observers);
        }
    }
}
