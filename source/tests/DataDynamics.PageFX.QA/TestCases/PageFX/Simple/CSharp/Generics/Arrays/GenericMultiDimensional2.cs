using System;

namespace GenericMultiDimensional2
{
    class X
    {
        static void Test<T>(T value)
        {
            T[, , ,] myArray = new T[2, 2, 2, 2];

            for (int i1 = 0; i1 < myArray.GetLength(0); i1++)
                for (int i2 = 0; i2 < myArray.GetLength(1); i2++)
                    for (int i3 = 0; i3 < myArray.GetLength(2); i3++)
                        for (int i4 = 0; i4 < myArray.GetLength(3); i4++)
                        {
                            myArray[i1, i2, i3, i4] = value;
                        }


            for (int i1 = 0; i1 < myArray.GetLength(0); i1++)
                for (int i2 = 0; i2 < myArray.GetLength(1); i2++)
                    for (int i3 = 0; i3 < myArray.GetLength(2); i3++)
                        for (int i4 = 0; i4 < myArray.GetLength(3); i4++)
                        {
                            Console.WriteLine("myArray[{0}, {1}, {2}, {3}]= {4}", i1, i2, i3, i4, myArray[i1, i2, i3, i4]);
                        }
        }

        static void Test1()
        {
            Console.WriteLine("----Test1");
            Test<int>(1);
            Test<string>("X");
        }

        static void Main()
        {
            Test1();
            Console.WriteLine("<%END%>");
        }

    }
}
