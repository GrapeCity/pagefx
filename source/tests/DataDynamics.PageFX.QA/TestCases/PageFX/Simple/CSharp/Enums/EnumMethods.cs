using System;

enum ABCD { A, B, C, D }
enum LongEnum : long { Name, Value }
enum ULongEnum : ulong { This, Is, A, Test = 0xffffffffffffffff }

[Flags]
enum FlagsEnum { Zero, One, Two }

[Flags]
enum LongFlagsEnum { Zero, One, Two }

enum UE : ulong { A = 1, B = 2, C = 4, D = 8, }

class Program
{
    static void TestGetNames()
    {
		Console.WriteLine("# TestGetNames");
        string[] names = Enum.GetNames(typeof(ABCD));
        int n = names.Length;
        for (int i = 0; i < n; ++i)
            Console.WriteLine(names[i]);
    }

    static void TestGetValues()
    {
		Console.WriteLine("# TestGetValues");
        Array vals = Enum.GetValues(typeof(ABCD));
        int n = vals.Length;
        for (int i = 0; i < n; ++i)
            Console.WriteLine(vals.GetValue(i));
    }

    static void TestGetName()
    {
		Console.WriteLine("# TestGetName");
        ABCD v = ABCD.B;
        Console.WriteLine(Enum.GetName(v.GetType(), v));
    }

    static void Main()
    {
        TestGetNames();
        TestGetValues();
        TestGetName();
        Console.WriteLine("<%END%>");
    }
}