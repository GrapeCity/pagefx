using System;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Core.IL
{
	[Flags]
    internal enum SEHFlags
    {
        Catch = 0x00,
        Filter = 0x01,
        Finally = 0x02,
        Fault = 0x04,
        TypeMask = 0x07,
    }

    /// <summary>
    /// Represents structured exception handler blocks
    /// </summary>
    internal struct SEHBlock
    {
        public SEHBlock(BufferedBinaryReader reader, bool fatFormat)
        {
            if (fatFormat)
            {
                Flags = (SEHFlags)reader.ReadInt32();
                TryOffset = reader.ReadInt32();
                TryLength = reader.ReadInt32();
                HandlerOffset = reader.ReadInt32();
                HandlerLength = reader.ReadInt32();
                Value = reader.ReadInt32();
            }
            else
            {
                Flags = (SEHFlags)reader.ReadInt16();
                TryOffset = reader.ReadInt16();
                TryLength = reader.ReadByte();
                HandlerOffset = reader.ReadInt16();
                HandlerLength = reader.ReadByte();
                Value = reader.ReadInt32();
            }
        }

        public readonly SEHFlags Flags;
        public readonly int TryOffset;
        public readonly int TryLength;
        public readonly int HandlerOffset;
        public readonly int HandlerLength;
        public readonly int Value;

        public SEHFlags Type
        {
            get { return Flags & SEHFlags.TypeMask; }
        }

        public int HandlerEnd
        {
            get { return HandlerOffset + HandlerLength; }
        }

        public override string ToString()
        {
            return string.Format("{0}({1}, {2}, {3}, {4})",
                                 Type, TryOffset, TryLength,
                                 HandlerOffset, HandlerLength);
        }
    }
}