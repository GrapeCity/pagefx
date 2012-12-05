using System;

delegate TResult Func<TResult>(TResult value);

delegate TResult Func<TArg, TResult>(TArg arg);

delegate TResult Func<TArg1, TArg2, TResult>(TArg1 arg1, TArg2 arg2);

delegate TResult Func<TArg1, TArg2, TArg3, TResult>(TArg1 arg1, TArg2 arg2, TArg3 arg3);

class Test
{
    static int Add10(int a)
    {
        return a + 10;
    }

    static string Concat(string s1, string s2)
    {
        return s1 + s2;
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        Func<int, int> f = Add10;
        Console.WriteLine(f(10));
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        Func<string, string, string> f = Concat;
        Console.WriteLine(f("aaa", "bbb"));
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}