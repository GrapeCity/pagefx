using System;

class Test1
{
    static void Main()
    {
        Console.WriteLine("ForWithIfElse");
        for (int i = 0; i < 5; ++i)
        {
            if ((i & 1) != 0)
                Console.WriteLine("odd");
            else
                Console.WriteLine("even");
        }
        Console.WriteLine("end");
        Console.WriteLine("<%END%>");
    }
}