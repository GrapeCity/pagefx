namespace DataDynamics.PageFX.Common.TypeSystem
{
	/// <summary>
	/// Base interface for polymorphic members (method or property).
	/// </summary>
    public interface IPolymorphicMember : ITypeMember
    {
        /// <summary>
        /// Idicates whether the member is abtract.
        /// </summary>
        bool IsAbstract { get; }

        /// <summary>
        /// Indicates whether the member is virtual (overridable)
        /// </summary>
        bool IsVirtual { get; }

        /// <summary>
        /// Indicates whether the member is final (can not be overriden)
        /// </summary>
        bool IsFinal { get; }

        /// <summary>
        /// Indicates whether member overrides implementation and return type.
        /// </summary>
        bool IsNewSlot { get; }

        /// <summary>
        /// Indicates whether the member overrides implementation of base type.
        /// </summary>
        bool IsOverride { get; }
    }
}