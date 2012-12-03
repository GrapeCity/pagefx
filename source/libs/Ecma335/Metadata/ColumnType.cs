namespace DataDynamics.PageFX.Ecma335.Metadata
{
	/// <summary>
	/// Enumerates MDB column types.
	/// </summary>
	public enum ColumnType
	{
		/// <summary>
		/// 4 byte constant.
		/// </summary>
		Int32,

		/// <summary>
		/// 2-byte constant.
		/// </summary>
		Int16,

		/// <summary>
		/// Index in strings heap
		/// </summary>
		StringIndex,

		/// <summary>
		/// Index in blob heap
		/// </summary>
		BlobIndex,

		/// <summary>
		/// Index in #GUID heap
		/// </summary>
		GuidIndex,

		/// <summary>
		/// Simple table index.
		/// </summary>
		SimpleIndex,

		/// <summary>
		/// Coded table index.
		/// </summary>
		CodedIndex
	}
}