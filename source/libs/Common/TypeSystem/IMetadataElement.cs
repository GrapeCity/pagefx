namespace DataDynamics.PageFX.Common.TypeSystem
{
    /// <summary>
    /// Interfaces CLI metadata element.
    /// </summary>
    public interface IMetadataElement
    {
        /// <summary>
        /// Gets value that identifies a metadata element. 
        /// </summary>
        int MetadataToken { get; }
    }

    public interface IMetadataTokenResolver
    {
        object ResolveMetadataToken(IMethod method, int token);
    }
}