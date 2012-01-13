using System;
using System.Collections.Generic;

class C
{
	private IEnumerator<KeyValuePair<string, object>> Test ()
	{
		yield break;
	}
	
	public static void Main ()
	{
        foreach (var v in Test())
        {
            Console.WriteLine(v.Key);
        }

        Console.WriteLine("<%END%>");
	}
}
