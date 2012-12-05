using System;

unsafe class Unsafe2
{
    private const int N = 5;

    static void i1()
    {
        sbyte[] arr = new sbyte[N];
        fixed (sbyte* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = (sbyte)i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void u1()
    {
        byte[] arr = new byte[N];
        fixed (byte* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = (byte)i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void i2()
    {
        short[] arr = new short[N];
        fixed (short* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = (short)i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void u2()
    {
        ushort[] arr = new ushort[N];
        fixed (ushort* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = (ushort)i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void i4()
    {
        int[] arr = new int[N];
        fixed (int* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void u4()
    {
        uint[] arr = new uint[N];
        fixed (uint* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = (uint)i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void i8()
    {
        long[] arr = new long[N];
        fixed (long* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void u8()
    {
        ulong[] arr = new ulong[N];
        fixed (ulong* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = (ulong)i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void f4()
    {
        float[] arr = new float[N];
        fixed (float* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void f8()
    {
        double[] arr = new double[N];
        fixed (double* ptr = &arr[0])
        {
            for (int i = 0; i < N; ++i)
            {
                *(ptr + i) = i;
                Console.WriteLine(*(ptr + i));
            }
        }
    }

    static void Main()
    {
        i1();
        u1();
        i2();
        u2();
        i4();
        u4();
        i8();
        u8();
        f4();
        f8();
    }
}