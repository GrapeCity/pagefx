using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class ThrowExceptionStatement : Statement, IThrowExceptionStatement
    {
        #region Constructors
        public ThrowExceptionStatement()
        {
            
        }

        public ThrowExceptionStatement(IExpression e)
        {
            _expression = e;
        }
        #endregion

        #region IThrowExceptionStatement Members
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
            var s = obj as IThrowExceptionStatement;
            if (s == null) return false;
            if (!Equals(s.Expression, _expression)) return false;
            return true;
        }

        private static readonly int _hs = typeof(IThrowExceptionStatement).GetHashCode();

        public override int GetHashCode()
        {
            if (_expression != null)
                return _expression.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion
    }
}