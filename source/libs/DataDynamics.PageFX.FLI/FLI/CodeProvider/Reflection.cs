using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AvmCodeProvider
    {
        public IInstruction[] TypeOf(IType type)
        {
            var code = new AbcCode(_abc);
            code.TypeOf(type);
            return code.ToArray();
        }

        public IInstruction[] SizeOf(IType type)
        {
            EnsureType(type);
            var code = new AbcCode(_abc);
            //TODO:
            return code.ToArray();
        }
    }
}