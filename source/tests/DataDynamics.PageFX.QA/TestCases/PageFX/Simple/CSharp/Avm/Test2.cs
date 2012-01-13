using System;

class MyError : Avm.Error
{
    public MyError(string str)
    {
        msg = str;
    }

    public string msg;
}

class Test2
{
    static void Main()
    {
        MyError err = new MyError("aaa");
        Console.WriteLine(err.msg);
        Console.WriteLine("<%END%>");
    }
}