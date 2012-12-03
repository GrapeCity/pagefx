using System.IO;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common
{
    /// <summary>
    /// Represents language infrastructure.
    /// </summary>
    public interface ILanguageInfrastructure
    {
        /// <summary>
        /// Gets the name of language infrastructure.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Deserializes specified file.
        /// </summary>
        /// <param name="path">path to file to deserialize.</param>
        /// <param name="format">specifies format of input file to deserialize.</param>
        /// <returns></returns>
        IAssembly Deserialize(string path, string format);

        /// <summary>
        /// Deserialized specified stream.
        /// </summary>
        /// <param name="input">stream to deserialize.</param>
        /// <param name="format">format of input stream to deserialize.</param>
        /// <returns></returns>
        IAssembly Deserialize(Stream input, string format);

        /// <summary>
        /// Serializes given assembly to specified file.
        /// </summary>
        /// <param name="assembly">assembly to serialize.</param>
        /// <param name="path">path to file to which you want to serialize given assembly.</param>
        /// <param name="format">format to which you want to serialize.</param>
        void Serialize(IAssembly assembly, string path, string format);

        /// <summary>
        /// Serializes given assembly to specified stream.
        /// </summary>
        /// <param name="assembly">assembly to serialize.</param>
        /// <param name="output">stream to which you want to serialize given assembly.</param>
        /// <param name="format">format to which you want to serialize.</param>
        void Serialize(IAssembly assembly, Stream output, string format);

        void Init();
    }
}