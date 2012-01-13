using System;
using System.IO;

namespace DataDynamics.PE
{
	/// <summary>
	/// COFF header.
	/// </summary>
	public sealed class COFFHeader
	{
        public PESignature Signature = PESignature.NT;
        public PEMachineId _PEMachine = PEMachineId.I386;
		public ushort NumberOfSections;
		public uint TimeStamp;
		public uint PointerToSymbolTable;
		public uint NumberOfSymbols;
		public ushort OptionalHeaderSize = 224;
        public PECharacteristics _PECharacteristics = (PECharacteristics)270;

		public COFFHeader(BufferedBinaryReader reader)
		{
            Signature = (PESignature)reader.ReadUInt16();
            if (Signature != PESignature.NT)
            {
                throw new BadImageFormatException("Invalid image format: cannot find PE signature.");
            }
            var peSig = (PESignature)reader.ReadUInt16();
            if (peSig != PESignature.NT2)
            {
                throw new BadImageFormatException("Invalid image format: cannot find PE signature.");
            }

            _PEMachine = (PEMachineId)reader.ReadUInt16();
            if (_PEMachine != PEMachineId.I386)
            {
                throw new BadImageFormatException("Invalid COFF header.");
            }

			NumberOfSections = reader.ReadUInt16();
			TimeStamp = reader.ReadUInt32();
			PointerToSymbolTable = reader.ReadUInt32();
			NumberOfSymbols = reader.ReadUInt32();
			OptionalHeaderSize = reader.ReadUInt16();
            _PECharacteristics = (PECharacteristics)reader.ReadUInt16();

			if (OptionalHeaderSize != 224)
			{
                throw new BadImageFormatException("Invalid optional header size in COFF header.");
			}

			if ((PointerToSymbolTable != 0) || (NumberOfSymbols != 0))
			{
                throw new BadImageFormatException("Invalid symbol table in COFF header.");
			}
		}
	}
}
