using System;

class DoWhileTest1
{
    static void f1()
    {
        Console.WriteLine("DoWhile1");
        int i = 0;
        do
        {
            Console.WriteLine("i");
            Console.WriteLine(i);
            ++i;
        } while (i < 5);
        Console.WriteLine("end");
    }

    static void f2()
    {
        Console.WriteLine("DoWhile2");
        int i = 0;
        do
        {
            if (i == 2)
            {
                ++i;
                continue;
            }
            Console.WriteLine("i");
            Console.WriteLine(i);
            ++i;
        } while (i < 5);
        Console.WriteLine("end");
    }

    static void Main()
    {
        f1();
        f2();
        Console.WriteLine("<%END%>");
    }
}