using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel.Expressions;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.Common.CodeModel.Statements
{
    public class LoopStatement : Statement, ILoopStatement
    {
        public LoopStatement()
        {
            _body = new StatementCollection(this);
        }

    	public LoopType LoopType { get; set; }

    	public IExpression Condition { get; set; }

    	public IStatementCollection Body
        {
            get { return _body; }
        }
        private readonly StatementCollection _body;

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Condition, _body }; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return false;
            var s = obj as ILoopStatement;
            if (s == null) return false;
            if (s.LoopType != LoopType) return false;
            if (!Equals(s.Condition, Condition)) return false;
            if (!Equals(s.Body, _body)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return LoopType.GetHashCode() ^ new object[]{Condition, _body}.EvalHashCode();
        }
    }
}