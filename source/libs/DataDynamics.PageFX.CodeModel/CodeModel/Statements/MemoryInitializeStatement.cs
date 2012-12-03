using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel.Expressions;

namespace DataDynamics.PageFX.Common.CodeModel.Statements
{
    public class MemoryInitializeStatement : Statement, IMemoryInitializeStatement
    {
    	public IExpression Value { get; set; }

    	public IExpression Offset { get; set; }

    	public IExpression Size { get; set; }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Value, Offset, Size }; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IMemoryInitializeStatement;
            if (s == null) return false;
            if (!Equals(s.Value, Value)) return false;
            if (!Equals(s.Offset, Offset)) return false;
            if (!Equals(s.Size, Size)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new []{Value, Offset, Size}.EvalHashCode();
        }
    }
}