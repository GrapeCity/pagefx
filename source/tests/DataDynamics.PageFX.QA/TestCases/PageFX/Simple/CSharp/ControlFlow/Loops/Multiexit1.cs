using System;

class MultiexitTest1
{
    static void f1()
    {
        Console.WriteLine("f1 begin");
        int i = 0;
        int j = 0;
        int k = 0;
        while (true)
        {
            Console.WriteLine("body begin");
            Console.WriteLine(i);
            Console.WriteLine(j);
            Console.WriteLine(k);
            ++i;
            if (i >= 4)
            {
                Console.WriteLine("break (i >= 4)");
                break;
            }
            if (i >= 2)
            {
                i = 0;
                ++k;
            }
            if (j >= 3)
            {
                Console.WriteLine("break (j >= 3)");
                break;
            }
            if (k >= 2)
            {
                Console.WriteLine("break (k >= 2)");
                break;
            }
            ++j;
            if (i == 1)
            {
                Console.WriteLine("continue");
                continue;
            }
            Console.WriteLine("body end");
        }
        Console.WriteLine("f1 end");
        Console.WriteLine("f1 end2");
        Console.WriteLine("f1 end3");
    }

    static void Main()
    {
        f1();
        Console.WriteLine("<%END%>");
    }
}