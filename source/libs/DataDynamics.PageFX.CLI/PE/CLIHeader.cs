using System;
using System.IO;

namespace DataDynamics.PE
{
    [Flags]
    public enum CLIFlags
    {
        ILOnly = 0x01,
        Required32bit = 0x02,
        StrongnameSigned = 0x08,
        TrackDebugData = 0x010000,
    }

    /// <summary>
    /// CLI header
    /// </summary>
    public sealed class CLIHeader
    {
        /// <summary>
        /// Size of header in bytes
        /// </summary>
        public uint Size = 72;

        /// <summary>
        /// The minimum version of the runtime required to run this program, currently 2.
        /// </summary>
        public ushort MajorRuntimeVersion = 2;

        /// <summary>
        /// The minor portion of the version, currently 0.
        /// </summary>
        public ushort MinorRuntimeVersion;

        /// <summary>
        /// RVA and size of the physical metadata
        /// </summary>
        public PEDataDirectory MetaData;

        /// <summary>
        /// Flags describing this runtime image.
        /// </summary>
        public CLIFlags Flags = CLIFlags.ILOnly;

        /// <summary>
        /// Token for the MethodDef or File of the entry point for the image
        /// </summary>
        public int EntryPointToken;

        /// <summary>
        /// RVA and size of implementation-specific resources.
        /// </summary>
        public PEDataDirectory Resources;

        /// <summary>
        /// RVA of the hash data for this PE file used by the CLI loader for binding and versioning
        /// </summary>
        public PEDataDirectory StrongNameSignature;

        /// <summary>
        /// Always 0
        /// </summary>
        public PEDataDirectory CodeManagerTable;

        /// <summary>
        /// RVA of an array of locations in the file that contain
        /// an array of function pointers (e.g., vtable slots).
        /// </summary>
        public PEDataDirectory VTableFixups;

        /// <summary>
        /// Always 0
        /// </summary>
        public PEDataDirectory ExportAddressTableJumps;

        /// <summary>
        /// Always 0
        /// </summary>
        public PEDataDirectory ManagedNativeHeader;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="reader"></param>
        public CLIHeader(BufferedBinaryReader reader)
        {
            Size = reader.ReadUInt32();
            MajorRuntimeVersion = reader.ReadUInt16();
            MinorRuntimeVersion = reader.ReadUInt16();
            MetaData = new PEDataDirectory(reader);
            Flags = (CLIFlags)reader.ReadUInt32();
            EntryPointToken = reader.ReadInt32();
            Resources = new PEDataDirectory(reader);
            StrongNameSignature = new PEDataDirectory(reader);
            CodeManagerTable = new PEDataDirectory(reader);
            VTableFixups = new PEDataDirectory(reader);
            ExportAddressTableJumps = new PEDataDirectory(reader);
            ManagedNativeHeader = new PEDataDirectory(reader);
        }
    }
}