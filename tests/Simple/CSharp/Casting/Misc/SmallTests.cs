using System;

class X
{
    static void Test1()
    {
        Console.WriteLine("--Test1");
        double x = 10.4;
        int i = (int)x;
        Console.WriteLine(x);
        Console.WriteLine(i);
    }

    static void Test2()
    {
        Console.WriteLine("--Test2");
        {
            int a = -1000;
            Console.WriteLine(a);
            Console.WriteLine((sbyte)a);
        }
        {
            int a = -1100;
            Console.WriteLine(a);
            Console.WriteLine((sbyte)a);
        }
    }

    static void Test3()
    {
        Console.WriteLine("--Test3");
        uint u = 0xffff;
        Console.WriteLine((byte)u);
    }

    static void Test4()
    {
        Console.WriteLine("--Test4");
        char c = '\x01ff';
        byte[] b = BitConverter.GetBytes(c);
        Console.WriteLine("{0:x}", b[0]);
        Console.WriteLine("{0:x}", b[1]);
    }

    static void Test5()
    {
        Console.WriteLine("--Test5");
        char c = '\x01ff';
        byte b0 = (byte)c;
        byte b1 = (byte)(c >> 8);
        Console.WriteLine(b0);
        Console.WriteLine(b1);
    }

    static void Test6()
    {
        Console.WriteLine("--Test6");
        sbyte v = -126;
        Console.WriteLine(v);
        Console.WriteLine("{0:x}", v);
    }

    static int[] A = { -100, 0, 100, 200, 300 };
    static uint[] B = { 0, 200, 500, 600,  100 };

    static void Test7()
    {
        Console.WriteLine("--Test7");
        int n = A.Length;
        for (int i = 0; i < n; ++i)
        {
            Console.WriteLine(Math.Min(A[i], B[i]));
            Console.WriteLine(Math.Max(A[i], B[i]));
        }
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();
        Test7();
        Console.WriteLine("<%END%>");
    }
}