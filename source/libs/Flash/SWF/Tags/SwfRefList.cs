using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Flash.Swf.Tags
{
    public sealed class SwfRefList : IReadOnlyList<int>
    {
		private readonly HashList<int, int> _list = new HashList<int, int>(x => x);

        public int[] ToArray()
        {
            return _list.ToArray();
        }

	    public int Count
        {
            get { return _list.Count; }
        }

        public int this[int index]
        {
            get { return _list[index]; }
        }

        public void Add(int id)
        {
			if (_list.Contains(id)) return;
            _list.Add(id);
        }

	    public IEnumerator<int> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

	    IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}