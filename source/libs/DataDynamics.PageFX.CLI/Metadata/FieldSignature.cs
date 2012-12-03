using System.IO;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.CLI.Metadata
{
	internal sealed class FieldSignature : MetadataSignature
	{
		public TypeSignature Type;

		public FieldSignature(TypeSignature type)
		{
			Type = type;
		}

		public override SignatureKind Kind
		{
			get { return SignatureKind.Field; }
		}

		public override string ToString()
		{
			return "FIELD " + Type;
		}

		public static FieldSignature Decode(BufferedBinaryReader reader)
		{
			var h = (SignatureFlags)reader.ReadUInt8();
			if ((h & SignatureFlags.TYPEMASK) != SignatureFlags.FIELD)
				throw new BadMetadataException("Incorrect signature for field.");
			return new FieldSignature(TypeSignature.Decode(reader));
		}
	}
}