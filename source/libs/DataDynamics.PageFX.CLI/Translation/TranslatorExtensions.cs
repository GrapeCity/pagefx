using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal static class TranslatorExtensions
	{
		public static Code Append(this Code code, IEnumerable<IInstruction> set)
		{
			if (set == null) return code;

			int index = code.Count;
			foreach (var instruction in set)
			{
				instruction.Index = index;
				code.Add(instruction);
				++index;
			}

			return code;
		}

		public static Code Swap(this Code code)
		{
			var i = code.Provider.Swap();
			if (i == null)
				throw new NotSupportedException("Swap instruction is not supported");
			code.Add(i);
			return code;
		}

		private static Code SwapIf(this Code code, bool condition)
		{
			return condition ? code.Swap() : code;
		}

		public static Code Cast(this Code code, IType source, IType target)
		{
			if (ReferenceEquals(target, source))
				return code;

			var il = code.Provider.Cast(source, target, false);
			code.AddRange(il);

			return code;
		}

		public static Code CastWithSwap(this Code code, IType source, IType target, bool swap)
		{
			if (ReferenceEquals(target, source))
				return code;

			return code.SwapIf(swap)
			           .Cast(source, target)
			           .SwapIf(swap);
		}

		private static IType GetBitwiseCD(IType leftType, IType rightType)
		{
			if (leftType.IsEnum)
				leftType = leftType.ValueType;
			if (rightType.IsEnum)
				rightType = rightType.ValueType;

			var l = leftType.SystemType();
			if (l == null)
				throw new ILTranslatorException();
			var r = rightType.SystemType();
			if (r == null)
				throw new ILTranslatorException();
			if (!l.IsNumeric)
				throw new ILTranslatorException();
			if (!r.IsNumeric)
				throw new ILTranslatorException();

			switch (l.Code)
			{
				case SystemTypeCode.Double:
					switch (r.Code)
					{
						case SystemTypeCode.Decimal:
							return rightType;

						default:
							return leftType;
					}

				case SystemTypeCode.Single:
					switch (r.Code)
					{
						case SystemTypeCode.Decimal:
						case SystemTypeCode.Double:
							return rightType;

						default:
							return leftType;
					}

				case SystemTypeCode.Boolean:
					return rightType;

				case SystemTypeCode.Int8:
				case SystemTypeCode.UInt8:
					if (r.Size <= 1)
						return leftType;
					return rightType;

				case SystemTypeCode.Int16:
				case SystemTypeCode.UInt16:
				case SystemTypeCode.Char:
					if (r.Size <= 2)
						return leftType;
					return rightType;

				case SystemTypeCode.Int32:
				case SystemTypeCode.UInt32:
					if (r.Size <= 4)
						return leftType;
					return rightType;

				case SystemTypeCode.Int64:
				case SystemTypeCode.UInt64:
					if (r.Size <= 8)
						return leftType;
					return rightType;

				case SystemTypeCode.Decimal:
				default:
					return leftType;
			}
		}

		public static Code CastOperands(this Code code, BinaryOperator op, ref IType left, ref IType right)
		{
			if (op == BinaryOperator.BitwiseAnd
			    || op == BinaryOperator.BitwiseOr
			    || op == BinaryOperator.ExclusiveOr)
			{
				var d = GetBitwiseCD(left, right);
				return CastOperands(code, ref left, ref right, d);
			}
			return CastOperands(code, ref left, ref right);
		}

		private static Code CastOperands(Code code, ref IType left, ref IType right)
		{
			var d = SystemTypes.GetCommonDenominator(left, right);
			if (d == null)
				return code;

			return CastOperands(code, ref left, ref right, d);
		}

		private static Code CastOperands(Code code, ref IType left, ref IType right, IType target)
		{
			code.CastWithSwap(right, target, false);
			code.CastWithSwap(left, target, true);

			left = target;
			right = target;

			return code;
		}

		public static bool IsSignedUnsigned(IType ltype, IType rtype)
		{
			return (ltype.IsSigned() && rtype.IsUnsigned())
			       || (ltype.IsUnsigned() && rtype.IsSigned());
		}

		public static Code ToUnsigned(this Code code, ref IType type, bool swap)
		{
			var ut = SystemTypes.ToUnsigned(type);
			if (ut == null || ReferenceEquals(type, ut))
				return code;

			code.CastWithSwap(type, ut, swap);
			
			type = ut;

			return code;
		}

		public static Code ToUnsigned(this Code code, ref IType left, ref IType right)
		{
			var u = SystemTypes.UInt32OR64(left, right);
			if (u != null)
			{
				code.CastWithSwap(right, u, false);
				code.CastWithSwap(left, u, true);
				left = u;
				right = u;
			}

			return code;
		}

		/// <summary>
		/// Called in analysis and translation phases to prepare eval stack for given basic block.
		/// </summary>
		/// <param name="bb">given basic block</param>
		public static void EnshureEvalStack(this Node bb)
		{
			//NOTE:
			//Taking into account that stack state must be the same before and after 
			//execution of basic block we can consider only first input edge
			var e = bb.FirstIn;
			if (e == null)
			{
				bb.Stack = new EvalStack();
				return;
			}

#if DEBUG
			DebugHooks.DoCancel();
#endif
			var from = e.From;
			var prevStack = @from.Stack;
			bb.Stack = prevStack != null ? prevStack.Clone() : new EvalStack();
		}
	}
}