using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class LazyType
	{
		private readonly AssemblyLoader _loader;
		private readonly TypeSignature _signature;
		private readonly Context _context;
		private IType _type;

		public LazyType(AssemblyLoader loader, TypeSignature signature, Context context)
		{
			_loader = loader;
			_signature = signature;
			_context = context;
		}

		public IType Value
		{
			get { return _type ?? (_type = ResolveType()); }
		}

		private IType ResolveType()
		{
			var type = _loader.ResolveType(_signature, _context);
			if (type == null)
				throw new InvalidOperationException();
			return type;
		}
	}
}
