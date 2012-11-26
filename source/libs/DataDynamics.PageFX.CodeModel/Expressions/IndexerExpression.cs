using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel.Expressions
{
    public sealed class IndexerExpression : Expression, IIndexerExpression
    {
    	public IndexerExpression()
        {    
        }

        public IndexerExpression(IPropertyReferenceExpression property)
        {
            Property = property;
        }

        public IndexerExpression(IPropertyReferenceExpression property, IExpression index)
        {
            Property = property;
            _index.Add(index);
        }

    	public IPropertyReferenceExpression Property { get; set; }

    	public IExpressionCollection Index
        {
            get { return _index; }
        }
        private readonly ExpressionCollection _index = new ExpressionCollection();

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Property, _index }; }
        }

    	public override IType ResultType
        {
            get { return Property.ResultType; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IIndexerExpression;
            if (e == null) return false;
            if (!Equals(e.Property, Property)) return false;
            if (!Equals(e.Index, _index)) return false;
            return true;
        }

        private static readonly int _hs = typeof(IIndexerExpression).GetHashCode();

        public override int GetHashCode()
        {
            return new object[]{Property, _index}.EvalHashCode() ^ _hs;
        }
    }
}