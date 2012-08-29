using System;

class CastPrimitivesTest
{
    static void f(sbyte a)
    {
        Console.WriteLine(a);
    }

    static void f(byte a)
    {
        Console.WriteLine(a);
    }

    static void f(short a)
    {
        Console.WriteLine(a);
    }

    static void f(ushort a)
    {
        Console.WriteLine(a);
    }

    static void f(char a)
    {
        Console.WriteLine((int)a);
    }

    static void f(int a)
    {
        Console.WriteLine(a);
    }

    static void f(uint a)
    {
        Console.WriteLine(a);
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
		Console.WriteLine("to u1:");
		byte u1 = (byte)v;
		Console.WriteLine(u1);
		Console.WriteLine("to i2:");
		var i2 = (short)v;
		Console.WriteLine(i2);
		Console.WriteLine("to u2:");
		var u2 = (ushort)v;
		Console.WriteLine(u2);
		Console.WriteLine("to i4:");
		var i4 = (int)v;
		Console.WriteLine(i4);
		Console.WriteLine("to u4:");
		var u4 = (uint)v;
		Console.WriteLine(u4);
		//Console.WriteLine("to ch:");
		//var c = (char)v;
		//Console.WriteLine(c);
		//f((long)v);
		//f((ulong)v);
		//f((float)v);
		//f((double)v);
    }

    static void test_i1()
    {
		Console.WriteLine("## i1 tests");
	    Console.WriteLine("### -100");
        test_i1(-100);
		Console.WriteLine("### 100");
        test_i1(100);
    }

    static void test_u1(byte v)
    {
	    Console.WriteLine(v);
	    Console.WriteLine("to i1:");
        f((sbyte)v);
        //f((byte)v);
		Console.WriteLine("to ch:");
        f((char)v);
		Console.WriteLine("to i2:");
        f((short)v);
		Console.WriteLine("to u2:");
        f((ushort)v);
		Console.WriteLine("to i4:");
        f((int)v);
		Console.WriteLine("to u4:");
        f((uint)v);
        //f((long)v);
        //f((ulong)v);
        //f((float)v);
        //f((double)v);
    }

    static void test_u1()
    {
		Console.WriteLine("## u1 tests");
        test_u1(0xFF);
        test_u1(0x88);
        test_u1(byte.MinValue);
        test_u1(byte.MinValue);
    }

    static void test_i2(short v)
    {
	    Console.WriteLine(v);
	    Console.WriteLine("to i1:");
        f((sbyte)v);
		Console.WriteLine("to u1:");
        f((byte)v);
		Console.WriteLine("to ch:");
        f((char)v);
        //f((short)v);
		Console.WriteLine("to i2:");
        f((ushort)v);
		Console.WriteLine("to i4:");
        f((int)v);
		Console.WriteLine("to u4:");
        f((uint)v);
        //f((long)v);
        //f((ulong)v);
        //f((float)v);
        //f((double)v);
    }

    static void test_i2()
    {
		Console.WriteLine("## i2 tests");
        test_i2(-1);
        test_i2(1000);
        test_i2(-1000);
        test_i2(short.MaxValue);
        test_i2(short.MinValue);
    }

    static void test_u2(ushort v)
    {
	    Console.WriteLine("## test u2");
        Console.WriteLine(v);
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
	    Console.WriteLine("## test i4");
        Console.WriteLine(v);
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
	    Console.WriteLine("## test u4");
        Console.WriteLine(v);
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
        //test_f4();
        //test_f8();
		Console.WriteLine("<%END%>");
    }
}
