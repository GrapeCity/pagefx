using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel.Expressions
{
    public class NewObjectExpression : Expression, INewObjectExpression, ITypeReferenceProvider
    {
    	public IMethod Constructor { get; set; }

    	public IType ObjectType { get; set; }

    	public IExpressionCollection Arguments
        {
            get { return _args; }
        }
        private readonly ExpressionCollection _args = new ExpressionCollection();

    	public override IType ResultType
        {
            get { return ObjectType; }
        }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {_args}; }
        }

    	public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { ObjectType };
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as INewObjectExpression;
            if (e == null) return false;
            if (e.ObjectType != ObjectType) return false;
            if (e.Constructor != Constructor) return false;
            if (!Equals(e.Arguments, _args)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return new object[]{ ObjectType, Constructor, _args}.EvalHashCode();
        }
    }
}