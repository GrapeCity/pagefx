using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class GenericInstance : CustomAttributeProvider, IGenericInstance
    {
		private readonly ITypeCollection _args;
		private ITypeMemberCollection _members;
		private IFieldCollection _fields;
		private IMethodCollection _methods;
		private IPropertyCollection _properties;
		private IEventCollection _events;

    	public GenericInstance(IGenericType type, IEnumerable<IType> args)
        {
    		if (type == null) throw new ArgumentNullException("type");
    		if (args == null) throw new ArgumentNullException("args");

    		_args = new TypeList(args);

			if (_args.Count != type.GenericParameters.Count)
                throw new InvalidOperationException();

            Type = type;
        }

        public GenericInstance(IGenericType type, params IType[] args)
            : this(type, (IEnumerable<IType>)args)
        {
        }

        private void InitMembers()
        {
            if (_members != null) return;

			_fields = new FieldList(this);
			_properties = new PropertyList(this);
			_events = new EventList(this);
			_methods = new MethodList(this);

	        _members = new TypeMemberCollection(this)
		        {
			        Fields = _fields,
			        Properties = _properties,
			        Events = _events,
			        Methods = _methods
		        };
        }

    	public IGenericType Type { get; set; }

    	public ITypeCollection GenericArguments
        {
            get { return _args; }
        }

    	#region IType Members
        public string Namespace
        {
            get
            {
                if (Type != null)
                    return Type.Namespace;
                return null;
            }
        }

        public string FullName
        {
            get { return _fullName ?? (_fullName = EvalName(TypeNameKind.FullName, TypeNameKind.FullName)); }
        }
        private string _fullName;

        public TypeKind TypeKind
        {
            get { return Type != null ? Type.TypeKind : TypeKind.Class; }
        }

        public bool IsAbstract
        {
            get { return Type != null && Type.IsAbstract; }
        }

        public bool IsSealed
        {
            get { return Type != null && Type.IsSealed; }
        }

	    public bool IsPartial
	    {
			get { return false; }
	    }

	    public bool IsBeforeFieldInit
        {
            get { return Type != null && Type.IsBeforeFieldInit; }
        }

        public bool IsInterface
        {
            get { return TypeKind == TypeKind.Interface; }
        }

        /// <summary>
        /// Determines whether this type is class.
        /// </summary>
        public bool IsClass
        {
            get { return TypeKind == TypeKind.Class; }
        }

        public bool IsArray
        {
            get { return false; }
        }

        /// <summary>
        /// Determines whether this type is enum type.
        /// </summary>
        public bool IsEnum
        {
            get { return TypeKind == TypeKind.Enum; }
        }

        public IMethod DeclaringMethod
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

        public IType BaseType
        {
            get { return _baseType ?? (_baseType = GenericType.Resolve(this, Type.BaseType)); }
        	set { throw new NotSupportedException(); }
        }
        private IType _baseType;

        public ITypeCollection Interfaces
        {
			get { return _ifaces ?? (_ifaces = new InterfaceList(this)); }
        }
        private ITypeCollection _ifaces;

        public IType ValueType
        {
            get { return Type != null ? Type.ValueType : null; }
        }

        public IFieldCollection Fields
        {
            get
            {
                InitMembers();
                return _fields;
            }
        }

        public IMethodCollection Methods
        {
            get
            {
                InitMembers();
                return _methods;
            }
        }

        public IPropertyCollection Properties
        {
            get
            {
                InitMembers();
                return _properties;
            }
        }

        public IEventCollection Events
        {
            get
            {
                InitMembers();
                return _events;
            }
        }

        public ITypeMemberCollection Members
        {
            get
            {
                InitMembers();
                return _members;
            }
        }

        public ClassLayout Layout
        {
            get { return Type != null ? Type.Layout : null; }
        	set { throw new NotSupportedException(); }
        }

	    #endregion

        #region Names

	    string EvalNameBase(TypeNameKind kind)
        {
            switch (kind)
            {
                case TypeNameKind.DisplayName:
                    return GenericType.ToDisplayName(Type.FullName);
                case TypeNameKind.FullName:
                    return Type.FullName;
                case TypeNameKind.SigName:
                    return Type.SigName;
                case TypeNameKind.Key:
                    return Type.FullName;
                case TypeNameKind.Name:
                    return Type.Name;
                case TypeNameKind.NestedName:
                    return Type.NestedName;
            }
            return Type.FullName;
        }

		string EvalName(TypeNameKind kind, TypeNameKind argKind)
		{
			return EvalName(kind, argKind, true);
		}

        string EvalName(TypeNameKind kind, TypeNameKind argKind, bool clr)
        {
            return EvalNameBase(kind) + GenericType.Format(_args, argKind, clr);
        }

        /// <summary>
        /// Gets unique key of this type. Used for <see cref="TypeFactory"/>.
        /// </summary>
        public string Key
        {
            get { return _key ?? (_key = EvalName(TypeNameKind.Key, TypeNameKind.Key)); }
        	internal set { _key = value; }
        }
        private string _key;

        /// <summary>
        /// Gets name of the type used in signatures.
        /// </summary>
        public string SigName
        {
            get { return _sigName ?? (_sigName = EvalName(TypeNameKind.SigName, TypeNameKind.SigName)); }
        }
        private string _sigName;

        /// <summary>
        /// Name with names of enclosing types.
        /// </summary>
        public string NestedName
        {
            get { return _nestedName ?? (_nestedName = EvalName(TypeNameKind.NestedName, TypeNameKind.FullName)); }
        }
        private string _nestedName;

        #endregion

        #region ITypeMember Members
        /// <summary>
        /// Gets the assembly in which the member is declared.
        /// </summary>
        public IAssembly Assembly
        {
            get
            {
                var module = Module;
                return module != null ? module.Assembly : null;
            }
        }

        /// <summary>
        /// Gets the module in which the member is defined. 
        /// </summary>
        public IModule Module
        {
            get { return Type != null ? Type.Module : null; }
        	set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public MemberType MemberType
        {
            get { return MemberType.Type; }
        }

        public string Name
        {
            get { return _name ?? (_name = EvalName(TypeNameKind.Name, TypeNameKind.FullName)); }
        	set { throw new NotSupportedException(); }
        }
        private string _name;

        public string DisplayName
        {
			get { return _displayName ?? (_displayName = EvalName(TypeNameKind.DisplayName, TypeNameKind.DisplayName, false)); }
        }
    	private string _displayName;

        public IType DeclaringType { get; set; }

        IType ITypeMember.Type
        {
            get { return Type; }
            set { Type = value as IGenericType; }
        }

        /// <summary>
        /// Gets visibility of this member.
        /// </summary>
        public Visibility Visibility
        {
            get { return Type != null ? Type.Visibility : Visibility.Public; }
        	set
            {
                if (Type != null)
                    Type.Visibility = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                if (DeclaringType != null && !DeclaringType.IsVisible)
                    return false;
                return Type == null || Type.IsVisible;
            }
        }

        public bool IsStatic
        {
            get { return Type != null && Type.IsStatic; }
        	set
            {
                if (Type != null)
                    Type.IsStatic = value;
            }
        }

        public bool IsSpecialName
        {
            get { return Type != null && Type.IsSpecialName; }
        	set
            {
                if (Type != null)
                    Type.IsSpecialName = value;
            }
        }

        public bool IsRuntimeSpecialName
        {
            get { return Type != null && Type.IsRuntimeSpecialName; }
        	set
            {
                if (Type != null)
                    Type.IsRuntimeSpecialName = value;
            }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken
        {
            get { return -1; }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region ICodeNode Members

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return Type != null ? Type.ChildNodes : null; }
        }

        /// <summary>
        /// Gets or sets user defined data assotiated with this object.
        /// </summary>
        public object Data { get; set; }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return FullName;
        }

        #endregion

        #region ITypeContainer Members

        public ITypeCollection Types
        {
            get { return Type != null ? Type.Types : null; }
        }

        #endregion

        #region IDocumentationProvider Members

        /// <summary>
        /// Gets or sets documentation of this member
        /// </summary>
        public string Documentation { get; set; }

        #endregion

        #region Object Override Members

        public override bool Equals(object obj)
        {
            return this.IsEqual(obj as IType);
        }

        public override int GetHashCode()
        {
            return this.EvalHashCode();
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        #endregion

		#region class LazyList

		private abstract class LazyList<T> : IReadOnlyList<T>, ICodeNode
    	{
    		private IReadOnlyList<T> _list;

    		private IReadOnlyList<T> List
    		{
    			get { return _list ?? (_list = Populate().Memoize()); }
    		}

    		protected abstract IEnumerable<T> Populate();
    		
    		public int Count
    		{
    			get { return List.Count; }
    		}

    		public T this[int index]
    		{
    			get { return List[index]; }
    		}

    		public IEnumerator<T> GetEnumerator()
    		{
    			return List.GetEnumerator();
    		}

    		IEnumerator IEnumerable.GetEnumerator()
    		{
    			return GetEnumerator();
    		}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return List.Cast<ICodeNode>(); }
			}

			public object Data { get; set; }

    		public string ToString(string format, IFormatProvider formatProvider)
    		{
    			return SyntaxFormatter.Format(this, format, formatProvider);
    		}
    	}

		#endregion

		#region class FieldList

		private sealed class FieldList : LazyList<IField>, IFieldCollection
		{
			private readonly GenericInstance _owner;

			public FieldList(GenericInstance owner)
			{
				_owner = owner;
			}

			public IField this[string name]
			{
				get { return this.FirstOrDefault(x => x.Name == name); }
			}

			public void Add(IField item)
			{
				throw new NotSupportedException();
			}

			protected override IEnumerable<IField> Populate()
			{
				return _owner.Type.Fields.Select(f => (IField)new FieldProxy(_owner, f));
			}
		}

		#endregion

		#region MethodList

		private sealed class MethodList : IMethodCollection
		{
			private readonly GenericInstance _owner;
			private IReadOnlyList<IMethod> _list;
			private IDictionary<string, List<IMethod>> _lookup;
			private IEnumerable<IMethod> _ctors;
			private IMethod _cctor;
			private bool _resolveStaticCtor = true;
			private readonly List<IMethod> _instances = new List<IMethod>();

			public MethodList(GenericInstance owner)
			{
				_owner = owner;
			}

			public int Count
			{
				get { return List.Count + _instances.Count; }
			}

			public IMethod this[int index]
			{
				get
				{
					if (_instances.Count == 0)
						return List[index];
					if (index < 0 || index >= Count)
						throw new ArgumentOutOfRangeException("index");
					return index < List.Count ? List[index] : _instances[index - List.Count];
				}
			}

			public IEnumerable<IMethod> Find(string name)
			{
				if (_lookup == null)
				{
					_lookup = this.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.ToList());
				}
				List<IMethod> list;
				return _lookup.TryGetValue(name, out list) ? list.AsReadOnly() : Enumerable.Empty<IMethod>();
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return this.Cast<ICodeNode>(); }
			}

			public object Data { get; set; }

			public IEnumerable<IMethod> Constructors
			{
				get { return _ctors ?? (_ctors = this.Where(x => x.IsConstructor).Memoize()); }
			}

			public IMethod StaticConstructor
			{
				get
				{
					if (_resolveStaticCtor)
					{
						_resolveStaticCtor = false;
						_cctor = Constructors.FirstOrDefault(x => x.IsStatic);
					}
					return _cctor;
				}
			}

			public void Add(IMethod method)
			{
				if (method == null)
					throw new ArgumentNullException("method");

				if (!method.IsGenericInstance)
					throw new InvalidOperationException();

				_instances.Add(method);

				// update lookup
				if (_lookup != null)
				{
					List<IMethod> list;
					if (!_lookup.TryGetValue(method.Name, out list))
					{
						list = new List<IMethod>();
						_lookup.Add(method.Name, list);
					}

					list.Add(method);
				}
			}

			public IEnumerator<IMethod> GetEnumerator()
			{
				foreach (var method in List)
				{
					yield return method;
				}
				foreach (var method in _instances)
				{
					yield return method;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			private IReadOnlyList<IMethod> List
			{
				get { return _list ?? (_list = Populate().Memoize()); }
			}

			private IEnumerable<IMethod> Populate()
			{
				return _owner.Type.Methods.Select(method => (IMethod)new MethodProxy(_owner, method));
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return SyntaxFormatter.Format(this, format, formatProvider);
			}
		}

		#endregion

		#region PropertyList

		private sealed class PropertyList : LazyList<IProperty>, IPropertyCollection
		{
			private readonly GenericInstance _owner;
			private IDictionary<string, IEnumerable<IProperty>> _groups;

			public PropertyList(GenericInstance owner)
			{
				_owner = owner;
			}

			public IEnumerable<IProperty> Find(string name)
			{
				if (_groups == null)
				{
					_groups = this.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => (IEnumerable<IProperty>)x);
				}
				IEnumerable<IProperty> group;
				return _groups.TryGetValue(name, out group) ? group : Enumerable.Empty<IProperty>();
			}

			public void Add(IProperty item)
			{
				throw new NotSupportedException();
			}

			protected override IEnumerable<IProperty> Populate()
			{
				return _owner.Type.Properties.Select(prop => (IProperty)new PropertyProxy(_owner, prop));
			}
		}

		#endregion

		#region EventList

		private sealed class EventList : LazyList<IEvent>, IEventCollection
		{
			private readonly GenericInstance _owner;

			public EventList(GenericInstance owner)
			{
				_owner = owner;
			}

			public IEvent this[string name]
			{
				get { return this.FirstOrDefault(x => x.Name == name); }
			}

			public void Add(IEvent item)
			{
				throw new NotSupportedException();
			}

			protected override IEnumerable<IEvent> Populate()
			{
				return _owner.Type.Events.Select(e => (IEvent)new EventProxy(_owner, e));
			}
		}

		#endregion

		#region TypeList

		private sealed class TypeList : LazyList<IType>, ITypeCollection
		{
			private readonly IEnumerable<IType> _types;

			public TypeList(IEnumerable<IType> types)
			{
				_types = types;
			}

			public IType FindType(string fullname)
			{
				//TODO PERF: add dictionary for quick search types by fullname
				return this.FirstOrDefault(t => t.FullName == fullname);
			}

			public void Add(IType type)
			{
				throw new NotSupportedException("This collection is readonly.");
			}

			public bool Contains(IType type)
			{
				return type != null && this.Any(x => ReferenceEquals(x, type));
			}

			protected override IEnumerable<IType> Populate()
			{
				return _types;
			}
		}

		#endregion

		private sealed class InterfaceList : LazyList<IType>, ITypeCollection
		{
			private readonly GenericInstance _owner;
			private IDictionary<string, IType> _lookup;

			public InterfaceList(GenericInstance owner)
			{
				_owner = owner;
			}

			public IType FindType(string fullname)
			{
				if (string.IsNullOrEmpty(fullname))
					return null;

				if (_lookup == null)
				{
					_lookup = this.ToDictionary(x => x.FullName, x => x);
				}

				IType type;
				return _lookup.TryGetValue(fullname, out type) ? type : null;
			}

			public void Add(IType type)
			{
				throw new NotSupportedException();
			}

			public bool Contains(IType type)
			{
				return type != null && this.Any(x => ReferenceEquals(x, type));
			}

			protected override IEnumerable<IType> Populate()
			{
				var type = _owner.Type;

				if (type.Interfaces == null)
					return Enumerable.Empty<IType>();

				return type.Interfaces.Select(x => GenericType.Resolve(_owner, null, x));
			}
		}
    }
}