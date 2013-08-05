namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Represents type property.
    /// </summary>
    public interface IProperty : IPolymorphicMember, IConstantProvider, IParameterizedMember
    {
		/// <summary>
		/// Indicates whether the property has default value.
		/// </summary>
        bool HasDefault { get; }

        /// <summary>
        /// Gets getter for the property.
        /// </summary>
        IMethod Getter { get; }

        /// <summary>
        /// Gerts setter for the property.
        /// </summary>
        IMethod Setter { get; }
    }

    /// <summary>
    /// Represents collection of <see cref="IProperty"/>s.
    /// </summary>
    public interface IPropertyCollection : IParameterizedMemberCollection<IProperty>
    {
		// TODO try to remove this method at least from this interface, provide explicit method for property registration where it is needed

        void Add(IProperty property);
    }
}