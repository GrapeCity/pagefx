using System;

namespace ltest03
{
    public delegate TResult Func<TArg0, TResult>(TArg0 arg0);

    class Demo
    {
        static Y F<X, Y>(int a, X value, Func<X, Y> f1)
        {
            return f1(value);
        }

        static void Test1()
        {
            Console.WriteLine("----Test1");
            object o = F(1, "1:15:30", s => TimeSpan.Parse(s));
            Console.WriteLine(o);
        }

        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}

