using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeProvider
{
    partial class CodeProviderImpl
    {
        public IEnumerable<IInstruction> NewArray(IType elemType)
        {
            //NOTE:
            //array size must be on stack [..., size]
            //stack transition: ..., size -> arr

            var code = new AbcCode(_abc);
            code.NewArray(elemType);

            _body.Flags |= AbcBodyFlags.HasNewArrayInstructions;
            
            return code;
        }

        public IEnumerable<IInstruction> SetArrayElem(IType elemType)
        {
            EnsureType(elemType);
            var code = new AbcCode(_abc);
            code.SetArrayElem(false);
            return code;
        }

        public IEnumerable<IInstruction> GetArrayElem(IType elemType)
        {
            //stack [arr, index]
            EnsureType(elemType);
            var code = new AbcCode(_abc);
            code.GetArrayElem(elemType, false);
            return code;
        }

        public IEnumerable<IInstruction> GetArrayLength()
        {
            var code = new AbcCode(_abc);
            code.Call(ArrayMethodId.GetLength);
            return code;
        }
    }
}