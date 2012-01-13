using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class MemoryCopyStatement : Statement, IMemoryCopyStatement
    {
        #region IMemoryCopyStatement Members
        public IExpression Destination
        {
            get { return _dst; }
            set { _dst = value; }
        }
        private IExpression _dst;

        public IExpression Source
        {
            get { return _src; }
            set { _src = value; }
        }
        private IExpression _src;

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
            get { return CMHelper.Enumerate(_dst, _src, _size); }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IMemoryCopyStatement;
            if (s == null) return false;
            if (!Equals(s.Destination, _dst)) return false;
            if (!Equals(s.Source, _src)) return false;
            if (!Equals(s.Size, _size)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_dst, _src, _size);
        }
        #endregion
    }
}