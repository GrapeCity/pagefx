using System;
using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    [XmlElementName("Members")]
    public class TypeMemberCollection : ITypeMemberCollection
    {
        #region Constructors
        public TypeMemberCollection(IType owner)
        {
            _owner = owner;
            _fields = new FieldCollection(owner);
            _methods = new MethodCollection(owner);
            _properties = new PropertyCollection(owner);
            _events = new EventCollection(owner);
        }
        readonly IType _owner;
        #endregion

        #region Public Properties
        public IFieldCollection Fields
        {
            get { return _fields; }
        }

        readonly FieldCollection _fields;

        public IMethodCollection Methods
        {
            get { return _methods; }
        }

        readonly MethodCollection _methods;

        public IPropertyCollection Properties
        {
            get { return _properties; }
        }

        readonly PropertyCollection _properties;

        public IEventCollection Events
        {
            get { return _events; }
        }
        readonly EventCollection _events;
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ITypeMemberCollection Members
        public int Count
        {
            get
            {
                return _fields.Count + _methods.Count + _properties.Count + _events.Count;
            }
        }

        public ITypeMember this[int index]
        {
            get
            {
                if (index < 0) return null;

                int n = _fields.Count;
                if (index < n)
                    return _fields[index];
                index -= n;

                n = _methods.Count;
                if (index < n)
                    return _methods[index];
                index -= n;

                n = _properties.Count;
                if (index < n)
                    return _properties[index];
                index -= n;

                n = _events.Count;
                if (index < n)
                    return _events[index];

                return null;
            }
        }

        public void Add(ITypeMember m)
        {
            if (m.DeclaringType == _owner)
                return;

            m.DeclaringType = _owner;

            switch (m.MemberType)
            {
                case TypeMemberType.Field:
                    _fields.Add((IField)m);
                    break;

                case TypeMemberType.Method:
                case TypeMemberType.Constructor:
                    _methods.Add((IMethod)m);
                    break;

                case TypeMemberType.Property:
                    _properties.Add((IProperty)m);
                    break;

                case TypeMemberType.Event:
                    _events.Add((IEvent)m);
                    break;
            }
        }

        public IEnumerator<ITypeMember> GetEnumerator()
        {
            foreach (var m in _fields)
                yield return m;
            foreach (var m in _methods)
                yield return m;
            foreach (var m in _properties)
                yield return m;
            foreach (var m in _events)
                yield return m;
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.TypeMembers; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Convert(this); }
        }

        public object Tag { get; set; }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }

    public class TypeMemberCollection<T> : IList<T>, IList, ICodeNode
        where T : ITypeMember
    {
        readonly IType _owner;
        protected readonly List<T> _list = new List<T>();

        public TypeMemberCollection(IType owner)
        {
            _owner = owner;
        }

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get
            {
                Type t = typeof(T);
                if (t == typeof(IField))
                    return CodeNodeType.Fields;
                if (t == typeof(IMethod))
                    return CodeNodeType.Methods;
                if (t == typeof(IProperty))
                    return CodeNodeType.Properties;
                if (t == typeof(IEvent))
                    return CodeNodeType.Events;
                return CodeNodeType.TypeMembers;
            }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Convert(this); }
        }

        /// <summary>
        /// Gets or sets user defined data assotiated with this object.
        /// </summary>
        public object Tag { get; set; }
        #endregion

        #region IList Implementation
        public void Add(T item)
        {
            item.DeclaringType = _owner;
            _list.Add(item);
        }

        int IList.Add(object value)
        {
            return ((IList)_list).Add(value);
        }

        bool IList.Contains(object value)
        {
            return ((IList)_list).Contains(value);
        }

        public void Clear()
        {
            _list.Clear();
        }

        int IList.IndexOf(object value)
        {
            return ((IList)_list).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            ((IList)_list).Insert(index, value);
        }

        void IList.Remove(object value)
        {
            ((IList)_list).Remove(value);
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return _list.Remove(item);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ((IList)_list).CopyTo(array, index);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)_list).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)_list).IsSynchronized; }
        }

        object IList.this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = (T)value; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        bool IList.IsFixedSize
        {
            get { return ((IList)_list).IsFixedSize; }
        }

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }
        #endregion

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}