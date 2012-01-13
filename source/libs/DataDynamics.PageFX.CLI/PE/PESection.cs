using System.Collections.Generic;
using System.IO;

namespace DataDynamics.PE
{
    /// <summary>
    /// PE File section header
    /// </summary>
    public sealed class PESection
    {
        public string Name;
        public int VirtualSize;
        public int VirtualAddress;
        public int SizeOfRawData;
        public int PointerToRawData;
        public int PointerToRelocations;
        public int PointerToLineNumbers;
        public ushort NumberOfRelocations;
        public ushort NumberOfLineNumbers;
        public PESectionCharacteristics Characteristics;

        public PESection(BufferedBinaryReader reader)
        {
            Name = reader.ReadAscii(8, true);
            VirtualSize = reader.ReadInt32();
            VirtualAddress = reader.ReadInt32();
            SizeOfRawData = reader.ReadInt32();
            PointerToRawData = reader.ReadInt32();
            PointerToRelocations = reader.ReadInt32();
            PointerToLineNumbers = reader.ReadInt32();
            NumberOfRelocations = reader.ReadUInt16();
            NumberOfLineNumbers = reader.ReadUInt16();
            Characteristics = (PESectionCharacteristics)reader.ReadInt32();
        }
    }

    public sealed class PESectionList : List<PESection>
    {
        public PESection this[string name]
        {
            get
            {
                return Find(delegate(PESection s) { return s.Name == name; });
            }
        }
    }
}