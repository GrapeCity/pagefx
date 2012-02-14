using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ConditionExpression : Expression, IConditionExpression
    {
        #region IConditionExpression Members
        public IExpression Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }
        private IExpression _condition;

        public IExpression TrueExpression
        {
            get { return _true; }
            set { _true = value; }
        }
        private IExpression _true;

        public IExpression FalseExpression
        {
            get { return _false; }
            set { _false = value; }
        }
        private IExpression _false;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { _condition, _true, _false }; }
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get
            {
                if (_true is IConstantExpression)
                {
                    if (_false != null)
                        return _false.ResultType;
                }
                return _true.ResultType;
            }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IConditionExpression;
            if (e == null) return false;
            if (!Equals(e.Condition, _condition)) return false;
            if (!Equals(e.TrueExpression, _true)) return false;
            if (!Equals(e.FalseExpression, _false)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_condition, _true, _true);
        }
        #endregion
    }
}