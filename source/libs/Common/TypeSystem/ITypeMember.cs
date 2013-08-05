using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Base interface for type members.
    /// </summary>
    public interface ITypeMember : ICustomAttributeProvider, ICodeNode, IDocumentationProvider
    {
        /// <summary>
        /// Gets the assembly in which the member is declared.
        /// </summary>
        IAssembly Assembly { get; }

        /// <summary>
        /// Gets the module in which the member is defined. 
        /// </summary>
        IModule Module { get; }

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        MemberType MemberType { get; }

        /// <summary>
        /// Gets the name of type member.
        /// </summary>
        string Name { get; }

		//TODO: FullName could be made as computable extension method, but there is problem with performance for generic types

        /// <summary>
        /// Gets the fullname of the member.
        /// </summary>
        string FullName { get; }

		//TODO: remove DisplayName, make it computable

        /// <summary>
        /// Gets the display name of this member.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets the type that declares this member.
        /// </summary>
		IType DeclaringType { get; }

        /// <summary>
        /// Gets the type of this member (for methods it's return type).
        /// </summary>
		IType Type { get; }

        /// <summary>
        /// Gets visibility of this member.
        /// </summary>
        Visibility Visibility { get; }

		/// <summary>
		/// Specifies whether member is static.
		/// </summary>
	    bool IsStatic { get; }

		/// <summary>
		/// Specifies whether the member has special name.
		/// </summary>
		bool IsSpecialName { get; }

		/// <summary>
		/// Specifies whether the member has runtime special name.
		/// </summary>
        bool IsRuntimeSpecialName { get; }
    }

    /// <summary>
    /// Represents collection of <see cref="ITypeMember"/>s.
    /// </summary>
    public interface ITypeMemberCollection : IReadOnlyList<ITypeMember>, ICodeNode
    {
		// TODO try to remove this method at least from this interface, provide explicit method for member registration where it is needed

        /// <summary>
        /// Adds member to collection.
        /// </summary>
        /// <param name="member">The member to add</param>
        void Add(ITypeMember member);
    }

	public enum MemberType
	{
		Field,
		Method,
		Constructor,
		Property,
		Event,
		Type,
	}

	public enum Visibility : byte
	{
		[CSharp("private")]
		[ActionScript("private")]
		PrivateScope,

		[CSharp("private")]
		[VB("Private")]
		[ActionScript("private")]
		NestedPrivate,

		[CSharp("protected")]
		[ActionScript("protected")]
		NestedProtected,

		[CSharp("protected internal")]
		[ActionScript("protected")]
		NestedProtectedInternal,

		[CSharp("internal")]
		[ActionScript("internal")]
		NestedInternal,

		[CSharp("public")]
		[ActionScript("public")]
		NestedPublic,

		[CSharp("private")]
		[ActionScript("private")]
		Private,

		[CSharp("protected")]
		[ActionScript("protected")]
		Protected,

		[CSharp("protected internal")]
		[ActionScript("protected")]
		ProtectedInternal,

		[CSharp("internal")]
		[ActionScript("internal")]
		Internal,

		[CSharp("public")]
		[ActionScript("public")]
		Public,
	}
}