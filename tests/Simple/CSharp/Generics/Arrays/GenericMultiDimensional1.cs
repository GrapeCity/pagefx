using System;

namespace GenericMultiDimensional1
{
    class X
    {
        class pixel<T>
        {
            public T x;
            public T y;
        };

        static void Test<T1, T2>(T2 value)
            where T1 : pixel<T2>
        {
            T1[, , ,] myArray = new T1[2, 2, 2, 2];

            for (int i1 = 0; i1 < myArray.GetLength(0); i1++)
                for (int i2 = 0; i2 < myArray.GetLength(1); i2++)
                    for (int i3 = 0; i3 < myArray.GetLength(2); i3++)
                        for (int i4 = 0; i4 < myArray.GetLength(3); i4++)
                        {
                            myArray[i1, i2, i3, i4] = (T1)(new pixel<T2>());
                            myArray[i1, i2, i3, i4].x = value;
                            myArray[i1, i2, i3, i4].y = value;
                        }


            for (int i1 = 0; i1 < myArray.GetLength(0); i1++)
                for (int i2 = 0; i2 < myArray.GetLength(1); i2++)
                    for (int i3 = 0; i3 < myArray.GetLength(2); i3++)
                        for (int i4 = 0; i4 < myArray.GetLength(3); i4++)
                        {
                            Console.WriteLine("myArray[{0}, {1}, {2}, {3}]= ({4} , {5})", i1, i2, i3, i4, myArray[i1, i2, i3, i4].x, myArray[i1, i2, i3, i4].y);
                        }
        }

        static void Test1()
        {
            Console.WriteLine("--- Test1");
            Test<pixel<double>, double>(2.718281828);
        }

        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }

    }
}
