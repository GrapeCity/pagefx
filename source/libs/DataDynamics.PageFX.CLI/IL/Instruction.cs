using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using DataDynamics.PageFX.CLI.CFG;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    class Instruction : IInstruction
    {
        #region IInstruction Members
        int IInstruction.Code
        {
            get { return _code.Value; }
        }

        /// <summary>
        /// Gets or sets zero based index of the instruction  in the IL stream.
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        int _index = -1;

        /// <summary>
        /// Instruction offset from the method begin
        /// </summary>
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        int _offset;

        public bool IsBranchTarget { get; set; }

        public bool IsEndOfTryFinally
        {
            get
            {
                if (!IsLeave) return false; //is this check need?
                var pb = Block as ProtectedBlock;
                if (pb == null) return false;
                if (pb.ExitPoint != this) return false;
                return pb.IsTryFinally;
            }
        }

        public void TranslateOffsets(IInstructionList list)
        {
            if (IsSwitch)
            {
                var offsets = (int[])Value;
                for (int j = 0; j < offsets.Length; ++j)
                {
                    int index = list.GetOffsetIndex(offsets[j]);
                    list[index].IsBranchTarget = true;
                    offsets[j] = index;
                }
            }
            else if (IsBranch)
            {
                int index = list.GetOffsetIndex((int)Value);
                list[index].IsBranchTarget = true;
                Value = index;
            }
        }
        #endregion

        #region Public Members
        /// <summary>
        /// Instruction Code
        /// </summary>
        public OpCode OpCode
        {
            get { return _code; }
            set { _code = value; }
        }
        OpCode _code;

        public string Name
        {
            get { return _code.Name; }
        }

        public InstructionCode Code
        {
            get { return (InstructionCode)_code.Value; }
        }

        public FlowControl FlowControl
        {
            get { return _code.FlowControl; }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to exit from method.
        /// </summary>
        public bool IsReturn
        {
            get { return FlowControl == FlowControl.Return; }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform switch.
        /// </summary>
        public bool IsSwitch
        {
            get { return Code == InstructionCode.Switch; }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform conditional branch.
        /// </summary>
        public bool IsConditionalBranch
        {
            get { return FlowControl == FlowControl.Cond_Branch; }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform unconditional branch.
        /// </summary>
        public bool IsUnconditionalBranch
        {
            get { return FlowControl == FlowControl.Branch; }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform conditional/unconditional branch.
        /// </summary>
        public bool IsBranch
        {
            get { return IsUnconditionalBranch || IsConditionalBranch; }
        }

        public bool IsThrow
        {
            get { return FlowControl == FlowControl.Throw; }
        }

        public bool IsCall
        {
            get
            {
                if (FlowControl == FlowControl.Call)
                {
                    switch (Code)
                    {
                        case InstructionCode.Newobj:
                        case InstructionCode.Newarr:
                            return false;
                    }
                    return true;
                }
                return false;
            }
        }

        public bool IsLeave
        {
            get { return Code == InstructionCode.Leave || Code == InstructionCode.Leave_S; }
        }

        public bool IsHandlerEnd
        {
            get { return Code == InstructionCode.Endfilter || Code == InstructionCode.Endfinally; }
        }

        public bool IsHandlerBegin
        {
            get
            {
                var hb = Block as HandlerBlock;
                if (hb  == null) return false;
                return hb.EntryIndex == Index;
            }
        }
        #endregion

        #region Operand
        /// <summary>
        /// Gets or sets instruction operand
        /// </summary>
        public object Value { get; set; }

        public int MetadataToken { get; set; }

        public ITypeMember Member
        {
            get 
            {
                if (_stateStack.Count == 0)
                    return Value as ITypeMember;
                return State.Member;
            }
            set { State.Member = value; }
        }

        public IField Field
        {
            get { return Member as IField; }
        }

        public IMethod Method
        {
            get { return Member as IMethod; }
        }

        public IType Type
        {
            get { return Member as IType; }
        }

        public bool IsGenericContext
        {
            get 
            {
                var member = Value as ITypeMember;
                if (member == null) return false;
                return GenericType.IsGenericContext(member);
            }
        }

        public object ConstantValue
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Ldc_I4:
                    case InstructionCode.Ldc_I4_S:
                        return (int)Value;
                    case InstructionCode.Ldc_I4_0: return 0;
                    case InstructionCode.Ldc_I4_1: return 1;
                    case InstructionCode.Ldc_I4_2: return 2;
                    case InstructionCode.Ldc_I4_3: return 3;
                    case InstructionCode.Ldc_I4_4: return 4;
                    case InstructionCode.Ldc_I4_5: return 5;
                    case InstructionCode.Ldc_I4_6: return 6;
                    case InstructionCode.Ldc_I4_7: return 7;
                    case InstructionCode.Ldc_I4_8: return 8;
                    case InstructionCode.Ldc_I4_M1: return -1;
                    case InstructionCode.Ldc_I8: return (long)Value;
                    case InstructionCode.Ldc_R4: return (float)Value;
                    case InstructionCode.Ldc_R8: return (double)Value;
                }
                return null;
            }
        }

        public bool IsZero
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Ldc_I4:
                    case InstructionCode.Ldc_I4_S:
                        return (int)Value == 0;
                    case InstructionCode.Ldc_I4_0: return true;
                    case InstructionCode.Ldc_I8: return (long)Value == 0;
                    case InstructionCode.Ldc_R4: return (float)Value == 0;
                    case InstructionCode.Ldc_R8: return (double)Value == 0;
                }
                return false;
            }
        }

        public bool IsOne
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Ldc_I4:
                    case InstructionCode.Ldc_I4_S:
                        return (int)Value == 1;
                    case InstructionCode.Ldc_I4_1: return true;
                    case InstructionCode.Ldc_I8: return (long)Value == 1;
                    case InstructionCode.Ldc_R4: return (float)Value == 1;
                    case InstructionCode.Ldc_R8: return (double)Value == 1;
                }
                return false;
            }
        }
        #endregion

        #region Internal Properties
        public Block Block { get; set; }

        /// <summary>
        /// Gets or sets basic block to which this instruction belongs
        /// </summary>
        public Node OwnerNode { get; set; }

        public InstructionState State
        {
            get { return _stateStack.Peek(); }
        }

        public void PushState()
        {
            _stateStack.Push(new InstructionState());
        }

        public void PopState()
        {
            _stateStack.Pop();
        }

        readonly Stack<InstructionState> _stateStack = new Stack<InstructionState>();

        public IParameter Parameter
        {
            get { return State.Parameter; }
            set { State.Parameter = value; }
        }

        /// <summary>
        /// Method for which this instruction computes parameter introduced in <see cref="Parameter"/> property.
        /// </summary>
        public IMethod ParameterFor
        {
            get { return State.ParameterFor; }
            set { State.ParameterFor = value; }
        }

        //stack to insert code to code to begin call or to begin store of static field
        public Stack BeginStack
        {
            get { return State.BeginStack; }
        }
        
        public Stack EndStack
        {
            get { return State.EndStack; }
        }

        public IType BoxingType
        {
            get { return State.BoxingType; }
            set { State.BoxingType = value; }
        }

        public Instruction ReceiverFor
        {
            get { return State.ReceiverFor; }
            set { State.ReceiverFor = value; }
        }

        internal bool Dumped;
        #endregion

        #region DebugInfo
        public SequencePoint SequencePoint
        {
            get;
            set;
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = "I: N V";

            var sb = new StringBuilder();
            int n = format.Length;
            for (int i = 0; i < n; ++i)
            {
                char c = format[i];
                switch (c)
                {
                    case 'I':
                    case 'i':
                        FormatInt(sb, format, ref i, _index);
                        break;

                    case 'O':
                    case 'o':
                        FormatInt(sb, format, ref i, _offset);
                        break;

                    case 'C':
                    case 'c':
                        FormatInt(sb, format, ref i, (int)Code);
                        break;

                    case 'N':
                    case 'n':
                        sb.Append(Name);
                        break;

                    case 'V':
                    case 'v':
                        AppendValue(sb, Value);
                        break;

                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString().Trim();
        }

        static void AppendValue(StringBuilder sb, object value)
        {
            if (value != null)
            {
                string str = value as string;
                if (str != null)
                {
                    sb.Append(Escaper.Escape(str));
                }
                else if (value is bool)
                {
                    sb.Append((bool)value ? "true" : "false");
                }
                else
                {
                    var offsets = value as int[];
                    if (offsets != null)
                    {
                        for (int i = 0; i < offsets.Length; ++i)
                        {
                            if (i > 0) sb.Append(", ");
                            sb.Append(offsets[i]);
                        }
                    }
                    else
                    {
                        sb.Append(value);
                    }
                }
            }
        }

        static void FormatInt(StringBuilder sb, string format, ref int i, int value)
        {
            if (i + 1 < format.Length)
            {
                char x = format[i + 1];
                if (x == 'x' || x == 'X')
                {
                    ++i;
                    sb.Append(value.ToString(x.ToString()));
                    return;
                }
            }
            sb.Append(value);
        }
        #endregion
    }

    class InstructionState
    {
        public ITypeMember Member { get; set; }

        public IParameter Parameter { get; set; }

        /// <summary>
        /// Method for which this instruction computes parameter introduced in <see cref="Parameter"/> property.
        /// </summary>
        public IMethod ParameterFor;

        //stack to insert code to code to begin call or to begin store of static field
        public Stack BeginStack
        {
            get
            {
                if (_beginStack == null)
                    _beginStack = new Stack();
                return _beginStack;
            }
        }
        Stack _beginStack;

        public Stack EndStack
        {
            get
            {
                if (_endStack == null)
                    _endStack = new Stack();
                return _endStack;
            }
        }
        Stack _endStack;

        public IType BoxingType;

        public Instruction ReceiverFor;
    }
}