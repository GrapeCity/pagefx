using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel.Expressions
{
    public sealed class ConditionExpression : Expression, IConditionExpression
    {
    	public IExpression Condition { get; set; }

    	public IExpression TrueExpression { get; set; }

    	public IExpression FalseExpression { get; set; }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Condition, TrueExpression, FalseExpression }; }
        }

    	public override IType ResultType
        {
            get
            {
                if (TrueExpression is IConstantExpression)
                {
                    if (FalseExpression != null)
                        return FalseExpression.ResultType;
                }
                return TrueExpression.ResultType;
            }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IConditionExpression;
            if (e == null) return false;
            if (!Equals(e.Condition, Condition)) return false;
            if (!Equals(e.TrueExpression, TrueExpression)) return false;
            if (!Equals(e.FalseExpression, FalseExpression)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new []{Condition, TrueExpression, FalseExpression}.EvalHashCode();
        }
    }
}