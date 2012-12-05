using System;

using T = System.Byte;

enum EInt8 : sbyte { A = -1, B, C, D }
enum EUInt8 : byte { A, B, C, D }
enum EInt16 : short { A = -1, B, C, D }
enum EUInt16 : ushort { A, B, C, D }
enum EInt32 : int { A = -1, B, C, D }
enum EUInt32 : uint { A, B, C, D }
enum EInt64 : long { A = -1, B, C, D }
enum EUInt64 : ulong { A, B, C, D }

class X
{
    static void Test1()
    {
        EInt64[] a = new EInt64[] { EInt64.A, EInt64.B, EInt64.C, EInt64.D };
        Console.WriteLine(a[0]);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}