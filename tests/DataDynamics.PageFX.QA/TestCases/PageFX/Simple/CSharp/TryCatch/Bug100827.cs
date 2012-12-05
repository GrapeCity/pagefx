using System;

internal class Test
{
    bool _running;

    void Test1()
    {
        Console.WriteLine("Test::Foo");
        throw new Exception("aaa");
    }

    public object Run()
    {
        try
        {
            _running = true;
            Test1();
            return this;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.GetType());
            throw;
        }
        finally
        {
        	Console.WriteLine("finally");
            _running = false;
        }
    }
    
    static void Main()
    {
        var test = new Test();
        try
        {
            test.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine("exception: {0}", e.GetType());
        }
        Console.WriteLine("<%END%>");
    }
}