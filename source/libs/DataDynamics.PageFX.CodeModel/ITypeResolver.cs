using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common
{
    public interface ITypeResolver
    {
        IType Resolve(string fullname);
    }
}