using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal interface IMetadataTable<T> : IReadOnlyList<T>
	{
		TableId Id { get; }
	}
}