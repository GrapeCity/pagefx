namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Item with constant value.
    /// </summary>
    public interface IConstantProvider
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        object Value { get; set; }
    }
}