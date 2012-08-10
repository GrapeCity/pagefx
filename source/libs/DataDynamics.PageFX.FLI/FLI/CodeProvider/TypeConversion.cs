using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AvmCodeProvider
    {
        public IInstruction[] As(IType type)
        {
            EnsureType(type);
            var code = new AbcCode(_abc);
            code.As(type);
            return code.ToArray();
        }

        public IInstruction[] Cast(IType source, IType target, bool checkOverflow)
        {
            EnsureType(target);
            var code = new AbcCode(_abc);
            code.Cast(source, target, checkOverflow);
            return code.ToArray();
        }

        public IInstruction[] BoxPrimitive(IType type)
        {
            var code = new AbcCode(_abc);
            code.BoxPrimitive(type);
            return code.ToArray();
        }

        public IInstruction[] Box(IType type)
        {
            EnsureType(type);
            var code = new AbcCode(_abc);
            code.Box(type);
            return code.ToArray();
        }

        public IInstruction[] Unbox(IType type)
        {
            EnsureType(type);
            var code = new AbcCode(_abc);
            code.Unbox(type);
            return code.ToArray();
        }

        public IInstruction[] UnwrapString()
        {
	        return new IInstruction[0];
        }
    }
}