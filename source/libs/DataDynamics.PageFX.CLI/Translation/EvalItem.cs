using System.Text;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.Values;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal struct EvalItem
	{
		public readonly IValue Value;
		public readonly Instruction Instruction;
		public readonly Instruction Last;
        
		public EvalItem(Instruction instruction)
		{
			Instruction = instruction;
			Value = null;
			Last = null;
		}

		public EvalItem(Instruction instruction, IValue value)
		{
			Instruction = instruction;
			Value = value;
			Last = null;
		}

		public EvalItem(Instruction instruction, Instruction last)
		{
			Instruction = instruction;
			Last = last;
			Value = null;
		}

		public IType Type
		{
			get { return Value == null ? null : Value.Type; }
		}

		public bool IsNull
		{
			get
			{
				var c = Value as ConstValue;
				if (c != null) return c.Value == null;
				return false;
			}
		}

		public bool IsToken
		{
			get { return Value.Kind == ValueKind.Token; }
		}

		public bool Is(InstructionCode c)
		{
			return Instruction.Code == c;
		}

		public bool IsInstance
		{
			get { return Instruction.Code == InstructionCode.Isinst; }
		}

		public bool IsTypeToken
		{
			get
			{
				if (Value != null)
				{
					var v = Value as TokenValue;
					if (v == null) return false;
					return v.member is IType;
				}

				var i = Instruction;
				if (i == null) return false;
				if (i.Code != InstructionCode.Ldtoken) return false;
				return i.Value is IType;
			}
		}

		public bool IsPointer
		{
			get { return Value.IsPointer; }
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			if (Instruction != null)
			{
				sb.Append(Instruction);
				if (Value != null)
					sb.Append("\n");
			}
			if (Value != null)
			{
				sb.Append(Value);
			}
			return sb.ToString();
		}
	}
}