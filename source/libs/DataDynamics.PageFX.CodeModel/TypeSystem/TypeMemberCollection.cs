using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    public sealed class TypeMemberCollection : ITypeMemberCollection
    {
		private IFieldCollection _fields;
		private IMethodCollection _methods;
		private IPropertyCollection _properties;
		private IEventCollection _events;
		private readonly IType _owner;

	    public TypeMemberCollection(IType owner)
        {
		    if (owner == null)
				throw new ArgumentNullException("owner");

		    _owner = owner;
            _fields = new FieldCollection(owner);
            _methods = new MethodCollection(owner);
            _properties = new PropertyCollection(owner);
            _events = new EventCollection(owner);
        }

	    public IFieldCollection Fields
        {
            get { return _fields; }
			set { _fields = value; }
        }
		
        public IMethodCollection Methods
        {
            get { return _methods; }
			set { _methods = value; }
        }

        public IPropertyCollection Properties
        {
            get { return _properties; }
			set { _properties = value; }
        }

        public IEventCollection Events
        {
            get { return _events; }
			set { _events = value; }
        }

	    #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

	    public int Count
        {
            get { return _fields.Count + _methods.Count + _properties.Count + _events.Count; }
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
            if (ReferenceEquals(m.DeclaringType, _owner))
                return;

            m.DeclaringType = _owner;

            switch (m.MemberType)
            {
                case MemberType.Field:
                    _fields.Add((IField)m);
                    break;

                case MemberType.Method:
                case MemberType.Constructor:
                    _methods.Add((IMethod)m);
                    break;

                case MemberType.Property:
                    _properties.Add((IProperty)m);
                    break;

                case MemberType.Event:
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

	    public CodeNodeType NodeType
        {
            get { return CodeNodeType.TypeMembers; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

        public object Tag { get; set; }

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