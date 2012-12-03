using System;
using System.IO;
using DataDynamics.PageFX.Common;

internal class ArithmeticGenerator
{
	private static readonly TypeCode[] Numerics =
			{
				TypeCode.Boolean,
				TypeCode.SByte,
				TypeCode.Byte,
				TypeCode.Int16,
				TypeCode.UInt16,
				TypeCode.Int32,
				TypeCode.UInt32,
				TypeCode.Int64,
				TypeCode.UInt64,
				TypeCode.Single,
				TypeCode.Double,
				TypeCode.Decimal,
				TypeCode.Char,
			};

	public static void Generate()
	{
		var code = new StringWriter();

		WriteFuncs(code, "Add", "+", BinaryOperator.Addition);
		WriteFuncs(code, "Sub", "-", BinaryOperator.Subtraction);
		WriteFuncs(code, "Mul", "*", BinaryOperator.Multiply);
		WriteFuncs(code, "Div", "/", BinaryOperator.Division);
		WriteFuncs(code, "Mod", "%", BinaryOperator.Modulus);
		WriteFuncs(code, "Or", "|", BinaryOperator.BitwiseOr);
		WriteFuncs(code, "And", "&", BinaryOperator.BitwiseAnd);
		WriteFuncs(code, "Xor", "^", BinaryOperator.ExclusiveOr);
		WriteFuncs(code, "LeftShift", "<<", BinaryOperator.LeftShift);
		WriteFuncs(code, "RightShift", ">>", BinaryOperator.RightShift);


		File.WriteAllText(@"c:\temp.txt", code.ToString());
	}

	private static void WriteFuncs(TextWriter code, string name, string opSyntax, BinaryOperator op)
	{
		code.WriteLine("\tprivate static readonly BinFuncs {0}Funcs = new BinFuncs", name);
		code.WriteLine("\t\t{");
		for (int i = 0; i < Numerics.Length; i++)
		{
			for (int j = 0; j < Numerics.Length; j++)
			{
				var x = Numerics[i];
				var y = Numerics[j];

				var px = "";
				var py = "";
				var sx = "";
				var sy = "";

				if (x == TypeCode.Boolean)
				{
					px = "(";
					sx = " ? 1 : 0)";
				}
				if (y == TypeCode.Boolean)
				{
					py = "(";
					sy = " ? 1 : 0)";
				}

				if (x == TypeCode.Char)
				{
					px = "(Int32)";
				}
				if (y == TypeCode.Char)
				{
					py = "(Int32)";
				}

				if (x == TypeCode.UInt64 && y.IsSigned())
				{
					px = "(Int64)";
				}
				if (y == TypeCode.UInt64 && x.IsSigned())
				{
					py = "(Int64)";
				}
				if (x == TypeCode.Decimal && IsFloat(y))
				{
					py = "(Decimal)";
				}
				if (y == TypeCode.Decimal && IsFloat(x))
				{
					px = "(Decimal)";
				}

				if (op.IsBitwise())
				{
					if (x == TypeCode.Single)
					{
						px = "(Int32)";
					}
					else if (x == TypeCode.Double || x == TypeCode.Decimal)
					{
						px = "(Int64)";

						if (y == TypeCode.UInt64)
						{
							py = "(Int64)";
						}
					}

					if (op.IsShift() && y != TypeCode.Int32 && y != TypeCode.Boolean)
					{
						py = "(Int32)";
					}
					else if (y == TypeCode.Single)
					{
						py = "(Int32)";
					}
					else if (y == TypeCode.Double || y == TypeCode.Decimal)
					{
						py = "(Int64)";

						if (x == TypeCode.UInt64)
						{
							px = "(Int64)";
						}
					}
				}				

				code.WriteLine("\t\t\t{{new KeyValuePair<TypeCode,TypeCode>(TypeCode.{0}, TypeCode.{1}), (x, y) => {3}({0})x{5} {2} {4}({1})y{6}}},", x, y, opSyntax, px, py, sx, sy);
			}
		}
		code.WriteLine("\t\t};");
		code.WriteLine();
	}

	private static bool IsFloat(TypeCode value)
	{
		switch (value)
		{
			case TypeCode.Single:
			case TypeCode.Double:
				return true;
		}
		return false;
	}
}