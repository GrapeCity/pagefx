using System;
using System.Collections;

class Program
{
    static void Test1()
    {
        String[,] s1 = { { "this", "is" }, { "a", "test" } };
        IEnumerator en = s1.GetEnumerator();

        Console.WriteLine(en.MoveNext());
        Console.WriteLine(en.Current); //this

        Console.WriteLine(en.MoveNext());
        Console.WriteLine(en.Current); //is

        Console.WriteLine(en.MoveNext());
        Console.WriteLine(en.Current); //a

        Console.WriteLine(en.MoveNext());
        Console.WriteLine(en.Current); //test

        Console.WriteLine(en.MoveNext());
    }

    static void Test2()
    {
        String[,] s1 = { { "aaa", "bbb", "ccc" }, { "ddd", "eee", "fff" } };
        Console.WriteLine(s1[0, 0]);
        Console.WriteLine(s1[0, 1]);
        Console.WriteLine(s1[0, 2]);
        Console.WriteLine(s1[1, 0]);
        Console.WriteLine(s1[1, 1]);
        Console.WriteLine(s1[1, 2]);
        foreach (string s in s1)
            Console.WriteLine(s);
    }

    static void Test3()
    {
        String[,,] s1 = { { { "aaa", "bbb", "ccc" }, { "ddd", "eee", "fff" } }, { { "aa", "bb", "cc" }, { "dd", "ee", "ff" } } };
        foreach (string s in s1)
            Console.WriteLine(s);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Console.WriteLine("<%END%>");
    }
}