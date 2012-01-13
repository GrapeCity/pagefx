using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class MemoryInitializeStatement : Statement, IMemoryInitializeStatement
    {
        #region IMemoryInitializeStatement Members
        public IExpression Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private IExpression _value;

        public IExpression Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        private IExpression _offset;

        public IExpression Size
        {
            get { return _size; }
            set { _size = value; }
        }
        private IExpression _size;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_value, _offset, _size); }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IMemoryInitializeStatement;
            if (s == null) return false;
            if (!Equals(s.Value, _value)) return false;
            if (!Equals(s.Offset, _offset)) return false;
            if (!Equals(s.Size, _size)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_value, _offset, _size);
        }
        #endregion
    }
}