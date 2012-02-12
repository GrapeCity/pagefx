using System.Linq;

namespace DataDynamics.PageFX.CLI.Metadata
{
    /// <summary>
    /// Represents Metadata Table.
    /// </summary>
    public sealed class MdbTable
    {
        #region Fields
        readonly MdbColumnList _columns = new MdbColumnList();
        string _desc;
        readonly MdbTableId _id;
        int _rowCount;
        int _rowSize;
        int _size;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the table id.
        /// </summary>
        public MdbTableId Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string Name
        {
            get { return _id.ToString(); }
        }

        /// <summary>
        /// Gets the table description.
        /// </summary>
        public string Description
        {
            get { return _desc; }
        }

        /// <summary>
        /// Gets or sets table offset.
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// Gets the table columns
        /// </summary>
        public MdbColumnList Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Gets the number of rows in this table.
        /// </summary>
        public int RowCount
        {
            get { return _rowCount; }
            internal set { _rowCount = value; }
        }

        /// <summary>
        /// Gets the size of row in bytes.
        /// </summary>
        public int RowSize
        {
            get { return _rowSize; }
            internal set { _rowSize = value; }
        }

        /// <summary>
        /// Gets the table size in bytes
        /// </summary>
        public int Size
        {
            get { return _size; }
            internal set { _size = value; }
        }

        public bool IsSorted { get; internal set; }

        /// <summary>
        /// Row cache
        /// </summary>
        internal MdbRow[] Rows { get; set; }
        #endregion

        #region Constructors
        internal MdbTable(MdbTableId id, params MdbColumn[] columns)
        {
        	_id = id;
        	foreach (var col in columns.Select(c => c.Clone()))
        	{
        		col.TableId = id;
        		_columns.Add(col);
        	}
        }

    	#endregion

        #region Object Overrides
        public override string ToString()
        {
            return string.Format("Table({0}, Rows = {1}, Size = {2}, RowSize = {3})",
                                 Name, _rowCount, _size, _rowSize);
        }
        #endregion
    }
}