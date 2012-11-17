using System;
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

		public IField Get(IType declType, int index)
		{
			var field = this[index];

			if (field.DeclaringType == null)
			{
				field.Type = ResolveFieldType(declType, index);

				if (field.Value == null)
				{
					field.Value = ResolveValue(Loader.Mdb, field.Type, index);
				}
			}

			return field;
		}

		protected override IField ParseRow(MdbRow row, int index)
		{
			var flags = (FieldAttributes)row[MDB.Field.Flags].Value;
			var name = row[MDB.Field.Name].String;

			var token = MdbIndex.MakeToken(MdbTableId.Field, index + 1);
			var value = Loader.Const[token];

			var field = new Field
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

			field.CustomAttributes = new CustomAttributes(Loader, field, token);

			return field;
		}

		private IType ResolveFieldType(IType declType, int index)
		{
			var row = Loader.Mdb.GetRow(MdbTableId.Field, index);
			var sigBlob = row[MDB.Field.Signature].Blob;
			var sig = MdbSignature.DecodeFieldSignature(sigBlob);

			return Loader.ResolveType(sig.Type, new Context(declType));
		}

		private static int GetOffset(MdbReader mdb, int fieldIndex)
		{
			var row = mdb.LookupRow(MdbTableId.FieldLayout, MDB.FieldLayout.Field, fieldIndex, true);
			return row == null ? -1 : (int)row[MDB.FieldLayout.Offset].Value;
		}

		private object ResolveValue(MdbReader mdb, IType fieldType, int fieldIndex)
		{
			var row = mdb.LookupRow(MdbTableId.FieldRVA, MDB.FieldRVA.Field, fieldIndex, true);
			if (row == null) return null;

			int size = GetTypeSize(fieldType);
			if (size > 0)
			{
				uint rva = row[MDB.FieldRVA.RVA].Value;
				var reader = Mdb.SeekRVA(rva);
				return reader.ReadBlock(size);
			}

			throw new InvalidOperationException();
		}

		private static int GetTypeSize(IType type)
		{
			if (type.Layout != null)
				return type.Layout.Size;
			var st = type.SystemType;
			if (st == null)
				return -1;
			return st.Size;
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
