using System;

class EndlessLoop1
{
    static void f1()
    {
        Console.WriteLine("Endless");
        int i = 0;
        while (true)
        {
            Console.WriteLine(i);
            if (i >= 5)
            {
                Console.WriteLine("break");
                break;
            }
            Console.WriteLine(i);
            ++i;
        }
        Console.WriteLine("end");
    }

    static void Main()
    {
        f1();
        Console.WriteLine("<%END%>");
    }
}