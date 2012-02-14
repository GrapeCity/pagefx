using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class IfStatement : Statement, IIfStatement
    {
        #region IIfStatement Members
        public IExpression Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        private IExpression _condition;

        public IStatementCollection Then
        {
            get { return _then ?? (_then = new StatementCollection(this)); }
        	set
            {
                if (value == null)
                {
                    if (_then != null)
                        _then.Clear();
                }
                else
                {
                    _then = value;
                    _then.ParentStatement = this;
                }
            }
        }
        private IStatementCollection _then;
        
        public IStatementCollection Else
        {
            get { return _else ?? (_else = new StatementCollection(this)); }
        	set
            {
                if (value == null)
                {
                    if (_else != null)
                        _else.Clear();
                }
                else
                {
                    _else = value;
                    _else.ParentStatement = this;
                }
            }
        }
        private IStatementCollection _else;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { _condition, _then, _else }; }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IIfStatement;
            if (s == null) return false;
            if (!Equals(s.Condition, _condition)) return false;
            if (!Equals(s.Then, _then)) return false;
            if (!Equals(s.Else, _else)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_condition, _then, _else);
        }
        #endregion
    }
}