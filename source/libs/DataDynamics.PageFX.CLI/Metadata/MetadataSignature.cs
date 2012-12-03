using System;
using System.IO;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.CLI.Metadata
{
	internal enum SignatureKind
    {
        Method = 0x00,
        Field = 0x06,
        LocalVars = 0x07,
        Property = 0x08,
    }

    [Flags]
    internal enum SignatureFlags : byte
    {
        GENERIC = 0x10,
        HASTHIS = 0x20,
        EXPLICITTHIS = 0x40,

        //One of this values identifies method signature
        DEFAULT = 0x00,
        C = 0x01,
        STDCALL = 0x02,
        THISCALL = 0x03,
        FASTCALL = 0x04,
        VARARG = 0x05,

        FIELD = 0x06,
        LOCAL = 0x07,
        PROPERTY = 0x08,
        TYPEMASK = 0xF,
    }

	internal abstract class MetadataSignature
    {
        public abstract SignatureKind Kind { get; }

		public static MetadataSignature DecodeMember(BufferedBinaryReader reader)
		{
			var flags = (SignatureFlags)reader.ReadUInt8();
			var type = flags & SignatureFlags.TYPEMASK;
			switch (type)
			{
				case SignatureFlags.FIELD:
					return new FieldSignature(TypeSignature.Decode(reader));
				case SignatureFlags.LOCAL:
					throw new NotImplementedException();
				default:
					return MethodSignature.Decode(reader, flags);
			}
		}
    }
}