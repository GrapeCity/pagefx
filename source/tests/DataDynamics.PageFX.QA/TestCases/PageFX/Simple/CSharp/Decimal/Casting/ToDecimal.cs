using System;

class X
{
    static void PrintDecimal(decimal d)
    {
        Console.WriteLine(d);
    }

    #region Int8
    private static readonly sbyte[] int8_arr = 
        {
            sbyte.MinValue,
            sbyte.MaxValue,
            0,
            10,
            20,
        };

    static void FromInt8()
    {
        Console.WriteLine("Int8");
        for (int i = 0; i < int8_arr.Length; ++i)
        {
            sbyte v = int8_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    #region UInt8
    private static readonly byte[] uint8_arr = 
        {
            byte.MinValue,
            byte.MaxValue,
            10,
            20,            
        };

    static void FromUInt8()
    {
        Console.WriteLine("UInt8");
        for (int i = 0; i < uint8_arr.Length; ++i)
        {
            byte v = uint8_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    #region Char
    private static readonly char[] arr_char =
        {
            'a', 'b', '\u2000',
        };

    static void FromChars()
    {
        Console.WriteLine("Char");
        for (int i = 0; i < arr_char.Length; ++i)
        {
            PrintDecimal((decimal)arr_char[i]);
        }
    }
    #endregion

    #region Int16
    private static readonly Int16[] int16_arr = 
        {
            Int16.MinValue,
            Int16.MaxValue,
            0,
            10,
            20,
        };

    static void FromInt16()
    {
        Console.WriteLine("Int16");
        for (int i = 0; i < int16_arr.Length; ++i)
        {
            Int16 v = int16_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    #region UInt16
    private static readonly UInt16[] uint16_arr = 
        {
            UInt16.MinValue,
            UInt16.MaxValue,
            0,
            10,
            20,
        };

    static void FromUInt16()
    {
        Console.WriteLine("UInt16");
        for (int i = 0; i < uint16_arr.Length; ++i)
        {
            UInt16 v = uint16_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    #region Int32
    private static readonly int[] int32_arr =
        {
            int.MinValue,
            int.MaxValue,
            0,
            1,
            -1,
            10,
        };

    static void FromInt32()
    {
        Console.WriteLine("Int32");
        for (int i = 0; i < int32_arr.Length; ++i)
        {
            int v = int32_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    #region UInt32
    private static readonly uint[] uint32_arr =
        {
            uint.MinValue,
            uint.MaxValue,
            10,
            20,
        };

    static void FromUInt32()
    {
        Console.WriteLine("UInt32");
        for (int i = 0; i < uint32_arr.Length; ++i)
        {
            uint v = uint32_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    #region Int64
    private static readonly Int64[] int64_arr =
        {
            Int64.MinValue,
            Int64.MaxValue,
            0,
            1,
            -1,
            10,
        };

    static void FromInt64()
    {
        Console.WriteLine("Int64");
        for (int i = 0; i < int64_arr.Length; ++i)
        {
            Int64 v = int64_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    #region UInt64
    private static readonly UInt64[] uint64_arr =
        {
            UInt64.MinValue,
            UInt64.MaxValue,
            10,
            20,
        };

    static void FromUInt64()
    {
        Console.WriteLine("UInt64");
        for (int i = 0; i < uint64_arr.Length; ++i)
        {
            UInt64 v = uint64_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    #region Single
    private static readonly float[] float_arr =
        {
            0f,
            0.1f,
            1.2f,
            -1.2f,
            10.7f,
        };

    static void FromSingle()
    {
        Console.WriteLine("Single");
        for (int i = 0; i < float_arr.Length; ++i)
        {
            float v = float_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    #region Double
    private static readonly double[] double_arr =
        {
            0,
            0.1,
            1.2,
            -1.2,
            10.7,
        };

    static void FromDouble()
    {
        Console.WriteLine("Double");
        for (int i = 0; i < double_arr.Length; ++i)
        {
            double v = double_arr[i];
            PrintDecimal((decimal)v);
        }
    }
    #endregion

    static void Main()
    {
        FromInt8();
        FromUInt8();
        FromInt16();
        FromUInt16();
        FromChars();
        FromInt32();
        FromUInt32();
        FromInt64();
        FromUInt64();
        FromSingle();
        FromDouble();
        Console.WriteLine("<%END%>");

    }
}