using System;
using System.Collections;

class X
{
    static void print(IEnumerable e)
    {
        foreach (object o in e)
            Console.WriteLine(o);
    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        print("aaa");
    }

    static void Test2(IComparable c)
    {
        Console.WriteLine(c.CompareTo("aaa"));
        Console.WriteLine(c.CompareTo(null));
    }

    static void Test2()
    {
        Console.WriteLine("--- Test2");
        Test2("aaa");
        Test2("bbb");
    }

    static void Test3(ICloneable c)
    {
        object obj = c.Clone();
        Console.WriteLine(Equals(obj, c));
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        Test3("aaa");
    }

    static void Test4(object obj)
    {
        try
        {
            string s = (string)obj;
            Console.WriteLine(s);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.GetType());
        }
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        Test4(new object());
        Test4(null);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Console.WriteLine("<%END%>");
    }
}