using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class InternalType : TypeImpl
	{
		private const TypeKind TypeKindNone = (TypeKind)0xFE;

		private static readonly Dictionary<string, TypeKind> TypeKindByBase =
			new Dictionary<string, TypeKind>
				{
					{"System.Enum", TypeKind.Enum},
					{"System.ValueType", TypeKind.Struct},
					{"System.Delegate", TypeKind.Delegate},
					{"System.MulticastDelegate", TypeKind.Delegate},
				};

		private readonly AssemblyLoader _loader;
		private bool _baseTypeResolved;

		public InternalType(AssemblyLoader loader)
		{
			_loader = loader;

			TypeKind = TypeKindNone;
		}

		public override IModule Module
		{
			get { return _loader.MainModule; }
			set { throw new NotSupportedException(); }
		}

		public override TypeKind TypeKind
		{
			get
			{
				var kind = base.TypeKind;
				if (kind == TypeKindNone)
				{
					kind = ResolveTypeKind(this);
					base.TypeKind = kind;
				}
				return kind;
			}
			set { base.TypeKind = value; }
		}

		public static TypeKind ResolveTypeKind(IType type)
		{
			var sysType = type.SystemType();
			if (sysType != null) return sysType.Kind;

			var baseType = type.BaseType;
			if (baseType != null)
			{
				TypeKind kind;
				if (TypeKindByBase.TryGetValue(baseType.FullName, out kind))
				{
					return kind;
				}
			}

			return TypeKind.Class;
		}

		protected override IType ResolveBaseType()
		{
			if (_baseTypeResolved) return null;

			_baseTypeResolved = true;

			return ResolveBaseType(_loader, this);
		}

		protected override IType ResolveDeclaringType()
		{
			return ResolveDeclaringType(_loader, this.RowIndex());
		}

		protected override ClassLayout ResolveLayout()
		{
			return ResolveLayout(_loader, this.RowIndex());
		}

		public static ClassLayout ResolveLayout(AssemblyLoader loader, int typeIndex)
		{
			var row = loader.Metadata.LookupRow(TableId.ClassLayout, Schema.ClassLayout.Parent, typeIndex, true);
			if (row == null)
				return null;

			var size = (int)row[Schema.ClassLayout.ClassSize].Value;
			var pack = (int)row[Schema.ClassLayout.PackingSize].Value;
			return new ClassLayout(size, pack);
		}

		public static IType ResolveBaseType(AssemblyLoader loader, IType type)
		{
			if (type.Is(SystemTypeCode.Object)) return null;

			var row = loader.Metadata.GetRow(TableId.TypeDef, type.RowIndex());

			SimpleIndex baseIndex = row[Schema.TypeDef.Extends].Value;

			return loader.GetTypeDefOrRef(baseIndex, new Context(type));
		}

		public static IType ResolveDeclaringType(AssemblyLoader loader, int index)
		{
			var row = loader.Metadata.LookupRow(TableId.NestedClass, Schema.NestedClass.Class, index, true);
			if (row == null) return null;

			int enclosingIndex = row[Schema.NestedClass.EnclosingClass].Index - 1;
			return loader.Types[enclosingIndex];
		}
	}
}
