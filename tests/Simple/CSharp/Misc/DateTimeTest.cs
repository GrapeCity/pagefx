using System;

class DateTimeTest
{
    static void TestParse()
    {
        DateTime dt = DateTime.Parse("2008-02-08");
        //Console.WriteLine(dt.Ticks);
    }

    static void Main()
    {
        TestParse();
        Console.WriteLine("<%END%>");
    }
}