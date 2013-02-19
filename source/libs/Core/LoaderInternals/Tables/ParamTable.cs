using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class ParamTable : MetadataTable<IParameter>
	{
		public ParamTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.Param; }
		}

		protected override IParameter ParseRow(MetadataRow row, int index)
		{
			return new ParameterImpl(Loader, row, index);
		}
	}
}
