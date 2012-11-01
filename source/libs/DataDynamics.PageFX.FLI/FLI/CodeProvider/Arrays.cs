using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AvmCodeProvider
    {
        public IInstruction[] NewArray(IType elemType)
        {
            //NOTE:
            //array size must be on stack [..., size]
            //stack transition: ..., size -> arr

            var code = new AbcCode(_abc);
            code.NewArray(elemType);

            _body.Flags |= AbcBodyFlags.HasNewArrayInstructions;
            
            return code.ToArray();
        }

        public IInstruction[] SetArrayElem(IType elemType)
        {
            EnsureType(elemType);
            var code = new AbcCode(_abc);
            code.SetArrayElem(false);
            return code.ToArray();
        }

        public IInstruction[] GetArrayElem(IType elemType)
        {
            //stack [arr, index]
            EnsureType(elemType);
            var code = new AbcCode(_abc);
            code.GetArrayElem(elemType, false);
            return code.ToArray();
        }

        public IInstruction[] GetArrayLength()
        {
            var code = new AbcCode(_abc);
            code.Call(ArrayMethodId.GetLength);
            return code.ToArray();
        }
    }
}