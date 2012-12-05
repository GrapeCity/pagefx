using System;

class X
{
    static void Test1()
    {
        Console.WriteLine("--Test1");
        string s1 = "aaa";
        string s2 = null;
        Console.WriteLine(s1 + s2);
    }

    static void Test2()
    {
        Console.WriteLine("--Test2");
        string s1 = null;
        string s2 = null;
        string s3 = s1 + s2;
        if (s3 == null)
        {
            Console.WriteLine("null + null = null");
        }
        else
        {
            Console.WriteLine(s3.Length);
            Console.WriteLine("empty");
        }
    }

    static void Test3()
    {
        Console.WriteLine("--Test3");
        string s = "";
        for (int i = 'a'; i <= 'd'; ++i)
        {
            s = (char)i + s;
        }
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