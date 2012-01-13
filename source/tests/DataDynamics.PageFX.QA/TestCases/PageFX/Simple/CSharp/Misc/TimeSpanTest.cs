using System;

class X
{
    static void Test1()
    {
        TimeSpan ts = new TimeSpan(1);
        Console.WriteLine(ts.ToString());
    }

    static void Test2()
    {
        TimeSpan t1 = new TimeSpan(1);
        string s = "justastring";
        Console.WriteLine(TimeSpan.Equals(t1, s));
        Console.WriteLine(TimeSpan.Equals(s, t1));
        Console.WriteLine(t1.Equals(s));
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}