

using System;

namespace TwoCtors
{

    class A1<T>
    {
        private T[,] arrVal;
        public T[,] ArrValue
        {
            get
            {
                return arrVal;
            }
            set
            {
                arrVal = value;
            }
        }

        public T[,] Test(int x, int y)
        {
            return new T[x, y];
        }
        
        public static void PrintArr(T[,] t)
        {
            for (int i1 = 0; i1 < t.GetLength(0); i1++)
                for (int i2 = 0; i2 < t.GetLength(1); i2++)
                    Console.WriteLine("t[{0}, {1}] = {2}", i1, i2, t[i1, i2]);
        }
    }

    class A2<T>
    {
        private T[,] arrVal;
        public T[,] ArrValue 
        { 
            get
            {
                return arrVal;
            }
            set
            {
                arrVal = value;
            }
        }

        public T[,] Test(int x, int y)
        {
            return new T[x, y];
        }

        public static void PrintArr(T[,] t)
        {
            for (int i1 = 0; i1 < t.GetLength(0); i1++)
                for (int i2 = 0; i2 < t.GetLength(1); i2++)
                    Console.WriteLine("t[{0}, {1}] = {2}", i1, i2, t[i1, i2]);
        }
    }

    class X
    {
        

        static void Test1()
        {
            Console.WriteLine("----Test1");
            A1<string> a1 = new A1<string>();
            A1<string>.PrintArr(a1.Test(5, 5));
            
        }

        static void Test2()
        {
            Console.WriteLine("----Test1");
            A2<string> a2 = new A2<string>();
            A2<string>.PrintArr(a2.Test(5, 5));
        }

        static void Main()
        {
            Test1();
            Test2();
            Console.WriteLine("<%END%>");
        }
    }

}
