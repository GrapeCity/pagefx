namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class PropertyExtensions
	{
		/// <summary>
		/// Returns true if the property is indexer.
		/// </summary>
		public static bool IsIndexer(this IProperty property)
		{
			return property.Parameters.Count > 0;
		}
	}
}