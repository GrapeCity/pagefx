using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.LoaderInternals.Collections;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Tables
{
	internal sealed class FieldTable : MetadataTable<IField>
	{
		public FieldTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.Field; }
		}

		protected override IField ParseRow(MetadataRow row, int index)
		{
			var flags = (FieldAttributes)row[Schema.Field.Flags].Value;
			var name = row[Schema.Field.Name].String;

			var token = SimpleIndex.MakeToken(TableId.Field, index + 1);
			var sigBlob = row[Schema.Field.Signature].Blob;
			var signature = FieldSignature.Decode(sigBlob);

			var field = new FieldImpl(Loader, signature)
				{
					MetadataToken = token,
					Visibility = ToVisibility(flags),
					IsStatic = ((flags & FieldAttributes.Static) != 0),
					IsConstant = ((flags & FieldAttributes.Literal) != 0),
					IsReadOnly = ((flags & FieldAttributes.InitOnly) != 0),
					IsSpecialName = ((flags & FieldAttributes.SpecialName) != 0),
					IsRuntimeSpecialName = ((flags & FieldAttributes.RTSpecialName) != 0),
					Name = name
				};

			field.CustomAttributes = new CustomAttributes(Loader, field);

			return field;
		}

		private static Visibility ToVisibility(FieldAttributes f)
		{
			var v = f & FieldAttributes.FieldAccessMask;
			switch (v)
			{
				case FieldAttributes.PrivateScope:
					return Visibility.PrivateScope;
				case FieldAttributes.Private:
					return Visibility.Private;
				case FieldAttributes.FamANDAssem:
				case FieldAttributes.FamORAssem:
					return Visibility.ProtectedInternal;
				case FieldAttributes.Assembly:
					return Visibility.Internal;
				case FieldAttributes.Family:
					return Visibility.Protected;
			}
			return Visibility.Public;
		}
	}
}
