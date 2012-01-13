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
using T = ICollection<Double>;

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
    bool?[] a = new bool?[] { new bool?(), new bool?(false), new bool?(true) };
    TestCore(a);
}

static void FromUInt8()
{
    Console.WriteLine("--- FromUInt8");
    byte?[] a = new byte?[] { new byte?(), new byte?(20), new byte?(30) };
    TestCore(a);
}

static void FromInt8()
{
    Console.WriteLine("--- FromInt8");
    sbyte?[] a = new sbyte?[] { new sbyte?(), new sbyte?(20), new sbyte?(30) };
    TestCore(a);
}

static void FromInt16()
{
    Console.WriteLine("--- FromInt16");
    short?[] a = new short?[] { new short?(), new short?(20), new short?(30) };
    TestCore(a);
}

static void FromUInt16()
{
    Console.WriteLine("--- FromUInt16");
    ushort?[] a = new ushort?[] { new ushort?(), new ushort?(20), new ushort?(30) };
    TestCore(a);
}

static void FromChar()
{
    Console.WriteLine("--- FromChar");
    char?[] a = new char?[] { new char?(), new char?('b'), new char?('c') };
    TestCore(a);
}

static void FromInt32()
{
    Console.WriteLine("--- FromInt32");
    int?[] a = new int?[] { new int?(), new int?(20), new int?(30) };
    TestCore(a);
}

static void FromUInt32()
{
    Console.WriteLine("--- FromUInt32");
    uint?[] a = new uint?[] { new uint?(), new uint?(20), new uint?(30) };
    TestCore(a);
}

static void FromInt64()
{
    Console.WriteLine("--- FromInt64");
    long?[] a = new long?[] { new long?(), new long?(20), new long?(30) };
    TestCore(a);
}

static void FromUInt64()
{
    Console.WriteLine("--- FromUInt64");
    ulong?[] a = new ulong?[] { new ulong?(), new ulong?(20), new ulong?(30) };
    TestCore(a);
}

static void FromDouble()
{
    Console.WriteLine("--- FromDouble");
    double?[] a = new double?[] { new double?(), new double?(20), new double?(30) };
    TestCore(a);
}

static void FromEInt8()
{
    Console.WriteLine("--- FromEInt8");
    EInt8?[] a = new EInt8?[] { new EInt8?(), new EInt8?(EInt8.B), new EInt8?(EInt8.C), new EInt8?(EInt8.D) };
    TestCore(a);
}

static void FromEUInt8()
{
    Console.WriteLine("--- FromEUInt8");
    EUInt8?[] a = new EUInt8?[] { new EUInt8?(EUInt8.A), new EUInt8?(EUInt8.B), new EUInt8?(EUInt8.C), new EUInt8?(EUInt8.D) };
    TestCore(a);
}

static void FromEInt16()
{
    Console.WriteLine("--- FromEInt16");
    EInt16?[] a = new EInt16?[] { new EInt16?(EInt16.A), new EInt16?(EInt16.B), new EInt16?(EInt16.C), new EInt16?(EInt16.D) };
    TestCore(a);
}

static void FromEUInt16()
{
    Console.WriteLine("--- FromEUInt16");
    EUInt16?[] a = new EUInt16?[] { new EUInt16?(EUInt16.A), new EUInt16?(EUInt16.B), new EUInt16?(EUInt16.C), new EUInt16?(EUInt16.D) };
    TestCore(a);
}

static void FromEInt32()
{
    Console.WriteLine("--- FromEInt32");
    EInt32?[] a = new EInt32?[] { new EInt32?(EInt32.A), new EInt32?(EInt32.B), new EInt32?(EInt32.C), new EInt32?(EInt32.D) };
    TestCore(a);
}

static void FromEUInt32()
{
    Console.WriteLine("--- FromEUInt32");
    EUInt32?[] a = new EUInt32?[] { new EUInt32?(EUInt32.A), new EUInt32?(EUInt32.B), new EUInt32?(EUInt32.C), new EUInt32?(EUInt32.D) };
    TestCore(a);
}

static void FromEInt64()
{
    Console.WriteLine("--- FromEInt64");
    EInt64?[] a = new EInt64?[] { new EInt64?(EInt64.A), new EInt64?(EInt64.B), new EInt64?(EInt64.C), new EInt64?(EInt64.D) };
    TestCore(a);
}

static void FromEUInt64()
{
    Console.WriteLine("--- FromEUInt64");
    EUInt64?[] a = new EUInt64?[] { new EUInt64?(EUInt64.A), new EUInt64?(EUInt64.B), new EUInt64?(EUInt64.C), new EUInt64?(EUInt64.D) };
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
    TestCore(new Point?[]
    { 
        new Point?(),
        new Point?(new Point(10, 10)),
        new Point?(new Point(20, 20)),
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