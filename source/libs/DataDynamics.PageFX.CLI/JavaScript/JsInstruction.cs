using System;
using System.Globalization;
using DataDynamics.PageFX.CLI.IL;

namespace DataDynamics.PageFX.CLI.JavaScript
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
			if (value != null)
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
					case TypeCode.Int64:
						writer.Write((int)((long)value >> 32));
						writer.Write(",");
						writer.Write((uint)((long)value & 0xffffffff));
						break;
					case TypeCode.UInt64:
						writer.Write((uint)((ulong)value >> 32));
						writer.Write(",");
						writer.Write((uint)((ulong)value & 0xffffffff));
						break;
					case TypeCode.Decimal:
						break;
					case TypeCode.Object:
						var obj = value as JsNode;
						if (obj == null)
							throw new NotSupportedException();
						obj.Write(writer);
						break;
					default:
						throw new NotSupportedException();
				}
			}

			writer.Write("]");			
		}
	}
}