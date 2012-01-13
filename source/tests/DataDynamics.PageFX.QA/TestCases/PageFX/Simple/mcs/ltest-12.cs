using System;

namespace ltest12
{
    public delegate void Func<TA>(TA arg0);

    class Demo
    {
        static void F<T>(T[] values, T value, Func<T> f)
        {
            Console.WriteLine(values[0]);
            f(value);
            Console.WriteLine(values[0]);
        }

        static int Test1()
        {
            Console.WriteLine("----Test1");
            int[] a = new int[] { 10 };
            F(a, 5, i => a[0] = i);

            if (a[0] != 5)
                return 1;

            return 0;
        }

        static void Main()
        {
            Console.WriteLine(Test1());
            Console.WriteLine("<%END%>");            
        }
    }
}
