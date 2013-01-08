using System;
using System.Drawing;
using System.IO;
using Microsoft.Win32;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class Storage
	{
		private const string RegPath = "HKEY_CURRENT_USER\\Software\\Data Dynamics\\PageFX\\QA";

		private static string GetKeyName(string path)
		{
			string res = RegPath;
			string key = Path.GetDirectoryName(path);
			if (!String.IsNullOrEmpty(key))
				res += "\\" + key;
			return res;
		}

		public static T GetValue<T>(string path, T defval)
		{
			try
			{
				string name = Path.GetFileName(path);
				string key = GetKeyName(path);
				return (T)Registry.GetValue(key, name, defval);
			}
			catch
			{
				return defval;
			}
		}

		public static bool GetValue(string path, bool defval)
		{
			try
			{
				string name = Path.GetFileName(path);
				string key = GetKeyName(path);
				var v = Registry.GetValue(key, name, defval);
				var c = v as IConvertible;
				if (c != null) return c.ToBoolean(null);
				return defval;
			}
			catch
			{
				return defval;
			}
		}

		public static Point GetValue(string path, Point defval)
		{
			string s = GetValue(path, "");
			if (String.IsNullOrEmpty(s)) return defval;
			int i = s.IndexOf(';');
			if (i < 0 || i >= s.Length - 1) return defval;
			string xs = s.Substring(0, i).Trim();
			string ys = s.Substring(i + 1).Trim();
			int x, y;
			if (!Int32.TryParse(xs, out x)) return defval;
			if (!Int32.TryParse(ys, out y)) return defval;
			return new Point(x, y);
		}

		public static Size GetValue(string path, Size defval)
		{
			string s = GetValue(path, "");
			if (String.IsNullOrEmpty(s)) return defval;
			int i = s.IndexOf(';');
			if (i < 0 || i >= s.Length - 1) return defval;
			string xs = s.Substring(0, i).Trim();
			string ys = s.Substring(i + 1).Trim();
			int x, y;
			if (!Int32.TryParse(xs, out x) || x < 0) return defval;
			if (!Int32.TryParse(ys, out y) || y < 0) return defval;
			return new Size(x, y);
		}

		public static void SetValue(string path, object value)
		{
			try
			{
				string name = Path.GetFileName(path);
				string key = GetKeyName(path);
				Registry.SetValue(key, name, value);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public static void SetValue(string path, Point pt)
		{
			SetValue(path, pt.X + ";" + pt.Y);
		}

		public static void SetValue(string path, Size value)
		{
			SetValue(path, value.Width + ";" + value.Height);
		}
	}
}
