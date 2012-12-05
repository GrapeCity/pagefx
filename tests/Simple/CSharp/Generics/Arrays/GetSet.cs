using System;

namespace GetSetTest
{
    class A<T>
    {
        public T[,] GetNewArr(int x, int y)
        {
            T[,] t = new T[x, y];
            return t;
        }

        public T1[,] GetNewArr<T1>(int x, int y)
        {
            T1[,] t = new T1[x, y];
            Console.WriteLine("GetNewArr<T1>");
            return t;
        }

        public T[,] InitArray(T[,] t, T value)
        {
            for (int i1 = 0; i1 < t.GetLength(0); i1++)
                for (int i2 = 0; i2 < t.GetLength(1); i2++)
                    t[i1, i2] = value;
            return t;
        }

        public void PrintArray(T[,] t)
        {
            for (int i1 = 0; i1 < t.GetLength(0); i1++)
                for (int i2 = 0; i2 < t.GetLength(1); i2++)
                    Console.WriteLine("t[{0}, {1}] = {2}", i1, i2, t[i1, i2]);
        }

        public void PrintArray<T1>(T1[,] t)
        {
            Console.WriteLine("PrintArray<T1>");

            for (int i1 = 0; i1 < t.GetLength(0); i1++)
                for (int i2 = 0; i2 < t.GetLength(1); i2++)
                    Console.WriteLine("t[{0}, {1}] = {2}", i1, i2, t[i1, i2]);
        }
    }

    class B<T>
    {
        private T[,] barr;

        public T[,] BArr
        {
            get
            {
                return barr;
            }
            set
            {
                barr = value;
            }
        }
    }

    class X
    {
        static void Test1()
        {
            Console.WriteLine("----Test1");
            A<double> a = new A<double>();
            B<double> b = new B<double>();
            b.BArr = a.GetNewArr(5, 5);
            b.BArr = a.InitArray(b.BArr, 2.718281828459045235360287);
            a.PrintArray(b.BArr);
        }


        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }
    }
}