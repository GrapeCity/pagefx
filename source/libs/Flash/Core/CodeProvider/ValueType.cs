using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Core.CodeProvider
{
    internal partial class CodeProviderImpl
    {
        public IEnumerable<IInstruction> InitObject(IType type)
        {
            var code = new AbcCode(_abc);
            code.InitObject(type);
            return code;
        }

        public IEnumerable<IInstruction> CopyValue(IType type)
        {
            var code = new AbcCode(_abc);
            code.CopyValue(type);
            return code.Count > 0 ? code : null;
        }

        public bool HasCopy(IType type)
        {
            return type.SupportsCopyMethods();
        }

        //stack transition: ..., this, value -> ...
        public IEnumerable<IInstruction> CopyToThis(IType valueType)
        {
            var code = new AbcCode(_abc);
            code.CopyFrom(valueType);
            return code;
        }
    }
}