using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.CodeProvider
{
    internal partial class CodeProviderImpl
    {
        public IEnumerable<IInstruction> Op(BinaryOperator op, IType left, IType right, IType result, bool checkOverflow)
        {
            var code = new AbcCode(_abc);
            code.Op(op, left, right, result, checkOverflow);
            return code;
        }
        
        public IEnumerable<IInstruction> Op(UnaryOperator op, IType type, bool checkOverflow)
        {
            var code = new AbcCode(_abc);
            code.Op(op, type, checkOverflow);
            return code;
        }

        public bool SupportIncrementOperators
        {
            get { return true; }
        }

        public IInstruction Increment(IType type)
        {
            if (type.Is(SystemTypeCode.Int32))
                return new Instruction(InstructionCode.Increment_i);
            return new Instruction(InstructionCode.Increment);
        }

        public IInstruction Decrement(IType type)
        {
            if (type.Is(SystemTypeCode.Int32))
                return new Instruction(InstructionCode.Decrement_i);
            return new Instruction(InstructionCode.Decrement);
        }
    }
}