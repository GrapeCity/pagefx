using System;

class X
{
    static void TestGetHashCode()
    {
        string s1 = "aaa";
        string s2 = "bbb";
        string s3 = "ccc";
        Console.WriteLine(s1.GetHashCode() == s2.GetHashCode());

        s1 = s3;
        s2 = s3;
        Console.WriteLine(s1.GetHashCode() == s2.GetHashCode());
    }

    static void TestEquals()
    {
        string s1 = "aaa";
        string s2 = "bbb";
        Console.WriteLine(Equals(s1, s2));
    }

    static void TestGetType()
    {
        string s = "aaa";
        Console.WriteLine(s.GetType().FullName);
    }

    static void tostr(object obj)
    {
        string s = obj.ToString();
        Console.WriteLine(s);
    }

    static void TestToString()
    {
        tostr("aaa");
        tostr(1);
    }

    static void Main()
    {
        TestGetHashCode();
        TestEquals();
        TestGetType();
        TestToString();
        Console.WriteLine("<%END%>");
    }
}