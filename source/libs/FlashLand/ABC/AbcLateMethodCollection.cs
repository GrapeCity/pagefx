using System.Collections.Generic;
using DataDynamics.PageFX.FlashLand.Core.Tools;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    internal class AbcLateMethodCollection
    {
        private readonly List<AbcMethod> _methods = new List<AbcMethod>();

        private struct Item
        {
            public readonly AbcMethodHandler handler;
            public readonly AbcCoder coder;

            public Item(AbcMethodHandler h)
            {
                handler = h;
                coder = null;
            }

            public Item(AbcCoder coder)
            {
                this.coder = coder;
                handler = null;
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
#if DEBUG
                DebugService.DoCancel();
#endif
                var method = _methods[i];
                var item = _items[i];
                if (item.handler != null)
                {
                    item.handler(method);
                }
                else
                {
                    var code = new AbcCode(method.ByteCode);
                    item.coder(code);
                    method.Finish(code);
                }
            }
        }
    }
}