using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.IO;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
    /// <summary>
    /// Represents row in metadata table.
    /// </summary>
    internal sealed class MetadataRow : IReadOnlyList<MetadataCell>
    {
	    private readonly MetadataCell[] _cells;

	    /// <summary>
	    /// Gets the index of this row.
	    /// </summary>
	    public int Index { get; private set; }

		/// <summary>
		/// Gets columns/cells count.
		/// </summary>
	    public int Count
	    {
			get { return _cells.Length; }
	    }

	    public MetadataCell this[int index]
        {
            get { return _cells[index]; }
        }

	    public MetadataCell this[MetadataColumn column]
        {
            get
            {
	            if (column == null)
					throw new ArgumentNullException("column");
	            return this[column.Index];
            }
        }

	    internal MetadataRow(MetadataReader metadata, MetadataTable table, BufferedBinaryReader reader, int index)
	    {
		    Index = index;

			int n = table.Columns.Count;
		    _cells = new MetadataCell[n];

			for (int i = 0; i < n; i++)
		    {
			    var column = table.Columns[i];
				Debug.Assert(column.Size != 0);
			    uint value = column.Size == 2 ? reader.ReadUInt16() : reader.ReadUInt32();

			    object data = null;
			    if (column.Type == ColumnType.CodedIndex)
			    {
				    value = column.CodedIndex.Decode(value);
			    }
			    else
			    {
				    data = GetCellData(metadata, column, value);
			    }

			    _cells[i] = new MetadataCell(column, value, data);
		    }
	    }

	    public override string ToString()
	    {
		    return this.Join("Row(", ")", ", ", x => x.ToString());
        }

		public IEnumerator<MetadataCell> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return GetEnumerator();
	    }

	    private static object GetCellData(MetadataReader metadata, MetadataColumn column, uint index)
	    {
		    switch (column.Type)
		    {
			    case ColumnType.StringIndex:
				    return metadata.FetchString(index);

			    case ColumnType.BlobIndex:
				    return metadata.FetchBlob(index);

			    case ColumnType.GuidIndex:
				    return metadata.FetchGuid(index);

			    default:
				    return null;
		    }
	    }
    }
}