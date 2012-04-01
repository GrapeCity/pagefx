using System.Text;

namespace DataDynamics
{
    public static class NameExtensions
    {
        public static string Capitalize(string s)
        {
        	if (!char.IsLower(s[0]))
        	{
        		return s;
        	}
        	var arr = s.ToCharArray();
        	arr[0] = char.ToUpper(arr[0]);
        	return new string(arr);
        }

        private static void ToLower(char[] c, int begin, int end)
        {
            for (int i = begin; i <= end && i < c.Length; ++i)
                c[i] = char.ToLower(c[i]);
        }

        private static string Recapitalize(string name)
        {
            if (string.IsNullOrEmpty(name)) return "";
            var c = name.ToCharArray();
            c[0] = char.ToUpper(c[0]);
            for (int i = 0; i < c.Length; ++i)
            {
                if (char.IsUpper(c[i]))
                {
                    if (i + 1 < c.Length)
                    {
                        if (char.IsUpper(c[i + 1])) //abr
                        {
                            int end = i;
                            for (; end < c.Length; ++end)
                            {
                                if (!char.IsUpper(c[end]))
                                {
                                    end -= 2;
                                    break;
                                }
                            }
                            if (end >= c.Length)
                                end = c.Length - 1;
                            int len = end - i + 1;
                            if (len >= 3)
                            {
                                ToLower(c, i + 1, end);
                            }
                            i = end;
                        }
                        else
                        {
                            for (++i; i < c.Length; ++i)
                            {
                                if (char.IsUpper(c[i]))
                                {
                                    --i;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return new string(c);
        }

        public static string ToPascalCase(this string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            if (name.Length <= 2) return name.ToUpper();

            var parts = name.Split('_');
            var s = new StringBuilder();
            foreach (string part in parts)
            {
            	s.Append(string.IsNullOrEmpty(part) ? "_" : Recapitalize(part));
            }
        	return s.ToString();
        }

        public static string MakeFullName(this string nameSpace, string name)
        {
            if (string.IsNullOrEmpty(nameSpace))
                return name;
            if (string.IsNullOrEmpty(name))
                return nameSpace;
            return nameSpace + "." + name;
        }
    }
}