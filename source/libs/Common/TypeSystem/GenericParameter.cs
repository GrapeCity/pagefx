using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public interface IGenericParameterConstraints : ITypeCollection
	{
		IType BaseType { get; }
	}

	/// <summary>
	/// Mutable generic parameter implementation.
	/// </summary>
    public sealed class GenericParameter : CustomAttributeProvider, IType
    {
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
        }

	    public bool IsStatic
        {
            get { return false; }
        }

        public bool IsSpecialName
        {
            get { return false; }
        }

        public bool IsRuntimeSpecialName
        {
            get { return false; }
        }

	    public IType DeclaringType { get; set; }

        public int Position { get; set; }

        public GenericParameterVariance Variance { get; set; }

        public GenericParameterSpecialConstraints SpecialConstraints { get; set; }

        public long Id { get; set; }

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
        }

        public string Namespace
        {
            get { return string.Empty; }
        }

        public string FullName
        {
            get { return Name; }
        }

        public TypeKind TypeKind
        {
            get { return TypeKind.GenericParameter; }
        }

        public bool IsAbstract { get { return false; } }
		public bool IsSealed { get { return false; } }
		public bool IsPartial { get { return false; } }
		public bool IsBeforeFieldInit { get { return false; } }
		public bool IsInterface { get { return false; } }
		public bool IsClass { get { return false; } }
		public bool IsArray { get { return false; } }
		public bool IsEnum { get { return false; } }

        public IMethod DeclaringMethod { get; set; }

		public IGenericParameterConstraints Constraints { get; set; }

	    public IType BaseType
	    {
		    get { return Constraints != null ? Constraints.BaseType : null; }
	    }

		public ITypeCollection Interfaces
        {
            get { return Constraints ?? TypeCollection.Empty; }
        }

		public IType ElementType
		{
			get { return null; }
		}

		public ITypeCollection Types
        {
            get { return TypeCollection.Empty; }
        }

		public ITypeCollection GenericParameters
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
            get { return PropertyCollection.Empty; }
        }

        public IEventCollection Events
        {
            get { return EventCollection.Empty; }
        }

        public ITypeMemberCollection Members
        {
            get { return TypeMemberCollection.Empty; }
        }

        public IType ValueType
        {
            get { return null; }
        }

        public IType Type
        {
            get { return null; }
        }

        public ClassLayout Layout
        {
            get { return null; }
        }

	    /// <summary>
        /// Gets unique key of this type. Used for <see cref="TypeFactory"/>.
        /// </summary>
        public string Key
        {
            get { return Name + Id; }
        }

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

		public ArrayDimensionCollection ArrayDimensions
		{
			get { return null; }
		}

		public GenericParameterInfo GetGenericParameterInfo()
		{
			return new GenericParameterInfo(Position, Variance, SpecialConstraints);
		}

		public IEnumerable<ICodeNode> ChildNodes
        {
            get { return Enumerable.Empty<ICodeNode>(); }
        }

    	/// <summary>
    	/// Gets or sets user defined data assotiated with this object.
    	/// </summary>
    	public object Data { get; set; }

	    public string ToString(string format, IFormatProvider formatProvider)
        {
            return Name;
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
}