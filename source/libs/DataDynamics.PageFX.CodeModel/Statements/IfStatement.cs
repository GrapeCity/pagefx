using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class IfStatement : Statement, IIfStatement
    {
    	public IExpression Condition { get; set; }

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

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Condition, _then, _else }; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IIfStatement;
            if (s == null) return false;
            if (!Equals(s.Condition, Condition)) return false;
            if (!Equals(s.Then, _then)) return false;
            if (!Equals(s.Else, _else)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new object[]{Condition, _then, _else}.EvalHashCode();
        }
    }
}