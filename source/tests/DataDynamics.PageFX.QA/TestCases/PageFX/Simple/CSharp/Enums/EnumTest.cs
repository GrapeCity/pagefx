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
    static void TestEquals()
    {
        ABCD a = ABCD.A;
        ABCD b = ABCD.B;
        Console.WriteLine(Equals(a, b));
    }

    static void TestToString()
    {
        Console.WriteLine(ABCD.A);
    }

    static void TestFlags()
    {
        Console.WriteLine(FlagsEnum.One | FlagsEnum.Two);
    }

    static void TestLongFlags()
    {
        Console.WriteLine(LongFlagsEnum.One | LongFlagsEnum.Two);
    }

    static void TestGetHashCode()
    {
        //ABCD a = ABCD.A;
        Enum a = new ABCD();
        Console.WriteLine(a.GetHashCode());
    }

#if NOT_PFX
    static void TestGetNames()
    {
        string[] names = Enum.GetNames(typeof(ABCD));
        int n = names.Length;
        for (int i = 0; i < n; ++i)
            Console.WriteLine(names[i]);
    }

    static void TestGetValues()
    {
        Array vals = Enum.GetValues(typeof(ABCD));
        int n = vals.Length;
        for (int i = 0; i < n; ++i)
            Console.WriteLine(vals.GetValue(i));
    }

    static void TestGetName()
    {
        ABCD v = ABCD.B;
        Console.WriteLine(Enum.GetName(v.GetType(), v));
    }
#endif

    static void TestLongEnum()
    {
        LongEnum v = LongEnum.Name;
        Console.WriteLine(v);
        long l = (long)v;
        Console.WriteLine(l);
    }

    static void TestEnumArray()
    {
        ABCD[] arr = new ABCD[] { ABCD.A, ABCD.B, ABCD.C, ABCD.D };
        for (int i = 0; i < arr.Length; ++i)
            Console.WriteLine(arr[i]);
    }

    static void TestEnumArrayToIntArray()
    {
        ABCD[] arr1 = new ABCD[] { ABCD.A, ABCD.B, ABCD.C, ABCD.D };
        int[] arr2 = new int[4];
        arr1.CopyTo(arr2, 0);
        for (int i = 0; i < arr2.Length; ++i)
            Console.WriteLine(arr2[i]);
    }

    static void TestULongEnum()
    {
        Console.WriteLine(ULongEnum.Test);
    }

    static void TestUE()
    {
        UE ue = UE.A | UE.B | UE.C | UE.D;
#if NOT_PFX
        string s = Enum.Format(typeof(UE), ue, "f");
        Console.WriteLine(s);
#else
        Console.WriteLine(ue);
#endif
    }

    static void Main()
    {
        Console.WriteLine("TestEquals");
        TestEquals();
        Console.WriteLine("TestToString");
        TestToString();
        Console.WriteLine("TestFlags");
        TestFlags();
        Console.WriteLine("TestGetHashCode");
        TestGetHashCode();
#if NOT_PFX
        Console.WriteLine("TestGetNames");
        TestGetNames();
        Console.WriteLine("TestGetValues");
        TestGetValues();
        Console.WriteLine("TestGetName");
        TestGetName();
#endif
        Console.WriteLine("TestLongEnum");
        TestLongEnum();
        Console.WriteLine("TestLongFlags");
        TestLongFlags();
        Console.WriteLine("TestEnumArray");
        TestEnumArray();
        Console.WriteLine("TestEnumArrayToIntArray");
        TestEnumArrayToIntArray();
        Console.WriteLine("TestULongEnum");
        TestULongEnum();
        Console.WriteLine("TestUE");
        TestUE();
        Console.WriteLine("<%END%>");
    }
}