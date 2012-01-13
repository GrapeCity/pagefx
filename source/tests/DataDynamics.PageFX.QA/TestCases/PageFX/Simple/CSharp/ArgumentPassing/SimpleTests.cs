using System;

struct Point
{
    public int x;
    public int y;
}

class X
{
    static void f(int a)
    {
        Console.WriteLine(a);
    }

    static void Test1()
    {
        f(10);
    }

    static void Test2()
    {
        Point pt = new Point();
        pt.x = 10;
        Console.WriteLine(pt.x);
    }

    static void Main()
    {
        //Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}