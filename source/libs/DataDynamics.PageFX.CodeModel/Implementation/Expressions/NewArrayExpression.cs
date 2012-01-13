using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class NewArrayExpression : Expression, INewArrayExpression, ITypeReferenceProvider
    {
        #region IArrayCreateExpression Members
        public IType ElementType
        {
            get { return _elementType; }
            set { _elementType = value; }
        }
        private IType _elementType;

        public IExpressionCollection Dimensions
        {
            get { return _dim; }
        }
        private readonly ExpressionCollection _dim = new ExpressionCollection();

        public IExpressionCollection Initializers
        {
            get { return _init; }
        }
        private readonly ExpressionCollection _init = new ExpressionCollection();
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_dim, _init); }
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get
            {
                return TypeFactory.MakeArray(_elementType, new ArrayDimensionCollection());
            }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as INewArrayExpression;
            if (e == null) return false;
            if (e.ElementType != _elementType) return false;
            if (!Equals(e.Dimensions, _dim)) return false;
            if (!Equals(e.Initializers, _init)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            if (_elementType != null)
                h ^= _elementType.GetHashCode();
            h ^= _dim.GetHashCode();
            h ^= _init.GetHashCode();
            return h;
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { _elementType, ResultType };
        }
        #endregion
    }
}