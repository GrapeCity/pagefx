using DataDynamics.PageFX.CLI.Collections;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class EventTable : MetadataTable<IEvent>
	{
		public EventTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.Event; }
		}

		protected override IEvent ParseRow(MdbRow row, int index)
		{
			var flags = (EventAttributes)row[MDB.Event.EventFlags].Value;
			var name = row[MDB.Event.Name].String;

			var token = MdbIndex.MakeToken(MdbTableId.Event, index + 1);
			var e = new Event
				{
					MetadataToken = token,
					Name = name,
					IsSpecialName = ((flags & EventAttributes.SpecialName) != 0),
					IsRuntimeSpecialName = ((flags & EventAttributes.RTSpecialName) != 0)
				};

			e.CustomAttributes = new CustomAttributes(Loader, e, token);

			return e;
		}
	}
}
