using System;
using System.Collections;

struct Point
{
    public int X, Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return string.Format("{{{0}, {1}}}", X, Y);
    }
}

enum EInt8 : sbyte { A = -1, B, C, D }
enum EUInt8 : byte { A, B, C, D }
enum EInt16 : short { A = -1, B, C, D }
enum EUInt16 : ushort { A, B, C, D }
enum EInt32 : int { A = -1, B, C, D }
enum EUInt32 : uint { A, B, C, D }
enum EInt64 : long { A = -1, B, C, D }
enum EUInt64 : ulong { A, B, C, D }

enum E2Int8 : sbyte { E = -1, F, G, K, }
enum E2UInt8 : byte { E, F, G, K, }
enum E2Int16 : short { E = -1, F, G, K, }
enum E2UInt16 : ushort { E, F, G, K, }
enum E2Int32 : int { E = -1, F, G, K, }
enum E2UInt32 : uint { E, F, G, K, }
enum E2Int64 : long { E = -1, F, G, K, }
enum E2UInt64 : ulong { E, F, G, K, }

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        decimal d = 0m;
        int n = Convert.ToInt32(d);
        Console.WriteLine(n);
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        char a = 'a';
        char b = 'b';
        string s = "" + a + b;
        Console.WriteLine(s);
    }

    static void Test3()
    {
        //Console.WriteLine("--- Test3");
        Point pt;
        pt.X = 10;
        pt.Y = 10;
        Console.WriteLine(pt);
    }

    static void Print(IEnumerable e)
    {
        foreach (object o in e)
            Console.WriteLine(o);
    }

    static void Test4()
    {
        //Console.WriteLine("--- Test4");
        Print(new int [] { 10, 20, 30 });
    }

    static void Test5()
    {
        string s = String.Concat(new Test(), new Test());
        Console.WriteLine(s);
    }

    static void Test6()
    {
        EInt16[] a = new EInt16[] { EInt16.A, EInt16.B, EInt16.C, EInt16.D };
        EUInt16[] b = ((object)a) as EUInt16[];
        EUInt16 i = b[0];
        Console.WriteLine(i);
    }

    static void Main()
    {
        Console.WriteLine("Hello!");
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();
        Console.WriteLine("<%END%>");
    }

    public override string ToString()
    {
        return "X";
    }
}