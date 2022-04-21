using System;
using System.Linq;
using ObservableDeque;
using Xunit;

namespace ObservableDequeUnitTests
{
    public class ObservableDequeTests
    {
        [Fact]
        public void Enqueue()
        {
            Deque<int> deque = new();

            deque.EnqueueFirst(420);
            deque.EnqueueFirst(42);
            deque.EnqueueLast(69);

            Assert.Equal(new [] { 42, 420, 69 }, deque.ToArray());
        }

        [Fact]
        public void Dequeue()
        {
            Deque<int> deque = new(new [] { 4, 3, 2, 1 });

            Assert.Equal(4, deque.DequeueFirst());
            Assert.Equal(1, deque.DequeueLast());
            Assert.Equal(2, deque.DequeueLast());
            Assert.Equal(3, deque.DequeueFirst());
        }

        [Fact]
        public void Clear()
        {
            Deque<string> deque = new(new [] { "foo", "baz" });

            deque.Clear();

            Assert.Empty(deque);
        }

        [Fact]
        public void Contains()
        {
            Deque<string> deque = new(new [] { "foo", "baz", "bar" });

            Assert.True(deque.Contains("foo"));
            Assert.True(deque.Contains("bar"));
            Assert.False(deque.Contains("fizz"));
            Assert.False(deque.Contains(null));
        }

        [Fact]
        public void Empty()
        {
            Deque<object> deque = new();

            Assert.Throws<InvalidOperationException>(() => deque.PeekFirst());
            Assert.Throws<InvalidOperationException>(() => deque.PeekLast());
            Assert.Throws<InvalidOperationException>(() => deque.DequeueFirst());
            Assert.Throws<InvalidOperationException>(() => deque.DequeueLast());
        }

        [Fact]
        public void TryDequeue()
        {
            Deque<char> deque = new(new [] { 'a', 'b' });

            Assert.True(deque.TryDequeueLast(out char last));
            Assert.Equal('b', last);

            Assert.True(deque.TryDequeueFirst(out char first));
            Assert.Equal('a', first);

            Assert.False(deque.TryDequeueFirst(out char empty));
            Assert.Equal(default, empty);
        }

        [Fact]
        public void TryPeek()
        {
            Deque<char> deque = new(new[] { 'a', 'b' });

            Assert.True(deque.TryPeekLast(out char last));
            Assert.Equal('b', last);

            Assert.True(deque.TryPeekFirst(out char first));
            Assert.Equal('a', first);

            deque.Clear();

            Assert.False(deque.TryDequeueFirst(out char empty));
            Assert.Equal(default, empty);
        }
    }
}