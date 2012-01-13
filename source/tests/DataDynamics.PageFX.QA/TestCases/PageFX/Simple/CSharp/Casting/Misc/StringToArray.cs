using System;

class X
{
    static string ToString(object o)
    {
        if (o == null) return "";
        if (o is byte[]) return Convert.ToBase64String((byte[])o);
        return o.ToString();
    }

    static void Test(object o)
    {
        string s = ToString(o);
        Console.WriteLine(s);
    }

    static void Main()
    {
        Test(null);
        Test("aaa");
        Test(new byte[] { 10, 20, 30 });
        Console.WriteLine("<%END%>");
    }
}