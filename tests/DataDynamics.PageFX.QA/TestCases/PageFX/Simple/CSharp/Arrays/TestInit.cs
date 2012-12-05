using System;

struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }
}

class Program
{
    static void Print(string[,] arr)
    {
        Console.WriteLine(arr.Length);
        for (int i = 0; i < arr.GetLength(0); ++i)
        {
            for (int j = 0; j < arr.GetLength(1); ++j)
            {
                Console.WriteLine(arr[i, j]);
            }
        }
    }

    static void Print(string[,,] arr)
    {
        Console.WriteLine(arr.Length);
        for (int i = 0; i < arr.GetLength(0); ++i)
        {
            for (int j = 0; j < arr.GetLength(1); ++j)
            {
                for (int k = 0; k < arr.GetLength(2); ++k)
                {
                    Console.WriteLine(arr[i, j, k]);
                }
            }
        }
    }

    static void Print(string[, , , ,] arr)
    {
        Console.WriteLine(arr.Length);
        for (int i0 = 0; i0 < arr.GetLength(0); ++i0)
        {
            for (int i1 = 0; i1 < arr.GetLength(1); ++i1)
            {
                for (int i2 = 0; i2 < arr.GetLength(2); ++i2)
                {
                    for (int i3 = 0; i3 < arr.GetLength(3); ++i3)
                    {
                        for (int i4 = 0; i4 < arr.GetLength(4); ++i4)
                        {
                            Console.WriteLine(arr[i0, i1, i2, i3, i4]);
                        }
                    }
                }
            }
        }
    }

    static void Print(Point[,] arr)
    {
        Console.WriteLine(arr.Length);
        for (int i = 0; i < arr.GetLength(0); ++i)
        {
            for (int j = 0; j < arr.GetLength(1); ++j)
            {
                Console.WriteLine(arr[i, j]);
            }
        }
    }

    static void Print(Point[,,] arr)
    {
        Console.WriteLine(arr.Length);
        for (int i = 0; i < arr.GetLength(0); ++i)
        {
            for (int j = 0; j < arr.GetLength(1); ++j)
            {
                for (int k = 0; k < arr.GetLength(2); ++k)
                {
                    Console.WriteLine(arr[i, j, k]);
                }
            }
        }
    }

    static void Test1()
    {
        Point[] arr = new Point[2];
        arr[0] = new Point(10, 10);
        arr[1] = new Point(20, 20);
        Console.WriteLine(arr[0]);
        Console.WriteLine(arr[1]);
    }

    static void Test2()
    {
        char[] c1 = { 'a', 'b', 'c', 'd' };
        char[] c2 = { 'a', 'b', 'c', 'd' };
        Console.WriteLine(c1.Length);
        Console.WriteLine(c2.Length);
    }

    static void Test3()
    {
        string[,] arr = { { "this", "is" }, { "a", "test" } };
        Print(arr);
    }

    static void Test4()
    {
        Point[,] arr = new Point[2, 2];
        Print(arr);
    }

    static void Test5()
    {
        Point[,] arr = 
        {
            {
                new Point(10, 10),
                new Point(20, 20),
            },
            {
                new Point(30, 30),
                new Point(40, 40),
            }
        };
        Print(arr);
    }

    static void Test6()
    {
        Point[, ,] arr = 
        {
            {
                {
                    new Point(10, 10),
                    new Point(20, 20),
                },
                {
                    new Point(30, 30),
                    new Point(40, 40),
                }
            },
            {
                {
                    new Point(60, 60),
                    new Point(70, 70),
                },
                {
                    new Point(80, 80),
                    new Point(90, 90),
                }
            }
        };
        Print(arr);
    }

    static void Test7()
    {
        string[,,] arr = { { { "1", "2" }, { "3", "4" } }, { { "5", "6" }, { "7", "8" } } };
        Print(arr);
    }

    static void Test8()
    {
        string[, , , ,] arr = 
        { 
            { { { { "1", "2" }, { "3", "4" } }, { { "5", "6" }, { "7", "8" } } } },
            { { { { "9", "10" }, { "11", "12" } }, { { "13", "14" }, { "15", "16" } } } } 
        };
        Print(arr);
    }

    static void Main()
    {
        Test1();
        Test2();
        Test3();
        Test4();
        Test5();
        Test6();
        Test7();
        Test8();
        Console.WriteLine("<%END%>");
    }
}