using System;
using System.Collections.Generic;


class Program
{
    class Fib
    {
        private static IEnumerable<long> fib()
        {
            yield return 0;
            long i = 0, j = 1;
            while (true)
            {
                yield return j;
                long temp = i;
                i = j; j = temp + j;
            }

        }

        public static void Print()
        {
            foreach (long n in fib())
            {
                if (n > 1000)
                    break;
                Console.WriteLine(n);
            }
        }
    }

    static void Test1()
    {
        Console.WriteLine("----Test1");
        Fib.Print();

    }
    
    static void Main(string[] args)
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}