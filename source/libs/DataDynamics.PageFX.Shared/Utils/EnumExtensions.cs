using System;
using System.Linq;
using System.Reflection;

namespace DataDynamics
{
	public static class EnumExtensions
	{
		public static string EnumString<T>(this T value) where T:struct 
		{
			if (!value.GetType().IsEnum)
				throw new ArgumentException("Value is not enum.", "value");

			const BindingFlags bf = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField;
			var fields = typeof(T).GetFields(bf);

			foreach (var field in fields)
			{
				var v = (T)field.GetValue(null);
				if (Equals(value, v))
				{
					var attrs = (StringAttribute[])field.GetCustomAttributes(typeof(StringAttribute), false);
					if (attrs.Length > 0)
						return attrs[0].Value;
					break;
				}
			}

			return value.ToString();
		}

		public static T EnumParse<T>(this string s, T defval, bool ignoreCase)
		{
			if (string.IsNullOrEmpty(s))
				return defval;

			const BindingFlags bf = BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField;
			var fields = typeof(T).GetFields(bf);

			foreach (var field in fields)
			{
				var attrs = (StringAttribute[])field.GetCustomAttributes(typeof(StringAttribute), false);
				if (attrs.Any(attr => String.Compare(attr.Value, s, ignoreCase) == 0))
				{
					return (T)field.GetValue(null);
				}
			}

			try
			{
				return (T)Enum.Parse(typeof(T), s, ignoreCase);
			}
			catch (Exception)
			{
				return defval;
			}
		}

		public static T EnumParse<T>(this string s, T defval)
		{
			return s.EnumParse(defval, true);
		}
	}
}