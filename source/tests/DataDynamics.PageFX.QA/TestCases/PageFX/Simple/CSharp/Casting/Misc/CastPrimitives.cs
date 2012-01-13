using System;

class CastPrimitivesTest
{
    static void f(sbyte a)
    {
        Console.WriteLine("i1 {0}", a);
    }

    static void f(byte a)
    {
        Console.WriteLine("u1 {0}", a);
    }

    static void f(short a)
    {
        Console.WriteLine("i2 {0}", a);
    }

    static void f(ushort a)
    {
        Console.WriteLine("u2 {0}", a);
    }

    static void f(char a)
    {
        Console.WriteLine("ch {0}", (int)a);
    }

    static void f(int a)
    {
        Console.WriteLine("i4 {0}", a);
    }

    static void f(uint a)
    {
        Console.WriteLine("u4 {0}", a);
    }

    //static void f(long a)
    //{
    //    Console.WriteLine("i8 {0}", a);
    //}

    //static void f(ulong a)
    //{
    //    Console.WriteLine("u8 {0}", a);
    //}

    //static void f(float a)
    //{
    //    Console.WriteLine("f4 {0}", a);
    //}

    //static void f(double a)
    //{
    //    Console.WriteLine("f8 {0}", a);
    //}

    static void test_i1(sbyte v)
    {
        Console.WriteLine("test_i1 {0}", v);
        //f((sbyte)v);
        f((byte)v);
        f((char)v);
        f((short)v);
        f((ushort)v);
        f((int)v);
        f((uint)v);
        //f((long)v);
        //f((ulong)v);
        //f((float)v);
        //f((double)v);
    }

    static void test_i1()
    {
        test_i1(-100);
        test_i1(100);
    }

    static void test_u1(byte v)
    {
        Console.WriteLine("test_u1 {0}", v);
        f((sbyte)v);
        //f((byte)v);
        f((char)v);
        f((short)v);
        f((ushort)v);
        f((int)v);
        f((uint)v);
        //f((long)v);
        //f((ulong)v);
        //f((float)v);
        //f((double)v);
    }

    static void test_u1()
    {
        test_u1(0xFF);
        test_u1(0x88);
        test_u1(byte.MinValue);
        test_u1(byte.MinValue);
    }

    static void test_i2(short v)
    {
        Console.WriteLine("test_i2 {0}", v);
        f((sbyte)v);
        f((byte)v);
        f((char)v);
        //f((short)v);
        f((ushort)v);
        f((int)v);
        f((uint)v);
        //f((long)v);
        //f((ulong)v);
        //f((float)v);
        //f((double)v);
    }

    static void test_i2()
    {
        test_i2(-1);
        test_i2(1000);
        test_i2(-1000);
        test_i2(short.MaxValue);
        test_i2(short.MinValue);
    }

    static void test_u2(ushort v)
    {
        Console.WriteLine("test_i2 {0}", v);
        f((sbyte)v);
        f((byte)v);
        f((char)v);
        f((short)v);
        //f((ushort)v);
        f((int)v);
        f((uint)v);
        //f((long)v);
        //f((ulong)v);
        //f((float)v);
        //f((double)v);
    }

    static void test_u2()
    {
        test_u2(1000);
        test_u2(ushort.MaxValue);
        test_u2(ushort.MinValue);
    }

    static void test_i4(int v)
    {
        Console.WriteLine("test_i4 {0}", v);
        f((sbyte)v);
        f((byte)v);
        f((char)v);
        f((short)v);
        f((ushort)v);
        //f((int)v);
        f((uint)v);
        //f((long)v);
        //f((ulong)v);
        //f((float)v);
        //f((double)v);
    }

    static void test_i4()
    {
        test_i4(-1);
        test_i4(100000);
        test_i4(-100000);
        test_i4(int.MaxValue);
        test_i4(int.MinValue);
    }

    static void test_u4(uint v)
    {
        Console.WriteLine("test_u4 {0}", v);
        f((sbyte)v);
        f((byte)v);
        f((char)v);
        f((short)v);
        f((ushort)v);
        f((int)v);
        //f((uint)v);
        //f((long)v);
        //f((ulong)v);
        //f((float)v);
        //f((double)v);
    }

    static void test_u4()
    {
        test_i4(100000);
        test_u4(uint.MaxValue);
        test_u4(uint.MinValue);
    }

    //static void test_i8(long v)
    //{
    //    Console.WriteLine("----- conv_i8 -----");
    //    f((sbyte)v);
    //    f((byte)v);
    //    f((char)v);
    //    f((short)v);
    //    f((ushort)v);
    //    f((int)v);
    //    f((uint)v);
    //    f((long)v);
    //    f((ulong)v);
    //    f((float)v);
    //    f((double)v);
    //}

    //static void test_u8(ulong v)
    //{
    //    Console.WriteLine("----- conv_u8 -----");
    //    f((sbyte)v);
    //    f((byte)v);
    //    f((char)v);
    //    f((short)v);
    //    f((ushort)v);
    //    f((int)v);
    //    f((uint)v);
    //    f((long)v);
    //    f((ulong)v);
    //    f((float)v);
    //    f((double)v);
    //}

    //static void test_f4(float v)
    //{
    //    Console.WriteLine("test_f4 {0}", v);
    //    f((sbyte)v);
    //    f((byte)v);
    //    f((char)v);
    //    f((short)v);
    //    f((ushort)v);
    //    f((int)v);
    //    f((uint)v);
    //    //f((long)v);
    //    //f((ulong)v);
    //    //f((float)v);
    //    f((double)v);
    //}

    //static void test_f4()
    //{
    //    test_f4(3.14f);
    //    test_f4(float.MaxValue);
    //    test_f4(float.MinValue);
    //}

    //static void test_f8(double v)
    //{
    //    Console.WriteLine("test_f8 {0}", v);
    //    f((sbyte)v);
    //    f((byte)v);
    //    f((char)v);
    //    f((short)v);
    //    f((ushort)v);
    //    f((int)v);
    //    f((uint)v);
    //    //f((long)v);
    //    //f((ulong)v);
    //    f((float)v);
    //    //f((double)v);
    //}

    //static void test_f8()
    //{
    //    test_f8(3.14);
    //    test_f8(double.MaxValue);
    //    test_f8(double.MinValue);
    //}

    static void Main()
    {
        test_i1();
        test_u1();
        test_i2();
        test_u2();
        test_i4();
        test_u4();
        Console.WriteLine("<%END%>");
        //test_f4();
        //test_f8();
    }
}
