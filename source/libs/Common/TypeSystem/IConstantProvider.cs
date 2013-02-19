namespace DataDynamics.PageFX.Common.TypeSystem
{
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