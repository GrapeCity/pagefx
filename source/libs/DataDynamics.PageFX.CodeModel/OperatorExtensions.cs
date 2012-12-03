using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common
{
    public static class OperatorExtensions
    {
        public static bool IsArithmetic(this BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Addition:
                case BinaryOperator.Subtraction:
                case BinaryOperator.Multiply:
                case BinaryOperator.Division:
                case BinaryOperator.Modulus:
                case BinaryOperator.LeftShift:
                case BinaryOperator.RightShift:
                case BinaryOperator.BitwiseOr:
                case BinaryOperator.BitwiseAnd:
                case BinaryOperator.ExclusiveOr:
                case BinaryOperator.BooleanOr:
                case BinaryOperator.BooleanAnd:
                    return true;
            }
            return false;
        }

        public static bool IsBitwise(this BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.LeftShift:
                case BinaryOperator.RightShift:
                case BinaryOperator.BitwiseOr:
                case BinaryOperator.BitwiseAnd:
                case BinaryOperator.ExclusiveOr:
                    return true;
            }
            return false;
        }

        public static bool IsBoolean(this BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.BooleanAnd:
                case BinaryOperator.BooleanOr:
                    return true;
            }
            return false;
        }

        public static bool IsRelation(this BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Equality:
                case BinaryOperator.Inequality:
                case BinaryOperator.LessThan:
                case BinaryOperator.LessThanOrEqual:
                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanOrEqual:
                    return true;
            }
            return false;
        }

        public static bool IsEquality(this BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Equality:
                case BinaryOperator.Inequality:
                    return true;
            }
            return false;
        }

        public static bool IsShift(this BinaryOperator op)
        {
            return op == BinaryOperator.LeftShift || op == BinaryOperator.RightShift;
        }

    	public static bool IsUnary(this BranchOperator op)
        {
            switch (op)
            {
                case BranchOperator.True:
                case BranchOperator.False:
                case BranchOperator.Null:
                case BranchOperator.NotNull:
                    return true;
            }
            return false;
        }

        public static bool IsBinary(this BranchOperator op)
        {
            switch (op)
            {
                case BranchOperator.Equality:
                case BranchOperator.Inequality:
                case BranchOperator.GreaterThan:
                case BranchOperator.GreaterThanOrEqual:
                case BranchOperator.LessThan:
                case BranchOperator.LessThanOrEqual:
                    return true;
            }
            return false;
        }

        public static BinaryOperator ToBinaryOperator(this BranchOperator op)
        {
            switch (op)
            {
                case BranchOperator.Equality:
                    return BinaryOperator.Equality;
                case BranchOperator.Inequality:
                    return BinaryOperator.Inequality;
                case BranchOperator.GreaterThan:
                    return BinaryOperator.GreaterThan;
                case BranchOperator.GreaterThanOrEqual:
                    return BinaryOperator.GreaterThanOrEqual;
                case BranchOperator.LessThan:
                    return BinaryOperator.LessThan;
                case BranchOperator.LessThanOrEqual:
                    return BinaryOperator.LessThanOrEqual;
                default:
                    throw new ArgumentOutOfRangeException("op");
            }
        }

        public static bool IsTrueOrFalse(this BranchOperator op)
        {
            switch (op)
            {
                case BranchOperator.False:
                case BranchOperator.True:
                    return true;
            }
            return false;
        }

		private static readonly string[] CastOpNames = { OpNames.Implicit, OpNames.Explicit, OpNames.True, OpNames.False };

        private static IEnumerable<IMethod> GetCastOperators(this IType type)
        {
        	return CastOpNames.SelectMany(name => type.Methods.Find(name));
        }

    	public static IMethod FindCastOperator(this IType type, IType source, IType target)
        {
            return FindCastOperator(type, source, target, true);
        }

        private static IMethod FindCastOperator(IType type, IType source, IType target, bool check)
        {
            var methods = type.GetCastOperators();
            var op = methods.FirstOrDefault(m => m.IsCastOperator(source, target));
            if (check && op == null)
            {
            	op = FindCastOperator(ReferenceEquals(type, source) ? target : source, source, target, false);
            	if (op != null) return op;
                throw new InvalidOperationException("Unable to find cast operator");
            }
            return op;
        }

        private static bool IsCastOperator(this IMethod method, IType source, IType target)
        {
	        return method.Parameters.Count == 1
	               && ReferenceEquals(method.Type, target)
	               && ReferenceEquals(method.Parameters[0].Type, source);
        }

	    public static IMethod FindMethod(this BinaryOperator op, IType left, IType right)
        {
            string name = op.GetMethodName();
            var set = left.Methods.Find(name);
            return set.FirstOrDefault(m => m.IsBinaryOperator(op, left, right));
        }

		public static IMethod FindMethod(this UnaryOperator op, IType type)
		{
			string name = op.GetMethodName();
			var ops = type.Methods.Find(name);
			return ops.FirstOrDefault(m => IsUnaryOperator(m, type));
		}

		public static IMethod FindBooleanOperator(this IType type, bool isTrue)
		{
			string name = isTrue ? OpNames.True : OpNames.False;
			var ops = type.Methods.Find(name);
			return ops.FirstOrDefault(IsBooleanOperator);
		}

        private static bool IsBinaryOperator(this IMethod method, BinaryOperator op, IType left, IType right)
        {
            if (method.Parameters.Count != 2) return false;
            if (!ReferenceEquals(method.Parameters[0].Type, left)) return false;
            if (op.IsShift()) return true;
            return ReferenceEquals(method.Parameters[1].Type, right);
        }

        private static bool IsUnaryOperator(IMethod method, IType type)
        {
	        return method.Parameters.Count == 1 && ReferenceEquals(method.Parameters[0].Type, type);
        }

	    private static bool IsBooleanOperator(IMethod method)
	    {
		    return method.Parameters.Count == 1 && method.Type.Is(SystemTypeCode.Boolean);
	    }

	    private static string GetMethodName(this BinaryOperator op)
		{
			return "op_" + op;
		}

		private static string GetMethodName(this UnaryOperator op)
		{
			switch (op)
			{
				case UnaryOperator.BitwiseNot:
					return "op_OnesComplement";

				case UnaryOperator.Negate:
					return "op_UnaryNegation";
			}
			return null;
		}
    }

    public static class OpNames
    {
        public const string False = "op_False";
        public const string True = "op_True";
        public const string Implicit = "op_Implicit";
        public const string Explicit = "op_Explicit";
    }
}