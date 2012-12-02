using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
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
        IModule Module { get; set; }

        /// <summary>
        /// Gets the kind of this member.
        /// </summary>
        MemberType MemberType { get; }

        /// <summary>
        /// Gets or sets the name of type member.
        /// </summary>
        string Name { get; set; }

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
        /// Gets or sets the type of this member (for methods it's return type)
        /// </summary>
        IType Type { get; set; }

        /// <summary>
        /// Gets visibility of this member.
        /// </summary>
        Visibility Visibility { get; set; }
        
        bool IsVisible { get; }

        bool IsStatic { get; set; }

        bool IsSpecialName { get; set; }

        bool IsRuntimeSpecialName { get; set; }
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
}