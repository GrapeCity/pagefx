using System;

class Parser
{
    public Parser(string pattern)
    {
        this.pattern = pattern;
    }

    private string pattern;
    private int ptr;

    public void ConsumeWhitespace(bool ignore)
    {
        while (ptr < pattern.Length)
        {
            if (pattern[ptr] == '(')
            {
                if (ptr + 3 >= pattern.Length)
                    return;

                if (pattern[ptr + 1] != '?' || pattern[ptr + 2] != '#')
                    return;

                ptr += 3;
                while (ptr < pattern.Length && pattern[ptr++] != ')')
                    /* ignore */
                    ;
            }
            else if (ignore && pattern[ptr] == '#')
            {
                while (ptr < pattern.Length && pattern[ptr++] != '\n')
                    /* ignore */
                    ;
            }
            else if (ignore && Char.IsWhiteSpace(pattern[ptr]))
            {
                while (ptr < pattern.Length && Char.IsWhiteSpace(pattern[ptr]))
                    ++ptr;
            }
            else
                return;
        }
    }
}

class X
{
    static void Main()
    {
        Parser p = new Parser("* ? ? ?");
        p.ConsumeWhitespace(true);
        Console.WriteLine("<%END%>");
    }
}