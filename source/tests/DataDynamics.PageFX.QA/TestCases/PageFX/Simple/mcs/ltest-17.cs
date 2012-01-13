using System;

namespace ltest17
{
    class TestCase
    {
        string a;
        string b;
        string c;

        public void Testing()
        {
            string z = a + b + "blah1" + c + "blah2";
            Action test = () =>
            {
                string x = a;
            };
            test();
        }

        static void Test1()
        {
            Console.WriteLine("----Test1");
            new TestCase().Testing();
        }

        public static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}
