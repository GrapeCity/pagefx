using System.IO;

namespace DataDynamics.PageFX.CLI.Metadata
{
    /// <summary>
    /// Metadata stream header. 
    /// </summary>
    /// <remarks>
    /// Metadata is represented using streams. The stream types are:
    /// 
    /// #Strings
    ///		contains token names etc... (no literal constants etc)
    ///	#Blob
    ///		internal metadata binary objects
    ///	#GUID
    ///		contains guids for all sorts of identifiers
    ///	#US
    ///		string table for user-defined strings, these can be loaded using ldstr IL instruction
    ///	#~  
    ///		compressed(optimized) metadata stream , contains metadata tables
    ///	#-
    ///		uncompressed metadata tables
    /// </remarks>
    internal sealed class MdbStream
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="reader"></param>
        public MdbStream(BufferedBinaryReader reader)
        {
            Offset = reader.ReadUInt32();
            Size = reader.ReadUInt32();
            Name = reader.ReadAlignedString(16);
        }

	    /// <summary>
	    /// Gets or sets the name of the stream.
	    /// </summary>
	    /// <remarks>
	    /// Stored on-disk as a null-terminated ASCII string,
	    /// rounded up to 4-byte boundary.
	    /// </remarks>
	    public string Name;

	    /// <summary>
	    /// Stream offset in PE file from metadata header
	    /// </summary>
	    public uint Offset;

	    /// <summary>
	    /// Specifies stream size.
	    /// </summary>
	    public uint Size;
    }
}