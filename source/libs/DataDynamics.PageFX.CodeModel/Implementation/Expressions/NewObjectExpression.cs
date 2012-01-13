using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class NewObjectExpression : Expression, INewObjectExpression, ITypeReferenceProvider
    {
        #region INewObjectExpression Members
        public IMethod Constructor
        {
            get { return _ctor; }
            set { _ctor = value; }
        }
        private IMethod _ctor;

        public IType ObjectType
        {
            get { return _objectType; }
            set { _objectType = value; }
        }
        private IType _objectType;

        public IExpressionCollection Arguments
        {
            get { return _args; }
        }
        private readonly ExpressionCollection _args = new ExpressionCollection();
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return _objectType; }
        }
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_args); }
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { _objectType };
        }
        #endregion

        #region Object Override Methods
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as INewObjectExpression;
            if (e == null) return false;
            if (e.ObjectType != _objectType) return false;
            if (e.Constructor != _ctor) return false;
            if (!Equals(e.Arguments, _args)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_objectType, _ctor, _args);
        }
        #endregion
    }
}