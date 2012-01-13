using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class IndexerExpression : Expression, IIndexerExpression
    {
        #region Constructors
        public IndexerExpression()
        {    
        }

        public IndexerExpression(IPropertyReferenceExpression property)
        {
            _property = property;
        }

        public IndexerExpression(IPropertyReferenceExpression property, IExpression index)
        {
            _property = property;
            _index.Add(index);
        }
        #endregion

        #region IIndexerExpression Members
        public IPropertyReferenceExpression Property
        {
            get { return _property; }
            set { _property = value; }
        }
        private IPropertyReferenceExpression _property;

        public IExpressionCollection Index
        {
            get { return _index; }
        }
        private readonly ExpressionCollection _index = new ExpressionCollection();
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_property, _index); }
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return _property.ResultType; }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IIndexerExpression;
            if (e == null) return false;
            if (!Equals(e.Property, _property)) return false;
            if (!Equals(e.Index, _index)) return false;
            return true;
        }

        private static readonly int _hs = typeof(IIndexerExpression).GetHashCode();

        public override int GetHashCode()
        {
            return Object2.GetHashCode(_property, _index) ^ _hs;
        }
        #endregion
    }
}