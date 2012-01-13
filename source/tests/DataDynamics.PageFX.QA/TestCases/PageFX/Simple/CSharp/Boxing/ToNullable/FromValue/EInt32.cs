using System;

enum EInt8 : sbyte { A = -1, B, C, D }
enum EUInt8 : byte { A, B, C, D }
enum EInt16 : short { A = -1, B, C, D }
enum EUInt16 : ushort { A, B, C, D }
enum EInt32 : int { A = -1, B, C, D }
enum EUInt32 : uint { A, B, C, D }
enum EInt64 : long { A = -1, B, C, D }
enum EUInt64 : ulong { A, B, C, D }

enum E2Int8 : sbyte { E = -1, F, G, H }
enum E2UInt8 : byte { E, F, G, H }
enum E2Int16 : short { E = -1, F, G, H }
enum E2UInt16 : ushort { E, F, G, H }
enum E2Int32 : int { E = -1, F, G, H }
enum E2UInt32 : uint { E, F, G, H }
enum E2Int64 : long { E = -1, F, G, H }
enum E2UInt64 : ulong { E, F, G, H }

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
using T = Nullable<EInt32>;

class Test
{

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

static void TestUnbox(object a)
{
    Console.WriteLine("-- unbox");
    try
    {
        var b = (T)a;
        Console.WriteLine(ReferenceEquals(a, b));
    }
    catch (Exception e)
    {
        Console.WriteLine(e.GetType());
    }
}

static void TestCore(object a)
{
    TestUnbox(a);
    TestIs(a);
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

static void FromObject()
{
    Console.WriteLine("--- FromObject");
    TestCore(new object());
}

static void FromStruct()
{
    Console.WriteLine("--- FromStruct");
    TestCore(new Point());
}

static void FromClass()
{
    Console.WriteLine("--- FromClass");
    TestCore(new Test());
}

static void FromString()
{
    Console.WriteLine("--- FromString");
    TestCore("Hello");
}

static void FromChar()
{
    Console.WriteLine("--- FromChar");
    TestCore('0');
    TestCore('a');
}

static void FromInt8()
{
    Console.WriteLine("--- FromInt8");
    TestCore(sbyte.MinValue);
    TestCore(sbyte.MaxValue);
}

static void FromUInt8()
{
    Console.WriteLine("--- UFromInt8");
    TestCore(byte.MinValue);
    TestCore(byte.MaxValue);
}

static void FromInt16()
{
    Console.WriteLine("--- FromInt16");
    TestCore(short.MinValue);
    TestCore(short.MaxValue);
}

static void FromUInt16()
{
    Console.WriteLine("--- FromUInt16");
    TestCore(ushort.MinValue);
    TestCore(ushort.MaxValue);
}

static void FromInt32()
{
    Console.WriteLine("--- FromInt32");
    TestCore(int.MinValue);
    TestCore(int.MaxValue);
}

static void FromUInt32()
{
    Console.WriteLine("--- FromUInt32");
    TestCore(uint.MinValue);
    TestCore(uint.MaxValue);
}

static void FromInt64()
{
    Console.WriteLine("--- FromInt64");
    TestCore(long.MinValue);
    TestCore(long.MaxValue);
}

static void FromUInt64()
{
    Console.WriteLine("--- FromUInt64");
    TestCore(ulong.MinValue);
    TestCore(ulong.MaxValue);
}

static void FromSingle()
{
    Console.WriteLine("--- FromSingle");
    TestCore(0f);
    TestCore(3.5f);
}

static void FromDouble()
{
    Console.WriteLine("--- FromDouble");
    TestCore(0.0d);
    TestCore(3.5d);
}

static void FromEnumInt8()
{
    Console.WriteLine("--- FromEnumInt8");
    TestCore(EInt8.A);
    TestCore(EInt8.B);
    TestCore(EInt8.C);
    TestCore(EInt8.D);
}

static void FromEnumUInt8()
{
    Console.WriteLine("--- FromEnumUInt8");
    TestCore(EUInt8.A);
    TestCore(EUInt8.B);
    TestCore(EUInt8.C);
    TestCore(EUInt8.D);
}

static void FromEnumInt16()
{
    Console.WriteLine("--- FromEnumInt16");
    TestCore(EInt16.A);
    TestCore(EInt16.B);
    TestCore(EInt16.C);
    TestCore(EInt16.D);
}

static void FromEnumUInt16()
{
    Console.WriteLine("--- FromEnumUInt16");
    TestCore(EUInt16.A);
    TestCore(EUInt16.B);
    TestCore(EUInt16.C);
    TestCore(EUInt16.D);
}

static void FromEnumInt32()
{
    Console.WriteLine("--- FromEnumInt32");
    TestCore(EInt32.A);
    TestCore(EInt32.B);
    TestCore(EInt32.C);
    TestCore(EInt32.D);
}

static void FromEnumUInt32()
{
    Console.WriteLine("--- FromEnumUInt32");
    TestCore(EUInt32.A);
    TestCore(EUInt32.B);
    TestCore(EUInt32.C);
    TestCore(EUInt32.D);
}

static void FromEnumInt64()
{
    Console.WriteLine("--- FromEnumInt64");
    TestCore(EInt64.A);
    TestCore(EInt64.B);
    TestCore(EInt64.C);
    TestCore(EInt64.D);
}

static void FromEnumUInt64()
{
    Console.WriteLine("--- FromEnumUInt64");
    TestCore(EUInt64.A);
    TestCore(EUInt64.B);
    TestCore(EUInt64.C);
    TestCore(EUInt64.D);
}

public static void Main()
{
    FromNull();
    FromNullable();
    FromObject();
    FromStruct();
    FromClass();
    FromString();
    FromChar();
    FromInt8();
    FromUInt8();
    FromInt16();
    FromUInt16();
    FromInt32();
    FromUInt32();
    FromInt64();
    FromUInt64();
    FromSingle();
    FromDouble();
    FromEnumInt8();
    FromEnumUInt8();
    FromEnumInt16();
    FromEnumUInt16();
    FromEnumInt32();
    FromEnumUInt32();
    FromEnumInt64();
    FromEnumUInt64();
    Console.WriteLine("<%END%>");
}
}
}