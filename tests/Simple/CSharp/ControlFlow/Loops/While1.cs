using System;

class WhileTest1
{
    static void f1()
    {
        Console.WriteLine("While1");
        int i = 0;
        while (i < 5)
        {
            Console.WriteLine("i");
            Console.WriteLine(i);
            ++i;
        }
        Console.WriteLine("end");
    }

    static void f2()
    {
        Console.WriteLine("While2");
        int i = 0;
        while (i < 5)
        {
            if (i == 2)
            {
                ++i;
                continue;
            }
            Console.WriteLine("i");
            Console.WriteLine(i);
            ++i;
        }
        Console.WriteLine("end");
    }

    static void f3()
    {
        Console.WriteLine("While3");
        int i = 0;
        while (i < 5)
        {
            if (i == 2)
            {
                ++i;
                break;
            }
            Console.WriteLine("i");
            Console.WriteLine(i);
            ++i;
        }
        Console.WriteLine("end");
    }

    static void Main()
    {
        f1();
        f2();
        f3();
        Console.WriteLine("<%END%>");
    }
}