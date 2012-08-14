using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;
using DataDynamics.PageFX.CLI.CFG;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal sealed class Instruction : IInstruction
    {
		public Instruction()
		{
			Index = -1;
		}

        #region IInstruction Members
        int IInstruction.Code
        {
            get { return OpCode.Value; }
        }

    	/// <summary>
    	/// Gets or sets zero based index of the instruction  in the IL stream.
    	/// </summary>
    	public int Index { get; set; }

    	/// <summary>
    	/// Instruction offset from the method begin
    	/// </summary>
    	public int Offset { get; set; }

    	public bool IsBranchTarget { get; set; }

        public bool IsEndOfTryFinally
        {
            get
            {
                if (!IsLeave) return false; //is this check need?
                var pb = SehBlock as TryCatchBlock;
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
    	public OpCode OpCode { get; set; }

    	public string Name
        {
            get { return OpCode.Name; }
        }

        public InstructionCode Code
        {
            get { return (InstructionCode)OpCode.Value; }
        }

        public FlowControl FlowControl
        {
            get { return OpCode.FlowControl; }
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
                var hb = SehBlock as HandlerBlock;
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

		public int BranchTarget
		{
			get { return Convert.ToInt32(Value, CultureInfo.InvariantCulture); }
		}

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

    	#endregion

        #region Internal Properties
		/// <summary>
		/// Gets or sets SEH block in which instruction is located.
		/// </summary>
        public Block SehBlock { get; set; }

        /// <summary>
        /// Gets or sets basic block to which this instruction belongs
        /// </summary>
        public Node BasicBlock { get; set; }

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

        private readonly Stack<InstructionState> _stateStack = new Stack<InstructionState>();

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

		internal bool Dumped { get; set; }

		public IType[] InputTypes { get; set; }

		public IType OutputType { get; set; }

    	#endregion

        //DebugInfo
        public SequencePoint SequencePoint { get; set; }

	    public override string ToString()
        {
            return ToString(null, null);
        }

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
                        FormatInt(sb, format, ref i, Index);
                        break;

                    case 'O':
                    case 'o':
                        FormatInt(sb, format, ref i, Offset);
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

    internal sealed class InstructionState
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
            get { return _beginStack ?? (_beginStack = new Stack()); }
        }
        private Stack _beginStack;

        public Stack EndStack
        {
            get { return _endStack ?? (_endStack = new Stack()); }
        }
        private Stack _endStack;

	    public IType BoxingType;

	    public Instruction ReceiverFor;
    }
}