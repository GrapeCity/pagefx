using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel
{
    public interface ITypeResolver
    {
        IType Resolve(string fullname);
    }
}