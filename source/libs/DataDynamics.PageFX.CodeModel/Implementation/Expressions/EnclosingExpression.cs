using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public abstract class EnclosingExpression : Expression, IEnclosingExpression
    {
        #region Constructors

    	protected EnclosingExpression()
        {
        }

    	protected EnclosingExpression(IExpression e)
        {
            _expression = e;
        }

        #endregion

        #region IEnclosingExpression Members
        public IExpression Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }
        private IExpression _expression;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_expression); }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IEnclosingExpression;
            if (e == null) return false;
            if (!Equals(e.Expression, _expression)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            if (_expression != null)
                h ^= _expression.GetHashCode();
            return h;
        }
        #endregion
    }
}