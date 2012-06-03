using System;
using System.Text;

internal class Test
{
    public object Run()
    {
    	var e = Encoding.Default;
    	return e;
    }
    
    static void Main()
    {
        var test = new Test();
        try
        {
            test.Run();
		}
        catch (Exception e)
        {
            Console.WriteLine("exception: {0}", e.GetType());
        }
        Console.WriteLine("<%END%>");
    }
}