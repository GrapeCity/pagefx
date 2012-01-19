using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CodeModel
{
    public class GenericInstance : CustomAttributeProvider, IGenericInstance
    {
        #region Constructors
        public GenericInstance(IGenericType type, IEnumerable<IType> args)
        {
            int n = 0;
            foreach (var arg in args)
            {
                _args.AddInternal(arg);
                ++n;
            }
            if (n != type.GenericParameters.Count)
                throw new InvalidOperationException();
            _genericType = type;
        }

        public GenericInstance(IGenericType type, params IType[] args)
            : this(type, (IEnumerable<IType>)args)
        {
        }

        void InitMembers()
        {
            if (_members != null) return;

            _members = new ReadOnlyMemberCollection();
            _fields = new FieldCollection(this);
            _methods = new MethodCollection(this);
            _properties = new PropertyCollection(this);
            _events = new ReadOnlyEventCollection();

            foreach (var member in _genericType.Members)
            {
                var f = member as IField;
                if (f != null)
                {
                    AddMember(new FieldProxy(this, f));
                    continue;
                }

                var method = member as IMethod;
                if (method != null)
                {
                    if (method.Association != null)
                        continue;

                    AddMember(new MethodProxy(this, method));
                    continue;
                }
            }

            foreach (var prop in _genericType.Properties)
            {
                var getter = prop.Getter;
                if (getter != null)
                {
                    getter = new MethodProxy(this, getter);
                    AddMember(getter);
                }

                var setter = prop.Setter;
                if (setter != null)
                {
                    setter = new MethodProxy(this, setter);
                    AddMember(setter);
                }

                var proxy = new PropertyProxy(this, prop, getter, setter);
                AddMember(proxy);
            }

            foreach (IEvent e in _genericType.Events)
            {
                var adder = e.Adder;
                if (adder != null)
                {
                    adder = new MethodProxy(this, adder);
                    AddMember(adder);
                }

                var remover = e.Remover;
                if (remover != null)
                {
                    remover = new MethodProxy(this, remover);
                    AddMember(remover);
                }

                var raiser = e.Raiser;
                if (raiser != null)
                {
                    raiser = new MethodProxy(this, raiser);
                    AddMember(raiser);
                }

                var proxy = new EventProxy(this, e, adder, remover, raiser);
                AddMember(proxy);
            }
        }

        public void AddMember(ITypeMember member)
        {
            if (_members == null)
                throw new InvalidOperationException();

            _members.AddInternal(member);

            var f = member as IField;
            if (f != null)
            {
                _fields.Add(f);
                return;
            }

            var method = member as IMethod;
            if (method != null)
            {
                _methods.Add(method);
                return;
            }

            var prop = member as IProperty;
            if (prop != null)
            {
                _properties.Add(prop);
                return;
            }

			var e = member as IEvent;
            if (e != null)
            {
                _events.AddInternal(e);
                return;
            }
        }

        ReadOnlyMemberCollection _members;
        FieldCollection _fields;
        MethodCollection _methods;
        PropertyCollection _properties;
        ReadOnlyEventCollection _events;
        #endregion

        #region IGenericTypeInstance Members
        public IGenericType Type
        {
            get { return _genericType; }
            set { _genericType = value; }
        }
        IGenericType _genericType;

        public ITypeCollection GenericArguments
        {
            get { return _args; }
        }
        readonly ReadOnlyTypeCollection _args = new ReadOnlyTypeCollection();
        #endregion

        #region IType Members
        public string Namespace
        {
            get
            {
                if (_genericType != null)
                    return _genericType.Namespace;
                return null;
            }
            set
            {
                if (_genericType != null)
                    _genericType.Namespace = value;
            }
        }

        public string FullName
        {
            get { return _fullName ?? (_fullName = EvalName(TypeNameKind.FullName, TypeNameKind.FullName)); }
        }
        private string _fullName;

        public TypeKind TypeKind
        {
            get { return _genericType != null ? _genericType.TypeKind : TypeKind.Class; }
        }

        public bool IsAbstract
        {
            get { return _genericType != null && _genericType.IsAbstract; }
        	set
            {
                if (_genericType != null)
                    _genericType.IsAbstract = value;
            }
        }

        public bool IsSealed
        {
            get { return _genericType != null && _genericType.IsSealed; }
        	set
            {
                if (_genericType != null)
                    _genericType.IsSealed = value;
            }
        }

        public bool IsBeforeFieldInit
        {
            get { return _genericType == null || _genericType.IsBeforeFieldInit; }
        	set
            {
                if (_genericType != null)
                    _genericType.IsBeforeFieldInit = value;
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating wheher the type is generated by compiler.
        /// </summary>
        public bool IsCompilerGenerated
        {
            get { return _genericType != null && _genericType.IsCompilerGenerated; }
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
            get { return _baseType ?? (_baseType = GenericType.Resolve(this, _genericType.BaseType)); }
        	set { throw new NotSupportedException(); }
        }
        private IType _baseType;

        public ITypeCollection Interfaces
        {
            get
            {
                if (_interfaces == null)
                {
                    _interfaces = new ReadOnlyTypeCollection();
                    if (_genericType.Interfaces != null)
                    {
                        foreach (var resolvedIface in _genericType.Interfaces.Select(iface => GenericType.Resolve(this, null, iface)))
                        {
                        	_interfaces.AddInternal(resolvedIface);
                        }
                    }
                }
                return _interfaces;
            }
        }
        private ReadOnlyTypeCollection _interfaces;

        public IType ValueType
        {
            get { return _genericType != null ? _genericType.ValueType : null; }
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
            get
            {
                if (_genericType != null)
                    return _genericType.Layout;
                return null;
            }
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
                    return GenericType.ToDisplayName(_genericType.FullName);
                case TypeNameKind.FullName:
                    return _genericType.FullName;
                case TypeNameKind.SigName:
                    return _genericType.SigName;
                case TypeNameKind.Key:
                    return _genericType.FullName;
                case TypeNameKind.Name:
                    return _genericType.Name;
                case TypeNameKind.NestedName:
                    return _genericType.NestedName;
            }
            return _genericType.FullName;
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
        string _key;

        /// <summary>
        /// Gets name of the type used in signatures.
        /// </summary>
        public string SigName
        {
            get { return _sigName ?? (_sigName = EvalName(TypeNameKind.SigName, TypeNameKind.SigName)); }
        }
        string _sigName;

        /// <summary>
        /// Name with names of enclosing types.
        /// </summary>
        public string NestedName
        {
            get { return _nestedName ?? (_nestedName = EvalName(TypeNameKind.NestedName, TypeNameKind.FullName)); }
        }
        string _nestedName;
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
            get
            {
            	return _genericType != null ? _genericType.Module : null;
            }
        	set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public TypeMemberType MemberType
        {
            get { return TypeMemberType.Type; }
        }

        public string Name
        {
            get { return _name ?? (_name = EvalName(TypeNameKind.Name, TypeNameKind.FullName)); }
        	set { throw new NotSupportedException(); }
        }
        string _name;

        public string DisplayName
        {
			get { return _displayName ?? (_displayName = EvalName(TypeNameKind.DisplayName, TypeNameKind.DisplayName, false)); }
        }
    	private string _displayName;

        public IType DeclaringType { get; set; }

        IType ITypeMember.Type
        {
            get { return _genericType; }
            set { _genericType = value as IGenericType; }
        }

        /// <summary>
        /// Gets visibility of this member.
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                if (_genericType != null)
                    return _genericType.Visibility;
                return Visibility.Public;
            }
            set
            {
                if (_genericType != null)
                    _genericType.Visibility = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                if (DeclaringType != null && !DeclaringType.IsVisible)
                    return false;

                if (_genericType != null)
                    return _genericType.IsVisible;
                return true;
            }
        }

        public bool IsStatic
        {
            get
            {
                if (_genericType != null)
                    return _genericType.IsStatic;
                return false;
            }
            set
            {
                if (_genericType != null)
                    _genericType.IsStatic = value;
            }
        }

        public bool IsSpecialName
        {
            get
            {
                if (_genericType != null)
                    return _genericType.IsSpecialName;
                return false;
            }
            set
            {
                if (_genericType != null)
                    _genericType.IsSpecialName = value;
            }
        }

        public bool IsRuntimeSpecialName
        {
            get
            {
                if (_genericType != null)
                    return _genericType.IsRuntimeSpecialName;
                return false;
            }
            set
            {
                if (_genericType != null)
                    _genericType.IsRuntimeSpecialName = value;
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
            get
            {
                if (_genericType != null)
                    return _genericType.ChildNodes;
                return null;
            }
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
            get
            {
                if (_genericType != null)
                    return _genericType.Types;
                return null;
            }
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
            return CMHelper.AreEquals(this, obj as IType);
        }

        public override int GetHashCode()
        {
            return CMHelper.GetHashCode(this);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }
}