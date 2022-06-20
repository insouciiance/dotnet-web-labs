using System;
using System.Collections;
using System.Collections.Generic;

namespace ObservableDeque
{
    public class Deque<T> : IEnumerable<T>, IReadOnlyCollection<T>, ICollection
    {
        private DequeNode<T> Head { get; set; }
        private DequeNode<T> Last { get; set; }

        public event EventHandler<ItemAddedEventArgs<T>> ItemAdded;
        public event EventHandler<ItemRemovedEventArgs<T>> ItemRemoved;
        public event EventHandler Cleared;

        public Deque() { }

        public Deque(IEnumerable<T> initialValues)
        {
            if (initialValues is null)
            {
                throw new ArgumentNullException(nameof(initialValues));
            }

            foreach (T value in initialValues)
            {
                EnqueueLast(value);
            }
        }

        public void EnqueueLast(T value)
        {
            if (Head is null)
            {
                Head = new DequeNode<T>(value);
                Last = Head;
            }
            else
            {
                DequeNode<T> newNode = new(value)
                {
                    Prev = Last
                };

                Last.Next = newNode;
                Last = newNode;
            }

            ItemAddedEventArgs<T> eventArgs = new(value, false);
            ItemAdded?.Invoke(this, eventArgs);

            Count++;
        }

        public void EnqueueFirst(T value)
        {
            if (Head is null)
            {
                Head = new DequeNode<T>(value);
                Last = Head;
            }
            else
            {
                DequeNode<T> newNode = new(value)
                {
                    Next = Head
                };

                Head.Prev = newNode;
                Head = newNode;
            }

            ItemAddedEventArgs<T> eventArgs = new(value, true);
            ItemAdded?.Invoke(this, eventArgs);

            Count++;
        }

        public T DequeueLast()
        {
            if (Head is null)
            {
                throw new InvalidOperationException("The deque was empty.");
            }

            T value = Last.Value;

            if (Last.Prev is not null)
            {
                Last.Prev.Next = null;
            }

            Last = Last.Prev;

            if (Last is null)
            {
                Head = null;
            }

            ItemRemovedEventArgs<T> eventArgs = new(value, false);
            ItemRemoved?.Invoke(this, eventArgs);

            Count--;

            return value;
        }

        public T DequeueFirst()
        {
            if (Head is null)
            {
                throw new InvalidOperationException("The deque was empty.");
            }

            T value = Head.Value;

            if (Head.Next is not null)
            {
                Head.Next.Prev = null;
            }

            Head = Head.Next;

            if (Head is null)
            {
                Last = null;
            }

            ItemRemovedEventArgs<T> eventArgs = new(value, true);
            ItemRemoved?.Invoke(this, eventArgs);

            Count--;

            return value;
        }

        public T PeekLast()
        {
            if (Head is null)
            {
                throw new InvalidOperationException("The deque was empty.");
            }

            return Last.Value;
        }

        public T PeekFirst()
        {
            if (Head is null)
            {
                throw new InvalidOperationException("The deque was empty.");
            }

            return Head.Value;
        }

        public bool TryDequeueLast(out T value)
        {
            if (Head is null)
            {
                value = default;
                return false;
            }

            value = DequeueLast();
            return true;
        }

        public bool TryDequeueFirst(out T value)
        {
            if (Head is null)
            {
                value = default;
                return false;
            }

            value = DequeueFirst();
            return true;
        }

        public bool TryPeekLast(out T value)
        {
            if (Head is null)
            {
                value = default;
                return false;
            }

            value = Last.Value;
            return true;
        }

        public bool TryPeekFirst(out T value)
        {
            if (Head is null)
            {
                value = default;
                return false;
            }

            value = Head.Value;
            return true;
        }

        public void Clear()
        {
            Head = null;
            Last = null;
            Count = 0;

            Cleared?.Invoke(this, EventArgs.Empty);
        }

        public bool Contains(T searchValue)
        {
            foreach (T value in this)
            {
                if (searchValue?.Equals(value) ?? value is null)
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            DequeNode<T> currentNode = Head;

            while (currentNode is not null)
            {
                yield return currentNode.Value;
                currentNode = currentNode.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(Array array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index >= array.Length)
            {
                throw new ArgumentException("The index value was invalid.", nameof(index));
            }

            using IEnumerator<T> listEnumerator = GetEnumerator();

            while (index < array.Length && listEnumerator.MoveNext())
            {
                ((IList)array)[index] = listEnumerator.Current;
                index++;
            }
        }

        int ICollection.Count => Count;

        public bool IsSynchronized => false;

        public object SyncRoot => new();

        public int Count { get; private set; }
    }
}
