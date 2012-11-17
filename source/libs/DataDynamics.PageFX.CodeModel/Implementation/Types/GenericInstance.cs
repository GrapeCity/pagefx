using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.Syntax;
using Enumerable = System.Linq.Enumerable;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class GenericInstance : CustomAttributeProvider, IGenericInstance
    {
		private readonly ReadOnlyTypeCollection _args;
		private ITypeMemberCollection _members;
		private IFieldCollection _fields;
		private IMethodCollection _methods;
		private IPropertyCollection _properties;
		private IEventCollection _events;

    	public GenericInstance(IGenericType type, IEnumerable<IType> args)
        {
    		if (type == null) throw new ArgumentNullException("type");
    		if (args == null) throw new ArgumentNullException("args");

    		_args = new ReadOnlyTypeCollection(args);

			if (_args.Count != type.GenericParameters.Count)
                throw new InvalidOperationException();

            Type = type;
        }

        public GenericInstance(IGenericType type, params IType[] args)
            : this(type, (IEnumerable<IType>)args)
        {
        }

        void InitMembers()
        {
            if (_members != null) return;

			var fields = new List<ITypeMember>();
			var methods = new List<ITypeMember>();
			var properties = new List<ITypeMember>();
			var events = new List<ITypeMember>();

        	Action<ITypeMember> addMember =
        		m =>
        			{
						switch (m.MemberType)
						{
							case MemberType.Field:
								fields.Add(m);
								break;
							case MemberType.Method:
							case MemberType.Constructor:
								methods.Add(m);
								break;
							case MemberType.Property:
								properties.Add(m);
								break;
							case MemberType.Event:
								events.Add(m);
								break;
						}
        			};

	        Func<IMethod, IMethod> addMethod = m =>
		        {
			        if (m == null) return null;
			        var proxy = new MethodProxy(this, m);
			        addMember(proxy);
			        return proxy;
		        };

            foreach (var member in Type.Members)
            {
                var f = member as IField;
                if (f != null)
                {
                    addMember(new FieldProxy(this, f));
                    continue;
                }

                var method = member as IMethod;
                if (method != null)
                {
                    if (method.Association != null)
                        continue;

                    addMember(new MethodProxy(this, method));
                    continue;
                }
            }

            foreach (var prop in Type.Properties)
            {
	            var getter = addMethod(prop.Getter);
	            var setter = addMethod(prop.Setter);
	            addMember(new PropertyProxy(this, prop, getter, setter));
            }

            foreach (var e in Type.Events)
            {
	            var adder = addMethod(e.Adder);
	            var remover = addMethod(e.Remover);
	            var raiser = addMethod(e.Raiser);

	            addMember(new EventProxy(this, e, adder, remover, raiser));
            }

        	var all = fields.Concat(methods).Concat(properties).Concat(events).AsReadOnlyList();

			_members = new ReadOnlyMemberCollection(all);
			_fields = new ReadOnlyFieldCollection(_members.Segment(0, fields.Count));
			_methods = new ReadOnlyMethodCollection(_members.Segment(fields.Count, methods.Count));
			_properties = new ReadOnlyPropertyCollection(_members.Segment(fields.Count + methods.Count, properties.Count));
			_events = new ReadOnlyEventCollection(_members.Segment(fields.Count + methods.Count + properties.Count, events.Count));
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
            set
            {
                if (Type != null)
                    Type.Namespace = value;
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
        	set
            {
                if (Type != null)
                    Type.IsAbstract = value;
            }
        }

        public bool IsSealed
        {
            get { return Type != null && Type.IsSealed; }
        	set
            {
                if (Type != null)
                    Type.IsSealed = value;
            }
        }

        public bool IsBeforeFieldInit
        {
            get { return Type == null || Type.IsBeforeFieldInit; }
        	set
            {
                if (Type != null)
                    Type.IsBeforeFieldInit = value;
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating wheher the type is generated by compiler.
        /// </summary>
        public bool IsCompilerGenerated
        {
            get { return Type != null && Type.IsCompilerGenerated; }
        	set { throw new NotSupportedException(); }
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

        public bool HasIEnumerableInstance { get; set; }

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
			get
			{
				return _ifaces ?? (_ifaces = Type.Interfaces != null
					                             ? new ReadOnlyTypeCollection(
						                               Type.Interfaces.Select(x => GenericType.Resolve(this, null, x)))
					                             : ReadOnlyTypeCollection.Empty);
			}
        }
        private ReadOnlyTypeCollection _ifaces;

        public IType ValueType
        {
            get { return Type != null ? Type.ValueType : null; }
        }

        public SystemType SystemType
        {
            get { return null; }
            set { throw new NotSupportedException(); }
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

        /// <summary>
        /// Gets or sets members defined with syntax of some language
        /// </summary>
        public string CustomMembers
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets or sets type source code.
        /// </summary>
        public string SourceCode
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        #endregion

        #region Names
        /// <summary>
        /// Gets c# keyword used for this type
        /// </summary>
        public string CSharpKeyword
        {
            get { return ""; }
        }

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

        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Type; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
            get { return Type != null ? Type.ChildNodes : null; }
        }

        /// <summary>
        /// Gets or sets user defined data assotiated with this object.
        /// </summary>
        public object Tag { get; set; }

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

		#region ReadOnlyFieldCollection

    	private abstract class WrapCollection<T> : IReadOnlyList<T>, ICodeNode where T:ITypeMember
    	{
    		protected readonly IReadOnlyList<ITypeMember> Members;

    		protected WrapCollection(IReadOnlyList<ITypeMember> members)
			{
				Members = members;
			}

    		public int Count
    		{
    			get { return Members.Count; }
    		}

    		public T this[int index]
    		{
    			get { return (T)Members[index]; }
    		}

    		public IEnumerator<T> GetEnumerator()
    		{
    			return Members.Cast<T>().GetEnumerator();
    		}

    		IEnumerator IEnumerable.GetEnumerator()
    		{
    			return GetEnumerator();
    		}

			public abstract CodeNodeType NodeType { get; }

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return Members.Cast<ICodeNode>(); }
			}

			public object Tag { get; set; }

    		public string ToString(string format, IFormatProvider formatProvider)
    		{
    			return SyntaxFormatter.Format(this, format, formatProvider);
    		}

    		public void Add(T member)
    		{
    			throw new NotSupportedException();
    		}
    	}

		private sealed class ReadOnlyFieldCollection : WrapCollection<IField>, IFieldCollection
		{
    		public ReadOnlyFieldCollection(IReadOnlyList<ITypeMember> fields)
				: base(fields)
			{
			}

			public override CodeNodeType NodeType
			{
				get { return CodeNodeType.Fields; }
			}

			public IField this[string name]
			{
				get { return (IField)Members.FirstOrDefault(x => x.Name == name); }
			}
		}

		#endregion

		#region ReadOnlyMethodCollection

		private sealed class ReadOnlyMethodCollection : WrapCollection<IMethod>, IMethodCollection
		{
			private readonly IDictionary<string, IEnumerable<IMethod>> _groups;
			private readonly IEnumerable<IMethod> _ctors;
			private readonly IMethod _cctor;

			public ReadOnlyMethodCollection(IReadOnlyList<ITypeMember> methods) : base(methods)
			{
				_groups = methods.Cast<IMethod>().GroupBy(x => x.Name).ToDictionary(x => x.Key, x => (IEnumerable<IMethod>)x);
				_ctors = methods.Cast<IMethod>().Where(x => x.IsConstructor);
				_cctor = _ctors.FirstOrDefault(x => x.IsStatic);
			}

			public IEnumerable<IMethod> Find(string name)
			{
				IEnumerable<IMethod> group;
				return _groups.TryGetValue(name, out group) ? group : Enumerable.Empty<IMethod>();
			}

			public override CodeNodeType NodeType
			{
				get { return CodeNodeType.Methods; }
			}

			public IEnumerable<IMethod> Constructors
			{
				get { return _ctors; }
			}

			public IMethod StaticConstructor
			{
				get { return _cctor; }
			}
		}

		#endregion

		#region ReadOnlyPropertyCollection

		private sealed class ReadOnlyPropertyCollection : WrapCollection<IProperty>, IPropertyCollection
		{
			private readonly IDictionary<string, IEnumerable<IProperty>> _groups;

			public ReadOnlyPropertyCollection(IReadOnlyList<ITypeMember> properties) : base(properties)
			{
				_groups = properties.Cast<IProperty>().GroupBy(x => x.Name).ToDictionary(x => x.Key, x => (IEnumerable<IProperty>)x);
			}

			public IEnumerable<IProperty> Find(string name)
			{
				IEnumerable<IProperty> group;
				return _groups.TryGetValue(name, out group) ? group : Enumerable.Empty<IProperty>();
			}

			public override CodeNodeType NodeType
			{
				get { return CodeNodeType.Properties; }
			}
		}

		#endregion

		#region ReadOnlyEventCollection

		private sealed class ReadOnlyEventCollection : WrapCollection<IEvent>, IEventCollection
		{
			public ReadOnlyEventCollection(IReadOnlyList<ITypeMember> events) : base(events)
			{
			}

			public IEvent this[string name]
			{
				get { return (IEvent)Members.FirstOrDefault(x => x.Name == name); }
			}

			public override CodeNodeType NodeType
			{
				get { return CodeNodeType.Events; }
			}
		}

		#endregion

		#region ReadOnlyTypeCollection

		private sealed class ReadOnlyTypeCollection : ITypeCollection
		{
			public static readonly ReadOnlyTypeCollection Empty = new ReadOnlyTypeCollection(Enumerable.Empty<IType>());
			
			private readonly IReadOnlyList<IType> _types;

			public ReadOnlyTypeCollection(IEnumerable<IType> types)
			{
				_types = types.AsReadOnlyList();
			}

			public int Count
			{
				get { return _types.Count; }
			}

			public IType this[int index]
			{
				get { return _types[index]; }
			}

			public IType this[string fullname]
			{
				get
				{
					//TODO PERF: add dictionary for quick search types by fullname
					return _types.FirstOrDefault(t => t.FullName == fullname);
				}
			}

			public void Add(IType type)
			{
				throw new NotSupportedException("This collection is readonly.");
			}

			public bool Contains(IType type)
			{
				return _types.Contains(type);
			}

			public CodeNodeType NodeType
			{
				get { return CodeNodeType.Types; }
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return _types.Cast<ICodeNode>(); }
			}

			public object Tag { get; set; }

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return SyntaxFormatter.Format(this, format, formatProvider);
			}

			public IEnumerator<IType> GetEnumerator()
			{
				return _types.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		#endregion
	}
}