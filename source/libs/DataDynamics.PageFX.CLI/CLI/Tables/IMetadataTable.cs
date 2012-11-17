using System.Collections.Generic;
using DataDynamics.PageFX.CLI.Metadata;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal interface IMetadataTable<T> : IReadOnlyList<T>
	{
		MdbTableId Id { get; }
	}
}