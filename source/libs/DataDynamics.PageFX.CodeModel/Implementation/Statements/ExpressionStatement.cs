using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ExpressionStatement : Statement, IExpressionStatement
    {
        #region Constructors
        public ExpressionStatement(IExpression e)
        {
            _expression = e;
        }
        #endregion

        #region IExpressionStatement Members
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
            var s = obj as IExpressionStatement;
            if (s == null) return false;
            if (!Equals(s.Expression, _expression)) return false;
            return true;
        }

        private static readonly int _hs = "ExpressionStatement".GetHashCode();

        public override int GetHashCode()
        {
            if (_expression != null)
                return _expression.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion
    }
}