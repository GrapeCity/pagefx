namespace DataDynamics.PageFX.CodeModel
{
    public interface IPolymorphicMember : ITypeMember
    {
        /// <summary>
        /// Gets or sets the flag indicating whether the member is abtract.
        /// </summary>
        bool IsAbstract { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the member is virtual (overridable)
        /// </summary>
        bool IsVirtual { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether the member is final (can not be overriden)
        /// </summary>
        bool IsFinal { get; set; }

        /// <summary>
        /// Gets or sets the flag indicating whether member overrides implementation and return type.
        /// </summary>
        bool IsNewSlot { get; set; }

        /// <summary>
        /// Gets the flag indicating whether the member overrides implementation of base type.
        /// </summary>
        bool IsOverride { get; }
    }
}