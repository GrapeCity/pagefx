using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class FieldTable : MetadataTable<IField>
	{
		public FieldTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.Field; }
		}

		protected override IField ParseRow(MdbRow row, int index)
		{
			var flags = (FieldAttributes)row[MDB.Field.Flags].Value;
			var name = row[MDB.Field.Name].String;

			var token = MdbIndex.MakeToken(MdbTableId.Field, index + 1);
			var value = Loader.Const[token];

			return new Field
				{
					MetadataToken = token,
					Visibility = ToVisibility(flags),
					IsStatic = ((flags & FieldAttributes.Static) != 0),
					IsConstant = ((flags & FieldAttributes.Literal) != 0),
					IsReadOnly = ((flags & FieldAttributes.InitOnly) != 0),
					IsSpecialName = ((flags & FieldAttributes.SpecialName) != 0),
					IsRuntimeSpecialName = ((flags & FieldAttributes.RTSpecialName) != 0),
					Name = name,
					Offset = GetOffset(Mdb, index),
					Value = value
				};
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

		private static int GetOffset(MdbReader mdb, int fieldIndex)
		{
			var row = mdb.LookupRowByIndex(MdbTableId.FieldLayout, MDB.FieldLayout.Field, fieldIndex);
			return row == null ? -1 : (int)row[MDB.FieldLayout.Offset].Value;
		}
	}
}
