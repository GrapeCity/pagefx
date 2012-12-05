using System;
using __AS3__.vec;

class X
{
    static void Test1()
    {
        Vector_double v = new Vector_double(1);
        v[0] = 10;
        Console.WriteLine(v[0]);

        Vector v2 = v;
        Console.WriteLine(v2[0]);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}