using System;

class X
{
    struct pixel
    {
        public int x;
        public int y;
    };

    static void Test1()
    {
        pixel[,,,] myArray = new pixel[4, 4, 4, 4];

        for (int i1 = 0; i1 < myArray.GetLength(0); i1++)
            for (int i2 = 0; i2 < myArray.GetLength(1); i2++)
                for (int i3 = 0; i3 < myArray.GetLength(2); i3++)
                    for (int i4 = 0; i4 < myArray.GetLength(3); i4++)
                    {
                        myArray[i1, i2, i3, i4].x = (i1 + i3) | (i2 + i4);
                        myArray[i1, i2, i3, i4].y = (i1 + i2) ^ (i3 + i4);
                    }


        for (int i1 = 0; i1 < myArray.GetLength(0); i1++)
            for (int i2 = 0; i2 < myArray.GetLength(1); i2++)
                for (int i3 = 0; i3 < myArray.GetLength(2); i3++)
                    for (int i4 = 0; i4 < myArray.GetLength(3); i4++)
                    {
                        Console.WriteLine("myArray[{0}, {1}, {2}, {3}]= ({4} , {5})", i1, i2, i3, i4, myArray[i1, i2, i3, i4].x, myArray[i1, i2, i3, i4].y);
                    }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}