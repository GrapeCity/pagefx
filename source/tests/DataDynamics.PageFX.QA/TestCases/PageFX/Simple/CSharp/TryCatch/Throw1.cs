using System;

class Err : Exception
{
    public Err(string err)
    {
        msg = err;
    }

    public string msg;
}

class Throw1
{
    static void f1()
    {
        throw new Err("aaa");
    }

    static void Test1()
    {
        try
        {
            f1();
        }
        catch (Err err)
        {
            Console.WriteLine(err.msg);
        }
        catch (Exception exc)
        {
            Console.WriteLine("Exception");
        }
    }

    static void f2()
    {
        throw new NotSupportedException();
    }

    static void Test2()
    {
        try
        {
            f2();
        }
        catch (NotSupportedException)
        {
            return;
        }
        catch (Exception)
        {
            Console.WriteLine("threw wrong exception type");
        }

        Console.WriteLine("Test2 shouldn't get this far");
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}