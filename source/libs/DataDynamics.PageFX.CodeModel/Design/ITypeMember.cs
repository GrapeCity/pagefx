using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Base interface for type members.
    /// </summary>
    public interface ITypeMember : IMetadataElement, ICustomAttributeProvider, ICodeNode, IDocumentationProvider
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
        TypeMemberType MemberType { get; }

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
    public interface ITypeMemberCollection : IEnumerable<ITypeMember>, ICodeNode
    {
        /// <summary>
        /// Gets number of members.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets member at specified index.
        /// </summary>
        /// <param name="index">zero-baded index of member to get</param>
        /// <returns></returns>
        ITypeMember this[int index] { get; }

        /// <summary>
        /// Adds member to collection.
        /// </summary>
        /// <param name="m">member to add</param>
        void Add(ITypeMember m);
    }
}