using System;
using System.Reflection;

namespace DataDynamics
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class StringAttribute : Attribute
    {
        public StringAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
        private readonly string _value;

        public static string EnumToString<T>(T value)
        {
            const BindingFlags bf = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField;
            var fields = typeof(T).GetFields(bf);
            foreach (var field in fields)
            {
                var v = (T)field.GetValue(null);
                if (Equals(value, v))
                {
                    var attrs =
                        (StringAttribute[])field.GetCustomAttributes(typeof(StringAttribute), false);
                    if (attrs != null && attrs.Length > 0)
                        return attrs[0].Value;
                    break;
                }
            }
            return value.ToString();
        }

        public static T ParseEnum<T>(string s, T defval, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(s))
                return defval;

            const BindingFlags bf = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField;
            var fields = typeof(T).GetFields(bf);
            foreach (var field in fields)
            {
                var attrs = (StringAttribute[])field.GetCustomAttributes(typeof(StringAttribute), false);
                if (attrs != null)
                {
                    int n = attrs.Length;
                    for (int i = 0; i < n; ++i)
                    {
                        if (string.Compare(attrs[i].Value, s, ignoreCase) == 0)
                            return (T)field.GetValue(null);
                    }
                }
            }
            return defval;
        }

        public static T ParseEnum<T>(string s, T defval)
        {
            return ParseEnum(s, defval, true);
        }
    }
}