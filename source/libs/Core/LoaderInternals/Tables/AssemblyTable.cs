using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class AssemblyTable : MetadataTable<IAssembly>
	{
		public AssemblyTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.Assembly; }
		}

		protected override IAssembly ParseRow(MetadataRow row, int index)
		{
			return new InternalAssembly(Loader, row);
		}
	}
}
