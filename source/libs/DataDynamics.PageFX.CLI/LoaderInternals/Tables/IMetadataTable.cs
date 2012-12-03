using System.Collections.Generic;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
{
	internal interface IMetadataTable<T> : IReadOnlyList<T>
	{
		TableId Id { get; }
	}
}