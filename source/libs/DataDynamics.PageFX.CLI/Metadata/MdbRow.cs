using System.Text;
using System.Linq;

namespace DataDynamics.PageFX.CLI.Metadata
{
    /// <summary>
    /// Represents row in MDB table.
    /// </summary>
    public sealed class MdbRow
    {
        /// <summary>
        /// Gets the index of this row.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }
        private readonly int _index;

        /// <summary>
        /// Gets an array of row cells.
        /// </summary>
        public MdbCell[] Cells
        {
            get { return _cells; }
        }

	    private readonly MdbCell[] _cells;

        public MdbCell this[int index]
        {
            get { return _cells[index]; }
        }

        public MdbCell this[MdbColumn column]
        {
            get { return _cells[column.Index]; }
        }

        public MdbCell? this[string name]
        {
            get { return _cells.FirstOrDefault(cell => cell.Name == name); }
        }

        internal MdbRow(int index, MdbCell[] cells)
        {
            _index = index;
            _cells = cells;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append("Row(");
            for (int i = 0; i < _cells.Length; ++i)
            {
                if (i > 0) s.Append(", ");
                s.Append(_cells[i].ToString());
            }
            s.Append(")");
            return s.ToString();
        }
    }
}