using System;

interface I
{
	string Params { get; }
}

class A : I
{
	internal string Params
	{
		get { return "A::Params"; }
	}

	string I.Params
    {
	    get { return Params; }
	}
}

class Program
{
    static void Main()
    {
        var obj = new A();
        Console.WriteLine(obj.Params);
        Console.WriteLine(((I)obj).Params);
        Console.WriteLine("<%END%>");
    }
}
