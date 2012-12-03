namespace DataDynamics.PageFX.Common.TypeSystem
{
    public interface ITypeResolver
    {
        IType Resolve(string fullname);
    }
}