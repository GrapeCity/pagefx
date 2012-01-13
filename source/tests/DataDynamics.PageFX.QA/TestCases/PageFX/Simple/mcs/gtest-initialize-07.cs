using System;

namespace initialize07
{
    public class A
    {
        public string Name { get; set; }

        public bool Matches(string s)
        {
            return Name == s;
        }
    }

    class M
    {
        public static void Main()
        {
            Console.WriteLine(Test1());
            Console.WriteLine("<%END%>");
        }

        private static int Test1()
        {
            if (!new A() { Name = "Foo" }.Matches("Foo"))
                return 1;

            return 0;
        }
    }
}