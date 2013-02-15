using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public class MultiMemberCollection<T> : IReadOnlyList<T>, ICodeNode
        where T: ITypeMember
    {
        private readonly List<T> _list = new List<T>();
		private readonly Hashtable _cache = new Hashtable();

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

	    public int Count
        {
            get { return _list.Count; }
        }

        public T this[int index]
        {
            get { return _list[index]; }
        }

	    public void Add(T member)
        {
			if (member == null)
				throw new ArgumentNullException("member");

            _list.Add(member);

            string name = member.Name;

            var list = _cache[name] as List<T>;
            if (list == null)
            {
                list = new List<T>();
                _cache[name] = list;
            }

            list.Add(member);

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

		public object Data { get; set; }

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return _list.Cast<ICodeNode>(); }
        }

	    public string ToString(string format, IFormatProvider formatProvider)
	    {
		    return SyntaxFormatter.Format(this, format, formatProvider);
	    }

		public override string ToString()
		{
			return ToString(null, null);
		}
    }
}