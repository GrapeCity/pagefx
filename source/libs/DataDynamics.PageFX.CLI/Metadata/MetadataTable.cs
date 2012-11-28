using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CLI.Metadata
{
    /// <summary>
    /// Represents metadata table.
    /// </summary>
    public sealed class MetadataTable
    {
	    /// <summary>
    	/// Gets the table id.
    	/// </summary>
    	public TableId Id { get; private set; }

    	/// <summary>
        /// Gets the table name.
        /// </summary>
        public string Name
        {
            get { return Id.ToString(); }
        }

    	/// <summary>
    	/// Gets the table description.
    	/// </summary>
    	public string Description { get; set; }

    	/// <summary>
        /// Gets or sets table offset.
        /// </summary>
        public long Offset { get; set; }

	    /// <summary>
	    /// Gets the table columns
	    /// </summary>
	    public MetadataColumnCollection Columns { get; private set; }

	    /// <summary>
    	/// Gets the number of rows in this table.
    	/// </summary>
    	public int RowCount { get; internal set; }

    	/// <summary>
    	/// Gets the size of row in bytes.
    	/// </summary>
    	public int RowSize { get; internal set; }

    	/// <summary>
    	/// Gets the table size in bytes
    	/// </summary>
    	public int Size { get; internal set; }

    	public bool IsSorted { get; internal set; }

        /// <summary>
        /// Row cache
        /// </summary>
        internal MetadataRow[] Rows { get; set; }

		// Lookup by reference key
		internal Dictionary<int, IList<int>> Lookup = new Dictionary<int, IList<int>>();
	    internal int LastLookupRowIndex;

    	internal MetadataTable(TableId id, params MetadataColumn[] columns)
        {
    		Columns = new MetadataColumnCollection();
    		Id = id;
			
        	foreach (var col in columns.Select(c => c.Clone()))
        	{
        		col.TableId = id;
        		Columns.Add(col);
        	}
        }

    	public override string ToString()
        {
            return string.Format("Table({0}, Rows = {1}, Size = {2}, RowSize = {3})",
                                 Name, RowCount, Size, RowSize);
        }
    }
}