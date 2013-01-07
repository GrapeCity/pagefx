namespace DataDynamics.PageFX.Common.TypeSystem
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
    }

    /// <summary>
    /// Represents collection of <see cref="IProperty"/>s.
    /// </summary>
    public interface IPropertyCollection : IParameterizedMemberCollection<IProperty>
    {
        void Add(IProperty property);
    }
}