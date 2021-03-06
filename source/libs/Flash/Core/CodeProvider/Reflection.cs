using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeProvider
{
    internal partial class CodeProviderImpl
    {
        public IEnumerable<IInstruction> TypeOf(IType type)
        {
            var code = new AbcCode(_abc);
            code.TypeOf(type);
            return code;
        }

        public IEnumerable<IInstruction> SizeOf(IType type)
        {
            EnsureType(type);
            var code = new AbcCode(_abc);
            //TODO:
            return code;
        }
    }
}