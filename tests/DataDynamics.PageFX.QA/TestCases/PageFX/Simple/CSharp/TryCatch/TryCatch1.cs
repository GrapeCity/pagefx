using System;

class TryCatch1
{
    static void Main()
    {
        try
        {
            Console.WriteLine("1");
            try
            {
                Console.WriteLine("2");
                try
                {
                    Console.WriteLine("most inner");
                }
                catch (Exception e)
                {
                    Console.WriteLine("catch1");
                }
                finally
                {
                    Console.WriteLine("finally1");
                }
                Console.WriteLine("3");
            }
            catch (Exception e)
            {
                Console.WriteLine("catch2");
            }
            Console.WriteLine("4");
        }
        finally
        {
            Console.WriteLine("finally2");
        }
        Console.WriteLine("<%END%>");
    }
}