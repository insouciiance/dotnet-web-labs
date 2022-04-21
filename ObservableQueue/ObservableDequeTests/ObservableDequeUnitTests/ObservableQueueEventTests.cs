using System;
using System.Collections.Generic;
using ObservableDeque;
using Xunit;

namespace ObservableDequeUnitTests
{
    public class ObservableDequeEventTests
    {
        [Fact]
        public void ItemAdded()
        {
            Deque<string> deque = new();

            List<(bool, string)> addedItems = new();

            deque.ItemAdded += (_, args) =>
            {
                addedItems.Add((args.IsFront, args.Item));
            };

            deque.EnqueueLast("foo");
            deque.EnqueueLast("baz");
            deque.EnqueueFirst("bar");

            Assert.Equal(new []
            {
                (false, "foo"),
                (false, "baz"),
                (true, "bar")
            }, addedItems);
        }

        [Fact]
        public void ItemRemoved()
        {
            Deque<string> deque = new(new[] { "foo", "baz", "bar" });

            List<(bool, string)> removedItems = new();

            deque.ItemRemoved += (_, args) =>
            {
                removedItems.Add((args.IsFront, args.Item));
            };

            deque.DequeueLast();
            deque.DequeueLast();
            deque.DequeueFirst();

            Assert.Equal(new[]
            {
                (false, "bar"),
                (false, "baz"),
                (true, "foo")
            }, removedItems);
        }

        [Fact]
        public void Cleared()
        {
            Deque<string> deque = new(new[] { "foo", "baz", "bar" });

            bool clearedEventFlag = false;

            deque.Cleared += (_, _) => clearedEventFlag = true;

            deque.Clear();

            Assert.True(clearedEventFlag);
        }
    }
}
