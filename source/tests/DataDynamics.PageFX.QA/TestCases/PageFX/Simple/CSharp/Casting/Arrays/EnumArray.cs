using System;

using T = System.Int64;

enum EInt64 : long { A = -1 , B, C, D, E, F, G, H, I, J, K, L, M, N, O, P }

class X
{
    static void TestCore(EInt64[] arr)
    {
        try
        {
            for (int i = 0; i < arr.Length; ++i)
                Console.WriteLine(arr[i]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void FromEInt64()
    {
        Console.WriteLine("--- Int64 Enum Test");
        EInt64[] a = new EInt64[10];
        TestCore(a);
    }

    static void Main()
    {
        FromEInt64();
        Console.WriteLine("<%END%>");
    }
}