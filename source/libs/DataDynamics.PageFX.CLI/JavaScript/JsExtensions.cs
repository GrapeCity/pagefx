using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal static class JsExtensions
	{
		public static void WriteBlock<T>(this JsWriter writer, ICollection<T> seq, string separator) where T : JsNode
		{
//			if (seq.Count <= 1)
//			{
//				writer.Write("{");
//				writer.Write(seq, separator);
//				writer.Write("}");
//				return;
//			}

			writer.WriteLine("{");
			writer.IncreaseIndent();
			writer.Write(seq, separator);
			writer.WriteLine();
			writer.DecreaseIndent();
			writer.Write("}");
		}

		public static void Write<T>(this JsWriter writer, IEnumerable<T> seq, string sep) where T:JsNode
		{
			if (seq == null) return;

			bool f = false;
			foreach (var node in seq)
			{
				if (f && !string.IsNullOrEmpty(sep)) writer.Write(sep);
				node.Write(writer);
				f = true;
			}
		}

		public static void WriteValues(this JsWriter writer, IEnumerable seq, string sep)
		{
			if (seq == null) return;
			bool f = false;
			foreach (var value in seq)
			{
				if (f) writer.Write(sep);
				writer.WriteValue(value);
				f = true;
			}
		}

		public static void WriteValue(this JsWriter writer, object value)
		{
			if (value == null)
			{
				writer.Write("null");
				return;
			}

			switch (Type.GetTypeCode(value.GetType()))
			{
				case TypeCode.Empty:
				case TypeCode.DBNull:
					writer.Write("null");
					break;
				
				case TypeCode.Boolean:
					writer.Write((bool)value ? "true" : "false");
					break;
				case TypeCode.Char:
					break;
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					writer.Write(Convert.ToString(value, CultureInfo.InvariantCulture));
					break;
				case TypeCode.DateTime:
					break;
				case TypeCode.String:
					writer.Write("'" + ((string)value).JsEscape() + "'");
					break;
				case TypeCode.Object:
					var obj = value as JsNode;
					if (obj != null)
					{
						obj.Write(writer);
						return;
					}

					var sequence = value as IEnumerable;
					if (sequence != null)
					{
						writer.Write("[");
						var sep = false;
						foreach (var item in sequence)
						{
							if (sep) writer.Write(",");
							WriteValue(writer, item);
							sep = true;
						}
						writer.Write("]");
						return;
					}

					throw new NotSupportedException();
					
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		//http://www.xinotes.org/notes/note/958/
		private static readonly Dictionary<char, string> JsEscapings =
			new Dictionary<char, string>
				{
					{'\'', "\\\'"},
					{'\"', "\\\""},
					{'\\', "\\\\"},
					{':', "\\:"},
					{'\r', "\\r"},
					{'\n', "\\n"},
				};

		/// <summary>
		/// Escapes specified string to be used as string argument in javascript functions.
		/// </summary>
		/// <param name="s">The string to escape.</param>
		/// <returns>An escaped string.</returns>
		public static string JsEscape(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}

			int index = -1;
			for (int i = 0; i < s.Length; i++)
			{
				if (JsEscapings.ContainsKey(s[i]))
				{
					index = i;
					break;
				}
			}

			if (index < 0)
			{
				return s;
			}

			var builder = new StringBuilder(2 * s.Length);
			builder.Append(s.Substring(0, index));

			for (int i = index; i < s.Length; i++)
			{
				string sub;
				if (JsEscapings.TryGetValue(s[i], out sub))
				{
					builder.Append(sub);
				}
				else
				{
					builder.Append(s[i]);
				}
			}

			return builder.ToString();
		}
	}
}