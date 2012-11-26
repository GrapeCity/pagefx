using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel.Expressions
{
    public sealed class StackAllocateExpression : Expression, IStackAllocateExpression
    {
    	public IType Type { get; set; }

    	public IExpression Expression { get; set; }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {Expression}; }
        }

    	public override IType ResultType
        {
            get { return Type; }
        }
    }
}