using System;

class Test
{
    public class ZClass<T, U>
    {
        public void MethodA(T arg)
        {
            Console.WriteLine("ZClass.MethodA(T arg)");
        }

        public void MethodA(U arg)
        {
            Console.WriteLine("ZClass.MethodA(U arg)");
        }

        public void MethodA()
        {
            Console.WriteLine("ZClass.MethodA()");
        }
    }
    
    static void Test1()
    {
        Console.WriteLine("--- Test1");
        ZClass<string, double> z = new ZClass<string, double>();
        z.MethodA("hi");
        z.MethodA(5.0);
        z.MethodA();
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }

}
