
namespace DataDynamics.PageFX.CodeModel.Syntax
{
    /// <summary>
    /// Represents facility to customize syntax formatting of code model for given language.
    /// </summary>
    public interface ISyntaxProvider
    {
        /// <summary>
        /// Gets the name of language
        /// </summary>
        string Language { get; }
    }

    public interface ISyntaxWriter
    {
    }
}