using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CodeModel
{
    public class MultiMemberCollection<T> : IReadOnlyList<T>
        where T: ITypeMember
    {
        private readonly List<T> _list = new List<T>();
		private readonly Hashtable _cache = new Hashtable();
		private readonly IType _owner;

        public MultiMemberCollection(IType owner)
        {
            _owner = owner;
        }

    	public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    	#region ISimpleList Members
        public int Count
        {
            get { return _list.Count; }
        }

        public T this[int index]
        {
            get { return _list[index]; }
        }
        #endregion

        public void Add(T member)
        {
            member.DeclaringType = _owner;
            _list.Add(member);
            string name = member.Name;
            var l = _cache[name] as List<T>;
            if (l == null)
            {
                l = new List<T>();
                _cache[name] = l;
            }
            l.Add(member);
            OnAdd(member);
        }

        protected virtual void OnAdd(T member)
        {
        }

    	public IEnumerable<T> Find(string name)
    	{
    		var result = _cache[name] as IEnumerable<T>;
    		return result ?? Enumerable.Empty<T>();
    	}

    	public IEnumerable<ICodeNode> ChildNodes
        {
            get { return _list.Cast<ICodeNode>(); }
        }
    }
}