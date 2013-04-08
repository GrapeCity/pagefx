using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
    /// Represents type.
    /// </summary>
    public interface IType : ITypeMember, ITypeContainer
    {
        /// <summary>
        /// Gets namespace where type is defined.
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// Gets kind of the type.
        /// </summary>
        TypeKind TypeKind { get; }
        
        /// <summary>
        /// Gets a flag specifing that this type is abstract.
        /// </summary>
        bool IsAbstract { get; }

        /// <summary>
        /// Gets a flag specifing that this type is sealed.
        /// </summary>
        bool IsSealed { get; }

		bool IsPartial { get; }

        bool IsBeforeFieldInit { get; }

		/// <summary>
        /// Determines whether this type is interface.
        /// </summary>
        bool IsInterface { get; }

        /// <summary>
        /// Determines whether this type is class.
        /// </summary>
        bool IsClass { get; }

        /// <summary>
        /// Determines whether this type is array.
        /// </summary>
        bool IsArray { get; }

        /// <summary>
        /// Determines whether this type is enum type.
        /// </summary>
        bool IsEnum { get; }

		/// <summary>
		/// Gets declaring method.
		/// </summary>
		IMethod DeclaringMethod { get; }

		/// <summary>
		/// Gets base type.
		/// </summary>
        IType BaseType { get; }

		/// <summary>
		/// Gets implemented interfaces.
		/// </summary>
        ITypeCollection Interfaces { get; }

		/// <summary>
		/// Gets element type.
		/// </summary>
		IType ElementType { get; }

		/// <summary>
		/// Gets underlying value type.
		/// </summary>
        IType ValueType { get; }
		
		/// <summary>
		/// Gets generic parameters.
		/// </summary>
		IGenericParameterCollection GenericParameters { get; }

        /// <summary>
        /// Gets the fields declared in this type.
        /// </summary>
        IFieldCollection Fields { get; }

        /// <summary>
        /// Gets the methods declared in this type.
        /// </summary>
        IMethodCollection Methods { get; }

        /// <summary>
        /// Gets the properties declared in this type.
        /// </summary>
        IPropertyCollection Properties { get; }

        /// <summary>
        /// Gets the events declared in this type.
        /// </summary>
        IEventCollection Events { get; }

        /// <summary>
        /// Gets the members declared in this type.
        /// </summary>
        ITypeMemberCollection Members { get; }

		/// <summary>
		/// Gets class layout.
		/// </summary>
        ClassLayout Layout { get; }

		/// <summary>
        /// Gets unique key of this type. Used for <see cref="TypeFactory"/>.
        /// </summary>
        string Key { get;  }

        /// <summary>
        /// Gets name of the type used in signatures.
        /// </summary>
        string SigName { get; }

        /// <summary>
        /// Name with names of enclosing types.
        /// </summary>
        string NestedName { get; }

		/// <summary>
		/// Gets array dimensions. Null for non-array types.
		/// </summary>
		ArrayDimensionCollection ArrayDimensions { get; }
    }

	public interface ITypeCollection : IReadOnlyList<IType>, ICodeNode
    {
		IType FindType(string fullname);

		void Add(IType type);

        bool Contains(IType type);
    }

	public enum TypeKind : byte
	{
		[CSharp("class")]
		[VB("Class")]
		[ActionScript("class")]
		Class,

		[CSharp("interface")]
		[VB("Interface")]
		[ActionScript("interface")]
		Interface,

		[CSharp("struct")]
		[VB("Struct")]
		[ActionScript("class")]
		Struct,

		[CSharp("enum")]
		[VB("Enumeration")]
		[ActionScript("class")]
		Enum,

		[CSharp("delegate")]
		Delegate,

		GenericParameter,

		Array,
		Reference,
		Pointer,

		[CSharp("struct")]
		[VB("Struct")]
		Primitive,
	}

	public enum TypeNameKind
    {
        FullName,
        Name,
        DisplayName,
        NestedName,
        SigName,
        Key,
        CSharpKeyword
    }
}