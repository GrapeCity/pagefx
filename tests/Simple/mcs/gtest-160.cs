using System;

class Fun<A, B> { }

class List<T>
{
    public List<T2> Map<T2>(Fun<T, T2> x)
    {
        return new List<T2>();
    }

    public void foo<T2>()
    {
        (new List<T2>()).Map<T>(new Fun<T2, T>());
    }
}


class X
{
    static void Test1()
    {
        Console.WriteLine("----Test1");
        List<int> list = new List<int>();
        try
        {
            list.Map(new Fun<int, double>()).foo<string>();
            Console.WriteLine("ok");
        }
        catch (Exception e)
        {
            Console.WriteLine("error");
        }
        
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}
