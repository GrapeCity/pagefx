using System.IO;

namespace DataDynamics.PE
{
    /// <summary>
    /// DOS header of PE file
    /// </summary>
    public sealed class DOSHeader
    {
        private readonly int OpenSize = 60;
        private readonly int CloseSize = 64;

        private byte[] open_data; // First 60 bytes of data
        private byte[] close_data; // Last 64 bytes of data

        // File address of new exe header.
        private uint _newHeaderOffset;

        public DOSHeader()
        {
            Init();
        }

        public DOSHeader(BufferedBinaryReader reader)
        {
            Read(reader);
        }

        public uint NewHeaderOffset
        {
            get { return _newHeaderOffset; }
        }

        public void Read(BufferedBinaryReader reader)
        {
            open_data = reader.ReadBlock(OpenSize);
            _newHeaderOffset = reader.ReadUInt32();
            close_data = reader.ReadBlock(CloseSize);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(open_data);
            writer.Write(_newHeaderOffset);
            writer.Write(close_data);
        }

        public void Init()
        {
            open_data = new byte[]
                {
                    0x4D, 0x5A, 0x0, 0x0, 0xE7, 0x0, 0x0, 0x0,
                    0x4, 0x0, 0x0, 0x0, 0xFF, 0xFF, 0x0, 0x0,
                    0xB8, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                    0x40, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0, 0x0
                };

            close_data = new byte[]
                {
                    0xE, 0x1F, 0xBA, 0xE, 0x0, 0xB4, 0x9, 0xCD,
                    0x21, 0xB8, 0x1, 0x4C, 0xCD, 0x21, 0x54, 0x68,
                    0x69, 0x73, 0x20, 0x70, 0x72, 0x6F, 0x67, 0x72,
                    0x61, 0x6D, 0x20, 0x63, 0x61, 0x6E, 0x6E, 0x6F,
                    0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x75, 0x6E,
                    0x20, 0x69, 0x6E, 0x20, 0x44, 0x4F, 0x53, 0x20,
                    0x6D, 0x6F, 0x64, 0x65, 0x2E, 0xD, 0xD, 0xA,
                    0x24, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0
                };
        }

        /// <summary>
        /// </summary>
        /// <param name="writer"></param>
        public void Dump(TextWriter writer)
        {
            writer.WriteLine(
                "New header offset   : {0}",
                _newHeaderOffset + " (0x" + _newHeaderOffset.ToString("X") + ")"
                );
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sw = new StringWriter();
            Dump(sw);
            return sw.ToString();
        }
    }
}