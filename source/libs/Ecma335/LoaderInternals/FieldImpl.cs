using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals
{
	internal sealed class FieldImpl : Field
	{
		private readonly AssemblyLoader _loader;
		private readonly FieldSignature _signature;
		private bool _valueResolved;
		private bool _offsetResolved;

		public FieldImpl(AssemblyLoader loader, FieldSignature signature)
		{
			_loader = loader;
			_signature = signature;
		}

		protected override IType ResolveType()
		{
			var type = _loader.ResolveType(_signature.Type, new Context(DeclaringType));
			if (type == null)
				throw new InvalidOperationException();
			return type;
		}

		protected override IType ResolveDeclaringType()
		{
			var type = _loader.ResolveDeclType(this);
			if (type == null)
				throw new InvalidOperationException();
			return type;
		}

		protected override object ResolveValue()
		{
			if (_valueResolved) return null;
			_valueResolved = true;
			SimpleIndex token = MetadataToken;
			return _loader.Const[token] ?? ResolveBlobValue(_loader.Metadata, Type, token.Index - 1);
		}

		protected override int ResolveOffset()
		{
			if (_offsetResolved) return -1;
			_offsetResolved = true;
			SimpleIndex token = MetadataToken;
			return GetOffset(_loader.Metadata, token.Index - 1);
		}

		private static int GetOffset(MetadataReader metadata, int fieldIndex)
		{
			var row = metadata.LookupRow(TableId.FieldLayout, Schema.FieldLayout.Field, fieldIndex, true);
			return row == null ? -1 : (int)row[Schema.FieldLayout.Offset].Value;
		}

		//TODO: return BufferedBinaryReader (as slice of (offset from RVA, field size)) instead of object

		private static object ResolveBlobValue(MetadataReader metadata, IType fieldType, int fieldIndex)
		{
			var row = metadata.LookupRow(TableId.FieldRVA, Schema.FieldRVA.Field, fieldIndex, true);
			if (row == null) return null;

			int size = GetTypeSize(fieldType);
			if (size > 0)
			{
				uint rva = row[Schema.FieldRVA.RVA].Value;
				var reader = metadata.MoveToVirtualAddress(rva);
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
	}
}