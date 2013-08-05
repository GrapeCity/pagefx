using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents base class for user-defined types.
    /// </summary>
    public class TypeImpl : TypeMember, IType
    {
		public static readonly IType[] EmptyTypes = new IType[0];

		private ClassLayout _layout;
		private string _namespace;
		private IType _baseType;
		private ITypeCollection _interfaces;
		private readonly TypeMemberCollection _members;
		private ITypeCollection _types;

	    public TypeImpl()
        {
            _members = new TypeMemberCollection();
        }

        public TypeImpl(TypeKind kind) : this()
        {
            TypeKind = kind;
        }

	    /// <summary>
        /// Gets the module in which the current type is defined. 
        /// </summary>
        public override IModule Module { get; set; }

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public override MemberType MemberType
        {
            get { return MemberType.Type; }
        }

	    /// <summary>
	    /// Gets kind of the type.
	    /// </summary>
	    public TypeKind TypeKind { get; set; }

	    /// <summary>
        /// Gets or sets flag specifing that this type is abstract.
        /// </summary>
        public bool IsAbstract
        {
            get { return GetModifier(Modifiers.Abstract); }
            set { SetModifier(value, Modifiers.Abstract); }
        }

        /// <summary>
        /// Gets or sets flag specifing that this type is sealed.
        /// </summary>
        public bool IsSealed
        {
            get { return GetModifier(Modifiers.Sealed); }
            set { SetModifier(value, Modifiers.Sealed); }
        }

		public bool IsPartial
		{
			get { return GetModifier(Modifiers.Partial); }
			set { SetModifier(value, Modifiers.Partial); }
		}

        /// <summary>
        /// Returns true if CLR must initialize the class before first static field access.
        /// </summary>
        public bool IsBeforeFieldInit
        {
            get { return GetModifier(Modifiers.BeforeFieldInit); }
            set { SetModifier(value, Modifiers.BeforeFieldInit); }
        }

        /// <summary>
        /// Determines whether this type is interface.
        /// </summary>
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

        /// <summary>
        /// Determines whether this type is array.
        /// </summary>
        public bool IsArray
        {
            get
            {
                //NOTE: Only compund type can be array
                return false;
            }
        }

        /// <summary>
        /// Determines whether this type is enum type.
        /// </summary>
        public bool IsEnum
        {
            get { return TypeKind == TypeKind.Enum; }
        }

        /// <summary>
        /// Gets or sets type namespace
        /// </summary>
        public string Namespace
        {
            get
            {
                if (DeclaringType != null)
                    return DeclaringType.Namespace;
                return _namespace;
            }
            set { _namespace = value; }
        }
        
        /// <summary>
        /// Gets the full name of this type.
        /// </summary>
        public override string FullName
        {
            get { return this.BuildFullName(); }
        }

	    public override string DisplayName
        {
            get { return this.BuildDisplayName(); }
        }

        /// <summary>
        /// Gets a <see cref="IMethod"/> that represents the declaring method, if the current <see cref="TypeImpl"/> represents a type parameter of a generic method.
        /// </summary>
        public IMethod DeclaringMethod { get; set; }

	    /// <summary>
	    /// Gets or sets base type.
	    /// </summary>
	    public IType BaseType
	    {
		    get { return _baseType ?? (_baseType = ResolveBaseType()); }
		    set { _baseType = value; }
	    }

	    protected virtual IType ResolveBaseType()
	    {
		    return null;
	    }

	    /// <summary>
        /// Gets list of interfaces implemented by this type.
        /// </summary>
        public ITypeCollection Interfaces
        {
            get { return _interfaces ?? (_interfaces = new SimpleTypeCollection()); }
			set { _interfaces = value; }
        }
        
        public ITypeMemberCollection Members
        {
            get { return _members; }
        }

	    public virtual ITypeCollection GenericParameters
	    {
			get { return TypeCollection.Empty; }
	    }

	    public ITypeCollection GenericArguments
	    {
			get { return TypeCollection.Empty; }
	    }

	    public IFieldCollection Fields
        {
            get { return _members.Fields; }
        }

        public IMethodCollection Methods
        {
            get { return _members.Methods; }
        }

        public IPropertyCollection Properties
        {
            get { return _members.Properties; }
        }

        public IEventCollection Events
        {
            get { return _members.Events; }
        }

		public IType ElementType
		{
			get { return null; }
		}

        public IType ValueType
        {
            get { return this.ResolveValueType(); }
        }

	    public virtual ClassLayout Layout
	    {
		    get { return _layout ?? (_layout = ResolveLayout()); }
		    set { _layout = value; }
	    }

	    protected virtual ClassLayout ResolveLayout()
	    {
		    return null;
	    }

	    /// <summary>
        /// Get all nested types.
        /// </summary>
        public ITypeCollection Types
        {
            get { return _types ?? (_types = new TypeCollection()); }
			set { _types = value; }
        }

	    public override string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }

	    #region Object Override Members
        public override bool Equals(object obj)
        {
            return this.IsEqual(obj as IType);
        }

        public override int GetHashCode()
        {
            string s = FullName;
            if (s != null)
                return s.GetHashCode();
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion

	    public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Types, Fields, Properties, Events, Methods }; }
        }

	    /// <summary>
        /// Gets unique key of this type. Used for <see cref="TypeFactory"/>.
        /// </summary>
        public string Key
        {
            get { return FullName; }
        }

        /// <summary>
        /// Gets name of the type used in signatures.
        /// </summary>
        public string SigName
        {
            get { return this.BuildSigName(); }
        }

        /// <summary>
        /// Name with names of enclosing types.
        /// </summary>
        public string NestedName
        {
            get { return this.BuildNestedName(); }
        }

	    public ArrayDimensionCollection ArrayDimensions
	    {
			get { return null; }
	    }

	    public GenericParameterInfo GetGenericParameterInfo()
	    {
		    return null;
	    }
    }
}