using System;
using System.Text;
using System.Text.RegularExpressions;

class X
{
    static string Test1(string s)
    {
		s = Regex.Replace(Regex.Replace(Regex.Replace(s, "\n", @"\\n"), "\r", @"\\r"), "\t", @"\\t");
		return ("'" + s + "'");
    }

    static void Main()
    {
    	Console.WriteLine(Test1("abc"));
    	Console.WriteLine(Test1("\n"));
    	Console.WriteLine(Test1("\r"));
    	Console.WriteLine(Test1("\t"));

        Regex SortRegex = new Regex(@"^((\[(?<ColName>.+)\])|(?<ColName>\S+))([ ]+(?<Order>ASC|DESC))?$",
                        RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        Console.WriteLine("<%END%>");
    }
}