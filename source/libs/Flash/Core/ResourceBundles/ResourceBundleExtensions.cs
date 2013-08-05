using System.IO;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.ResourceBundles
{
	internal static class ResourceBundleExtensions
	{
		public static string GetResourceBundleName(this AbcMetaEntry e)
		{
			if (e.Items.Count <= 0)
				return null;
			var val = e.Items[0].Value;
			if (val == null) return null;
			string s = val.Value;
			return string.IsNullOrEmpty(s) ? null : s;
		}

		public static bool IsResourceBundleComment(this string line)
		{
			if (string.IsNullOrEmpty(line)) return false;
			return line[0] == '#' || line[0] == '!';
		}

		public static string[] GetResourceBundleLines(this Stream stream)
		{
			using (var reader = new StreamReader(stream))
				return reader.GetResourceBundleLines();
		}

		public static string[] GetResourceBundleLines(this TextReader reader)
		{
			return reader.ReadLines(true, line => line.Length != 0 && !IsResourceBundleComment(line));
		}
	}
}