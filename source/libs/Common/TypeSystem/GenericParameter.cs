using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public interface IGenericParameterConstraints : ITypeCollection
	{
		IType BaseType { get; }
	}

    public sealed class GenericParameter : CustomAttributeProvider, IGenericParameter
    {
        #region ITypeMember Members
        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        public MemberType MemberType
        {
            get { return MemberType.Type; }
        }

        public string Name { get; set; }

        public string DisplayName
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets visibility of this member.
        /// </summary>
        public Visibility Visibility
        {
            get { return Visibility.Public; }
            set { }
        }

	    public bool IsStatic
        {
            get { return false; }
            set { throw new NotSupportedException(); }
        }

        public bool IsSpecialName
        {
            get { return false; }
            set { }
        }

        public bool IsRuntimeSpecialName
        {
            get { return false; }
            set { }
        }

		#endregion

        #region IGenericParameter Members
        public IType DeclaringType { get; set; }

        public int Position { get; set; }

        public GenericParameterVariance Variance { get; set; }

        public GenericParameterSpecialConstraints SpecialConstraints { get; set; }

        public long ID { get; set; }
        #endregion

        #region IType Members

        /// <summary>
        /// Gets the assembly in which the type is declared.
        /// </summary>
        public IAssembly Assembly
        {
            get
            {
                var mod = Module;
                if (mod != null)
                    return mod.Assembly;
                return null;
            }
        }

        /// <summary>
        /// Gets the module in which the current type is defined. 
        /// </summary>
        public IModule Module
        {
            get
            {
                if (DeclaringType != null)
                    return DeclaringType.Module;
                if (DeclaringMethod != null)
                    return DeclaringMethod.Module;
                return null;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public string Namespace
        {
            get { return string.Empty; }
            set { throw new NotSupportedException(); }
        }

        public string FullName
        {
            get { return Name; }
        }

        public TypeKind TypeKind
        {
            get { return TypeKind.GenericParameter; }
            set { throw new NotSupportedException(); }
        }

        public bool IsAbstract
        {
            get { return false; }
            set { }
        }

        public bool IsSealed
        {
            get { return false; }
            set { }
        }

	    public bool IsPartial
	    {
			get { return false; }
		    set { throw new NotSupportedException(); }
	    }

	    public bool IsBeforeFieldInit
        {
            get { return false; }
            set { }
        }

        /// <summary>
        /// Determines whether this type is interface.
        /// </summary>
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
            get { return false; }
        }

        /// <summary>
        /// Determines whether this type is enum type.
        /// </summary>
        public bool IsEnum
        {
            get { return false; }
        }

        public IMethod DeclaringMethod { get; set; }

		public IGenericParameterConstraints Constraints { get; set; }

	    public IType BaseType
	    {
		    get { return Constraints != null ? Constraints.BaseType : null; }
		    set { throw new NotSupportedException(); }
	    }

		public ITypeCollection Interfaces
        {
            get { return Constraints ?? TypeCollection.Empty; }
        }

        public ITypeCollection Types
        {
            get { return TypeCollection.Empty; }
        }

        public IFieldCollection Fields
        {
            get { return FieldCollection.Empty; }
        }

        public IMethodCollection Methods
        {
            get { return MethodCollection.Empty; }
        }

        public IPropertyCollection Properties
        {
            get { return null; }
        }

        public IEventCollection Events
        {
            get { return null; }
        }

        public ITypeMemberCollection Members
        {
            get { return null; }
        }

        public IType ValueType
        {
            get { return null; }
        }

        public IType Type
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

        public ClassLayout Layout
        {
            get { return null; }
            set { throw new NotSupportedException(); }
        }

	    /// <summary>
        /// Gets unique key of this type. Used for <see cref="TypeFactory"/>.
        /// </summary>
        public string Key
        {
            get { return _key ?? (_key = Name + ID); }
        }
        private string _key;

        /// <summary>
        /// Gets name of the type used in signatures.
        /// </summary>
        public string SigName
        {
            get { return Name; }
        }

        /// <summary>
        /// Name with names of enclosing types.
        /// </summary>
        public string NestedName
        {
            get { return Name; }
        }

        #endregion

        #region ICodeNode Members

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return null; }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Data { get; set; }

    	#endregion

        #region IFormatable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Name;
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
    }

    public sealed class GenericParameterCollection : List<IGenericParameter>, IGenericParameterCollection
    {
        #region IGenericParameterCollection Members

        public IGenericParameter this[string name]
        {
            get { return Find(p => p.Name == name); }
        }

        #endregion

        #region ICodeNode Members

	    public IEnumerable<ICodeNode> ChildNodes
        {
            get { return this.Cast<ICodeNode>(); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Data { get; set; }

    	#endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion
    }
}