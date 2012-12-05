using System;

namespace gtest403
{
    public struct ExS<T>
    {
        public void Bar()
        {
            Console.WriteLine(typeof(T) == typeof(int));
        }
    }

    interface I 
    {
        void Bar();
    }

    public struct S<T> : I
    {
        public void Foo()
        {
            Console.WriteLine("S<T>::Foo()");
            Console.WriteLine(typeof(T) == typeof(int));
        }

        public void Bar()
        {
            Console.WriteLine("S<T>::Bar()");
            Foo();
        }
    }

    class T
    {
        public static void Main()
        {
            S<int> i;
            ExS<bool> e;
            i.Foo();
            e.Bar();
            i.Bar();
            Console.WriteLine("<%END%>");
        }
    }

}