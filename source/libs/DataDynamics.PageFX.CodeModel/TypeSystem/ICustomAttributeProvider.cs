using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents something with custom attributes
    /// </summary>
    public interface ICustomAttributeProvider : IMetadataElement
    {
        /// <summary>
        /// Gets collection of custom attributes.
        /// </summary>
		ICustomAttributeCollection CustomAttributes { get; }
    }

    /// <summary>
    /// Represents custom attribute.
    /// </summary>
    public interface ICustomAttribute : ICodeNode, ICloneable
    {
        /// <summary>
        /// Gets or sets type name.
        /// </summary>
        string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the type of this custom attribute.
        /// </summary>
        IType Type { get; set; }

        /// <summary>
        /// Gets the constructor used to intialize this custom attribute.
        /// </summary>
        IMethod Constructor { get; set; }

        /// <summary>
        /// Gets or sets the owner of this custom attributes.
        /// </summary>
        ICustomAttributeProvider Owner { get; set; }

        /// <summary>
        /// Gets the collection of arguments passed to constructor.
        /// </summary>
        IArgumentCollection Arguments { get; }

        IArgumentCollection FixedArguments { get; }

        IArgumentCollection NamedArguments { get; }
    }

    /// <summary>
    /// Represents collection of <see cref="ICustomAttribute"/>s.
    /// </summary>
    public interface ICustomAttributeCollection : IReadOnlyList<ICustomAttribute>, ICodeNode
    {
        ICustomAttribute[] this[IType type] { get; }

        ICustomAttribute[] this[string typeFullName] { get; }

	    void Add(ICustomAttribute attribute);
    }
}

