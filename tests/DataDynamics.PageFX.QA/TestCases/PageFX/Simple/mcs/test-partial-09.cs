// The standard says this doesn't have to have the 'abstract' modifier
using System;

public partial class Foo
{
	public string myId;
}

public abstract partial class Foo
{
	public string Id { get { return myId; } }
}

public class Bar : Foo
{
	public Bar (string id) { myId = id; }
}

public class PartialAbstractCompilationError
{
	public static void Main ()
	{
        Console.WriteLine(typeof(Foo).IsAbstract);
        Console.WriteLine("<%END%>");
	}
}

