using System;

enum ABCD { A, B, C, D }

[Flags]
enum FlagsEnum { Zero, One, Two }

class Program
{
    static void TestEquals()
    {
		Console.WriteLine("# TestEquals");
        ABCD a = ABCD.A;
        ABCD b = ABCD.B;
        Console.WriteLine(Equals(a, b));
    }

    static void TestToString()
    {
	    Console.WriteLine("# TestToString");
        Console.WriteLine(ABCD.A);
    }

    static void TestFlags()
    {
	    Console.WriteLine("# TestFlags");
        Console.WriteLine(FlagsEnum.One | FlagsEnum.Two);
    }

    static void TestGetHashCode()
    {
	    Console.WriteLine("# TestGetHashCode");
        //ABCD a = ABCD.A;
        Enum a = new ABCD();
        Console.WriteLine(a.GetHashCode());
    }

    static void Main()
    {
        TestEquals();
        TestToString();
        TestFlags();
        //TestGetHashCode();
        Console.WriteLine("<%END%>");
    }
}