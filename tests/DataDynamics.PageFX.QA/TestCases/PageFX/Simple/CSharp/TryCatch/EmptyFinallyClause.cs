using System;

partial class Program
{
	partial void Foo();
	partial void Bar();
    
    static void Main()
    {
    	var program = new Program();

		try
		{
			Console.WriteLine("Hello!");
		} 
		finally
		{
			program.Foo();
			program.Bar();
		}
        
        Console.WriteLine("<%END%>");
    }
}