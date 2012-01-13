using System;

class Err : Exception
{
    public Err(string err)
    {
        msg = err;
    }

    public string msg;
}

class Throw2
{
    static void f1()
    {
        throw new Err("aaa");
    }

    static void f2()
    {
        try
        {
            f1();
        }
        catch (Err err)
        {
            Console.WriteLine(err.msg);
            err.msg = "bbb";
            throw;
        }
    }

    static void Main()
    {
        try
        {
            f2();
        }
        catch (Err err)
        {
            Console.WriteLine(err.msg);
        }
        catch (Exception exc)
        {
            Console.WriteLine("Exception");
        }
        Console.WriteLine("<%END%>");
    }
}