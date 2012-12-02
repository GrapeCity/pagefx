namespace DataDynamics.PageFX.CodeModel.TypeSystem
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