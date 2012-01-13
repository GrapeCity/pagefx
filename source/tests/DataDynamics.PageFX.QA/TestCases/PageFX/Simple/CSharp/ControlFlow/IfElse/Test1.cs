using System;

class Test1
{
    static void f1(int a, int b, int c)
    {
        Console.WriteLine("f1");
        if (a > b)
        {
            Console.WriteLine("a > b");
            if (b > c)
            {
                Console.WriteLine("b > c");
                Console.WriteLine("1");
            }
            else
            {
                Console.WriteLine("b <= c");
                Console.WriteLine("2");
            }
            Console.WriteLine("3");
        }
        else
        {
            Console.WriteLine("a <= b");
            if (b < c)
            {
                Console.WriteLine("b < c");
                Console.WriteLine("4");
            }
            else
            {
                Console.WriteLine("b >= c");
                Console.WriteLine("5");
            }
            Console.WriteLine("6");
        }
        Console.WriteLine("7");
        if (b < c)
        {
            Console.WriteLine("b < c");
            Console.WriteLine("8");
        }
        Console.WriteLine("9");
    }

    static void Main()
    {
        f1(10, 20, 30);
        f1(10, 20, 15);
        f1(20, 10, 30);
        f1(20, 10, 15);
        Console.WriteLine("<%END%>");
    }
}