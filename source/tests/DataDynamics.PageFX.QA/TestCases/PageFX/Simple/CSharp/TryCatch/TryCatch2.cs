using System;

class TryCatch2
{
    static void f(int a, int b)
    {
        Console.WriteLine("begin");
        try
        {
            Console.WriteLine("try 1 begin");
            for (int i = 0; i < 5; ++i)
            {
                if (i > 0)
                {
                    try
                    {
                        Console.WriteLine("try 1.1 begin");
                        if (a > b)
                        {
                            Console.WriteLine("a > b");
                            Console.WriteLine(a - i);
                        }
                        else
                        {
                            Console.WriteLine("a <= b");
                            Console.WriteLine(b - i);
                        }
                        Console.WriteLine("try 1.1 end");
                    }
                    catch
                    {
                        Console.WriteLine("catch 1.1");
                    }
                    finally
                    {
                        Console.WriteLine("finally 1.1");
                    }
                }
                else
                {
                    for (int k = 0; k < 5; ++k)
                    {
                        Console.WriteLine(a + b - k);
                    }
                }
            }
            Console.WriteLine("try 1 end");
        }
        catch (Exception e)
        {
            Console.WriteLine("catch 1");
        }
        Console.WriteLine("end");
    }

    static void Main()
    {
        for (int a = 0; a < 5; ++a)
        {
            for (int b = 0; b < 5; ++b)
            {
                f(a, b);
            }
        }
        Console.WriteLine("<%END%>");
    }
}