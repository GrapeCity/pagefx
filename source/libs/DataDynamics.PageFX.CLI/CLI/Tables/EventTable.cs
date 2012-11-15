﻿using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.CLI.Tables
{
	internal sealed class EventTable : MetadataTable<IEvent>
	{
		public EventTable(AssemblyLoader loader) : base(loader, MdbTableId.Event)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.Event; }
		}

		protected override IEvent ParseRow(int index)
		{
			var row = Mdb.GetRow(MdbTableId.Event, index);

			var flags = (EventAttributes)row[MDB.Event.EventFlags].Value;
			var name = row[MDB.Event.Name].String;

			return new Event
				{
					MetadataToken = MdbIndex.MakeToken(MdbTableId.Event, index + 1),
					Name = name,
					IsSpecialName = ((flags & EventAttributes.SpecialName) != 0),
					IsRuntimeSpecialName = ((flags & EventAttributes.RTSpecialName) != 0),
				};
		}
	}
}
