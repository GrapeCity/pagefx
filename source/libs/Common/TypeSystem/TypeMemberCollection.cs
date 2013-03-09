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
		public static readonly ITypeMemberCollection Empty = new EmptyImpl();

		private readonly IFieldCollection _fields;
		private readonly IMethodCollection _methods;
		private readonly IPropertyCollection _properties;
		private readonly IEventCollection _events;

	    public TypeMemberCollection()
	    {
		    _fields = new FieldCollection();
		    _methods = new MethodCollection();
		    _properties = new PropertyCollection();
		    _events = new EventCollection();
	    }

	    public TypeMemberCollection(IFieldCollection fields, IMethodCollection methods, IPropertyCollection properties, IEventCollection events)
		{
			if (fields == null) throw new ArgumentNullException("fields");
			if (methods == null) throw new ArgumentNullException("methods");
			if (properties == null) throw new ArgumentNullException("properties");
			if (events == null) throw new ArgumentNullException("events");

			_fields = fields;
			_methods = methods;
			_properties = properties;
			_events = events;
		}

	    public IFieldCollection Fields
        {
            get { return _fields; }
        }
		
        public IMethodCollection Methods
        {
            get { return _methods; }
        }

        public IPropertyCollection Properties
        {
            get { return _properties; }
        }

        public IEventCollection Events
        {
            get { return _events; }
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

		private sealed class EmptyImpl : ITypeMemberCollection
		{
			public IEnumerator<ITypeMember> GetEnumerator()
			{
				return Enumerable.Empty<ITypeMember>().GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int Count
			{
				get { return 0; }
			}

			public ITypeMember this[int index]
			{
				get { throw new ArgumentOutOfRangeException(); }
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return Enumerable.Empty<ICodeNode>(); }
			}

			public object Data { get; set; }

			public void Add(ITypeMember member)
			{
				throw new NotSupportedException();
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
}