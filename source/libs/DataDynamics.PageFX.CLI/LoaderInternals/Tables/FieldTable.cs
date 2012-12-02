using System;
using DataDynamics.PageFX.CLI.LoaderInternals.Collections;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
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

		private static int GetOffset(MetadataReader metadata, int fieldIndex)
		{
			var row = metadata.LookupRow(TableId.FieldLayout, Schema.FieldLayout.Field, fieldIndex, true);
			return row == null ? -1 : (int)row[Schema.FieldLayout.Offset].Value;
		}

		private static object ResolveBlobValue(MetadataReader metadata, IType fieldType, int fieldIndex)
		{
			var row = metadata.LookupRow(TableId.FieldRVA, Schema.FieldRVA.Field, fieldIndex, true);
			if (row == null) return null;

			int size = GetTypeSize(fieldType);
			if (size > 0)
			{
				uint rva = row[Schema.FieldRVA.RVA].Value;
				var reader = metadata.SeekRVA(rva);
				return reader.ReadBytes(size);
			}

			throw new InvalidOperationException();
		}

		private static int GetTypeSize(IType type)
		{
			if (type.Layout != null)
				return type.Layout.Size;
			var st = type.SystemType();
			return st == null ? -1 : st.Size;
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
			private readonly FieldSignature _signature;
			private IType _type;
			private IType _declType;
			private int _offset = -100;
			private object _value;

			public MetaField(AssemblyLoader loader, IField field, FieldSignature signature)
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
				SimpleIndex token = _field.MetadataToken;
				return _loader.Const[token] ?? ResolveBlobValue(_loader.Metadata, Type, token.Index - 1);
			}

			private int ResolveOffset()
			{
				SimpleIndex token = _field.MetadataToken;
				return GetOffset(_loader.Metadata, token.Index - 1);
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
