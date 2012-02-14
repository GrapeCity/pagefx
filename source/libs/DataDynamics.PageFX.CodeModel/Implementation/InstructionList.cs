using System.Collections.Generic;
using System.Text;

namespace DataDynamics.PageFX.CodeModel
{
    public class InstructionList<T> : List<T>, IInstructionList
        where T : class, IInstruction
    {
        #region IInstructionList Members
        public int GetOffsetIndex(int offset)
        {
            int n = Count;
            if (n <= 0) return -1;

            int max = this[n - 1].Offset;
            if (offset >= max)
                return n - 1;

            int i = Algorithms.BinarySearch<T>(this, 0, n, item => item.Offset - offset);
            return i;
        }

        public T FindByOffset(int offset)
        {
            int i = GetOffsetIndex(offset);
            if (i >= 0)
                return this[i];
            return null;
        }

        public void TranslateOffsets()
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
                this[i].TranslateOffsets(this);
        }

        IInstruction IInstructionList.FindByOffset(int offset)
        {
            return FindByOffset(offset);
        }
        #endregion

        #region IList<IInstruction> Members
        int IList<IInstruction>.IndexOf(IInstruction item)
        {
            return IndexOf((T)item);
        }

        void IList<IInstruction>.Insert(int index, IInstruction item)
        {
            Insert(index, (T)item);
        }

        IInstruction IList<IInstruction>.this[int index]
        {
            get { return this[index]; }
            set { this[index] = value as T; }
        }
        #endregion

        #region ICollection<IInstruction> Members
        void ICollection<IInstruction>.Add(IInstruction item)
        {
            Add((T)item);
        }

        bool ICollection<IInstruction>.Contains(IInstruction item)
        {
            return Contains((T)item);
        }

        void ICollection<IInstruction>.CopyTo(IInstruction[] array, int arrayIndex)
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
                array[i + arrayIndex] = this[i];
        }

        bool ICollection<IInstruction>.Remove(IInstruction item)
        {
            return Remove((T)item);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region IEnumerable<IInstruction> Members
        IEnumerator<IInstruction> IEnumerable<IInstruction>.GetEnumerator()
        {
        	foreach (var item in this)
        	{
        		yield return item;
        	}
        }
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (var instruction in this)
            {
                s.AppendLine(instruction.ToString());
            }
            return s.ToString();
        }
        #endregion
    }
}