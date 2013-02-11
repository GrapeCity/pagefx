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

        /// <summary>
        /// Gets the fullname of the member.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the display name of this member.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets or sets type that declares this member.
        /// </summary>
		IType DeclaringType { get; set; }

        /// <summary>
        /// Gets the type of this member (for methods it's return type)
        /// </summary>
		IType Type { get; set; }

        /// <summary>
        /// Gets visibility of this member.
        /// </summary>
        Visibility Visibility { get; }

	    bool IsStatic { get; }

        bool IsSpecialName { get; }

        bool IsRuntimeSpecialName { get; }
    }

    /// <summary>
    /// Represents collection of <see cref="ITypeMember"/>s.
    /// </summary>
    public interface ITypeMemberCollection : IReadOnlyList<ITypeMember>, ICodeNode
    {
        /// <summary>
        /// Adds member to collection.
        /// </summary>
        /// <param name="m">member to add</param>
        void Add(ITypeMember m);
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