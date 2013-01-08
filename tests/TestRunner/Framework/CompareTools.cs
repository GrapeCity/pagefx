using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class CompareTools
	{
		public static string CompareFiles(string path1, string path2)
		{
			var a = File.ReadAllBytes(path1);
			var b = File.ReadAllBytes(path2);
			return CompareByteArrays(a, b);
		}

		public static string CompareByteArrays(byte[] a, byte[] b, out int index)
		{
			index = -1;
			var err = new StringBuilder();
			int an = a.Length;
			int bn = b.Length;
			if (an != bn)
				err.AppendFormat("Length of byte arrays is different. {0} != {1}.\n", an, bn);

			int n = Math.Min(an, bn);
			for (int i = 0; i < n; ++i)
			{
				if (a[i] != b[i])
				{
					index = i;
					err.AppendFormat("Byte arrays are different at index {0}. Expected {1}, but was {2}.\n",
					                 i, a[i], b[i]);
					break;
				}
			}

			if (err.Length > 0)
				return err.ToString();
			return null;
		}

		public static string CompareByteArrays(byte[] a, byte[] b)
		{
			int index;
			return CompareByteArrays(a, b, out index);
		}

		private class Lines
		{
			readonly List<string> _1 = new List<string>();
			readonly List<string> _2 = new List<string>();
			int _diff;
			int _maxlen;

			public void Add(string line1, string line2)
			{
				if (line1 == null)
					line1 = "";
				if (line2 == null)
					line2 = "";
				_1.Add(line1);
				_2.Add(line2);
				_maxlen = Math.Max(_maxlen, line1.Length);
			}

			public void SetDiffLine()
			{
				_diff = _1.Count;
			}

			string FormatLine(int i)
			{
				int n = _1.Count;
				int maxNumLen = (n + ". ").Length;

				int num = i + 1;
				string s = num + ". ";
				s = s.PadLeft(maxNumLen);
				string f = String.Format("{{0,-{0}}}{{1}}", _maxlen + 4);

				string l1 = _1[i];
				string l2 = _2[i];
				return s + String.Format(f, l1, l2);
			}

			public override string ToString()
			{
				var sb = new StringBuilder(256);
				sb.AppendFormat("Outputs are different at line {0}.", _diff);
				sb.AppendLine();
				sb.AppendLine("Output:");

				int n = Math.Min(_1.Count, _2.Count);
				for (int i = 0; i < n; ++i)
				{
					sb.AppendLine(FormatLine(i));
				}
				return sb.ToString();
			}
		}

		public static string CompareLines(string text1, string text2, bool ignoreCase)
		{
			if (String.IsNullOrEmpty(text1))
			{
				if (!String.IsNullOrEmpty(text2))
				{
					return "Outputs are different. Second output is not empty, but first is empty.";
				}
			}
			if (String.IsNullOrEmpty(text2))
			{
				return "Outputs are different. Second output is empty, but first is not empty.";
			}

			bool notFast = !GlobalOptions.IsNUnitSession;

			using (var reader1 = new StringReader(text1))
			using (var reader2 = new StringReader(text2))
			{
				var lines = new Lines();
				while (true)
				{
					string line1 = reader1.ReadLine();
					string line2 = reader2.ReadLine();
					if (line1 == null)
					{
						if (line2 == null)
						{
							//ok!!!
							return null;
						}

						lines.Add(null, line2);
						lines.SetDiffLine();

						if (notFast)
							ReadRestLines(reader1, reader2, null, line2, lines);
						break;
					}

					if (line2 == null)
					{
						lines.Add(line1, null);
						lines.SetDiffLine();

						if (notFast)
							ReadRestLines(reader1, reader2, line1, null, lines);
						break;
					}

					if (line1.Length == 0)
					{
						if (line2 == "null")
						{
							lines.Add(line1, line2);
							continue;
						}
					}

					lines.Add(line1, line2);

					if (String.Compare(line1, line2, ignoreCase) != 0)
					{
						lines.SetDiffLine();
						if (notFast)
							ReadRestLines(reader1, reader2, line1, line2, lines);
						break;


					}
				}
				return lines.ToString();

			}
		}

		private static void ReadRestLines(TextReader reader1, TextReader reader2, string line1, string line2, Lines lines)
		{
			while (true)
			{
				if (line1 != null)
					line1 = reader1.ReadLine();
				if (line2 != null)
					line2 = reader2.ReadLine();
				if (line1 == null && line2 == null)
					break;
				lines.Add(line1, line2);
			}
		}
	}
}
