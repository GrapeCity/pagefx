using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Tables
{
	internal interface IMetadataTable<T> : IReadOnlyList<T>
	{
		TableId Id { get; }
	}
}