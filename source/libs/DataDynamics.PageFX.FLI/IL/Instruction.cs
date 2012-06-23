using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.IL
{
    /// <summary>
    /// Represents AVM+ instruction.
    /// </summary>
    public class Instruction : IInstruction, ICloneable, ISupportXmlDump
    {
        #region Shared Members
        public static Instruction GetLocal(int index)
        {
            if (index == 0) return new Instruction(InstructionCode.Getlocal0);
            if (index == 1) return new Instruction(InstructionCode.Getlocal1);
            if (index == 2) return new Instruction(InstructionCode.Getlocal2);
            if (index == 3) return new Instruction(InstructionCode.Getlocal3);
            return new Instruction(InstructionCode.Getlocal, index);
        }

        public static Instruction SetLocal(int index)
        {
            if (index == 0) return new Instruction(InstructionCode.Setlocal0);
            if (index == 1) return new Instruction(InstructionCode.Setlocal1);
            if (index == 2) return new Instruction(InstructionCode.Setlocal2);
            if (index == 3) return new Instruction(InstructionCode.Setlocal3);
            return new Instruction(InstructionCode.Setlocal, index);
        }

        public static Instruction If(BranchOperator op)
        {
            switch (op)
            {
                case BranchOperator.True:
                    return new Instruction(InstructionCode.Iftrue);
                case BranchOperator.False:
                    return new Instruction(InstructionCode.Iffalse);
                case BranchOperator.Equality:
                    return new Instruction(InstructionCode.Ifeq);
                case BranchOperator.Inequality:
                    return new Instruction(InstructionCode.Ifne);
                case BranchOperator.LessThan:
                    return new Instruction(InstructionCode.Iflt);
                case BranchOperator.LessThanOrEqual:
                    return new Instruction(InstructionCode.Ifle);
                case BranchOperator.GreaterThan:
                    return new Instruction(InstructionCode.Ifgt);
                case BranchOperator.GreaterThanOrEqual:
                    return new Instruction(InstructionCode.Ifge);
                default:
                    throw new ArgumentOutOfRangeException("op");
            }
        }
        #endregion

        #region Constructors
        public Instruction()
        {
        }

        public Instruction(InstructionCode code)
        {
            Code = code;
            Copy(Instructions.GetInstruction(code));
        }

        public Instruction(InstructionCode code, params object [] values) 
            : this(code)
        {
            if (_operands != null)
            {
                int n = _operands.Length;
                if (n != values.Length)
                    throw new InvalidOperationException("invalid number of operands");
                for (int i = 0; i < n; ++i)
                    _operands[i].Value = values[i];
            }
            else
            {
                Debug.Assert(values == null || values.Length == 0);
            }
        }
        #endregion

        #region IInstruction Members
        public int MetadataToken { get; set; }

        int IInstruction.Code
        {
            get { return (int)Code; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        int _index;

        /// <summary>
        /// Gets or sets offset of the instruction from the begin of method body
        /// </summary>
        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }
        int _offset;

        /// <summary>
        /// Gets the size of instruction in bytes.
        /// </summary>
        public int Size
        {
            get
            {
                int size = 1;
                if (_operands != null)
                {
                	size += _operands.Sum(op => op.Size);
                }
                return size;
            }
        }

        public object Value
        {
            get { return _operands; }
        }

        /// <summary>
        /// Gets or sets flag indicating whether there are branches to this instruction.
        /// </summary>
        public bool IsBranchTarget
        {
            get { return IsFlag(InstructionFlags.BranchTarget); }
            set { SetFlag(InstructionFlags.BranchTarget, value); }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to exit from method.
        /// </summary>
        public bool IsReturn
        {
            get
            {
                return Code == InstructionCode.Returnvoid 
                    || Code == InstructionCode.Returnvalue;
            }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to throw exception.
        /// </summary>
        public bool IsThrow
        {
            get { return Code == InstructionCode.Throw; }
        }

        public bool IsSEHTarget
        {
            get { return IsFlag(InstructionFlags.SEHTarget); }
            set { SetFlag(InstructionFlags.SEHTarget, value); }
        }

        public bool IsGetLocal
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Getlocal:
                    case InstructionCode.Getlocal0:
                    case InstructionCode.Getlocal1:
                    case InstructionCode.Getlocal2:
                    case InstructionCode.Getlocal3:
                        return true;
                }
                return false;
            }
        }

        public bool IsSetLocal
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Setlocal:
                    case InstructionCode.Setlocal0:
                    case InstructionCode.Setlocal1:
                    case InstructionCode.Setlocal2:
                    case InstructionCode.Setlocal3:
                        return true;
                }
                return false;
            }
        }

        public int LocalIndex
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Setlocal0:
                    case InstructionCode.Getlocal0:
                        return 0;

                    case InstructionCode.Setlocal1:
                    case InstructionCode.Getlocal1:
                        return 1;

                    case InstructionCode.Setlocal2:
                    case InstructionCode.Getlocal2:
                        return 2;
                    case InstructionCode.Setlocal3:
                    case InstructionCode.Getlocal3:
                        return 3;

                    case InstructionCode.Setlocal:
                    case InstructionCode.Getlocal:
                        if (!HasOperands) return -1;
                        return _operands[0].ToInt32();
                }
                return -1;
            }
        }

        public bool IsPushConst
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Pushnull:
                    case InstructionCode.Pushundefined:
                    case InstructionCode.Pushstring:
                    case InstructionCode.Pushint:
                    case InstructionCode.Pushbyte:
                    case InstructionCode.Pushdouble:
                    case InstructionCode.Pushfalse:
                    case InstructionCode.Pushnamespace:
                    case InstructionCode.Pushnan:
                    case InstructionCode.Pushshort:
                    case InstructionCode.Pushtrue:
                    case InstructionCode.Pushuint:
                        return true;
                }
                return false;
            }
        }
        #endregion

        #region TranslateOffsets, TranslateIndices
        const int SW_Default = 0;
        const int SW_MaxIndex = 1;
        const int SW_Cases = 2;

        internal void VerifyBranchTargets(int n)
        {
            if (IsBranch)
            {
                int index = BranchTargetIndex;
                if (index < 0 || index >= n)
                    throw new InvalidOperationException();
            }
            else if (IsSwitch)
            {
                int index = DefaultCase;
                if (index < 0 || index >= n)
                    throw new InvalidOperationException();
                int[] cases = Cases;
                if (cases.Length == 0)
                    throw new InvalidOperationException();
                for (int i = 0; i < cases.Length; ++i)
                {
                    index = cases[i];
                    if (index < 0 || index >= n)
                        throw new InvalidOperationException();
                }
            }
        }

        internal void ResolveBranchTargets()
        {
            if (BranchTarget != null)
            {
                _operands[0].Value = BranchTarget.Index + BranchShift;
            }
        }

        public void TranslateOffsets(IInstructionList list)
        {
            if (IsSwitch)
            {
                int origin = _offset;

            	var def = _operands[SW_Default];
                int offset = origin + (int)def.Value;
                int index = list.GetOffsetIndex(offset);
                list[index].IsBranchTarget = true;
                def.Value = index;

                var offsets = (int[])_operands[SW_Cases].Value;
                for (int i = 0; i < offsets.Length; ++i)
                {
                    offset = origin + offsets[i];
                    index = list.GetOffsetIndex(offset);
                    list[index].IsBranchTarget = true;
                    offsets[i] = index;
                }
            }
            else if (IsBranch)
            {
                var op = _operands[0];
                int offset = _offset + 4 + (int)op.Value;
                int index = list.GetOffsetIndex(offset);
                list[index].IsBranchTarget = true;
                op.Value = index;
            }
        }

        public void TranslateIndices(IInstructionList list)
        {
            //Note: The base value for the program counter is the instruction address following the instruction.
            if (IsSwitch)
            {
                int origin = _offset;

                //default case offset
                var def = _operands[SW_Default];
                int defIndex = (int)def.Value;
                def.Value = list[defIndex].Offset - origin;

                //cases
                var cases = (int[])_operands[SW_Cases].Value;
                for (int j = 0; j < cases.Length; ++j)
                {
                    int index = cases[j];
                    cases[j] = list[index].Offset - origin;
                }
            }
            else if (IsBranch)
            {
                int origin = _offset + 4;
                int index = BranchTargetIndex;
                var op = _operands[0];
                if (index >= list.Count)
                    index = list.Count - 1;
                op.Value = list[index].Offset - origin;
            }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the name of instruction
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description of this instruction
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets category of this instruction
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets instruction code
        /// </summary>
        public InstructionCode Code { get; set; }

        InstructionFlags _flags;

        bool IsFlag(InstructionFlags f)
        {
            return (_flags & f) != 0;
        }

        void SetFlag(InstructionFlags f, bool value)
        {
            if (value) _flags |= f;
            else _flags &= ~f;
        }

        public bool IsUsed
        {
            get { return IsFlag(InstructionFlags.Used); }
            set { SetFlag(InstructionFlags.Used, value); }
        }

        /// <summary>
        /// Gets or sets instruction operands
        /// </summary>
        public Operand[] Operands
        {
            get { return _operands; }
            set { _operands = value; }
        }
        Operand[] _operands;

        /// <summary>
        /// Returns true if instruction has operands.
        /// </summary>
        public bool HasOperands
        {
            get { return _operands != null && _operands.Length > 0; }
        }

        public object Operand
        {
            get
            {
                if (HasOperands)
                    return _operands[0].Value;
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the number of frame references made by the instruction
        /// </summary>
        public int FrameUse { get; set; }

        /// <summary>
        /// Gets or sets the number of frame sets performed by the instruction
        /// </summary>
        public int FrameSet { get; set; }

        #region StackPop
        /// <summary>
        /// Gets or sets the number of pop operations from eval stack made by the instruction  (-1 => variable)
        /// </summary>
        public int StackPop
        {
            get
            {
                if (_stackPop < 0)
                {
                    if (_operands != null)
                    {
                        switch (Code)
                        {
                            case InstructionCode.Call:
                                return GetArgCount(2, 0);

                            case InstructionCode.Construct:
                            case InstructionCode.Constructsuper:
                                return GetArgCount(1, 0);

                            case InstructionCode.Callmethod:
                            case InstructionCode.Callstatic:
                                return GetArgCount(1, 1);

                            case InstructionCode.Callsuper:
                            case InstructionCode.Callproperty:
                            case InstructionCode.Constructprop:
                            case InstructionCode.Callproplex:
                            case InstructionCode.Callsupervoid:
                            case InstructionCode.Callpropvoid:
                                return GetArgCount(1, 0, 1);

                            //..., name1, value1, name2, value2, ..., nameN, valueN => ..., newobj
                            case InstructionCode.Newobject:
                                return GetArgCount(0, 0) * 2;

                            case InstructionCode.Newarray:
                                return GetArgCount(0, 0);

                            case InstructionCode.Setproperty:
                            case InstructionCode.Initproperty:
                                {
                                    int n = GetMultinamePops(0);
                                    if (n >= 0) return n + 2;
                                    return 2;
                                }

                            case InstructionCode.Getproperty:
                            case InstructionCode.Deleteproperty:
                                {
                                    int n = GetMultinamePops(0);
                                    if (n >= 0) return n + 1;
                                    return 1;
                                }
                        }
                    }
                }
                return _stackPop;
            }
            set { _stackPop = value; }
        }
        int _stackPop;

        int GetArgCount(int prefix, int argCountIndex)
        {
            if (_operands[argCountIndex].Value != null)
            {
                int arg_count = (int)_operands[argCountIndex].Value;
                return prefix + arg_count;
            }
            return -1;
        }

        int GetMultinamePops(int index)
        {
            if (_operands[index].Value != null)
            {
                var mname = _operands[index].Value as AbcMultiname;
                if (mname == null)
                    throw new BadInstructionException((int)Code);
                return mname.StackPop;
            }
            return -1;
        }

        int GetArgCount(int prefix, int nameIndex, int argCountIndex)
        {
            int n = GetMultinamePops(nameIndex);
            if (n < 0) return -1;
            int n2 = GetArgCount(prefix, argCountIndex);
            if (n2 < 0) return -1;
            return n + n2;
        }
        #endregion

        /// <summary>
        /// Gets or sets the number of push operations to eval stack made by the instruction.
        /// </summary>
        public int StackPush { get; set; }

        /// <summary>
        /// Gets number of pop operations from scope stack made by instruction.
        /// </summary>
        public int ScopePop
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Popscope:
                        return 1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets number of push operations to scope stack made by instruction.
        /// </summary>
        public int ScopePush
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Pushscope:
                    case InstructionCode.Pushwith:
                        return 1;
                }
                return 0;
            }
        }

        /// <summary>
        /// true if the instruction might throw an exception, false if not
        /// </summary>
        public bool CanThrow
        {
            get { return IsFlag(InstructionFlags.CanThrow); }
            set { SetFlag(InstructionFlags.CanThrow, value); }
        }

        /// <summary>
        /// Gets or sets flag specifing whether the instruction is decprecated.
        /// </summary>
        public bool IsDeprecated
        {
            get { return IsFlag(InstructionFlags.Deprecated); }
            set { SetFlag(InstructionFlags.Deprecated, value); }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform switch.
        /// </summary>
        public bool IsSwitch
        {
            get { return Code == InstructionCode.Lookupswitch; }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform conditional branch.
        /// </summary>
        public bool IsConditionalBranch
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Ifeq:
                    case InstructionCode.Iffalse:
                    case InstructionCode.Ifge:
                    case InstructionCode.Ifgt:
                    case InstructionCode.Ifle:
                    case InstructionCode.Iflt:
                    case InstructionCode.Ifne:
                    case InstructionCode.Ifnge:
                    case InstructionCode.Ifngt:
                    case InstructionCode.Ifnle:
                    case InstructionCode.Ifnlt:
                    case InstructionCode.Ifstricteq:
                    case InstructionCode.Ifstrictne:
                    case InstructionCode.Iftrue:
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform unconditional branch.
        /// </summary>
        public bool IsUnconditionalBranch
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Abs_jump:
                    case InstructionCode.Jump:
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform conditional/unconditional branch.
        /// </summary>
        public bool IsBranch
        {
            get { return IsConditionalBranch || IsUnconditionalBranch; }
        }

        public bool IsBranchOrSwitch
        {
            get
            {
                return IsBranch || IsSwitch;
            }
        }

        /// <summary>
        /// Returns true if instruction can be end of basic block.
        /// </summary>
        public bool IsEndOfBlock
        {
            get
            {
                return IsBranch || IsSwitch || IsReturn || IsThrow || IsBranchTarget;
            }
        }

        public Instruction BranchTarget { get; set; }

        public int BranchShift { get; set; }

        public int BranchTargetIndex
        {
            get
            {
                if (BranchTarget != null)
                    return BranchTarget.Index + BranchShift;
                return _operands[0].ToInt32() + BranchShift;
            }
            set
            {
                if (!IsBranch)
                    throw new InvalidOperationException();
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                _operands[0].Value = value;
            }
        }

        public IEnumerable<int> GetTargets()
        {
            if (IsBranch)
            {
                yield return BranchTargetIndex;
            }
            else if (IsSwitch)
            {
                int defcase = DefaultCase;
                if (defcase >= 0)
                    yield return defcase;

                var cases = Cases;
                if (cases != null)
                {
                    foreach (var i in cases)
                        yield return i;
                }
            }
        }

        public void GotoNext(Instruction t)
        {
            BranchTarget = t;
            BranchShift = 1;
        }

        public int DefaultCase
        {
            get
            {
                if (IsSwitch)
                {
                    var op =_operands[0];
                    if (op != null)
                        return (int)op.Value;
                }
                return -1;
            }
            set
            {
                if (!IsSwitch)
                    throw new InvalidOperationException();
                _operands[0].Value = value;
            }
        }

        public int[] Cases
        {
            get
            {
                if (IsSwitch)
                {
                    var op = _operands[SW_Cases];
                    if (op != null)
                        return op.Value as int[];
                }
                return null;
            }
            set
            {
                if (!IsSwitch)
                    throw new InvalidOperationException();
                if (value == null)
                    throw new ArgumentNullException("value");
                _operands[SW_MaxIndex].Value = value.Length - 1; //max index
                _operands[SW_Cases].Value = value;
            }
        }
        #endregion

        #region ICloneable Members
        /// <summary>
        /// Clones the instruction.
        /// </summary>
        /// <returns>a new copy of the instruction</returns>
        public Instruction Clone()
        {
            var clone = new Instruction();
            clone.Copy(this);
            return clone;
        }

        void Copy(Instruction from)
        {
            Code = from.Code;
            _flags = from._flags;
            Name = from.Name;
            Description = from.Description;
            Category = from.Category;
            FrameSet = from.FrameSet;
            FrameUse = from.FrameUse;
            if (from._operands != null)
            {
                int n = from._operands.Length;
                _operands = new Operand[n];
                for (int k = 0; k < n; ++k)
                    _operands[k] = from._operands[k].Clone();
            }
            _stackPop = from._stackPop;
            StackPush = from.StackPush;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region IO
        public static Instruction Read(ILStream list, AbcMethodBody body, SwfReader reader)
        {
            int offset = (int)reader.Position;
            var code = (InstructionCode)reader.ReadUInt8();
            var instruction = Instructions.GetInstruction(code);
            instruction.Offset = offset;
            var ops = instruction.Operands;
            if (code == InstructionCode.Lookupswitch)
            {
                instruction.Operands[0].Value = reader.ReadInt24(); //default case offset
                int n = (int)reader.ReadUIntEncoded(); //max_index
                ops[1].Value = n;
                var cases = new int[n + 1];
                for (int i = 0; i <= n; ++i)
                    cases[i] = reader.ReadInt24();
                ops[2].Value = cases;
            }
            else if (ops != null)
            {
                int n = ops.Length;
                for (int i = 0; i < n; ++i)
                {
                    var op = ops[i];
                    op.Read(body, reader);
                }
            }
            return instruction;
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteUInt8((byte)Code);
            if (_operands != null)
            {
                foreach (var op in _operands)
                {
                    op.Write(writer);
                }
            }
        }

        public byte[] ToByteArray()
        {
            if (!HasOperands)
            {
                return new[] { (byte)Code };
            }
            using (var writer = new SwfWriter())
            {
                Write(writer);
                return writer.ToByteArray();
            }
        }
        #endregion

        #region Dump
        public void DumpXml(XmlWriter writer)
        {
            if (IsUsed)
            {
                writer.WriteStartElement("i");
                writer.WriteAttributeString("name", Name);
                writer.WriteAttributeString("code", string.Format("0x{0:X2}", ((int)Code)));
                if (IsDeprecated)
                    writer.WriteAttributeString("deprecated", "true");
                writer.WriteAttributeString("frame-use", FrameUse.ToString());
                writer.WriteAttributeString("frame-set", FrameSet.ToString());
                writer.WriteAttributeString("pop", _stackPop.ToString());
                writer.WriteAttributeString("push", StackPush.ToString());
                writer.WriteAttributeString("throws", CanThrow ? "1" : "0");
                writer.WriteAttributeString("desc", Description);

                if (_operands != null)
                {
                    foreach (var op in _operands)
                        op.DumpXml(writer);
                }

                writer.WriteEndElement();
            }
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var i = obj as Instruction;
            if (i == null) return false;
            if (i.Code != Code) return false;
            if (!i._operands.EqualsTo(_operands)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return (int)Code ^ Algorithms.EvalHashCode(_operands);
        }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(format))
                format = "O: N V";

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
                        AppendValue(sb);
                        break;

                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString().Trim();
        }

        public bool IsAccessor
        {
            get
            {
                switch (Code)
                {
                    case InstructionCode.Getproperty:
                    case InstructionCode.Setproperty:
                    case InstructionCode.Getsuper:
                    case InstructionCode.Setsuper:
                        return true;
                }
                return false;
            }
        }

        bool IsValueOnly
        {
            get
            {
                if (IsAccessor) return true;
                switch (Code)
                {
                    case InstructionCode.Debugfile:
                    case InstructionCode.Debugline:
                        return true;
                }
                return false;
            }
        }

        void AppendValue(StringBuilder sb)
        {
            if (_operands == null) return;
        	bool valueOnly = IsValueOnly;
            for (int i = 0; i < _operands.Length; ++i)
            {
                if (i > 0) sb.Append(", ");
				var op = _operands[i];
				//if (op.Value == null && IsBranch && BranchTarget != null)
				if (IsBranch)
				{
					sb.AppendFormat("index = {0}", BranchTargetIndex);
					continue;
				}
				sb.Append(op.ToString(valueOnly));
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

        #region Verify
        public void Verify()
        {
            var i = Instructions.GetInstruction(Code);
            if (i.Operands != null)
            {
                if (_operands == null)
                    throw new BadFormatException(string.Format("Instruction {0} has no operands", Code));
                int n = i.Operands.Length;
                if (_operands.Length != n)
                    throw new BadFormatException(
                        string.Format("Instruction {0} has incorrect {1} number of operands. Must be {2}.",
                                      Code, _operands.Length, n));
                for (int k = 0; k < n; ++k)
                {
                    if (i.Operands[k].Type != _operands[k].Type)
                        throw new BadFormatException(
                            string.Format("Instruction {0} has invalid type {1} of operand {2}. Must be {3}.",
                                          Code, _operands[k].Type, k, i.Operands[k].Type));
                    if (_operands[k].Value == null)
                        throw new BadFormatException(string.Format("Instruction {0} has null value", Code));
                }
            }
        }
        #endregion

        #region ImportOperands
        public void ImportOperands(AbcFile abc)
        {
            if (HasOperands)
            {
                foreach (var op in _operands)
                {
                    var c = op.Value as IAbcConst;
                    if (c != null)
                    {
                        c = abc.ImportConst(c);
                        op.Value = c;
                    }
                }
            }
        }
        #endregion
    }

    [Flags]
    enum InstructionFlags
    {
        None = 0,
        Used = 1,
        CanThrow = 2,
        Deprecated = 4,
        BranchTarget = 8,
        SEHTarget = 16,
    }
}