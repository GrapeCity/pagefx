using System;
using System.Text;
using System.Text.RegularExpressions;

class X
{
    // Regex to parse the Sort string.
    //static Regex SortRegex = new Regex(@"^((\[(?<ColName>.+)\])|(?<ColName>\S+))([ ]+(?<Order>ASC|DESC))?$",
    //                    RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

    static void Main()
    {
        Regex SortRegex = new Regex(@"^((\[(?<ColName>.+)\])|(?<ColName>\S+))([ ]+(?<Order>ASC|DESC))?$",
                        RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
        Console.WriteLine("<%END%>");
    }
}