using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class FieldTable : MetadataTable<IField>
	{
		public FieldTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.Field; }
		}

		protected override IField ParseRow(MetadataRow row, int index)
		{
			return new InternalField(Loader, row, index);
		}
	}
}
