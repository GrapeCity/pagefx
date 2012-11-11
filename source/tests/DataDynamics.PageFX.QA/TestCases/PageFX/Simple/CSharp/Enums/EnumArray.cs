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
    static void TestEnumArray()
    {
		Console.WriteLine("# TestEnumArray");
        ABCD[] arr = new ABCD[] { ABCD.A, ABCD.B, ABCD.C, ABCD.D };
        for (int i = 0; i < arr.Length; ++i)
            Console.WriteLine(arr[i]);
    }

    static void TestEnumArrayToIntArray()
    {
		Console.WriteLine("# TestEnumArrayToIntArray");
        ABCD[] arr1 = new ABCD[] { ABCD.A, ABCD.B, ABCD.C, ABCD.D };
        int[] arr2 = new int[4];
        arr1.CopyTo(arr2, 0);
        for (int i = 0; i < arr2.Length; ++i)
            Console.WriteLine(arr2[i]);
    }

    static void Main()
    {
        TestEnumArray();
        TestEnumArrayToIntArray();
        Console.WriteLine("<%END%>");
    }
}