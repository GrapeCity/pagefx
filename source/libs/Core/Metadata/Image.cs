using System;
using System.IO;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Core.Metadata
{
	/// <summary>
    /// PE file image.
    /// </summary>
    internal sealed class Image
    {
	    public TargetArchitecture Architecture;
	    public ModuleKind Kind;
	    public ModuleCharacteristics Characteristics;
		public ModuleAttributes Attributes;

		public uint EntryPointToken;

		public Section[] Sections;
	    public DataDirectory Debug;
	    public DataDirectory Resources;
	    public DataDirectory StrongName;

	    /// <summary>
        /// Translate Relative Virtual Address (RVA) to the actual Virtual Address (VA)
        /// </summary>
        /// <param name="rva">RVA to translate.</param>
        public int ResolveVirtualAddress(uint rva)
        {
            foreach (var section in Sections)
            {
                if (rva >= section.VirtualAddress && rva < section.VirtualAddress + section.SizeOfRawData)
                {
                    return (int)(section.PointerToRawData + rva - section.VirtualAddress);
                }
            }

			throw new ArgumentOutOfRangeException("rva");
        }

		public void Load(BufferedBinaryReader reader)
		{
			if (reader.Length < 128)
				throw new BadImageFormatException();

			// - DOSHeader
			// PE					2
			// Start				58
			// Lfanew				4
			// End					64
			if (reader.ReadUInt16() != 0x5a4d)
				throw new BadImageFormatException();
			reader.Advance(58);
			reader.Position = reader.ReadUInt32();

			// PE NT signature
			if (reader.ReadUInt32() != 0x00004550)
				throw new BadImageFormatException();

			// - PEFileHeader

			Architecture = ReadArchitecture(reader); // 2 bytes
			ushort numberOfSections = reader.ReadUInt16();

			// TimeDateStamp		4
			// PointerToSymbolTable	4
			// NumberOfSymbols		4
			// OptionalHeaderSize	2
			reader.Advance(14);

			// Characteristics		2
			ushort characteristics = reader.ReadUInt16();

			DataDirectory cli;
			ushort subsystem, dll_characteristics;
			ReadOptionalHeaders(reader, out subsystem, out dll_characteristics, out cli);

			Kind = ResolveModuleKind(characteristics, subsystem);
			Characteristics = (ModuleCharacteristics)dll_characteristics;

			ReadSections(reader, numberOfSections);
			ReadCliHeader(reader, cli);
		}

		private static TargetArchitecture ReadArchitecture(BufferedBinaryReader reader)
		{
			var target = (TargetArchitecture)reader.ReadUInt16();
			if (!Enum.IsDefined(typeof(TargetArchitecture), target))
				throw new NotSupportedException();
			return target;
		}

		private static void ReadOptionalHeaders(BufferedBinaryReader reader, out ushort subsystem, out ushort dll_characteristics, out DataDirectory cli)
		{
			// - PEOptionalHeader
			//   - StandardFieldsHeader

			// Magic				2
			bool pe64 = reader.ReadUInt16() == 0x20b;

			//						pe32 || pe64

			// LMajor				1
			// LMinor				1
			// CodeSize				4
			// InitializedDataSize	4
			// UninitializedDataSize4
			// EntryPointRVA		4
			// BaseOfCode			4
			// BaseOfData			4 || 0

			//   - NTSpecificFieldsHeader

			// ImageBase			4 || 8
			// SectionAlignment		4
			// FileAlignement		4
			// OSMajor				2
			// OSMinor				2
			// UserMajor			2
			// UserMinor			2
			// SubSysMajor			2
			// SubSysMinor			2
			// Reserved				4
			// ImageSize			4
			// HeaderSize			4
			// FileChecksum			4
			reader.Advance(66);

			// SubSystem			2
			subsystem = reader.ReadUInt16();

			// DLLFlags				2
			dll_characteristics = reader.ReadUInt16();
			// StackReserveSize		4 || 8
			// StackCommitSize		4 || 8
			// HeapReserveSize		4 || 8
			// HeapCommitSize		4 || 8
			// LoaderFlags			4
			// NumberOfDataDir		4

			//   - DataDirectoriesHeader

			// ExportTable			8
			// ImportTable			8
			// ResourceTable		8
			// ExceptionTable		8
			// CertificateTable		8
			// BaseRelocationTable	8

			reader.Advance(pe64 ? 88 : 72);

			// Debug				8
			var Debug = ReadDataDirectory(reader);

			// Copyright			8
			// GlobalPtr			8
			// TLSTable				8
			// LoadConfigTable		8
			// BoundImport			8
			// IAT					8
			// DelayImportDescriptor8
			reader.Advance(56);

			// CLIHeader			8
			cli = ReadDataDirectory(reader);

			if (cli.IsEmpty)
				throw new BadImageFormatException();

			// Reserved				8
			reader.Advance(8);
		}

		private void ReadSections(BufferedBinaryReader reader, int count)
		{
			var sections = new Section[count];

			for (int i = 0; i < count; i++)
			{
				var section = new Section
					{
						Name = reader.ReadZeroTerminatedString(8)
					};

				// VirtualSize		4
				reader.Advance(4);

				section.VirtualAddress = reader.ReadUInt32();
				section.SizeOfRawData = reader.ReadUInt32();
				section.PointerToRawData = reader.ReadUInt32();

				// PointerToRelocations		4
				// PointerToLineNumbers		4
				// NumberOfRelocations		2
				// NumberOfLineNumbers		2
				// Characteristics			4
				reader.Advance(16);

				sections[i] = section;
			}

			Sections = sections;
		}

		private void ReadCliHeader(BufferedBinaryReader reader, DataDirectory cliHeader)
		{
			MoveTo(reader, cliHeader);

			// - CLIHeader

			// Cb						4
			// MajorRuntimeVersion		2
			// MinorRuntimeVersion		2
			reader.Advance(8);

			var metadata = ReadDataDirectory(reader);
			Attributes = (ModuleAttributes)reader.ReadUInt32();
			// EntryPointToken			4
			EntryPointToken = reader.ReadUInt32();
			// Resources				8
			Resources = ReadDataDirectory(reader);
			// StrongNameSignature		8
			StrongName = ReadDataDirectory(reader);
			// CodeManagerTable			8
			// VTableFixups				8
			// ExportAddressTableJumps	8
			// ManagedNativeHeader		8

			MoveTo(reader, metadata);
		}

		private static DataDirectory ReadDataDirectory(BufferedBinaryReader reader)
		{
			return new DataDirectory(reader.ReadUInt32(), reader.ReadUInt32());
		}

		private void MoveTo(Stream reader, DataDirectory directory)
		{
			reader.Position = ResolveVirtualAddress(directory.VirtualAddress);
		}

		private static ModuleKind ResolveModuleKind(ushort characteristics, ushort subsystem)
		{
			if ((characteristics & 0x2000) != 0) // ImageCharacteristics.Dll
				return ModuleKind.Dll;

			if (subsystem == 0x2 || subsystem == 0x9) // SubSystem.WindowsGui || SubSystem.WindowsCeGui
				return ModuleKind.Windows;

			return ModuleKind.Console;
		}
    }

	internal sealed class Section
	{
		public string Name;
		public uint VirtualAddress;
		public uint SizeOfRawData;
		public uint PointerToRawData;
	}

	internal struct DataDirectory
	{
		public readonly uint VirtualAddress;
		public readonly uint Size;

		public DataDirectory(uint rva, uint size)
		{
			VirtualAddress = rva;
			Size = size;
		}

		public bool IsEmpty
		{
			get { return VirtualAddress == 0 && Size == 0; }
		}
	}

	internal enum TargetArchitecture
	{
		I386 = 0x014c,
		AMD64 = 0x8664,
		IA64 = 0x0200,
		ARMv7 = 0x01c4,
	}

	internal enum ModuleKind
	{
		Dll,
		Windows,
		Console
	}

	[Flags]
	internal enum ModuleAttributes
	{
		ILOnly = 1,
		Required32Bit = 2,
		StrongNameSigned = 8,
		Preferred32Bit = 0x00020000,
	}

	[Flags]
	internal enum ModuleCharacteristics
	{
		HighEntropyVA = 0x0020,
		DynamicBase = 0x0040,
		NoSEH = 0x0400,
		NXCompat = 0x0100,
		AppContainer = 0x1000,
		TerminalServerAware = 0x8000,
	}
}