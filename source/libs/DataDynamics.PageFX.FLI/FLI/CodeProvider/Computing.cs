using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AvmCodeProvider
    {
        public IInstruction[] Op(BinaryOperator op, IType left, IType right, IType result, bool checkOverflow)
        {
            var code = new AbcCode(_abc);
            code.Op(op, left, right, result, checkOverflow);
            return code.ToArray();
        }
        
        public IInstruction[] Op(UnaryOperator op, IType type, bool checkOverflow)
        {
            var code = new AbcCode(_abc);
            code.Op(op, type, checkOverflow);
            return code.ToArray();
        }

        public bool SupportIncrementOperators
        {
            get { return true; }
        }

        public IInstruction Increment(IType type)
        {
            if (type == SystemTypes.Int32)
                return new Instruction(InstructionCode.Increment_i);
            return new Instruction(InstructionCode.Increment);
        }

        public IInstruction Decrement(IType type)
        {
            if (type == SystemTypes.Int32)
                return new Instruction(InstructionCode.Decrement_i);
            return new Instruction(InstructionCode.Decrement);
        }
    }
}