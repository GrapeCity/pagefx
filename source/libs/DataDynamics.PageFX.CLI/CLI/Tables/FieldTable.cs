using System.Collections.Generic;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class FieldTable : MetadataTable<IField>
	{
		private readonly FieldLayout _layout = new FieldLayout();

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
					Offset = _layout.Find(Mdb, index),
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

		private sealed class FieldLayout
		{
			private readonly Dictionary<int,int> _offsets = new Dictionary<int, int>();
			private int _lastIndex;

			public int Find(MdbReader mdb, int fieldIndex)
			{
				int offset;
				if (_offsets.TryGetValue(fieldIndex, out offset))
					return offset;

				int n = mdb.GetRowCount(MdbTableId.FieldLayout);
				for (; _lastIndex < n; _lastIndex++)
				{
					var row = mdb.GetRow(MdbTableId.FieldLayout, _lastIndex);

					int owner = row[MDB.FieldLayout.Field].Index - 1;
					offset = (int)row[MDB.FieldLayout.Offset].Value;

					_offsets.Add(owner, offset);

					if (owner == fieldIndex)
					{
						_lastIndex++;
						return offset;
					}
				}

				return -1;
			}
		}
	}
}
