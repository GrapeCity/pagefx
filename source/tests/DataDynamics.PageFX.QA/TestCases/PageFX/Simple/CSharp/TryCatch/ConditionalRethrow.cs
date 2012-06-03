using System;

internal class Test
{
    bool _running;

    void Test1()
    {
        Console.WriteLine("Test::Foo");
        throw new Exception("aaa");
    }

	public class TryCatchStatement
	{
		public object Catch { get; set; }
	}

    public object Run(TryCatchStatement statement)
    {
        try
        {
            _running = true;
            Test1();
            return this;
        }
        catch (Exception e)
        {
			if (statement.Catch != null)
			{
				Console.WriteLine(statement.Catch);
			}
			else
			{
				throw;
			}
        }
        finally
        {
            _running = false;
        }
    	return null;
    }
    
    static void Main()
    {
        var test = new Test();
        try
        {
            test.Run(new TryCatchStatement
                     	{
                     		Catch = "catch me"
                     	});
        	test.Run(new TryCatchStatement());
		}
        catch (Exception e)
        {
            Console.WriteLine("exception: {0}", e.GetType());
        }
        Console.WriteLine("<%END%>");
    }
}