using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.CodeModel
{
    public static class OpHelper
    {
        public static bool IsArithmetic(BinaryOperator op)
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

        public static bool IsBitwise(BinaryOperator op)
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

        public static bool IsBoolean(BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.BooleanAnd:
                case BinaryOperator.BooleanOr:
                    return true;
            }
            return false;
        }

        public static bool IsRelation(BinaryOperator op)
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

        public static bool IsEquality(BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Equality:
                case BinaryOperator.Inequality:
                    return true;
            }
            return false;
        }

        public static bool IsShift(BinaryOperator op)
        {
            return op == BinaryOperator.LeftShift || op == BinaryOperator.RightShift;
        }

        public static bool IsUnary(BranchOperator op)
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

        public static bool IsBinary(BranchOperator op)
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

        public static BinaryOperator ToBinaryOperator(BranchOperator op)
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

        public static bool IsTrueOrFalse(BranchOperator op)
        {
            switch (op)
            {
                case BranchOperator.False:
                case BranchOperator.True:
                    return true;
            }
            return false;
        }

        static readonly string[] CastOpNames = { OpNames.Implicit, OpNames.Explicit, OpNames.True, OpNames.False };

        static IEnumerable<IMethod> GetCastOperators(IType type)
        {
        	return CastOpNames.SelectMany(opName => type.Methods[opName]);
        }

    	public static IMethod FindCastOperator(IType type, IType source, IType target)
        {
            return FindCastOperator(type, source, target, true);
        }

        static IMethod FindCastOperator(IType type, IType source, IType target, bool check)
        {
            var ops = GetCastOperators(type);
            var op = Algorithms.Find(ops, m => IsCastOperator(m, source, target));
            if (check && op == null)
            {
            	op = FindCastOperator(type == source ? target : source, source, target, false);
            	if (op != null) return op;
                throw new InvalidOperationException("Unable to find cast operator");
            }
            return op;
        }

        static bool IsCastOperator(IMethod m, IType source, IType target)
        {
            if (!IsCastOperator(m)) return false;
            if (m.Type != target) return false;
            if (m.Parameters[0].Type != source) return false;
            return true;
        }

        static bool IsCastOperator(IMethod m)
        {
            if (m.Parameters.Count != 1) return false;
            //Name has been already checked.
            //string name = m.Name;
            //switch (name)
            //{
            //    case OpNames.Implicit:
            //    case OpNames.Explicit:
            //    case OpNames.True:
            //    case OpNames.False:
            //        return true;
            //}
            return true;
        }

        public static string GetOpName(BinaryOperator op)
        {
            return "op_" + op;
        }

        public static string GetOpName(UnaryOperator op)
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

        public static bool IsShiftOperator(BinaryOperator op)
        {
            return op == BinaryOperator.LeftShift || op == BinaryOperator.RightShift;
        }

        public static IMethod FindOperator(BinaryOperator op, IType left, IType right)
        {
            string opName = GetOpName(op);
            var set = left.Methods[opName];
            return Algorithms.Find(set, m => IsBinaryOperator(m, op, left, right));
        }

        static bool IsBinaryOperator(IMethod m, BinaryOperator op, IType left, IType right)
        {
            if (m.Parameters.Count != 2) return false;
            //if (m.Name != GetOpName(op)) return false; //already checked
            if (m.Parameters[0].Type != left) return false;
            if (IsShiftOperator(op)) return true;
            if (m.Parameters[1].Type != right) return false;
            return true;
        }

        public static IMethod FindOperator(UnaryOperator op, IType type)
        {
            string opName = GetOpName(op);
            var ops = type.Methods[opName];
            return Algorithms.Find(ops, m => IsUnaryOperator(m, op, type));
        }

        public static bool IsUnaryOperator(IMethod m, UnaryOperator op, IType type)
        {
            if (m.Parameters.Count != 1) return false;
            //if (m.Name != GetOpName(op)) return false; //already checked
            if (m.Parameters[0].Type != type) return false;
            return true;
        }

        public static IMethod FindBooleanOperator(IType type, bool isTrue)
        {
            string opName = isTrue ? OpNames.True : OpNames.False;
            var ops = type.Methods[opName];
            return Algorithms.Find(ops, IsBooleanOperator);
        }

        static bool IsBooleanOperator(IMethod m)
        {
            if (m.Parameters.Count != 1) return false;
            if (m.Type != SystemTypes.Boolean) return false;
            //name is already checked.
            //if (isTrue) return m.Name == OpNames.True;
            //return m.Name == OpNames.False;
            return true;
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