using System;
using System.Globalization;

class X
{
    static void Expect(int ch, int expected)
    {
        if (ch != expected)
        {
            string s = String.Format(CultureInfo.InvariantCulture,
                                     "expected '{0}' ({1:X}) but found '{2}' ({3:X})",
                                     (char)expected,
                                     expected,
                                     ch < 0 ? (object)"EOF" : (char)ch,
                                     ch);

            Console.WriteLine(s);
        }
    }

    static void Test1()
    {
        Expect(-1, '<');
        Expect('<', '>');
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}