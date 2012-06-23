using System.Collections.Generic;
using DataDynamics.Collections;

namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Represents type property.
    /// </summary>
    public interface IProperty : IPolymorphicMember, IConstantProvider, IParameterizedMember
    {
        bool HasDefault { get; set; }

        /// <summary>
        /// Gets or sets getter for this property.
        /// </summary>
        IMethod Getter { get; set; }

        /// <summary>
        /// Gerts or sets setter for this property.
        /// </summary>
        IMethod Setter { get; set; }

        /// <summary>
        /// Gets or sets property initializer.
        /// </summary>
        IExpression Initializer { get; set; }

        /// <summary>
        /// Returns true if the property is indexer.
        /// </summary>
        bool IsIndexer { get; }
    }

    /// <summary>
    /// Represents collection of <see cref="IProperty"/>s.
    /// </summary>
    public interface IPropertyCollection : IReadOnlyList<IProperty>, ICodeNode
    {
        void Add(IProperty property);

        /// <summary>
        /// Finds properties by given name.
        /// </summary>
        /// <param name="name">name of properties to find.</param>
        /// <returns>set of properties with given name</returns>
        IEnumerable<IProperty> this[string name] { get; }

        IProperty this[string name, IType arg1] { get; }

        IProperty this[string name, IType arg1, IType arg2] { get; }

        IProperty this[string name, IType arg1, IType arg2, IType arg3] { get; }

        IProperty this[string name, params IType[] types] { get; }
    }
}