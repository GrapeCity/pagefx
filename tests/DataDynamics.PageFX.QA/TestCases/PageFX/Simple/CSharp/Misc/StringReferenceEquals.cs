using System;

class X
{
    public const string c1 = "aaa";
    public const string c2 = "bbb";

    static void Test(string s)
    {
        Console.WriteLine(s);
        Console.WriteLine(ReferenceEquals(s, c1) || ReferenceEquals(s, c2));
    }

    static void Main()
    {
        string s1 = "aaa";
        Test(s1);
        Test("bbb");
        Test("qqq");

        s1 = "a";
        string s2 = "a";
        string s3 = "b";
        Test(s1 + s2 + s1);
        Console.WriteLine("<%END%>");
    }
}