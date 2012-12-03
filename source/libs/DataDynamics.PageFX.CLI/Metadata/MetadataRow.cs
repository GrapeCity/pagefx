using System.Text;
using System.Linq;

namespace DataDynamics.PageFX.Ecma335.Metadata
{
    /// <summary>
    /// Represents row in metadata table.
    /// </summary>
    public sealed class MetadataRow
    {
	    /// <summary>
	    /// Gets the index of this row.
	    /// </summary>
	    public int Index { get; private set; }

	    /// <summary>
	    /// Gets an array of row cells.
	    /// </summary>
	    public MetadataCell[] Cells { get; private set; }

	    internal object Object { get; set; }

	    public MetadataCell this[int index]
        {
            get { return Cells[index]; }
        }

        public MetadataCell this[MetadataColumn column]
        {
            get { return Cells[column.Index]; }
        }

        public MetadataCell? this[string name]
        {
            get { return Cells.FirstOrDefault(cell => cell.Name == name); }
        }

        internal MetadataRow(int index, MetadataCell[] cells)
        {
            Index = index;
            Cells = cells;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append("Row(");
            for (int i = 0; i < Cells.Length; ++i)
            {
                if (i > 0) s.Append(", ");
                s.Append(Cells[i].ToString());
            }
            s.Append(")");
            return s.ToString();
        }
    }
}