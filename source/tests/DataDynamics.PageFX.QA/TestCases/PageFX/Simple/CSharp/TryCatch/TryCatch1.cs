using System;

class TryCatch1
{
    static void Main()
    {
        try
        {
            Console.WriteLine("--");
            try
            {
                Console.WriteLine("--");
                try
                {
                    Console.WriteLine("body");
                }
                catch (Exception e)
                {
                    Console.WriteLine("catch");
                }
                finally
                {
                    Console.WriteLine("finally");
                }
                Console.WriteLine("--");
            }
            catch (Exception e)
            {
                Console.WriteLine("catch");
            }
            Console.WriteLine("--");
        }
        finally
        {
            Console.WriteLine("finally");
        }
        Console.WriteLine("<%END%>");
    }
}