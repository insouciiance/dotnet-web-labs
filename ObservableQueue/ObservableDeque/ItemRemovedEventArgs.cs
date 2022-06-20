using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableDeque
{
    public class ItemRemovedEventArgs<T> : EventArgs
    {
        public T Item { get; init; }

        public bool IsFront { get; init; }

        public ItemRemovedEventArgs(T item, bool isFront)
        {
            Item = item;
            IsFront = isFront;
        }
    }
}
