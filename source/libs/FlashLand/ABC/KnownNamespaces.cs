using System;
using DataDynamics.PageFX.FlashLand.Core;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	internal sealed class KnownNamespaces
	{
		private readonly AbcFile _abc;
		private readonly AbcNamespace[] _cache = new AbcNamespace[7];

		public KnownNamespaces(AbcFile abc)
		{
			_abc = abc;
		}

		/// <summary>
		/// Returns global package namespace.
		/// </summary>
		public AbcNamespace GlobalPackage
		{
			get { return Get(KnownNamespace.Global); }
		}

		public AbcNamespace Get(KnownNamespace id)
		{
			int i = (int)id;
			return _cache[i] ?? (_cache[i] = Create(id));
		}

		private AbcNamespace Create(KnownNamespace id)
		{
			switch (id)
			{
				case KnownNamespace.Global:
					return _abc.DefinePackage(_abc.EmptyString);
				case KnownNamespace.Internal:
					return _abc.DefineInternalNamespace(_abc.EmptyString);
				case KnownNamespace.BodyTrait:
					return _abc.DefineInternalNamespace(_abc.EmptyString);
				case KnownNamespace.AS3:
					return _abc.DefinePublicNamespace(AS3.NS2006);
				case KnownNamespace.MxInternal:
					return _abc.DefinePublicNamespace(MX.NamespaceInternal2006);
				case KnownNamespace.PfxPackage:
					return _abc.DefinePackage(Const.Namespaces.PFX);
				case KnownNamespace.PfxPublic:
					return _abc.DefinePublicNamespace(Const.Namespaces.PFX);
				default:
					throw new ArgumentOutOfRangeException("id");
			}
		}
	}

	internal enum KnownNamespace : byte
	{
		Global,
		Internal,
		BodyTrait,
		AS3,
		MxInternal,
		PfxPackage,
		PfxPublic,
	}
}
