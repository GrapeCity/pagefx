using System;

class Test
{
	static void Main()
	{
		Avm.Object properties = new Avm.Object();
		properties.SetProperty("DocumentId", "abc");
		var id = properties.GetProperty("DocumentId");
		Console.WriteLine(id);
		Console.WriteLine("<%END%>");
	}
}