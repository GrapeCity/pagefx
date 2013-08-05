namespace DataDynamics.PageFX.Common.TypeSystem
{
	// TODO consider to rename to IValueProvider

    /// <summary>
    /// Item with constant value.
    /// </summary>
    public interface IConstantProvider
    {
        /// <summary>
        /// Gets the item value.
        /// </summary>
        object Value { get; }
    }
}