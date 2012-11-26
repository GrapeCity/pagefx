//http://en.wikipedia.org/wiki/De_Morgan's_laws
//http://planetmath.org/encyclopedia/DeMorgansLaws.html
//!(a && b) == !a || !b
//!(a || b) == !a && !b

//Clauses:
//a && b == !(!a || !b)
//a || b == !(!a && !b)
//a && (b || c) == (a && b) || (a && c)
//a || (b && c) == (a || b) && (a || c) 
//a ? b : c == (a || b) && (!a || c)
//a ? b : c == (a && b) || (!a && c)

//Ternary Operator to Boolean Expression
//e = a ? b : c
//if b == 1 then e = a || c
//if b == 0 then e = !a && c
//if c == 0 then e = a && b
//if c == 1 then e = !a || b

using System;
using DataDynamics.PageFX.CodeModel.Expressions;

namespace DataDynamics.PageFX.CodeModel
{
    public static class BooleanAlgebra
    {
	    public static int ToBooleanConstant(IExpression e)
        {
            var ce = e as IConstantExpression;
            if (ce != null)
            {
                var c = ce.Value as IConvertible;
                if (c != null)
                {
                    bool v = c.ToBoolean(null);
                    return v ? 1 : 0;
                }
            }
            return -1;
        }

        public static bool IsFalse(IExpression e)
        {
            return ToBooleanConstant(e) == 0;
        }

        public static bool IsTrue(IExpression e)
        {
            return ToBooleanConstant(e) == 1;
        }

        public static bool IsBooleanOperator(BinaryOperator op)
        {
            return op == BinaryOperator.BooleanAnd || op == BinaryOperator.BooleanOr;
        }

        public static BinaryOperator InvertBooleanOperator(BinaryOperator op)
        {
            if (op == BinaryOperator.BooleanAnd)
                return BinaryOperator.BooleanOr;
            return BinaryOperator.BooleanAnd;
        }

        public static bool IsBooleanExpression(IExpression e)
        {
            var be = e as IBinaryExpression;
            if (be != null)
                return IsBooleanOperator(be.Operator);
            return false;
        }

        private static IExpression Simplify(IConditionExpression e)
        {
            var a = e.Condition;
            e.TrueExpression = Simplify(e.TrueExpression);
            e.FalseExpression = Simplify(e.FalseExpression);
            var b = e.TrueExpression;
            var c = e.FalseExpression;

            int v = ToBooleanConstant(b);
            if (v == 1) return new BinaryExpression(a, c, BinaryOperator.BooleanOr);
            if (v == 0) return new BinaryExpression(Not(a), c, BinaryOperator.BooleanAnd);

            v = ToBooleanConstant(c);
            if (v == 0) return new BinaryExpression(a, b, BinaryOperator.BooleanAnd);
            if (v == 1) return new BinaryExpression(Not(a), b, BinaryOperator.BooleanOr);

            return e;
        }

        private static IExpression Not(IBinaryExpression be, BinaryOperator op)
        {
            var left = Not(be.Left);
            var right = Not(be.Right);
            left = Simplify(left);
            right = Simplify(right);
            return new BinaryExpression(left, right, op);
        }

        private static IBinaryExpression BinExp(IBinaryExpression be, BinaryOperator op)
        {
            var left = Simplify(be.Left);
            var right = Simplify(be.Right);
            return new BinaryExpression(left, right, op);
        }

        private static IBinaryExpression InvertRelation(IBinaryExpression be)
        {
            var op = be.Operator;
            switch (op)
            {
                case BinaryOperator.LessThan:
                    return BinExp(be, BinaryOperator.GreaterThanOrEqual);
                case BinaryOperator.LessThanOrEqual:
                    return BinExp(be, BinaryOperator.GreaterThan);
                case BinaryOperator.GreaterThan:
                    return BinExp(be, BinaryOperator.LessThanOrEqual);
                case BinaryOperator.GreaterThanOrEqual:
                    return BinExp(be, BinaryOperator.LessThan);
                case BinaryOperator.Equality:
                    return BinExp(be, BinaryOperator.Inequality);
                case BinaryOperator.Inequality:
                    return BinExp(be, BinaryOperator.Equality);
            }
            return be;
        }

        private static IExpression InvertBoolOrRelation(IBinaryExpression be)
        {
            var op = be.Operator;
            switch (op)
            {
                case BinaryOperator.BooleanOr:
                    return Not(be, BinaryOperator.BooleanAnd);
                case BinaryOperator.BooleanAnd:
                    return Not(be, BinaryOperator.BooleanOr);
            }
            return InvertRelation(be);
        }

        public static IExpression Simplify(IUnaryExpression e)
        {
            if (e.Operator == UnaryOperator.BooleanNot)
            {
                var be = e.Expression as IBinaryExpression;
                if (be != null)
                {
                    var be2 = InvertBoolOrRelation(be);
                    if (!ReferenceEquals(be2, be)) return be2;
                }
                var e2 = Simplify(e.Expression);
                if (!ReferenceEquals(e2, e.Expression))
                    return new UnaryExpression(e2, UnaryOperator.BooleanNot);
            }
            return e;
        }

        private static IExpression SimplifyEquality(IExpression left, IExpression right, BinaryOperator op)
        {
            if (left.ResultType.Is(SystemTypeCode.Boolean))
            {
                if (op == BinaryOperator.Equality)
                {
                    int c = ToBooleanConstant(right);
                    if (c == 0) return Not(left);
                    if (c == 1) return left;
                }
                else if (op == BinaryOperator.Inequality)
                {
                    int c = ToBooleanConstant(right);
                    if (c == 0) return left;
                    if (c == 1) return Not(left);
                }
            }
            return null;
        }

        public static bool IsNot(IExpression e)
        {
            var ue = e as IUnaryExpression;
            if (ue == null) return false;
            return ue.Operator == UnaryOperator.BooleanNot;
        }

        public static bool IsRelationOperator(BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.LessThan:
                case BinaryOperator.LessThanOrEqual:
                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanOrEqual:
                case BinaryOperator.Equality:
                case BinaryOperator.Inequality:
                    return true;
            }
            return false;
        }

        public static bool IsRelation(IExpression e)
        {
            var be = e as IBinaryExpression;
            if (be == null) return false;
            return IsRelationOperator(be.Operator);
        }

        public static IExpression Simplify(IBinaryExpression e)
        {
            var op = e.Operator;
            if (IsBooleanOperator(op))
            {
                var left = Simplify(e.Left);
                var right = Simplify(e.Right);
                if (IsNot(left) && IsRelation(right))
                {
                    op = InvertBooleanOperator(op);
                    left = Not(left);
                    right = InvertRelation(right as IBinaryExpression);
                    return new BinaryExpression(left, right, op);
                }
                if (!ReferenceEquals(left, e.Left) || !ReferenceEquals(right, e.Right))
                    return new BinaryExpression(left, right, op);
                return e;
            }

            var e2 = SimplifyEquality(e.Left, e.Right, op);
            if (e2 != null) return e2;

            e2 = SimplifyEquality(e.Right, e.Left, op);
            if (e2 != null) return e2;

            return e;
        }

        public static IExpression Simplify(IExpression e)
        {
            var type = e.ResultType;
            if (type.Is(SystemTypeCode.Boolean))
            {
                var ce = e as IConditionExpression;
                if (ce != null)
                    return Simplify(ce);

                var ue = e as IUnaryExpression;
                if (ue != null)
                    return Simplify(ue);

                var be = e as IBinaryExpression;
                if (be != null)
                    return Simplify(be);

                return e;
            }
            return SimplifyAlt(e);
        }

        //Simplifies non boolean expression
        private static IExpression SimplifyAlt(IExpression e)
        {
            var ce = e as IConditionExpression;
            if (ce != null)
            {
                var cond = ce.Condition;
                var ue = cond as IUnaryExpression;
                if (ue != null && ue.Operator == UnaryOperator.BooleanNot)
                {
                    ce.Condition = ue.Expression;
                    var t = ce.TrueExpression;
                    ce.TrueExpression = ce.FalseExpression;
                    ce.FalseExpression = t;
                }
            }
            return e;
        }

        public static IExpression Not(IExpression e)
        {
            var ue = e as IUnaryExpression;
            if (ue != null)
            {
                if (ue.Operator == UnaryOperator.BooleanNot)
                    return ue.Expression;
            }

            IExpression e2;
            var be = e as IBinaryExpression;
            if (be != null)
            {
                e2 = InvertBoolOrRelation(be);
                if (!ReferenceEquals(e2, e)) return e2;
            }

            e2 = ToBool(e, BinaryOperator.Equality);
            if (!ReferenceEquals(e2, e)) return e2;

            return new UnaryExpression(e, UnaryOperator.BooleanNot);
        }

        public static IExpression And(IExpression left, IExpression right)
        {
            IBinaryExpression be = new BinaryExpression(left, right, BinaryOperator.BooleanAnd);
            return Simplify(be);
        }

        public static IExpression Or(IExpression left, IExpression right)
        {
            IBinaryExpression be = new BinaryExpression(left, right, BinaryOperator.BooleanOr);
            return Simplify(be);
        }

        private static IExpression ToBool(IExpression e, BinaryOperator op)
        {
            var type = e.ResultType;
	        if (type.Is(SystemTypeCode.Boolean))
	        {
		        return e;
	        }

	        var st = type.SystemType();
	        if (st != null)
	        {
		        switch (st.Code)
		        {
			        case SystemTypeCode.Int8:
			        case SystemTypeCode.UInt8:
			        case SystemTypeCode.Int16:
			        case SystemTypeCode.UInt16:
			        case SystemTypeCode.Int32:
			        case SystemTypeCode.UInt32:
			        case SystemTypeCode.Int64:
			        case SystemTypeCode.UInt64:
			        case SystemTypeCode.Single:
			        case SystemTypeCode.Double:
			        case SystemTypeCode.Decimal:
				        return new BinaryExpression(e, new ConstExpression(0), op);

			        case SystemTypeCode.Char:
				        return new BinaryExpression(e, new ConstExpression('\0'), op);
		        }
	        }
			
	        return new BinaryExpression(e, new ConstExpression(null), op);
        }

        public static IExpression ToBool(IExpression e)
        {
            var ce = e as IConstantExpression;
            if (ce != null)
            {
                var c = ce.Value as IConvertible;
                if (c != null)
                {
                    bool v = c.ToBoolean(null);
                    ce.Value = v;
                    return ce;
                }
            }
            return ToBool(e, BinaryOperator.Inequality);
        }
    }
}