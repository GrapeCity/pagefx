using System;

interface I<A, B, C>
{
    void F<T>(T t) where T : A;
    void G<T>(T t) where T : B;
    void H<T>(T t) where T : C;
}

class Cls : I<object, Cls, string>
{
    public void F<T>(T t)
    {
        Console.WriteLine(typeof(T));
    }

    public void G<T>(T t) where T : Cls
    {
        Console.WriteLine(typeof(T));
    }

    public void H<U>(U u) where U : class
    {
        Console.WriteLine(typeof(U));
    }

    void I<object, Cls, string>.H<T>(T t)
    {
        Console.WriteLine(t);
    }
}

class Test
{
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        try
        {
            Cls obj = new Cls();
            obj.H(obj);
            Console.WriteLine("ok");
        }
        catch (Exception e)
        {
            Console.WriteLine("error " + e.GetType());
        }
    }

    static void Test2()
    {
        Console.WriteLine("--- Test1");
        try
        {
            Cls obj = new Cls();
            ((I<object, Cls, string>)obj).H("aaa");
            Console.WriteLine("ok");
        }
        catch (Exception e)
        {
            Console.WriteLine("error " + e.GetType());
        }
    }

    static void Main ()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }

}
