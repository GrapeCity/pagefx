using System;

class TryCatch4
{
    static void Main()
    {
        try
        {
            Console.WriteLine("--");
            try
            {
                Console.WriteLine("body");
            }
            catch(Exception e)
            {
                Console.WriteLine("catch");
                throw;
            }
            Console.WriteLine("--");
        }
        catch (Exception e)
        {
            Console.WriteLine("catch");
        }
        finally
        {
            Console.WriteLine("finally");
        }
        Console.WriteLine("<%END%>");
    }
}