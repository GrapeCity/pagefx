using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeProvider
{
    internal partial class CodeProviderImpl
    {
        public IEnumerable<IInstruction> As(IType type)
        {
            EnsureType(type);
            var code = new AbcCode(_abc);
            code.As(type);
            return code;
        }

        public IEnumerable<IInstruction> Cast(IType source, IType target, bool checkOverflow)
        {
            EnsureType(target);
            var code = new AbcCode(_abc);
            code.Cast(source, target, checkOverflow);
            return code;
        }

        public IEnumerable<IInstruction> BoxPrimitive(IType type)
        {
            var code = new AbcCode(_abc);
            code.BoxPrimitive(type);
            return code;
        }

        public IEnumerable<IInstruction> Box(IType type)
        {
            EnsureType(type);
            var code = new AbcCode(_abc);
            code.Box(type);
            return code;
        }

        public IEnumerable<IInstruction> Unbox(IType type)
        {
            EnsureType(type);
            var code = new AbcCode(_abc);
            code.Unbox(type);
            return code;
        }
    }
}