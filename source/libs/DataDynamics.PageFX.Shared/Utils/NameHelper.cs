using System.Text;

namespace DataDynamics
{
    public static class NameHelper
    {
        public static string Cap(string s)
        {
            if (char.IsLower(s[0]))
            {
                var arr = s.ToCharArray();
                arr[0] = char.ToUpper(arr[0]);
                return new string(arr);
            }
            return s;
        }

        public static void ToLower(char[] c, int begin, int end)
        {
            for (int i = begin; i <= end && i < c.Length; ++i)
                c[i] = char.ToLower(c[i]);
        }

        public static string Recap(string name)
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

        public static string Rename(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            if (name.Length <= 2) return name.ToUpper();

            var parts = name.Split('_');
            var s = new StringBuilder();
            foreach (string part in parts)
            {
            	s.Append(string.IsNullOrEmpty(part) ? "_" : Recap(part));
            }
        	return s.ToString();
        }

        public static string MakeFullName(string ns, string name)
        {
            if (string.IsNullOrEmpty(ns))
                return name;
            if (string.IsNullOrEmpty(name))
                return ns;
            return ns + "." + name;
        }
    }
}