using System;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.IL;

namespace DataDynamics.PageFX.FLI
{
    partial class AvmCodeProvider
    {
        public bool SupportBranchOperator(BranchOperator op)
        {
            return true;
        }

        static IInstruction[] If(InstructionCode code)
        {
            return new IInstruction[] {new Instruction(code, 0)};
        }

        public IInstruction[] Branch(BranchOperator op, IType left, IType right)
        {
            if (op.IsUnary())
            {
                if (op == BranchOperator.Null)
                {
                    var code = new AbcCode(_abc);
                    //NOTE: old code - not working with nullable types
                    //code.PushNull();
                    //code.Add(InstructionCode.Ifeq);
                    code.IsNull(left, right);
                    code.Add(InstructionCode.Iftrue);
                    return code.ToArray();
                }

                if (op == BranchOperator.NotNull)
                {
                    var code = new AbcCode(_abc);
                    //NOTE: old code - not working with nullable types
                    //code.PushNull();
                    //code.Add(InstructionCode.Ifne);
                    code.IsNull(left, right);
                    code.Add(InstructionCode.Iffalse);
                    return code.ToArray();
                }

                if (left.IsDecimalOrInt64())
                {
                    var code = new AbcCode(_abc);
                    bool isTrue = op == BranchOperator.True;
                    var abcOp = _generator.DefineBooleanOperator(left, isTrue);
                    //TODO: Should we enshure not null value onto the stack???
                    //AbcMethod abcOp = _generator.DefineTruthOperator(left, isTrue);
                    //code.LoadStaticReceiver(abcOp);
                    //code.Swap();
                    code.Call(abcOp);
                    code.Add(isTrue ? InstructionCode.Iftrue : InstructionCode.Iffalse);
                    return code.ToArray();
                }
            }
            else if (InternalTypeExtensions.IsDecimalOrInt64(left, right))
            {
                var code = new AbcCode(_abc);
                var opm = _generator.DefineOperator(op, left, right);
                code.Call(opm);
                code.Add(InstructionCode.Iftrue);
                return code.ToArray();
            }

            switch (op)
            {
                case BranchOperator.True:
                    return If(InstructionCode.Iftrue);

                case BranchOperator.False:
                    return If(InstructionCode.Iffalse);

                case BranchOperator.Equality:
                    return If(InstructionCode.Ifeq);

                case BranchOperator.Inequality:
                    return If(InstructionCode.Ifne);

                case BranchOperator.LessThan:
                    return If(InstructionCode.Iflt);

                case BranchOperator.LessThanOrEqual:
                    return If(InstructionCode.Ifle);

                case BranchOperator.GreaterThan:
                    return If(InstructionCode.Ifgt);

                case BranchOperator.GreaterThanOrEqual:
                    return If(InstructionCode.Ifge);

                default:
                    throw new NotSupportedException();
            }
        }

        public IInstruction Branch(int index)
        {
            return new Instruction(InstructionCode.Jump, index);
        }

        public IInstruction Branch()
        {
            return Branch(0);
        }

        public bool IsLabel(IInstruction i)
        {
            return i.Code == (int)InstructionCode.Label;
        }

        public IInstruction Label()
        {
            return new Instruction(InstructionCode.Label);
        }

        public void SetBranchTarget(IInstruction br, int index)
        {
            var i = br as Instruction;
            if (i == null)
                throw new ArgumentException("Invalid instruction");
            i.BranchTargetIndex = index;
        }

        public bool DonotCopyReturnValue { get; set; }

        public IInstruction[] Return(bool isvoid)
        {
            var code = new AbcCode(_abc);
            if (isvoid)
            {
                if (!IsCtorAsStaticCall)
                    code.ReturnVoid();
            }
            else
            {
                if (!DonotCopyReturnValue)
                    code.CopyValue(_method.Type);
                code.ReturnValue();
            }
            return code.ToArray();
        }

        public bool IsSwitchSupported
        {
            get { return true; }
        }

        public IInstruction Switch(int caseCount)
        {
            return new Instruction(InstructionCode.Lookupswitch);
        }

        public void SetCaseTargets(IInstruction sw, int[] targets, int defaultTarget)
        {
            var i = (Instruction)sw;
            i.DefaultCase = defaultTarget;
            i.Cases = targets;
        }

        readonly AvmResolver _resolver = new AvmResolver();
    }
}