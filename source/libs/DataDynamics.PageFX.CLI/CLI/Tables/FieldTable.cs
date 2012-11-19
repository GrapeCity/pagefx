using System;
using DataDynamics.PageFX.CLI.Collections;
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
			var sigBlob = row[MDB.Field.Signature].Blob;
			var signature = MdbSignature.DecodeFieldSignature(sigBlob);

			var field = new Field
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

			field.Meta = new MetaField(Loader, field, signature);
			field.CustomAttributes = new CustomAttributes(Loader, field);

			return field;
		}

		private static int GetOffset(MdbReader mdb, int fieldIndex)
		{
			var row = mdb.LookupRow(MdbTableId.FieldLayout, MDB.FieldLayout.Field, fieldIndex, true);
			return row == null ? -1 : (int)row[MDB.FieldLayout.Offset].Value;
		}

		private static object ResolveBlobValue(MdbReader mdb, IType fieldType, int fieldIndex)
		{
			var row = mdb.LookupRow(MdbTableId.FieldRVA, MDB.FieldRVA.Field, fieldIndex, true);
			if (row == null) return null;

			int size = GetTypeSize(fieldType);
			if (size > 0)
			{
				uint rva = row[MDB.FieldRVA.RVA].Value;
				var reader = mdb.SeekRVA(rva);
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

		private sealed class MetaField : IMetaField
		{
			private readonly AssemblyLoader _loader;
			private readonly IField _field;
			private readonly MdbFieldSignature _signature;
			private IType _type;
			private IType _declType;
			private int _offset = -100;
			private object _value;

			public MetaField(AssemblyLoader loader, IField field, MdbFieldSignature signature)
			{
				_loader = loader;
				_field = field;
				_signature = signature;
				_value = this;
			}

			public IType Type
			{
				get { return _type ?? (_type = ResolveType()); }
			}

			public IType DeclaringType
			{
				get { return _declType ?? (_declType = ResolveDeclType()); }
			}

			public object Value
			{
				get { return _value == this ? _value = ResolveValue() : _value; }
			}

			public int Offset
			{
				get { return _offset == -100 ? _offset = ResolveOffset() : _offset; }
			}

			private object ResolveValue()
			{
				MdbIndex token = _field.MetadataToken;
				return _loader.Const[token] ?? ResolveBlobValue(_loader.Mdb, Type, token.Index - 1);
			}

			private int ResolveOffset()
			{
				MdbIndex token = _field.MetadataToken;
				return GetOffset(_loader.Mdb, token.Index - 1);
			}

			private IType ResolveType()
			{
				var type = _loader.ResolveType(_signature.Type, new Context(DeclaringType));
				if (type == null)
					throw new InvalidOperationException();
				return type;
			}

			private IType ResolveDeclType()
			{
				var type = _loader.ResolveDeclType(_field);
				if (type == null)
					throw new InvalidOperationException();
				return type;
			}
		}
	}
}
