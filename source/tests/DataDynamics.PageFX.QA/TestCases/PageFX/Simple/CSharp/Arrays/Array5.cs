using System;

class A
{
    public int id;
    public string name;

    public A(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}

class Array5
{
    static void Main()
    {
        A[] arr = new A[3];
        arr[0] = new A(1, "aaa");
        arr[1] = new A(2, "bbb");
        arr[2] = new A(3, "ccc");
        for (int i = 0; i < 3; ++i)
        {
            Console.WriteLine(arr[i].id);
            Console.WriteLine(arr[i].name);
        }
        Console.WriteLine("<%END%>");
    }
}