using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.IL;

namespace DataDynamics.PageFX.Ecma335.Translation
{
	internal static class CastingOperations
	{
		public static IType ResolveSystemType(this Code code, SystemTypeCode typeCode)
		{
			return code.Method.DeclaringType.SystemType(typeCode);
		}

		public static Code CastWithSwap(this Code code, IType source, IType target, bool swap)
		{
			if (ReferenceEquals(target, source))
				return code;

			return code.SwapIf(swap)
			           .Cast(source, target)
			           .SwapIf(swap);
		}

		private static IType GetBitwiseResultType(IType leftType, IType rightType)
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
				var d = GetBitwiseResultType(left, right);
				return code.CastOperands(ref left, ref right, d);
			}
			return code.CastOperands(ref left, ref right);
		}

		public static Code CastOperands(this Code code, ref IType left, ref IType right)
		{
			var d = NumericType.GetCommonType(left, right);
			if (d == null)
				return code;

			return code.CastOperands(ref left, ref right, d);
		}

		public static Code CastOperands(this Code code, ref IType left, ref IType right, IType target)
		{
			code.CastWithSwap(right, target, false);
			code.CastWithSwap(left, target, true);

			left = target;
			right = target;

			return code;
		}

		public static bool IsSignedUnsigned(IType left, IType right)
		{
			return (left.IsSigned() && right.IsUnsigned())
			       || (left.IsUnsigned() && right.IsSigned());
		}

		public static Code ToUnsigned(this Code code, ref IType type, bool swap)
		{
			var ut = type.ToUnsigned();
			if (ut == null || ReferenceEquals(type, ut))
				return code;

			code.CastWithSwap(type, ut, swap);
			
			type = ut;

			return code;
		}

		public static Code ToUnsigned(this Code code, ref IType left, ref IType right)
		{
			var u = NumericType.UInt32OR64(left, right);
			if (u != null)
			{
				code.CastWithSwap(right, u, false);
				code.CastWithSwap(left, u, true);
				left = u;
				right = u;
			}

			return code;
		}

		public static Code CastToInt32(this Code code, ref IType type)
		{
			var target = code.ResolveSystemType(SystemTypeCode.Int32);
			code.Cast(type, target);
			type = target;
			return code;
		}
	}
}
