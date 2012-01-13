namespace DataDynamics.PageFX.CodeModel
{
    /// <summary>
    /// Interfaces CLI metadata element.
    /// </summary>
    public interface IMetadataElement
    {
        /// <summary>
        /// Gets or sets value that identifies a metadata element. 
        /// </summary>
        int MetadataToken { get; set; }
    }

    public interface IMetadataTokenResolver
    {
        object ResolveMetadataToken(int token);

        object ResolveMetadataToken(IMethod method, int token);
    }
}