namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface IGenericType : IType
    {
        IGenericParameterCollection GenericParameters { get; }
    }

    public interface IGenericInstance : IType
    {
        new IGenericType Type { get; }
        ITypeCollection GenericArguments { get; }
    }
}