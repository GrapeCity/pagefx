// Compiler options: -langversion:default

using System;

namespace Test1
{
    public partial class Foo
    {
        internal static System.Collections.IEnumerable E()
        {
            yield return "a";
        }
    }
}

class X
{
    static void Main()
    {
        Console.WriteLine(Test1());
        Console.WriteLine("<%END%>");
    }

    private static int Test1()
    {
        Console.WriteLine("----Test1");
        foreach (string s in global::Test1.Foo.E())
        {
            Console.WriteLine(s);
            if (s != "a")
                return 1;

            return 0;
        }
        return 2;
    }
}

