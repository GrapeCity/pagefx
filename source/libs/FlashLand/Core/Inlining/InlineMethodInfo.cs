using System;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.IL;

namespace DataDynamics.PageFX.FlashLand.Core.Inlining
{
	internal sealed class InlineMethodInfo
	{
		public readonly QName Name;
		public readonly InlineKind Kind;

		public InlineMethodInfo(QName name, InlineKind kind)
		{
			Name = name;
			Kind = kind;
		}

		public InstructionCode Op
		{
			get
			{
				switch (Name.Name)
				{
					case "+":
						return InstructionCode.Add;
					case "-":
						return InstructionCode.Subtract;
					case "/":
						return InstructionCode.Divide;
					case "*":
						return InstructionCode.Multiply;
					default:
						throw new InvalidOperationException();
				}
			}
		}
	}

	internal enum InlineKind
	{
		Function,
		Property,
		Operator
	}
}