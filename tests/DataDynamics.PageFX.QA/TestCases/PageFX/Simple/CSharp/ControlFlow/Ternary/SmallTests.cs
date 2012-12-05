using System;

class Program
{
    static void Test1(bool f)
    {
        int[] arr = new int[f ? 4 : 8];
        Console.WriteLine(arr.Length);
    }

    static void Test1()
    {
        Test1(true);
        Test1(false);
    }

    static void print(string s, uint n)
    {
        Console.WriteLine(s);
        Console.WriteLine(n);
    }

    static void print(string s, uint n, bool f)
    {
        Console.WriteLine(s);
        Console.WriteLine(n);
        Console.WriteLine(f);
    }

    static void print(uint a, string s, uint b)
    {
        Console.WriteLine(a);
        Console.WriteLine(s);
        Console.WriteLine(b);
    }

    static void print(uint a, string s, uint b, bool f)
    {
        Console.WriteLine(a);
        Console.WriteLine(s);
        Console.WriteLine(b);
        Console.WriteLine(f);
    }

    static void Test2()
    {
        int i = 123;
        print("aaa", (uint)(i >= 0 ? i : -i));
    }

    static void Test3()
    {
        int a = 123;
        int b = 567;
        print((uint)(a >= 0 ? a : -a), "aaa", (uint)(b >= 0 ? b : -b));
    }

    static void Test4()
    {
        int n = 5;
        for (int i = 0; i < n; ++i)
        {
            print("aaa", (uint)(i >= n / 2 ? i : -i));
        }
    }

    static void Test5()
    {
        int n = 5;
        for (int i = 0; i < n; ++i)
        {
            bool f1 = (i & 1) == 0;
            bool f2 = (i % n) == 0;
            print("aaa", (uint)(i >= n / 2 && (f1 || f2) ? i : -i));
        }
    }

    static void Test6()
    {
        int n = 5;
        for (int i = 0; i < n; ++i)
        {
            bool f1 = (i & 1) == 0;
            bool f2 = (i % n) == 0;
            bool f3 = (i - n) % 2 == 0;
            print("aaa", (uint)(i >= n / 2 && (f1 || f2) ? i : -i), f1 && f2 && f3);
        }
    }

    static void print(uint v)
    {
        Console.WriteLine(v);
    }

    static void Test7(int value)
    {
        print(value >= 0 ? (uint)value : (uint)-value);
    }

    static void Test7()
    {
        Test7(100);
        Test7(-100);
    }

    static void print(ulong v)
    {
        Console.WriteLine(v);
    }

    static void Test8(long value)
    {
        print(value >= 0 ? (ulong)value : (ulong)-value);
    }

    static void Test8()
    {
        Test8(100L);
        Test8(-100L);
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
        Test8();
        Console.WriteLine("<%END%>");
    }
}