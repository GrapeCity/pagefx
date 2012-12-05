using System;

namespace gtest404
{
    public static class Program
    {
        static void Test1()
        {
            Console.WriteLine("--- Test1");
            Class2<int> c = new Class2<int>();
            c.DoSomething += Hello;
            c.Raise();
            c.DoSomething -= Hello;
            c.Raise();
        }

        static void Test2()
        {
            Console.WriteLine("--- Test2");
            Class2<int> c = new Class2<int>();
            c.Raise();
        }

        static void Hello(object sender, EventArgs e)
        {
            Console.WriteLine("Hello!");
        }

        public static void Main()
        {
            Test1();
            Test2();
            Console.WriteLine("<%END%>");
        }
    }

    public abstract class Class1<T1>
    {
        protected event EventHandler doSomething;

        public void Raise()
        {
            if (doSomething != null)
                doSomething(this, EventArgs.Empty);
        }
    }

    public class Class2<T> : Class1<T>
    {
        public event EventHandler DoSomething
        {
            add { this.doSomething += value; }
            remove { this.doSomething -= value; }
        }
    }
}