using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ArrayIndexerExpression : Expression, IArrayIndexerExpression
    {
        #region Constructors
        public ArrayIndexerExpression()
        {
        }

        public ArrayIndexerExpression(IExpression arr, IExpression index)
        {
            _arr = arr;
            _index.Add(index);
        }
        #endregion

        #region IArrayIndexerExpression Members
        public IExpression Array
        {
            get { return _arr; }
            set { _arr = value; }
        }
        private IExpression _arr;

        public IExpressionCollection Index
        {
            get { return _index; }
        }
        private readonly ExpressionCollection _index = new ExpressionCollection();
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_arr, _index); }
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get
            {
                if (_arr == null) return null;
                var type = _arr.ResultType as IArrayType;
                if (type == null) return null;
                return type.ElementType;
            }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IArrayIndexerExpression;
            if (e == null) return false;
            //if (e.ElementType != _elementType) return false;
            if (!Equals(e.Array, _arr)) return false;
            if (!Equals(e.Index, _index)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_arr, _index);
        }
        #endregion
    }
}