using System.IO;

namespace DataDynamics.PageFX.CLI.Metadata
{
    public sealed class MdbHeader
    {
        public MdbHeader(BufferedBinaryReader reader)
        {
            Signature = reader.ReadUInt32();
            if (Signature != 0x424A5342)
            {
                throw new BadMetadataException("Invalid metadata header.");
            }

            MajorVersion = reader.ReadUInt16();
            MinorVersion = reader.ReadUInt16();

            if ((MajorVersion != 1) || (MinorVersion != 1))
            {
                throw new BadMetadataException("Invalid metadata version.");
            }

            reader.ReadUInt32(); //reserved
            int verLen = reader.ReadInt32();
            VersionString = reader.ReadUtf8(verLen);

            // align for dword boundary		
            reader.Align4();

            Flags = reader.ReadUInt16();
            
            int n = reader.ReadUInt16();
            _streams = new MdbStream[n];
            for (int i = 0; i < n; i++)
            {
                _streams[i] = new MdbStream(reader);
                reader.Align4();
            }
        }

        /// <summary>
        /// Netadata header signature
        /// </summary>
        public uint Signature
        {
            get;
            set;
        }

        /// <summary>
        /// Major version
        /// </summary>
        public ushort MajorVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Minor version
        /// </summary>
        public ushort MinorVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Framework version
        /// </summary>
        public string VersionString
        {
            get;
            set;
        }

        public FrameworkVersion FrameworkVersion
        {
            get { return FrameworkInfo.ParseVersion(VersionString, FrameworkVersion.NET_2_0); }
        }

        /// <summary>
        /// Metadata flags
        /// </summary>
        public int Flags
        {
            get;
            set;
        }

        /// <summary>
        /// Metadata streams information
        /// </summary>
        public MdbStream[] Streams
        {
            get { return _streams; }
        }
        private readonly MdbStream[] _streams;
    }
}