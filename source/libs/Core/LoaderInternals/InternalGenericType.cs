using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class InternalGenericType : GenericType
	{
		private const TypeKind TypeKindNone = (TypeKind)0xFE;

		private readonly AssemblyLoader _loader;
		private bool _baseTypeResolved;

		public InternalGenericType(AssemblyLoader loader, IEnumerable<IGenericParameter> parameters)
			: base(parameters)
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
					kind = InternalType.ResolveTypeKind(this);
					base.TypeKind = kind;
				}
				return kind;
			}
			set { base.TypeKind = value; }
		}

		protected override IType ResolveBaseType()
		{
			if (_baseTypeResolved) return null;

			_baseTypeResolved = true;

			return InternalType.ResolveBaseType(_loader, this);
		}

		protected override IType ResolveDeclaringType()
		{
			return InternalType.ResolveDeclaringType(_loader, this.RowIndex());
		}

		protected override ClassLayout ResolveLayout()
		{
			return InternalType.ResolveLayout(_loader, this.RowIndex());
		}
	}
}