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
    static void Test()
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
        Test();
        Console.WriteLine("<%END%>");
    }
}