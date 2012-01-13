using System;

public class GenericException<T> : Exception
{
    public GenericException() { }
    public GenericException(string message) : base(message) { }
    public GenericException(string message, T var)
        : base(message)
    {
        Console.WriteLine(var);
    }
    public GenericException(string message, Exception inner) : base(message, inner) { }

    public GenericException(object obj)
    {
    }
}

delegate void Code();

class A<T>
{
    public void Foo()
    {
        try
        {
            throw new GenericException<T>();
        }
        catch(GenericException<T> e)
        {
            //Console.WriteLine(e.GetType());
            Console.WriteLine("ok");
        }
    }

    public void Bar()
    {
        try
        {
            throw new GenericException<T>(new A<GenericException<T>>());
        }
        catch (GenericException<T> e)
        {
            //Console.WriteLine(e.GetType());
            Console.WriteLine("ok");
        }
    }
}

class B<T> where T : Exception
{
    public void CatchT(Code code)
    {
        try
        {
            code();
        }
        catch (T e1)
        {
            Console.WriteLine("T: " + e1.Message);
        }
        catch (Exception e2)
        {
            Console.WriteLine("T was not catched");
        }
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        try
        {
            throw new GenericException<int>("Generic exception");
        }
        catch (Exception e)
        {
            //Console.WriteLine(e.GetType());
            Console.WriteLine("ok");
        }
    }
    
    static void Test2()
    {
        Console.WriteLine("--- Test2");
        try
        {
            throw new GenericException<int>("Generic exception");
        }
        catch (GenericException<int> e)
        {
            //Console.WriteLine(e.GetType());
            Console.WriteLine("ok");
        }
    }

    static void Test3()
    {
        Console.WriteLine("--- Test3");
        try
        {
            try
            {
                throw new GenericException<int>("Generic exception");
            }
            catch (GenericException<double> e)
            {
                //Console.WriteLine("Foo" + e.GetType());
                Console.WriteLine("error");
            }
        }
        catch (GenericException<int> e)
        {
            //Console.WriteLine(e.GetType());
            Console.WriteLine("ok");
        }
    }

    static void Test4()
    {
        Console.WriteLine("--- Test4");
        try
        {
            A<int> a = new A<int>();
            a.Foo();
        }
        catch (Exception e)
        {
            Console.WriteLine("error");
        }
        try
        {
            A<string> a = new A<string>();
            a.Foo();
        }
        catch (Exception e)
        {
            Console.WriteLine("error");
        }
    }

    static void Test5()
    {
        Console.WriteLine("--- Test5");
        try
        {
            A<int> a = new A<int>();
            a.Bar();
        }
        catch (Exception e)
        {
            Console.WriteLine("error");
        }
        try
        {
            A<string> a = new A<string>();
            a.Bar();
        }
        catch (Exception e)
        {
            Console.WriteLine("error");
        }
    }

    static void Test6()
    {
        Console.WriteLine("--- Test6");
        B<Exception> b = new B<Exception>();
        b.CatchT(delegate () { throw new Exception("Exception"); });
        b.CatchT(delegate() { throw new GenericException<int>("GenericException<int>"); });
        b.CatchT(delegate() { throw new GenericException<Exception>("GenericException<Exception>"); });
    }

    static void Test7()
    {
        Console.WriteLine("--- Test7");
        B<GenericException<int>> b = new B<GenericException<int>>();
        b.CatchT(delegate() { throw new Exception("Exception"); });
        b.CatchT(delegate() { throw new GenericException<int>("GenericException<int>"); });
        b.CatchT(delegate() { throw new GenericException<Exception>("GenericException<Exception>"); });
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();
        Test7();
        Console.WriteLine("<%END%>");
    }
}

