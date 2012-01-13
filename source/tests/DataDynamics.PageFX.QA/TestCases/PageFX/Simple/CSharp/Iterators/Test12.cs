using System;

class X
{
    public System.Collections.IEnumerable a()
    {
        lock (this)
        {
            yield return "a";
            yield return "b";
        }
    }

    static void Main()
    {
        X x = new X();
        foreach (object o in x.a())
        {
            Console.WriteLine(o);
        }
        Console.WriteLine("<%END%>");
    }
}
