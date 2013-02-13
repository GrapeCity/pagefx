using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class PropertyTable : MetadataTable<IProperty>
	{
		public PropertyTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.Property; }
		}

		protected override IProperty ParseRow(MetadataRow row, int index)
		{
			return new PropertyImpl(Loader, row, index);
		}
	}
}
