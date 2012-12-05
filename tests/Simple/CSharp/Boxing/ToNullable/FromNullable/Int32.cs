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
using T = Nullable<Int32>;

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
    TestCore(new Point?());
    TestCore(new Point?(new Point(10, 10)));
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
    Console.WriteLine("--- FromChar?");
    TestCore(new char?());
    TestCore(new char?('0'));
    TestCore(new char?('a'));
}

static void FromInt8()
{
    Console.WriteLine("--- FromInt8?");
    TestCore(new sbyte?());
    TestCore(new sbyte?(sbyte.MinValue));
    TestCore(new sbyte?(sbyte.MaxValue));
}

static void FromUInt8()
{
    Console.WriteLine("--- UFromInt8?");
    TestCore(new byte?());
    TestCore(new byte?(byte.MinValue));
    TestCore(new byte?(byte.MaxValue));
}

static void FromInt16()
{
    Console.WriteLine("--- FromInt16?");
    TestCore(new short?());
    TestCore(new short?(short.MinValue));
    TestCore(new short?(short.MaxValue));
}

static void FromUInt16()
{
    Console.WriteLine("--- FromUInt16?");
    TestCore(new ushort?());
    TestCore(new ushort?(ushort.MinValue));
    TestCore(new ushort?(ushort.MaxValue));
}

static void FromInt32()
{
    Console.WriteLine("--- FromInt32?");
    TestCore(new int?());
    TestCore(new int?(int.MinValue));
    TestCore(new int?(int.MaxValue));
}

static void FromUInt32()
{
    Console.WriteLine("--- FromUInt32?");
    TestCore(new uint?());
    TestCore(new uint?(uint.MinValue));
    TestCore(new uint?(uint.MaxValue));
}

static void FromInt64()
{
    Console.WriteLine("--- FromInt64?");
    TestCore(new long?());
    TestCore(new long?(long.MinValue));
    TestCore(new long?(long.MaxValue));
}

static void FromUInt64()
{
    Console.WriteLine("--- FromUInt64?");
    TestCore(new ulong?());
    TestCore(new ulong?(ulong.MinValue));
    TestCore(new ulong?(ulong.MaxValue));
}

static void FromSingle()
{
    Console.WriteLine("--- FromSingle?");
    TestCore(new float?());
    TestCore(new float?(0f));
    TestCore(new float?(3.5f));
}

static void FromDouble()
{
    Console.WriteLine("--- FromDouble?");
    TestCore(new double?());
    TestCore(new double?(0.0d));
    TestCore(new double?(3.5d));
}

static void FromEnumInt8()
{
    Console.WriteLine("--- FromEnumInt8?");
    TestCore(new EInt8?());
    TestCore(new EInt8?(EInt8.A));
    TestCore(new EInt8?(EInt8.B));
    TestCore(new EInt8?(EInt8.C));
    TestCore(new EInt8?(EInt8.D));

}

static void FromEnumUInt8()
{
    Console.WriteLine("--- FromEnumUInt8?");
    TestCore(new EUInt8?());
    TestCore(new EUInt8?(EUInt8.A));
    TestCore(new EUInt8?(EUInt8.B));
    TestCore(new EUInt8?(EUInt8.C));
    TestCore(new EUInt8?(EUInt8.D));
}

static void FromEnumInt16()
{
    Console.WriteLine("--- FromEnumInt16?");
    TestCore(new EInt16?());
    TestCore(new EInt16?(EInt16.A));
    TestCore(new EInt16?(EInt16.B));
    TestCore(new EInt16?(EInt16.C));
    TestCore(new EInt16?(EInt16.D));
}

static void FromEnumUInt16()
{
    Console.WriteLine("--- FromEnumUInt16?");
    TestCore(new EUInt16?());
    TestCore(new EUInt16?(EUInt16.A));
    TestCore(new EUInt16?(EUInt16.B));
    TestCore(new EUInt16?(EUInt16.C));
    TestCore(new EUInt16?(EUInt16.D));
}

static void FromEnumInt32()
{
    Console.WriteLine("--- FromEnumInt32?");
    TestCore(new EInt32?());
    TestCore(new EInt32?(EInt32.A));
    TestCore(new EInt32?(EInt32.B));
    TestCore(new EInt32?(EInt32.C));
    TestCore(new EInt32?(EInt32.D));
}

static void FromEnumUInt32()
{
    Console.WriteLine("--- FromEnumUInt32?");
    TestCore(new EUInt32?(EUInt32.A));
    TestCore(new EUInt32?(EUInt32.B));
    TestCore(new EUInt32?(EUInt32.C));
    TestCore(new EUInt32?(EUInt32.D));
}

static void FromEnumInt64()
{
    Console.WriteLine("--- FromEnumInt64?");
    TestCore(new EInt64?(EInt64.A));
    TestCore(new EInt64?(EInt64.B));
    TestCore(new EInt64?(EInt64.C));
    TestCore(new EInt64?(EInt64.D));
}

static void FromEnumUInt64()
{
    Console.WriteLine("--- FromEnumUInt64?");
    TestCore(new EUInt64?(EUInt64.A));
    TestCore(new EUInt64?(EUInt64.B));
    TestCore(new EUInt64?(EUInt64.C));
    TestCore(new EUInt64?(EUInt64.D));
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