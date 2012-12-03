using DataDynamics.PageFX.CLI.LoaderInternals.Collections;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Metadata;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
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
			var flags = (EventAttributes)row[Schema.Event.EventFlags].Value;
			var name = row[Schema.Event.Name].String;

			var token = SimpleIndex.MakeToken(TableId.Event, index + 1);
			var e = new Event
				{
					MetadataToken = token,
					Name = name,
					IsSpecialName = ((flags & EventAttributes.SpecialName) != 0),
					IsRuntimeSpecialName = ((flags & EventAttributes.RTSpecialName) != 0)
				};

			e.CustomAttributes = new CustomAttributes(Loader, e);

			return e;
		}
	}
}
