using System;
using System.Collections;
using System.Collections.Generic;

enum EInt8 : sbyte { A = -1, B, C, D }
enum EUInt8 : byte { A, B, C, D }
enum EInt16 : short { A = -1, B, C, D }
enum EUInt16 : ushort { A, B, C, D }
enum EInt32 : int { A = -1, B, C, D }
enum EUInt32 : uint { A, B, C, D }
enum EInt64 : long { A = -1, B, C, D }
enum EUInt64 : ulong { A, B, C, D }

enum E2Int8 : sbyte { E = -1, F, G, K,}
enum E2UInt8 : byte { E, F, G, K,}
enum E2Int16 : short { E = -1, F, G, K,}
enum E2UInt16 : ushort { E, F, G, K,}
enum E2Int32 : int { E = -1, F, G, K,}
enum E2UInt32 : uint { E, F, G, K,}
enum E2Int64 : long { E = -1, F, G, K,}
enum E2UInt64 : ulong { E, F, G, K,}

struct Point :
    IComparable<Point>,
    IComparable<int>,
    IEquatable<Point>,
    IEquatable<int>,
    IComparable
{
    public int x, y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }

    public int Len
    {
        get { return (int)Math.Sqrt(x*x + y*y); }
    }

    public int CompareTo(Point p)
    {
        return Len - p.Len;
    }

    public int CompareTo(int value)
    {
        return Len - value;
    }

    public bool Equals(Point other)
    {
        return x == other.x && y == other.y;
    }

    public bool Equals(int other)
    {
        return Len == other;
    }

    public int CompareTo(Object obj)
    {
        if (obj is Point)
            return CompareTo((Point)obj);
        if (obj is int)
            return CompareTo((int)obj);
        throw new InvalidOperationException();
    }
}

namespace PageFX
{
using T = IEnumerable<UInt32>;

class X
{

static void print(T set)
{
    foreach (var item in set)
        Console.WriteLine(item);
}

static void TestAs(object a)
{
    Console.WriteLine("-- as");
    try
    {
        var b = a as T;
        print(b);
        Console.WriteLine(ReferenceEquals(a, b));
    }
    catch (Exception e)
    {
        Console.WriteLine(e.GetType());
    }
}

static void TestIs(object a)
{
    Console.WriteLine("-- is");
    try
    {
        Console.WriteLine(a is T);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.GetType());
    }
}

static void TestCast(object a)
{
    Console.WriteLine("-- cast");
    try
    {
        var b = (T)a;
        print(b);
        Console.WriteLine(ReferenceEquals(a, b));
    }
    catch (Exception e)
    {
        Console.WriteLine(e.GetType());
    }
}

static void TestCore(object a)
{
    TestAs(a);
    TestIs(a);
    TestCast(a);
}

static void FromBool()
{
    Console.WriteLine("--- FromBool");
    bool[] a = new bool[] { true, false, true };
    TestCore(a);
}

static void FromUInt8()
{
    Console.WriteLine("--- FromUInt8");
    byte[] a = new byte[] { 10, 20, 30 };
    TestCore(a);
}

static void FromInt8()
{
    Console.WriteLine("--- FromInt8");
    sbyte[] a = new sbyte[] { 10, 20, 30 };
    TestCore(a);
}

static void FromInt16()
{
    Console.WriteLine("--- FromInt16");
    short[] a = new short[] { 10, 20, 30 };
    TestCore(a);
}

static void FromUInt16()
{
    Console.WriteLine("--- FromUInt16");
    ushort[] a = new ushort[] { 10, 20, 30 };
    TestCore(a);
}

static void FromChar()
{
    Console.WriteLine("--- FromChar");
    char[] a = new char[] { 'a', 'b', 'c' };
    TestCore(a);
}

static void FromInt32()
{
    Console.WriteLine("--- FromInt32");
    int[] a = new int[] { 10, 20, 30 };
    TestCore(a);
}

static void FromUInt32()
{
    Console.WriteLine("--- FromUInt32");
    uint[] a = new uint[] { 10, 20, 30 };
    TestCore(a);
}

static void FromInt64()
{
    Console.WriteLine("--- FromInt64");
    long[] a = new long[] { 10, 20, 30 };
    TestCore(a);
}

static void FromUInt64()
{
    Console.WriteLine("--- FromUInt64");
    ulong[] a = new ulong[] { 10, 20, 30 };
    TestCore(a);
}

static void FromDouble()
{
    Console.WriteLine("--- FromDouble");
    double[] a = new double[] { 10, 20, 30 };
    TestCore(a);
}

static void FromEInt8()
{
    Console.WriteLine("--- FromEInt8");
    EInt8[] a = new EInt8[] { EInt8.A, EInt8.B, EInt8.C, EInt8.D };
    TestCore(a);
}

static void FromEUInt8()
{
    Console.WriteLine("--- FromEUInt8");
    EUInt8[] a = new EUInt8[] { EUInt8.A, EUInt8.B, EUInt8.C, EUInt8.D };
    TestCore(a);
}

static void FromEInt16()
{
    Console.WriteLine("--- FromEInt16");
    EInt16[] a = new EInt16[] { EInt16.A, EInt16.B, EInt16.C, EInt16.D };
    TestCore(a);
}

static void FromEUInt16()
{
    Console.WriteLine("--- FromEUInt16");
    EUInt16[] a = new EUInt16[] { EUInt16.A, EUInt16.B, EUInt16.C, EUInt16.D };
    TestCore(a);
}

static void FromEInt32()
{
    Console.WriteLine("--- FromEInt32");
    EInt32[] a = new EInt32[] { EInt32.A, EInt32.B, EInt32.C, EInt32.D };
    TestCore(a);
}

static void FromEUInt32()
{
    Console.WriteLine("--- FromEUInt32");
    EUInt32[] a = new EUInt32[] { EUInt32.A, EUInt32.B, EUInt32.C, EUInt32.D };
    TestCore(a);
}

static void FromEInt64()
{
    Console.WriteLine("--- FromEInt64");
    EInt64[] a = new EInt64[] { EInt64.A, EInt64.B, EInt64.C, EInt64.D };
    TestCore(a);
}

static void FromEUInt64()
{
    Console.WriteLine("--- FromEUInt64");
    EUInt64[] a = new EUInt64[] { EUInt64.A, EUInt64.B, EUInt64.C, EUInt64.D };
    TestCore(a);
}

static void FromNull()
{
    Console.WriteLine("--- FromNull");
    TestCore(null);
}

static void FromNullable()
{
    Console.WriteLine("--- FromNullable");
    TestCore(new int?());
}

static void FromString()
{
    Console.WriteLine("--- FromString");
    TestCore("aaa");
}

static void FromObject()
{
    Console.WriteLine("--- FromObject");
    TestCore(new object());
}

static void From2DArray()
{
    Console.WriteLine("--- From2DArray");
    T[,] arr = new T[1, 1];
    TestCore(arr);
}

static void FromObjectArray()
{
    Console.WriteLine("--- FromObjectArray");
    object[] a = new object[10];
    TestCore(a);
}

static void FromStruct()
{
    Console.WriteLine("--- FromStruct");
    TestCore(new Point[]
    { 
        new Point(10, 10),
        new Point(20, 20),
        new Point(30, 30),
    });
}

static void Main()
{
    FromBool();
    FromInt8();
    FromUInt8();
    FromInt16();
    FromUInt16();
    FromChar();
    FromInt32();
    FromUInt32();
    FromInt64();
    FromUInt64();
    FromDouble();
    FromEInt8();
    FromEUInt8();
    FromEInt16();
    FromEUInt16();
    FromEInt32();
    FromEUInt32();
    FromEInt64();
    FromEUInt64();
    FromNull();
    FromNullable();
    FromString();
    FromObject();
    From2DArray();
    FromObjectArray();
    FromStruct();
    Console.WriteLine("<%END%>");
}
}
}