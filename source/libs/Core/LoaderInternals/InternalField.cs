using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class InternalField : MemberBase, IField
	{
		private readonly FieldAttributes _flags;
		private readonly FieldSignature _signature;
		private IType _declaringType;
		private IType _type;
		private bool _offsetResolved;
		private int _offset = -1;
		private bool _valueResolved;
		private object _value;

		public InternalField(AssemblyLoader loader, MetadataRow row, int index)
			: base(loader, TableId.Field, index)
		{
			_flags = (FieldAttributes)row[Schema.Field.Flags].Value;
			Name = row[Schema.Field.Name].String;

			var sigBlob = row[Schema.Field.Signature].Blob;
			_signature = FieldSignature.Decode(sigBlob);
		}

		public string Documentation { get; set; }

		public MemberType MemberType
		{
			get { return MemberType.Field; }
		}

		public string FullName
		{
			get { return this.BuildFullName(); }
		}

		public IType DeclaringType
		{
			get { return _declaringType ?? (_declaringType = ResolveDeclaringType()); }
		}

		public IType Type
		{
			get { return _type ?? (_type = ResolveType()); }
		}

		public Visibility Visibility
		{
			get
			{
				var v = _flags & FieldAttributes.FieldAccessMask;
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

		public bool IsStatic
		{
			get { return (_flags & FieldAttributes.Static) != 0; }
		}

		public bool IsSpecialName
		{
			get { return (_flags & FieldAttributes.SpecialName) != 0; }
		}

		public bool IsRuntimeSpecialName
		{
			get { return (_flags & FieldAttributes.RTSpecialName) != 0; }
		}

		public object Value
		{
			get { return _value ?? (_value = ResolveValue()); }
		}

		public int Offset
		{
			get
			{
				if (_offset == -1)
					_offset = ResolveOffset();
				return -1;
			}
		}

		public bool IsConstant
		{
			get { return (_flags & FieldAttributes.Literal) != 0; }
		}

		public bool IsReadOnly
		{
			get { return (_flags & FieldAttributes.InitOnly) != 0; }
		}

		public IField ProxyOf
		{
			get { return null; }
		}

		public int Slot { get; set; }

		private IType ResolveType()
		{
			var type = Loader.ResolveType(_signature.Type, new Context(DeclaringType));
			if (type == null)
				throw new InvalidOperationException();
			return type;
		}

		private IType ResolveDeclaringType()
		{
			var type = Loader.ResolveDeclType(this);
			if (type == null)
				throw new InvalidOperationException();
			return type;
		}

		private object ResolveValue()
		{
			if (_valueResolved) return null;
			_valueResolved = true;
			SimpleIndex token = MetadataToken;
			return Loader.Const[token] ?? ResolveBlobValue(Loader.Metadata, Type, token.Index - 1);
		}

		private int ResolveOffset()
		{
			if (_offsetResolved) return -1;
			_offsetResolved = true;
			SimpleIndex token = MetadataToken;
			return GetOffset(Loader.Metadata, token.Index - 1);
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
				return metadata.SliceAtVirtualAddress(rva, size);
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