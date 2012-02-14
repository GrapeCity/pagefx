using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class StackAllocateExpression : Expression, IStackAllocateExpression
    {
        #region IStackAllocateExpression Members
        public IType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private IType _type;

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
            get { return new ICodeNode[] {_expression}; }
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return _type; }
        }
        #endregion

        #region Object Override Members
        #endregion
    }
}