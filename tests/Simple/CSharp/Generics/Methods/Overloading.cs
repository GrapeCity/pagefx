using System;

class Test
{
    public class ZClass<T, U>
    {
        public void MethodA<T1>(T arg)
        {
            Console.WriteLine("ZClass.MethodA<T1>(T arg)");
        }

        public void MethodA<T1>(U arg)
        {
            Console.WriteLine("ZClass.MethodA<T1>(U arg)");
        }

        public void MethodA<T1>()
        {
            Console.WriteLine("ZClass.MethodA<T1>()");
        }

        public T1 MethodA<T1>(T tArg, U uArg)
        {
            Console.WriteLine("ZClass.MethodA<T1>()");
            return default(T1);
        }

    }

    static void Test1()
    {
        Console.WriteLine("--- Test1");
        ZClass<string, double> z = new ZClass<string, double>();
        z.MethodA<string>("hi"); // 1
        z.MethodA<short>(2.71828); // 2
        z.MethodA<int>(); // 2
        int r = z.MethodA<int>("hi", 5.5);
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }

}
