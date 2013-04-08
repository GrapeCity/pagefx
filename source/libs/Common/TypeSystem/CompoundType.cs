using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public abstract class CompoundType : IType
    {
	    protected CompoundType(IType elementType)
        {
            ElementType = elementType;
        }

	    public IType ElementType { get; private set; }

	    public string Namespace
        {
            get { return ElementType != null ? ElementType.Namespace : null; }
        }

        public string FullName
        {
            get { return _fullName ?? (_fullName = ElementType.FullName + NameSuffix); }
        }
        private string _fullName;

        public abstract TypeKind TypeKind { get; }

        public bool IsAbstract
        {
            get { return ElementType != null && ElementType.IsAbstract; }
        }

        public bool IsSealed
        {
            get { return ElementType != null && ElementType.IsSealed; }
        }

	    public bool IsPartial
	    {
			get { return false; }
	    }

	    public bool IsBeforeFieldInit
        {
            get { return ElementType != null && ElementType.IsBeforeFieldInit; }
        }

	    public bool IsInterface
        {
            get { return false; }
        }

        /// <summary>
        /// Determines whether this type is class.
        /// </summary>
        public bool IsClass
        {
            get { return false; }
        }

        public bool IsArray
        {
            get { return TypeKind == TypeKind.Array; }
        }

        /// <summary>
        /// Determines whether this type is enum type.
        /// </summary>
        public bool IsEnum
        {
            get { return false; }
        }

        public IMethod DeclaringMethod
        {
            get { return null; }
        }

        public virtual IType BaseType
        {
            get { return ElementType != null ? ElementType.BaseType : null; }
        }

        public virtual ITypeCollection Interfaces
        {
            get { return ElementType != null ? ElementType.Interfaces : TypeCollection.Empty; }
        }

        public virtual ITypeCollection Types
        {
            get { return ElementType != null ? ElementType.Types : null; }
        }

	    public IGenericParameterCollection GenericParameters
	    {
			get { return GenericParameterCollection.Empty; }
	    }

	    public virtual IFieldCollection Fields
        {
            get { return ElementType != null ? ElementType.Fields : null; }
        }

        public virtual IMethodCollection Methods
        {
            get { return ElementType != null ? ElementType.Methods : null; }
        }

        public virtual IPropertyCollection Properties
        {
            get { return ElementType != null ? ElementType.Properties : null; }
        }

        public virtual IEventCollection Events
        {
            get { return ElementType != null ? ElementType.Events : null; }
        }

        public virtual ITypeMemberCollection Members
        {
            get { return ElementType != null ? ElementType.Members : null; }
        }

        public IType ValueType
        {
            get { return ElementType != null ? ElementType.ValueType : null; }
        }

        public virtual ClassLayout Layout
        {
            get { return ElementType != null ? ElementType.Layout : null; }
        	set { throw new NotSupportedException(); }
        }

	    /// <summary>
        /// Gets unique key of this type. Used for <see cref="TypeFactory"/>.
        /// </summary>
        public string Key
        {
            get { return _key ?? (_key = ElementType.Key + NameSuffix); }
        }
        private string _key;
        
        /// <summary>
        /// Gets name of the type used in signatures.
        /// </summary>
        public string SigName
        {
            get { return _sigName ?? (_sigName = ElementType.SigName + SigSuffix); }
        }
        private string _sigName;

        protected abstract string SigSuffix { get; }

        /// <summary>
        /// Name with names of enclosing types.
        /// </summary>
        public string NestedName
        {
            get { return ElementType.NestedName + NameSuffix; }
        }

	    public virtual ArrayDimensionCollection ArrayDimensions
	    {
			get { return null; }
	    }

	    /// <summary>
        /// Gets the assembly in which the type is declared.
        /// </summary>
        public IAssembly Assembly
        {
            get { return ElementType != null ? ElementType.Assembly : null; }
        }

        /// <summary>
        /// Gets the module in which the current type is defined. 
        /// </summary>
        public IModule Module
        {
            get { return ElementType != null ? ElementType.Module : null; }
        }

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public MemberType MemberType
        {
            get { return MemberType.Type; }
        }

        public virtual string NameSuffix
        {
            get { return ""; }
        }

        public string Name
        {
            get { return _name ?? (_name = ElementType.Name + NameSuffix); }
        }
        private string _name;

        public string DisplayName
        {
            get { return _displayName ?? (_displayName = ElementType.DisplayName + NameSuffix); }
        }
        private string _displayName;

        public IType DeclaringType
        {
            get { return null; }
        }

        public IType Type
        {
            get { return null; }
        }

        /// <summary>
        /// Gets visibility of this member.
        /// </summary>
        public Visibility Visibility
        {
            get { return ElementType != null ? ElementType.Visibility : Visibility.Private; }
        }

	    public bool IsStatic
        {
            get { return ElementType != null && ElementType.IsStatic; }
        }

        public bool IsSpecialName
        {
            get { return ElementType != null && ElementType.IsSpecialName; }
        }

        public bool IsRuntimeSpecialName
        {
            get { return ElementType != null && ElementType.IsRuntimeSpecialName; }
        }

        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        public int MetadataToken
        {
            get { return -1; }
        }

	    public ICustomAttributeCollection CustomAttributes
        {
            get { return ElementType != null ? ElementType.CustomAttributes : null; }
        }

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {ElementType}; }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Data { get; set; }

	    public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return FullName;
        }

	    /// <summary>
    	/// Gets or sets documentation of this member
    	/// </summary>
    	public string Documentation { get; set; }

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
    }

    public sealed class PointerType : CompoundType
    {
        public PointerType(IType elementType)
            : base(elementType)
        {
        }

        public override TypeKind TypeKind
        {
            get { return TypeKind.Pointer; }
        }

        public override string NameSuffix
        {
            get { return "*"; }
        }

        protected override string SigSuffix
        {
            get { return "_ptr"; }
        }
    }

    public sealed class ReferenceType : CompoundType
    {
        public ReferenceType(IType elementType)
            : base(elementType)
        {
        }

        public override TypeKind TypeKind
        {
            get { return TypeKind.Reference; }
        }

        public override string NameSuffix
        {
            get { return "&"; }
        }

        protected override string SigSuffix
        {
            get { return "_ref"; }
        }
    }
}