using System;
using System.IO;

namespace DataDynamics.PE
{
	/// <summary>
	/// PE File Header
	/// </summary>
	public sealed class PEHeader
	{
        public ushort Magic;
		public byte MajorLinkerVersion;
		public byte MinorLinkerVersion;
		public int CodeSize;
		public int InitializedDataSize;
		public int UninitializedDataSize;
		public int EntryPointRva;
		public int CodeBase;
		public int DataBase;

		public PEHeader(BufferedBinaryReader reader)
		{
			Magic = 0x10B;
			MajorLinkerVersion = 6;
			MinorLinkerVersion = 0;
			CodeSize = 0;
			InitializedDataSize = 512;
			UninitializedDataSize = 0;
			EntryPointRva = 0;
			CodeBase = 8192;
			DataBase = 0;

			Magic = reader.ReadUInt16();
			MajorLinkerVersion = reader.ReadUInt8();
			MinorLinkerVersion = reader.ReadUInt8();

			if (Magic != 0x010B || MajorLinkerVersion < 6 || MinorLinkerVersion != 0)
			{
                throw new BadImageFormatException("Invalid PE Header.");
			}

			CodeSize = reader.ReadInt32();
			InitializedDataSize = reader.ReadInt32();
			UninitializedDataSize = reader.ReadInt32();
			EntryPointRva = reader.ReadInt32();
			CodeBase = reader.ReadInt32();
			DataBase = reader.ReadInt32();
		}
	}
}
