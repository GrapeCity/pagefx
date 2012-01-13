using System;
using System.Collections.Generic;

namespace DataDynamics
{
    public class ActionSelector
    {
        interface IItem
        {
            bool Run(object obj);
        }

        class Item<T> : IItem
            where T : class
        {
            readonly Action<T> _action;

            public Item(Action<T> action)
            {
                _action = action;
            }

            public bool Run(object obj)
            {
                var o = obj as T;
                if (o != null)
                {
                    _action(o);
                    return true;
                }
                return false;
            }
        }

        readonly List<IItem> _items = new List<IItem>();

        public void Add<T>(Action<T> action)
            where T : class
        {
            _items.Add(new Item<T>(action));
        }

        public bool Run(object obj)
        {
            foreach (var item in _items)
            {
                if (item.Run(obj))
                    return true;
            }
            return false;
        }
    }
}