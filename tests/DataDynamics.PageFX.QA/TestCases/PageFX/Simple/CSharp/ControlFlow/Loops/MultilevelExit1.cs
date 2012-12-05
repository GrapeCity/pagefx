using System;

class MultilevelExitTest1
{
    static void f1()
    {
        Console.WriteLine("MultilevelExit1");
        int i = 0;
        int k = 0;
        while (true)
        {
            Console.WriteLine(k);
            Console.WriteLine(i);
            while (true)
            {
                if (k >= 2)
                {
                    Console.WriteLine("Goto Exit");
                    goto Exit;
                }
                if (i >= 5)
                {
                    i = 0;
                    break;
                }
                ++i;
            }
            ++k;
        }
    Exit:
        Console.WriteLine("Exit");
    }

    static void f2()
    {
        Console.WriteLine("MultilevelExit2");
        int i = 0;
        int k = 0;
        while (true)
        {
            Console.WriteLine(k);
            Console.WriteLine(i);
            while (true)
            {
                if (k >= 2)
                {
                    Console.WriteLine("Goto Exit");
                    goto Exit;
                }
                if (i >= 5)
                {
                    i = 0;
                    break;
                }
                if (i >= 7)
                {
                    i = 0;
                    Console.WriteLine("Goto Exit");
                    goto Exit;
                }
                ++i;
            }
            ++k;
        }
    Exit:
        Console.WriteLine("Exit");
    }

    static void f3()
    {
        Console.WriteLine("MultilevelExit3");
        int i = 0;
        int k = 0;
        while (true)
        {
            Console.WriteLine(k);
            Console.WriteLine(i);
            while (true)
            {
                if (k >= 3)
                {
                    Console.WriteLine("Goto Exit2");
                    goto Exit2;
                }
                if (k >= 2)
                {
                    Console.WriteLine("Goto Exit1");
                    goto Exit1;
                }
                if (i >= 5)
                {
                    i = 0;
                    break;
                }
                ++i;
            }
            ++k;
        }
    Exit1:
        Console.WriteLine("Exit1");
    Exit2:
        Console.WriteLine("Exit2");
    }

    static void Main()
    {
        f1();
        f2();
        f3();
        Console.WriteLine("<%END%>");
    }
}