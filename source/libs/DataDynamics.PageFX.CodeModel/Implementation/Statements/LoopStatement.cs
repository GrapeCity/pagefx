using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class LoopStatement : Statement, ILoopStatement
    {
        public LoopStatement()
        {
            _body = new StatementCollection(this);
        }

        #region ILoopStatement Members
        public LoopType LoopType
        {
            get { return _loopType; }
            set { _loopType = value; }
        }
        private LoopType _loopType;

        public IExpression Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        private IExpression _condition;
        
        public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_condition, _body); }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return false;
            var s = obj as ILoopStatement;
            if (s == null) return false;
            if (s.LoopType != _loopType) return false;
            if (!Equals(s.Condition, _condition)) return false;
            if (!Equals(s.Body, _body)) return false;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _loopType.GetHashCode() ^ Object2.GetHashCode(_condition, _body);
        }
        #endregion
    }
}