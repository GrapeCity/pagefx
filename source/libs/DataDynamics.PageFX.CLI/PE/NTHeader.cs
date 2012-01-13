using System;
using System.IO;

namespace DataDynamics.PE
{
	/// <summary>
	/// NT header
	/// </summary>
	public sealed class NTHeader
	{
        public uint ImageBase = 0x400000;
        public uint SectionAlignment = 0x2000;
        public uint FileAlignment = 0x200;
        public ushort MajorOperatingSystemVersion = 4;
        public ushort MinorOperatingSystemVersion;
        public ushort MajorImageVersion;
        public ushort MinorImageVersion;
        public ushort MajorSubsystemVersion = 4;
        public ushort MinorSubsystemVersion;
		public uint Reserved;
        public uint ImageSize;
        public uint HeaderSize;
        public uint FileChecksum;
        public ushort SubSystem;
        public ushort DllCharacteristics;
        public uint StackReserveSize = 0x100000;
        public uint StackCommitSize = 0x100;
        public uint HeapReserveSize = 0x100000;
        public uint HeapCommitSize = 0x100;
        public uint LoaderFlags;
        public uint NumberOfDataDirectories = 16;

        public NTHeader(BufferedBinaryReader reader)
		{
			ImageBase = reader.ReadUInt32();
			SectionAlignment = reader.ReadUInt32();
			FileAlignment = reader.ReadUInt32();
			MajorOperatingSystemVersion = reader.ReadUInt16();
			MinorOperatingSystemVersion = reader.ReadUInt16();
			MajorImageVersion = reader.ReadUInt16();
			MinorImageVersion = reader.ReadUInt16();
			MajorSubsystemVersion = reader.ReadUInt16();
			MinorSubsystemVersion = reader.ReadUInt16();
			Reserved = reader.ReadUInt32();
			ImageSize = reader.ReadUInt32();
			HeaderSize = reader.ReadUInt32();
			FileChecksum = reader.ReadUInt32();
			SubSystem = reader.ReadUInt16();
			DllCharacteristics = reader.ReadUInt16();
			StackReserveSize = reader.ReadUInt32();
			StackCommitSize = reader.ReadUInt32();
			HeapReserveSize = reader.ReadUInt32();
			HeapCommitSize = reader.ReadUInt32();
			LoaderFlags = reader.ReadUInt32();
			NumberOfDataDirectories = reader.ReadUInt32();
			
			if ((FileAlignment != 512) && (FileAlignment != 4096))
			{
                throw new BadImageFormatException("Invalid file alignment in NT header.");
			}

			if (NumberOfDataDirectories < 16)
			{
                throw new BadImageFormatException("Invalid number of data directories in NT header.");
			}
		}
	}
}
