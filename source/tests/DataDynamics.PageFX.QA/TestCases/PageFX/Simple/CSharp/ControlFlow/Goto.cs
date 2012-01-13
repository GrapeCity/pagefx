using System;

class Goto
{
    static void f1()
    {
        Console.WriteLine("endless loop");
        int i = 0;
        L1:
        if (i >= 5)
            goto L2;

        Console.WriteLine(i);
        ++i;

        goto L1;

        L2:
        Console.WriteLine("end");
    }

    static void f2(int s)
    {
        Console.WriteLine("dfa");
        L1:
        if (s == 1)
        {
            Console.WriteLine("1");
            goto L3;
        }
        L2:
        if (s == 2)
        {
            Console.WriteLine("2");
            goto L4;
        }
        L3:
        if (s == 3)
        {
            Console.WriteLine("3");
            goto L2;
        }
        else
        {
            goto L5;
        }
        L4:
        if (s == 4)
        {
            Console.WriteLine("4");
            goto L3;
        }
        L5:
        if (s == 5)
        {
            Console.WriteLine("5");
            goto L1;
        }
    }

    static void Main()
    {
        f1();
        f2(1);
        Console.WriteLine("<%END%>");
    }
}