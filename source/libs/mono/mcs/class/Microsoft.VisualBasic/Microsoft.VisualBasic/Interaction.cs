namespace Microsoft.VisualBasic
{
    using Microsoft.VisualBasic.CompilerServices;
    using System;

    [StandardModule]
    public sealed class Interaction
    {
        public static object Choose(double Index, params object[] Choice)
        {
            int index = (int) Math.Round((double) (Conversion.Fix(Index) - 1.0));
            if (Choice.Rank != 1)
            {
                throw new ArgumentException(Utils.GetResourceString("Argument_RankEQOne1", new string[] { "Choice" }));
            }
            if ((index >= 0) && (index <= Choice.GetUpperBound(0)))
            {
                return Choice[index];
            }
            return null;
        }

        public static object IIf(bool Expression, object TruePart, object FalsePart)
        {
            if (Expression)
            {
                return TruePart;
            }
            return FalsePart;
        }

        internal static T IIf<T>(bool Condition, T TruePart, T FalsePart)
        {
            if (Condition)
            {
                return TruePart;
            }
            return FalsePart;
        }

        private static void InsertNumber(ref string Buffer, long Num, long Spaces)
        {
            string expression = Conversions.ToString(Num);
            InsertSpaces(ref Buffer, Spaces - Strings.Len(expression));
            Buffer = Buffer + expression;
        }

        private static void InsertSpaces(ref string Buffer, long Spaces)
        {
            while (Spaces > 0L)
            {
                Buffer = Buffer + " ";
                Spaces -= 1L;
            }
        }

        public static string Partition(long Number, long Start, long Stop, long Interval)
        {
            string buffer = null;
            long num = 0;
            bool flag = false;
            bool flag2 = false;
            long num2;
            long num3 = 0;
            if (Start < 0L)
            {
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Start" }));
            }
            if (Stop <= Start)
            {
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Stop" }));
            }
            if (Interval < 1L)
            {
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Interval" }));
            }
            if (Number < Start)
            {
                num3 = Start - 1L;
                flag = true;
            }
            else if (Number > Stop)
            {
                num = Stop + 1L;
                flag2 = true;
            }
            else if (Interval == 1L)
            {
                num = Number;
                num3 = Number;
            }
            else
            {
                num = (((Number - Start) / Interval) * Interval) + Start;
                num3 = (num + Interval) - 1L;
                if (num3 > Stop)
                {
                    num3 = Stop;
                }
                if (num < Start)
                {
                    num = Start;
                }
            }
            string expression = Conversions.ToString((long) (Stop + 1L));
            string str3 = Conversions.ToString((long) (Start - 1L));
            if (Strings.Len(expression) > Strings.Len(str3))
            {
                num2 = Strings.Len(expression);
            }
            else
            {
                num2 = Strings.Len(str3);
            }
            if (flag)
            {
                expression = Conversions.ToString(num3);
                if (num2 < Strings.Len(expression))
                {
                    num2 = Strings.Len(expression);
                }
            }
            if (flag)
            {
                InsertSpaces(ref buffer, num2);
            }
            else
            {
                InsertNumber(ref buffer, num, num2);
            }
            buffer = buffer + ":";
            if (flag2)
            {
                InsertSpaces(ref buffer, num2);
                return buffer;
            }
            InsertNumber(ref buffer, num3, num2);
            return buffer;
        }

        public static object Switch(params object[] VarExpr)
        {
            if (VarExpr != null)
            {
                int length = VarExpr.Length;
                int index = 0;
                if ((length % 2) != 0)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "VarExpr" }));
                }
                while (length > 0)
                {
                    if (Conversions.ToBoolean(VarExpr[index]))
                    {
                        return VarExpr[index + 1];
                    }
                    index += 2;
                    length -= 2;
                }
            }
            return null;
        }
    }
}

