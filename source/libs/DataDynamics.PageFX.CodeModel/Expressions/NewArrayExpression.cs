using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel.Expressions
{
    public sealed class NewArrayExpression : Expression, INewArrayExpression, ITypeReferenceProvider
    {
    	public IType ElementType { get; set; }

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

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { _dim, _init }; }
        }

    	public override IType ResultType
        {
            get { return TypeFactory.MakeArray(ElementType, new ArrayDimensionCollection()); }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as INewArrayExpression;
            if (e == null) return false;
            if (e.ElementType != ElementType) return false;
            if (!Equals(e.Dimensions, _dim)) return false;
            if (!Equals(e.Initializers, _init)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            if (ElementType != null)
                h ^= ElementType.GetHashCode();
            h ^= _dim.GetHashCode();
            h ^= _init.GetHashCode();
            return h;
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { ElementType, ResultType };
        }
    }
}