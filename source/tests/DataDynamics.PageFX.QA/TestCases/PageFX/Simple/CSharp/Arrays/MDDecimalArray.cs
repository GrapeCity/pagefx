using System;

class X
{
    static void Test1()
    {
        decimal[,] arr = { 
				{1m, 0, 1m}, {1.234567890m, 1, 1.2m}, 
				{1.234567890m, 2, 1.23m}, {1.23450000001m, 3, 1.235m}, 
				{1.2355m, 3, 1.236m}, 
				{1.234567890m, 4, 1.2346m}, {1.23567890m, 2, 1.24m}, 
				{47893764694.4578563236436621m, 7, 47893764694.4578563m},
				{-47893764694.4578563236436621m, 9, -47893764694.457856324m},
				{-47893764694.4578m, 5, -47893764694.4578m}
			};

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            Console.WriteLine(arr[i, 0]);
            Console.WriteLine(arr[i, 1]);
            Console.WriteLine(arr[i, 2]);
        }
    }

    static void Main()
    {
        Test1();
        Console.WriteLine("<%END%>");
    }
}