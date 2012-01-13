namespace DataDynamics.PageFX
{
    public class SequencePoint
    {
        public string File { get; set; }

        public int Offset { get; set; }

        public int StartRow { get; set; }

        public int StartColumn { get; set; }

        public int EndRow { get; set; }

        public int EndColumn { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}({2}:{3} - {4}:{5})",
                                 Offset, File,
                                 StartRow, StartColumn,
                                 EndRow, EndColumn);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj" /> parameter is null.</exception><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var p = obj as SequencePoint;
            if (p == null) return false;
            if (p.Offset != Offset) return false;
            if (p.StartRow != StartRow) return false;
            if (p.StartColumn != StartColumn) return false;
            if (p.EndRow != EndRow) return false;
            if (p.EndColumn != EndColumn) return false;
            return string.Compare(p.File, File, true) == 0;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return Algorithms.GetHashCode(Offset, StartRow, StartColumn,
                                          EndRow, EndColumn, File);
        }
    }
}