namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface IGenericInstance : IType
    {
        ITypeCollection GenericArguments { get; }
    }
}