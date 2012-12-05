using System;

namespace gtest387
{
    class C
    {
        public static void Main()
        {
            sbyte? s = null;
            long? tt = +s;
            Console.WriteLine(tt != null);
            

            long? l = null;
            l = +l;
            Console.WriteLine(l != null);
            Console.WriteLine("<%END%>");
        }
    }

}
