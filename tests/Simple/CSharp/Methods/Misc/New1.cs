using System;

namespace PageFX.Tests
{
    interface IInterface
    {
        object GetValue();
    }

    interface IInterface2 : IInterface
    {
        new IInterface GetValue();
    }

    class Impl : IInterface
    {
        public Impl()
        {
        }

        public Impl(int v)
        {
            value = v;
        }

        public int value;

        public object GetValue()
        {
            return value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    class Impl2 : IInterface2
    {
        public Impl value = new Impl(100);

        public IInterface GetValue()
        {
            return value;
        }

        object IInterface.GetValue()
        {
            return GetValue();
        }
    }

    class Program
    {
        static void Main()
        {
            Impl obj = new Impl(10);
            Console.WriteLine(obj.GetValue());

            Impl2 obj2 = new Impl2();
            Console.WriteLine(obj2.GetValue());
            Console.WriteLine("<%END%>");
        }
    }
}