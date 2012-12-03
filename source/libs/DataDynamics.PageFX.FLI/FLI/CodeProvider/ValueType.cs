using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AvmCodeProvider
    {
        public IInstruction[] InitObject(IType type)
        {
            var code = new AbcCode(_abc);
            code.InitObject(type);
            return code.ToArray();
        }

        public IInstruction[] CopyValue(IType type)
        {
            var code = new AbcCode(_abc);
            code.CopyValue(type);
            if (code.Count > 0)
                return code.ToArray();
            return null;
        }

        public bool HasCopy(IType type)
        {
            return InternalTypeExtensions.HasCopy(type);
        }

        //stack transition: ..., this, value -> ...
        public IInstruction[] CopyToThis(IType valueType)
        {
            var code = new AbcCode(_abc);
            code.CopyFrom(valueType);
            return code.ToArray();
        }
    }
}