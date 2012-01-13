using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public interface IThisReferenceExpression : IExpression
    {
        IType Type { get; set; }
    }

    public interface IBaseReferenceExpression : IExpression
    {
        IType Type { get; set; }
    }

    public interface IArgumentReferenceExpression : IExpression
    {
        IParameter Argument { get; set; }
    }

    public interface IMemberReferenceExpression : IExpression
    {
        IExpression Target { get; set; }
        ITypeMember Member { get; set; }
    }

    public interface IFieldReferenceExpression : IMemberReferenceExpression
    {
        IField Field { get; set; }
    }

    public interface IPropertyReferenceExpression : IMemberReferenceExpression
    {
        IProperty Property { get; set; }
    }

    public interface IMethodReferenceExpression : IMemberReferenceExpression
    {
        IMethod Method { get; set; }
    }

    public interface IEventReferenceExpression : IMemberReferenceExpression
    {
        IEvent Event { get; set; }
    }

    public interface ITypeReferenceExpression : IExpression
    {
        IType Type { get; set; }
    }

    public interface IVariableDeclarationExpression : IExpression
    {
        IVariable Variable { get; set; }
    }

    public interface IVariableReferenceExpression : IExpression
    {
        IVariable Variable { get; set; }
    }

    public interface ITypeReferenceProvider
    {
        IEnumerable<IType> GetTypeReferences();
    }
}