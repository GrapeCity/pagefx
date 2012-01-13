namespace DataDynamics.Collections
{
    public class LinkedList<T>
    {
        class Node
        {
            public Node Prev
            {
                get { return _prev; }
                set
                {
                    if (value != _prev)
                    {
                        _prev = value;
                        if (value != null)
                            value.Next = this;
                    }
                }
            }
            private Node _prev;

            public Node Next
            {
                get { return _next; }
                set
                {
                    if (value != _next)
                    {
                        _next = value;
                        if (value != null)
                            value.Prev = this;
                    }
                }
            }
            private Node _next;

            public readonly T Item;

            public Node(T item)
            {
                Item = item;
            }
        }

        private Node _first;
        private Node _last;

        public void Append(T item)
        {
            Append(new Node(item));
        }

        private void Append(Node node)
        {
            if (_last == null) _first = node;
            else _last.Next = node;
            _last = node;
        }

        public bool Remove(T item)
        {
            for (var node = _first; node != null; node = node.Next)
            {
                if (Equals(node.Item, item))
                {
                    Remove(node);
                    return true;
                }
            }
            return false;
        }

        private void Remove(Node node)
        {
            var prev = node.Prev;
            var next = node.Next;

            node.Prev = null;
            node.Next = null;

            if (prev != null)
                prev.Next = next;
            if (next != null)
                next.Prev = prev;

            if (prev == null)
                _first = next;
            if (next == null)
                _last = prev;
        }
    }
}