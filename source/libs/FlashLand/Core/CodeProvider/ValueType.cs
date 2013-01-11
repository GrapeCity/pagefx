using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeProvider
{
    internal partial class CodeProviderImpl
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
            return type.SupportsCopyMethods();
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