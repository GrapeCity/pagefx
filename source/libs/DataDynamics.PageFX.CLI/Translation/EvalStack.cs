//NOTE: Stack values can be vars, args, fields, array elements, metadata tokens

using System.Collections.Generic;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.Values;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal sealed class EvalStack : Stack<EvalItem>
    {
        public EvalStack Clone()
        {
            var s = new EvalStack();
            var arr = ToArray();
            for (int i = arr.Length - 1; i >= 0; i--)
                s.Push(arr[i]);
            return s;
        }

        public Instruction Pop(IMethod method, Instruction instruction)
        {
            int n = CIL.GetPopCount(method, instruction);
            //NOTE:
            //Special case for handler blocks
            //It can be pop, stloc instructions
            if (n == 1 && Count == 0)
            {
                return instruction;
            }

            var last = instruction;
            while (n-- > 0)
            {
                var v = Pop();
                last = v.Last;
            }
            return last;
        }

        public void Push(Instruction instruction, Instruction last)
        {
            int n = CIL.GetPushCount(instruction);
            while (n-- > 0)
            {
                Push(new EvalItem(instruction, last));
            }
        }

        public void Push(Instruction instr, IValue value)
        {
            Push(new EvalItem(instr, value));
        }

        public void PushResult(Instruction instr, IType type)
        {
            Push(instr, new ComputedValue(type));
        }
    }
}