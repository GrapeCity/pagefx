namespace DataDynamics.PageFX.CodeModel.TypeSystem
{
    public interface IGenericType : IType
    {
        IGenericParameterCollection GenericParameters { get; }
    }

    public interface IGenericInstance : IType
    {
        new IGenericType Type { get; set; }
        ITypeCollection GenericArguments { get; }
    }
}