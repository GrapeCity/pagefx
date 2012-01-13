using System.Collections;
using System.Collections.Generic;

namespace DataDynamics.PageFX.FLI.SWF
{
    public interface IIDList : IEnumerable<int>
    {
        int Count { get; }
        int this[int index] { get; }
        void Add(int id);
    }

    internal class IDList : IIDList
    {
        private readonly List<int> _list = new List<int>();
        private readonly Hashtable _hash = new Hashtable();

        public int[] ToArray()
        {
            return _list.ToArray();
        }

        #region IIDList Members
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
            if (_hash.Contains(id))
                return;
            _list.Add(id);
            _hash.Add(id, this);
        }
        #endregion

        #region IEnumerable<int> Members
        public IEnumerator<int> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion
    }
}