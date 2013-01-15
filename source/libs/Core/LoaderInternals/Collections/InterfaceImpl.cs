using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Collections
{
	internal sealed class InterfaceImpl : LazyTypeList
	{
		private readonly AssemblyLoader _loader;
		private readonly IType _owner;

		public InterfaceImpl(AssemblyLoader loader, IType owner)
		{
			_loader = loader;
			_owner = owner;
		}

		private int OwnerIndex
		{
			get { return _owner.RowIndex(); }
		}

		protected override IEnumerable<IType> Populate()
		{
			var rows = _loader.Metadata.LookupRows(TableId.InterfaceImpl, Schema.InterfaceImpl.Class, OwnerIndex, true);
			return rows.Select(row =>
				{
					SimpleIndex ifaceIndex = row[Schema.InterfaceImpl.Interface].Value;
					var iface = _loader.GetTypeDefOrRef(ifaceIndex, new Context(_owner));
					if (iface == null)
						throw new BadMetadataException();
					return iface;
				});
		}
	}
}
