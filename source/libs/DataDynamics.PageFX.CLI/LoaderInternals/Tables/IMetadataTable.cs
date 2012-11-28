using System.Collections.Generic;
using DataDynamics.PageFX.CLI.Metadata;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
{
	internal interface IMetadataTable<T> : IReadOnlyList<T>
	{
		TableId Id { get; }
	}
}