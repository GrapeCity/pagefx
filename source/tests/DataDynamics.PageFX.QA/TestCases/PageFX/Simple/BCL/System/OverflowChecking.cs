using System;

internal delegate void Func();

class X
{
    private enum BinOp
    {
        Add,
        Sub,
        Mul,
        Div
    }

    static string ToString(BinOp op)
    {
        switch (op)
        {
            case BinOp.Add:
                return "+";
            case BinOp.Sub:
                return "-";
            case BinOp.Mul:
                return "*";
            case BinOp.Div:
                return "/";
            default:
                throw new ArgumentOutOfRangeException("op");
        }
    }

    static object Op(object x, object y, BinOp op)
    {
        TypeCode type = Type.GetTypeCode(x.GetType());
        checked
        {
            switch (op)
            {
                case BinOp.Add:
                    switch (type)
                    {
                        case TypeCode.Byte:
                            return (byte)x + (byte)y;
                        case TypeCode.SByte:
                            return (sbyte)x + (sbyte)y;
                        case TypeCode.Int16:
                            return (short)x + (short)y;
                        case TypeCode.UInt16:
                            return (ushort)x + (ushort)y;
                        case TypeCode.Int32:
                            return (int)x + (int)y;
                        case TypeCode.UInt32:
                            return (uint)x + (uint)y;
                        case TypeCode.Int64:
                            return (long)x + (long)y;
                        case TypeCode.UInt64:
                            return (ulong)x + (ulong)y;
                        default:
                            throw new InvalidOperationException();
                    }

                case BinOp.Sub:
                    switch (type)
                    {
                        case TypeCode.Byte:
                            return (byte)x - (byte)y;
                        case TypeCode.SByte:
                            return (sbyte)x - (sbyte)y;
                        case TypeCode.Int16:
                            return (short)x - (short)y;
                        case TypeCode.UInt16:
                            return (ushort)x - (ushort)y;
                        case TypeCode.Int32:
                            return (int)x - (int)y;
                        case TypeCode.UInt32:
                            return (uint)x - (uint)y;
                        case TypeCode.Int64:
                            return (long)x - (long)y;
                        case TypeCode.UInt64:
                            return (ulong)x - (ulong)y;
                        default:
                            throw new InvalidOperationException();
                    }

                case BinOp.Mul:
                    switch (type)
                    {
                        case TypeCode.Byte:
                            return (byte)x * (byte)y;
                        case TypeCode.SByte:
                            return (sbyte)x * (sbyte)y;
                        case TypeCode.Int16:
                            return (short)x * (short)y;
                        case TypeCode.UInt16:
                            return (ushort)x * (ushort)y;
                        case TypeCode.Int32:
                            return (int)x * (int)y;
                        case TypeCode.UInt32:
                            return (uint)x * (uint)y;
                        case TypeCode.Int64:
                            return (long)x * (long)y;
                        case TypeCode.UInt64:
                            return (ulong)x * (ulong)y;
                        default:
                            throw new InvalidOperationException();
                    }

                case BinOp.Div:
                    switch (type)
                    {
                        case TypeCode.Byte:
                            return (byte)x / (byte)y;
                        case TypeCode.SByte:
                            return (sbyte)x / (sbyte)y;
                        case TypeCode.Int16:
                            return (short)x / (short)y;
                        case TypeCode.UInt16:
                            return (ushort)x / (ushort)y;
                        case TypeCode.Int32:
                            return (int)x / (int)y;
                        case TypeCode.UInt32:
                            return (uint)x / (uint)y;
                        case TypeCode.Int64:
                            return (long)x / (long)y;
                        case TypeCode.UInt64:
                            return (ulong)x / (ulong)y;
                        default:
                            throw new InvalidOperationException();
                    }

                default:
                    throw new ArgumentOutOfRangeException("op");
            }
        }
    }

    static void Check(object x, object y, BinOp op, bool ovf)
    {
        string exp = string.Format("{0} {1} {2}", x, ToString(op), y);
        Console.WriteLine("Overflow is {0}expected in ({1})!!!", ovf ? "" : "un", exp);
        try
        {
            checked
            {
                Console.WriteLine(Op(x, y, op));
                if (ovf)
                    Console.WriteLine("error PFC0001: no expected overflow for ({0})", exp);
                else
                    Console.WriteLine("ok");
            }
        }
        catch (OverflowException)
        {
            if (ovf)
                Console.WriteLine("ok");
            else
                Console.WriteLine("error PFC0002: unexpected overflow for ({0})", exp);
        }
    }

    static void TestOvfSigned()
    {
        {
            Console.WriteLine("--- Int8");
            sbyte a = sbyte.MaxValue;
            sbyte b = 1;
            sbyte z = 0;
            Check(a, b, BinOp.Add, true);
            Check(a, z, BinOp.Add, false);

            a = sbyte.MinValue;
            Check(a, b, BinOp.Sub, true);
        }

        {
            Console.WriteLine("--- Int16");
            short a = short.MaxValue;
            short b = 1;
            short z = 0;
            Check(a, b, BinOp.Add, true);
            Check(a, z, BinOp.Add, false);

            a = short.MinValue;
            Check(a, b, BinOp.Sub, true);
        }

        {
            Console.WriteLine("--- Int32");
            Check(int.MaxValue, 1, BinOp.Add, true);
            Check(int.MaxValue, 0, BinOp.Add, false);
            Check(int.MinValue, 1, BinOp.Sub, true);
            Check(int.MaxValue, int.MaxValue, BinOp.Mul, true);
        }

        {
            Console.WriteLine("--- Int64");
            long a = long.MaxValue;
            long b = 1;
            long z = 0;
            Check(a, b, BinOp.Add, true);
            Check(a, z, BinOp.Add, false);

            a = long.MinValue;
            Check(a, b, BinOp.Sub, true);
        }

        //{
        //    sbyte a = sbyte.MaxValue;
        //    sbyte b = 1;
        //    Op(a, b, BinOp.Add);
        //}

        //{
        //    long a = long.MaxValue;
        //    long b = 1;
        //    Op(a, b, BinOp.Add);
        //}
    }

    static void TestOvfUnsigned()
    {
        {
            Console.WriteLine("--- UInt32");
            uint a = uint.MaxValue;
            uint b = 1u;
            Check(a, b, BinOp.Add, true);
            Check(a, b, BinOp.Mul, false);

            b = a;
            Check(a, b, BinOp.Mul, true);
        }

        {
            Console.WriteLine("--- UInt64");
            ulong a = ulong.MaxValue;
            ulong b = 1u;
            Check(a, b, BinOp.Add, true);
            Check(a, b, BinOp.Mul, false);

            b = a;
            Check(a, b, BinOp.Mul, true);
        }
    }

    static void Main()
    {
        try
        {
            TestOvfSigned();
            TestOvfUnsigned();
        }
        catch (Exception e)
        {
            Console.WriteLine("Unexpected exception");
        }
        Console.WriteLine("<%END%>");
    }
}