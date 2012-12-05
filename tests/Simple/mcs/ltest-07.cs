using System;

namespace ltest07
{
    delegate void D();
    delegate void E(bool b);

    public class C
    {
        static void Test(D d)
        {
        }

        static void Test(object o, D d)
        {
        }

        static void Test(D d1, D d2)
        {
        }

        static void Test2(object o, E e)
        {
        }

        public static void Test1()
        {
            D e = () => { };
            e = (D)null;
            e = default(D);
            Test(() => { });
            Test(1, () => { });
            Test(() => { }, () => { });
            Test2(null, (foo) => { });
        }

        public static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}
