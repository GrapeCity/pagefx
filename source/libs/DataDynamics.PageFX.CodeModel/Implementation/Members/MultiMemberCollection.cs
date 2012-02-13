using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.Collections;

namespace DataDynamics.PageFX.CodeModel
{
    public class MultiMemberCollection<T> : ISimpleList<T>
        where T: ITypeMember
    {
        readonly List<T> _list = new List<T>();
        readonly Hashtable _cache = new Hashtable();
        readonly IType _owner;

        public MultiMemberCollection(IType owner)
        {
            _owner = owner;
        }

        #region IEnumerable Members
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

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

        public IEnumerable<T> this[string name]
        {
            get
            {
                var result = _cache[name] as IEnumerable<T>;
                if (result == null)
                    return EmptyEnumerable<T>.Instance;
                return result;
            }
        }

        public T this[string name, Predicate<T> predicate]
        {
            get
            {
                foreach (var m in this[name])
                {
                    if (predicate(m))
                        return m;
                }
                return default(T);
            }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return _list.Cast<ICodeNode>(); }
        }
    }
}