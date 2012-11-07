//NOTE: Stack values can be vars, args, fields, array elements, metadata tokens

using System.Collections.Generic;
using System.Text;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    #region EvalItem
    internal struct EvalItem
    {
        public IValue value;
        public Instruction instruction;
        public Instruction last;
        
        public EvalItem(Instruction instruction)
        {
            this.instruction = instruction;
            value = null;
            last = null;
        }

        public EvalItem(Instruction instruction, IValue value)
        {
            this.instruction = instruction;
            this.value = value;
            last = null;
        }

        public EvalItem(Instruction instruction, Instruction last)
        {
            this.instruction = instruction;
            this.last = last;
            value = null;
        }

        public IType Type
        {
            get { return value == null ? null : value.Type; }
        }

        public bool IsNull
        {
            get
            {
                var c = value as ConstValue;
                if (c != null) return c.value == null;
                return false;
            }
        }

        public bool IsToken
        {
            get { return value.Kind == ValueKind.Token; }
        }

        public bool Is(InstructionCode c)
        {
            return instruction.Code == c;
        }

        public bool IsInstance
        {
            get { return instruction.Code == InstructionCode.Isinst; }
        }

        public bool IsTypeToken
        {
            get
            {
                if (value != null)
                {
                    var v = value as TokenValue;
                    if (v == null) return false;
                    return v.member is IType;
                }
                else
                {
                    var i = instruction;
                    if (i == null) return false;
                    if (i.Code != InstructionCode.Ldtoken) return false;
                    return i.Value is IType;
                }
            }
        }

        public bool IsPointer
        {
            get { return value.IsPointer; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (instruction != null)
            {
                sb.Append(instruction.ToString());
                if (value != null)
                    sb.Append("\n");
            }
            if (value != null)
            {
                sb.Append(value.ToString());
            }
            return sb.ToString();
        }
    }
    #endregion

    internal class EvalStack : Stack<EvalItem>
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
                last = v.last;
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