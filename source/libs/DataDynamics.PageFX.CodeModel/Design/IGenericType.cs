namespace DataDynamics.PageFX.CodeModel
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