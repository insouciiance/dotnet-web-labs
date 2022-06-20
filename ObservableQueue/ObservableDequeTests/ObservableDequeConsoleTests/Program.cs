using System;
using ObservableDeque;

namespace ObservableDequeTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Deque<string> deque = new();

            deque.ItemAdded += (sender, eventArgs) =>
            {
                Console.WriteLine($"Item {eventArgs.Item} added, to the {(eventArgs.IsFront ? "front" : "back")}");
            };

            deque.ItemRemoved += (sender, eventArgs) =>
            {
                Console.WriteLine($"Item {eventArgs.Item} removed, to the {(eventArgs.IsFront ? "front" : "back")}");
            };

            deque.Cleared += (sender, eventArgs) =>
            {
                Console.WriteLine("Deque cleared.");
            };

            deque.EnqueueLast("first");
            deque.EnqueueLast("second");
            deque.EnqueueLast("third");
            deque.EnqueueLast("fourth");
            deque.EnqueueLast(null);
            deque.EnqueueLast("fifth");

            string[] newValues = new string[43];
            deque.CopyTo(newValues, 40);

            foreach (string s in newValues)
            {
                Console.WriteLine(s ?? "[null]");
            }

            Console.WriteLine(deque.PeekFirst());
            Console.WriteLine(deque.PeekLast());
        }
    }
}
