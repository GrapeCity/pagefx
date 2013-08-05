using System.Collections.Generic;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Abc
{
    internal sealed class AbcLateMethodCollection
    {
        private readonly List<AbcMethod> _methods = new List<AbcMethod>();

        private struct Item
        {
            public readonly AbcMethodHandler Handler;
            public readonly AbcCoder Coder;

            public Item(AbcMethodHandler h)
            {
                Handler = h;
                Coder = null;
            }

            public Item(AbcCoder coder)
            {
                Coder = coder;
                Handler = null;
            }
        }

        private readonly List<Item> _items = new List<Item>();
        
        public void Add(AbcMethod method, AbcMethodHandler coder)
        {
            _methods.Add(method);
            _items.Add(new Item(coder));
        }

        public void Add(AbcMethod method, AbcCoder coder)
        {
            _methods.Add(method);
            _items.Add(new Item(coder));
        }

        public void Finish()
        {
            int n = _methods.Count;
            for (int i = 0; i < n; ++i)
            {
                var method = _methods[i];
                var item = _items[i];
                if (item.Handler != null)
                {
                    item.Handler(method);
                }
                else
                {
                    var code = new AbcCode(method.Abc);
                    item.Coder(code);
                    method.Finish(code);
                }
            }
        }
    }

	internal delegate void AbcMethodHandler(AbcMethod method);
}