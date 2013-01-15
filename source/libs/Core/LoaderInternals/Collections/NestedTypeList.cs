using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Collections
{
	internal sealed class NestedTypeList : LazyTypeList
	{
		private readonly IType _owner;
		private readonly AssemblyLoader _loader;

		public NestedTypeList(AssemblyLoader loader, IType owner)
		{
			_loader = loader;
			_owner = owner;
		}

		public override IType FindType(string fullname)
		{
			return this.FirstOrDefault(t => t.Name == fullname || t.FullName == fullname);
		}

		protected override IEnumerable<IType> Populate()
		{
			var ownerIndex = _owner.RowIndex();
			var rows = _loader.Metadata.LookupRows(TableId.NestedClass, Schema.NestedClass.EnclosingClass, ownerIndex, true);
			return rows.Select(
				row =>
					{
						int index = row[Schema.NestedClass.Class].Index - 1;
						var type = _loader.Types[index];
						type.DeclaringType = _owner;
						return type;
					});
		}
	}
}
