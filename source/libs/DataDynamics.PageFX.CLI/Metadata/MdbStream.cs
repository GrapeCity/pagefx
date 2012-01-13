using System;
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
    public sealed class MdbStream : IEquatable<MdbStream>
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="reader"></param>
        public MdbStream(BufferedBinaryReader reader)
        {
            Offset = reader.ReadUInt32();
            Size = reader.ReadUInt32();
            Name = reader.ReadNullTerminatedAsciiString();
        }

        /// <summary>
        /// Gets or sets the name of the stream.
        /// </summary>
        /// <remarks>
        /// Stored on-disk as a null-terminated ASCII string,
        /// rounded up to 4-byte boundary.
        /// </remarks>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Stream offset in PE file from metadata header
        /// </summary>
        public uint Offset
        {
            get;
            set;
        }

        /// <summary>
        /// Stream size
        /// </summary>
        public uint Size
        {
            get;
            set;
        }

        public bool Equals(MdbStream s)
        {
            if (s == null) return false;
            if (Offset != s.Offset) return false;
            if (Size != s.Size) return false;
            if (!Equals(Name, s.Name)) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as MdbStream);
        }

        public override int GetHashCode()
        {
            int h = (int)Offset;
            h = 29 * h + (int)Size;
            h = 29 * h + (Name != null ? Name.GetHashCode() : 0);
            return h;
        }

        public override string ToString()
        {
            return string.Format("MDStream({0}, Offset = {1}, Size = {2})",
                                 Name, Offset, Size);
        }
    }
}