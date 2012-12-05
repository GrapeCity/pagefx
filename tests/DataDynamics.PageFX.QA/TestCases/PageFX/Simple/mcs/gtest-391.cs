using System;

namespace gtest391
{
    class C
    {
        static bool Test1()
        {
            bool? xx = null;
            return xx ?? true;
        }

        public static void Main()
        {
            Console.WriteLine(Test1());

            string a = null;
            string b = null ?? "a";
            Console.WriteLine(b != "a");
            
            int? i = null ?? null;
            Console.WriteLine(i != null);
            
            object z = a ?? null;
            Console.WriteLine(i != null);
            
            string p = default(string) ?? "a";
            Console.WriteLine(p != "a");
            
            string p2 = "x" ?? "a";
            Console.WriteLine(p2 != "x");

            Console.WriteLine("<%END%>");
        }
    }

}

