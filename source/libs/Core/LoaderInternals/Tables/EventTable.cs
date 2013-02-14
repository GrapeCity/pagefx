using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class EventTable : MetadataTable<IEvent>
	{
		public EventTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.Event; }
		}

		protected override IEvent ParseRow(MetadataRow row, int index)
		{
			return new EventImpl(Loader, row, index);
		}
	}
}
