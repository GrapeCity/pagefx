using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class TypeMemberCollection : ITypeMemberCollection
    {
		private IFieldCollection _fields = new FieldCollection();
		private IMethodCollection _methods = new MethodCollection();
		private IPropertyCollection _properties = new PropertyCollection();
		private IEventCollection _events = new EventCollection();

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

        public void Add(ITypeMember member)
        {
	        if (member == null)
				throw new ArgumentNullException("member");

	        //TODO: avoid duplicates

	        switch (member.MemberType)
            {
                case MemberType.Field:
                    _fields.Add((IField)member);
                    break;

                case MemberType.Method:
                case MemberType.Constructor:
                    _methods.Add((IMethod)member);
                    break;

                case MemberType.Property:
                    _properties.Add((IProperty)member);
                    break;

                case MemberType.Event:
                    _events.Add((IEvent)member);
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

        public object Data { get; set; }

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