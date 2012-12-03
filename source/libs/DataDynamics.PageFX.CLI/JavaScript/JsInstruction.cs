using System;
using System.Globalization;
using DataDynamics.PageFX.Ecma335.IL;

namespace DataDynamics.PageFX.Ecma335.JavaScript
{
	internal sealed class JsInstruction : JsNode
	{
		private readonly Instruction _instruction;
		private readonly object _value;

		public JsInstruction(Instruction instruction, object value)
		{
			_instruction = instruction;
			_value = value;
		}

		public override void Write(JsWriter writer)
		{
			writer.Write("/* {0} */", _instruction.ToString("I: N", CultureInfo.InvariantCulture));
			writer.Write("[");
			writer.Write((int)_instruction.Code);

			var value = _value;
			if (value != null && !Ignore(_instruction.Code))
			{
				writer.Write(", ");

				switch (Type.GetTypeCode(value.GetType()))
				{
					case TypeCode.Boolean:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.String:
						writer.WriteValue(value);
						break;
					case TypeCode.Object:
						var var = value as JsVar;
						writer.WriteValue(var != null ? var.Id() : value);
						break;
					default:
						throw new NotSupportedException();
				}
			}

			writer.Write("]");			
		}

		private static bool Ignore(InstructionCode c)
		{
			switch (c)
			{
				case InstructionCode.Constrained:
					return true;
			}
			return false;
		}
	}
}