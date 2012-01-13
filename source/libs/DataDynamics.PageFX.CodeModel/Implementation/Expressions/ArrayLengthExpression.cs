namespace DataDynamics.PageFX.CodeModel
{
    public class ArrayLengthExpression : Expression, IArrayLengthExpression
    {
        #region Costructors
        public ArrayLengthExpression(IExpression array)
        {
            _array = array;
        }
        #endregion

        #region IArrayLengthExpression Members
        public IExpression Array
        {
            get { return _array; }
            set { _array = value; }
        }
        private IExpression _array;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return SystemTypes.Int32; }
        }
        #endregion

        #region Object Override Methods
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IArrayLengthExpression;
            if (e == null) return false;
            if (!Equals(e.Array, _array)) return false;
            return true;
        }

        private static readonly int _hs = ".Length".GetHashCode();

        public override int GetHashCode()
        {
            if (_array != null)
                return _array.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion
    }
}