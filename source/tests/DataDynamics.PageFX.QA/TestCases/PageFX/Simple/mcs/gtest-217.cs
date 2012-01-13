using System;
using System.Collections.Generic;

public delegate R Fun<A1, R>(A1 x);

class MyTest
{
    private static void Test1()
    {
        Console.WriteLine("----Test1");
        foreach (Object d in Map<int, int, String, Object>
                            (delegate(int x) { return x.ToString(); },
                     FromTo(10, 20)))
            Console.WriteLine(d);
    }
    
    public static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }

    // Map with argument/result co/contravariance:
    // Aa=argument, Rr=result, Af=f's argument, Rf=f's result

    public static IEnumerable<Rr> Map<Aa, Af, Rf, Rr>(Fun<Af, Rf> f,
                                                   IEnumerable<Aa> xs)
        where Aa : Af
        where Rf : Rr
    {
        foreach (Aa x in xs)
            yield return f(x);    // gmcs 1.1.9 bug: cannot convert Aa to Af
    }

    // FromTo : int * int -> int stream

    public static IEnumerable<int> FromTo(int from, int to)
    {
        for (int i = from; i <= to; i++)
            yield return i;
    }
}


