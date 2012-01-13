using System;

class DoWhileTest2
{
    static void f1()
    {
        Console.WriteLine("DoWhile1");
        int i = 0;
        do
        {
            if (i == 2)
            {
                ++i;
                break;
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
        Console.WriteLine("<%END%>");
    }
}