using DataDynamics.PageFX.CodeModel.Expressions;

namespace DataDynamics.PageFX.CodeModel.TypeSystem
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
    public interface IPropertyCollection : IParameterizedMemberCollection<IProperty>
    {
        void Add(IProperty property);
    }
}