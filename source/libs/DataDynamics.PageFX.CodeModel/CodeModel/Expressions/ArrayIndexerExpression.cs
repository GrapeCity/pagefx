using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel.Expressions
{
    public sealed class ArrayIndexerExpression : Expression, IArrayIndexerExpression
    {
    	public ArrayIndexerExpression()
        {
        }

        public ArrayIndexerExpression(IExpression arr, IExpression index)
        {
            Array = arr;
            _index.Add(index);
        }

    	public IExpression Array { get; set; }

    	public IExpressionCollection Index
        {
            get { return _index; }
        }
        private readonly ExpressionCollection _index = new ExpressionCollection();

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] { Array, _index }; }
        }

    	public override IType ResultType
        {
            get
            {
                if (Array == null) return null;
                var type = Array.ResultType as IArrayType;
                if (type == null) return null;
                return type.ElementType;
            }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IArrayIndexerExpression;
            if (e == null) return false;
            //if (e.ElementType != _elementType) return false;
            if (!Equals(e.Array, Array)) return false;
            if (!Equals(e.Index, _index)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new object[]{Array, _index}.EvalHashCode();
        }
    }
}