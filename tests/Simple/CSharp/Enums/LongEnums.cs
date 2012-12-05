using System;

enum LongEnum : long { Name, Value }
enum ULongEnum : ulong { This, Is, A, Test = 0xffffffffffffffff }

[Flags]
enum LongFlagsEnum { Zero, One, Two }

class Program
{
	static void TestEquals()
	{
		Console.WriteLine("# TestEquals");
		var a = LongEnum.Name;
		var b = LongEnum.Value;
		Console.WriteLine(Equals(a, b));
	}

	static void TestToString()
	{
		Console.WriteLine("# TestToString");
		LongEnum v = LongEnum.Name;
		Console.WriteLine(v);
		Console.WriteLine(ULongEnum.Test);
	}

	static void TestCast()
	{
		Console.WriteLine("# TestCast");
		LongEnum v = LongEnum.Name;
		long l = (long)v;
		Console.WriteLine(l);
	}

    static void TestFlags()
    {
	    Console.WriteLine("# TestFlags");
        Console.WriteLine(LongFlagsEnum.One | LongFlagsEnum.Two);
    }

    static void TestGetHashCode()
    {
		Console.WriteLine("# TestGetHashCode");
        Enum a = new LongEnum();
        Console.WriteLine(a.GetHashCode());
    }

    static void Main()
    {
        TestEquals();
        TestToString();
        TestFlags();
        TestGetHashCode();
        Console.WriteLine("<%END%>");
    }
}