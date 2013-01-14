using System.Reflection;
using System.Text;
using DataDynamics.PageFX.Common.IO;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
	internal sealed class MethodSignature : MetadataSignature
	{
		public bool IsProperty;
		public CallingConventions CallingConventions = CallingConventions.Standard;
		public int GenericParamCount;
		public TypeSignature Type;
		public TypeSignature[] Params;

		public override SignatureKind Kind
		{
			get { return IsProperty ? SignatureKind.Property : SignatureKind.Method; }
		}

		internal static MethodSignature Decode(BufferedBinaryReader reader, SignatureFlags flags)
		{
			var type = (flags & SignatureFlags.TYPEMASK);
			if (!(type == SignatureFlags.PROPERTY || (type >= SignatureFlags.DEFAULT && type <= SignatureFlags.VARARG)))
				throw new BadMetadataException("Incorrect signature for method.");

			var sig = new MethodSignature();
			if (type == SignatureFlags.PROPERTY)
				sig.IsProperty = true;

			if ((flags & SignatureFlags.HASTHIS) != 0)
				sig.CallingConventions |= CallingConventions.HasThis;
			if ((flags & SignatureFlags.EXPLICITTHIS) != 0)
				sig.CallingConventions |= CallingConventions.ExplicitThis;
			if (type == SignatureFlags.VARARG)
			{
				sig.CallingConventions &= CallingConventions.Standard;
				sig.CallingConventions |= CallingConventions.VarArgs;
			}

			if ((flags & SignatureFlags.GENERIC) != 0)
				sig.GenericParamCount = reader.ReadPackedInt();

			int paramCount = reader.ReadPackedInt();

			sig.Type = TypeSignature.Decode(reader);
			sig.Params = new TypeSignature[paramCount];
			for (int i = 0; i < paramCount; ++i)
			{
				sig.Params[i] = TypeSignature.Decode(reader);
			}
			return sig;
		}

		internal static MethodSignature Decode(BufferedBinaryReader reader)
		{
			var flags = (SignatureFlags)reader.ReadUInt8();
			return Decode(reader, flags);
		}

		public override string ToString()
		{
			var s = new StringBuilder();
			s.Append(IsProperty ? "PROPERTY " : "METHOD ");

			if (GenericParamCount > 0)
			{
				s.Append("GENERIC<");
				s.Append(GenericParamCount);
				s.Append('>');
			}

			if (Type != null)
			{
				s.Append(" ");
				s.Append(Type);
				s.Append(" ");
			}

			if (Params != null)
			{
				s.Append("(");
				for (int i = 0; i < Params.Length; ++i)
				{
					if (i > 0) s.Append(", ");
					s.Append(Params[i]);
				}
				s.Append(")");
			}

			return s.ToString();
		}
	}
}