using System;
using System.Text;

namespace ObservableDeque
{
    public record DequeNode<T>(T Value)
    {
        public DequeNode<T> Next { get; set; }
        public DequeNode<T> Prev { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new();

            builder.Append($"[Prev: {(Prev is null ? "[null]" : Prev.Value)}]");
            builder.Append($"[Current: {Value}]");
            builder.Append($"[Prev: {(Next is null ? "[null]" : Next.Value)}]");

            return builder.ToString();
        }
    }
}
