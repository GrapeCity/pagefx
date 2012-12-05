using System;

namespace gtest375
{

    public class X
    {
        public static bool Compute(int x)
        {
            return x == null;
        }

        public static bool Compute2(int x)
        {
            return x != null;
        }

        static void Main()
        {
            Console.WriteLine(Compute(1));
            Console.WriteLine(Compute2(1));
            Console.WriteLine("<%END%>");
        }
    }

}