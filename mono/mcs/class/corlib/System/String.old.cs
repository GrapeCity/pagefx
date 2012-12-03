//CHANGED
//
// System.String.cs
//
// Authors:
//   Patrik Torstensson
//   Jeffrey Stedfast (fejj@ximian.com)
//   Dan Lewis (dihlewis@yahoo.co.uk)
//   Sebastien Pouliot  <sebastien@ximian.com>
//   Marek Safar (marek.safar@seznam.cz)
//
// (C) 2001 Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004-2005 Novell (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Text;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;

#if NET_2_0
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Mono.Globalization.Unicode;
#endif

using avmstr = Avm.String;

namespace System
{
    [Serializable]
#if NET_2_0
	public sealed class String : IConvertible, ICloneable, IEnumerable, IComparable, IComparable<String>, IEquatable <String>, IEnumerable<char>
#else
    public sealed class String : IConvertible, ICloneable, IEnumerable, IComparable
#endif
    {
        internal avmstr m_value;

        public static implicit operator avmstr(String s)
        {
            if (s == null) return null;
            return s.m_value;
        }

        public static implicit operator String(avmstr s)
        {
            if (s == null) return null;
            return new String(s);
        }

        #region Constructors
        internal String()
        {
            m_value = avm.EmptyString;
        }

        internal String(avmstr value)
        {
            m_value = value;
        }

        #region unsafe ctors
        //TODO: Remove
        //[CLSCompliant(false), MethodImplAttribute(MethodImplOptions.InternalCall)]
        //unsafe public extern String(char* value);

        //[CLSCompliant(false), MethodImplAttribute(MethodImplOptions.InternalCall)]
        //unsafe public extern String(char* value, int startIndex, int length);

        //[CLSCompliant(false), MethodImplAttribute(MethodImplOptions.InternalCall)]
        //unsafe public extern String(sbyte* value);

        //[CLSCompliant(false), MethodImplAttribute(MethodImplOptions.InternalCall)]
        //unsafe public extern String(sbyte* value, int startIndex, int length);

        //[CLSCompliant(false), MethodImplAttribute(MethodImplOptions.InternalCall)]
        //unsafe public extern String(sbyte* value, int startIndex, int length, Encoding enc);
        #endregion

        public String(char[] value, int startIndex, int length)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "< 0");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "< 0");
            if (startIndex > value.Length - length)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex + length > value.Length");
            m_value = avm.EmptyString;
            while (length-- > 0)
            {
                char c = value[startIndex];
                m_value = avm.Concat(m_value, avmstr.fromCharCode(c));
                ++startIndex;
            }
        }

        public String(char[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            m_value = avm.EmptyString;
            int n = value.Length;
            for (int i = 0; i < n; ++i)
            {
                m_value = avm.Concat(m_value, avmstr.fromCharCode(value[i]));
            }
        }

        public String(char c, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "count < 0");
            m_value = avm.EmptyString;
            while (count-- > 0)
            {
                m_value = avm.Concat(m_value, avmstr.fromCharCode(c));
            }
        }
        #endregion

        public static readonly String Empty = new String(avm.EmptyString);

        private static String NewEmpty()
        {
            return new String();
        }

        #region Equals
        public static bool Equals(string a, string b)
        {
            if (a == null) return b == null;
            if (b == null) return false;
            return a.m_value == b.m_value;
        }
        
        public static bool operator ==(String a, String b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(String a, String b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            string s = obj as string;
            if (s == null) return false;
            return s.m_value == m_value;
        }

        public bool Equals(String value)
        {
            return Equals(this, value);
        }
        #endregion

        [IndexerName("Chars")]
        public char this[int index]
        {
            get
            {
                if (index < 0 || index >= m_value.length)
                    throw new IndexOutOfRangeException();
                return m_value[index];
            }
            internal set
            {
                //TODO: OPTIMIZE!
                char[] c = ToCharArray();
                c[index] = value;
                m_value = avmstr.fromCharCode(c);
            }
        }

        public int Length
        {
            get { return m_value.length; }
        }

        public object Clone()
        {
            return new String(m_value);
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.String;
        }

        #region ToCharArray & CopyTo
        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            // LAMESPEC: should I null-terminate?
            if (destination == null)
                throw new ArgumentNullException("destination");

            if (sourceIndex < 0 || destinationIndex < 0 || count < 0)
                throw new ArgumentOutOfRangeException();

            // re-ordered to avoid possible integer overflow
            if (sourceIndex > Length - count)
                throw new ArgumentOutOfRangeException("sourceIndex", "sourceIndex + count > Length");
            // re-ordered to avoid possible integer overflow
            if (destinationIndex > destination.Length - count)
                throw new ArgumentOutOfRangeException("destinationIndex", "destinationIndex + count > destination.Length");

            while (count-- > 0)
            {
                destination[destinationIndex] = this[sourceIndex];
                ++sourceIndex;
                ++destinationIndex;
            }
        }

        public static String Copy(String str)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            return new String(str.m_value);
        }

        public static String CopyInternal(String str)
        {
            if (str == null) return null;
            return new String(str.m_value);
        }

        internal static String FastCopy(String str)
        {
            if (str == null) return null;
            return new String(str.m_value);
        }

        public char[] ToCharArray()
        {
            return ToCharArray(0, Length);
        }

        public char[] ToCharArray(int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "< 0");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "< 0");
            // re-ordered to avoid possible integer overflow
            if (startIndex > Length - length)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex + length > Length");

            //TODO: Optimze!!!
            char[] result = new char[length];
            for (int i = 0; i < length; ++i)
                result[i] = this[startIndex + i];
            return result;
        }
        #endregion

        #region Split
        public String[] Split(params char[] separator)
        {
            return Split(separator, Int32.MaxValue);
        }

        public String[] Split(char[] separator, int count)
        {
            if (separator == null || separator.Length == 0)
                separator = WhiteChars;

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (count == 0)
                return new String[0];

            if (count == 1)
                return new String[1] {ToString()};

            int n = Length;
            if (n == 0)
                return new String[1] {ToString()};

            Avm.Array arr = avm.NewArray();
            int startIndex = 0;
            int last = 0;
            int sepLength = separator.Length;
            int endIndex = last;
            while (last < n)
            {
                bool isSep = false;
                for (int i = 0; i < sepLength; ++i)
                {
                    if (m_value[last] == separator[i])
                    {
                        isSep = true;
                        break;
                    }
                }

                if (isSep)
                {
                    if (last == 0)
                    {
                        arr.push(avm.EmptyString);
                    }
                    else if (arr.length == count - 1)
                    {
                        arr.push(m_value.substring(startIndex));
                        break;
                    }
                    else
                    {
                        endIndex = last;
                        if (startIndex == last)
                            endIndex = last + 1;
                        arr.push(m_value.substring(startIndex, endIndex));
                    }
                    ++last;
                    startIndex = last;
                }
                else
                {
                    ++last;
                }
            }

            if (arr.length < count)
            {
                arr.push(m_value.substring(startIndex));
            }

            n = (int)arr.length;
            string[] res = new string[n];
            for (int i = 0; i < n; ++i)
                res[i] = new string((avmstr)arr[i]);

            return res;
        }

#if NET_2_0
		[ComVisible (false)]
		[MonoDocumentationNote ("optimization")]
		public String[] Split (char[] separator, int count, StringSplitOptions options)
		{
			if (separator == null || separator.Length == 0)
				return Split (WhiteChars, count, options);

			if (count < 0)
				throw new ArgumentOutOfRangeException ("count", "Count cannot be less than zero.");
			if ((options != StringSplitOptions.None) && (options != StringSplitOptions.RemoveEmptyEntries))
				throw new ArgumentException ("options must be one of the values in the StringSplitOptions enumeration", "options");

			bool removeEmpty = (options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;

			if (!removeEmpty)
				return Split (separator, count);
			else {
				/* FIXME: Optimize this */
				String[] res = Split (separator, count);
				int n = 0;
				for (int i = 0; i < res.Length; ++i)
					if (res [i] == String.Empty)
						n ++;
				if (n > 0) {
					String[] arr = new String [res.Length - n];
					int pos = 0;
					for (int i = 0; i < res.Length; ++i)
						if (res [i] != String.Empty)
							arr [pos ++] = res [i];
					return arr;
				}
				else
					return res;
			}
		}

		[ComVisible (false)]
		public String[] Split (string[] separator, int count, StringSplitOptions options)
		{
			if (separator == null || separator.Length == 0)
				return Split (WhiteChars, count, options);

			if (count < 0)
				throw new ArgumentOutOfRangeException ("count", "Count cannot be less than zero.");
			if ((options != StringSplitOptions.None) && (options != StringSplitOptions.RemoveEmptyEntries))
				throw new ArgumentException ("Illegal enum value: " + options + ".", "options");

			bool removeEmpty = (options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;

			if (count == 0 || (this == String.Empty && removeEmpty))
				return new String [0];

			ArrayList arr = new ArrayList ();

			int pos = 0;
			while (pos < this.Length) {
				int matchIndex = -1;
				int matchPos = Int32.MaxValue;

				// Find the first position where any of the separators matches
				for (int i = 0; i < separator.Length; ++i) {
					string sep = separator [i];
					if (sep == null || sep == String.Empty)
						continue;

					int match = IndexOf (sep, pos);
					if (match > -1 && match < matchPos) {
						matchIndex = i;
						matchPos = match;
					}
				}

				if (matchIndex == -1)
					break;

				if (matchPos == pos && removeEmpty) {
					pos = matchPos + separator [matchIndex].Length;
				}
				else {
					arr.Add (this.Substring (pos, matchPos - pos));

					pos = matchPos + separator [matchIndex].Length;

					if (arr.Count == count - 1) {
						break;
					}
				}
			}

			if (arr.Count == 0)
				return new String [] { this };
			else {
				if (removeEmpty && pos == this.Length) {
					String[] res = new String [arr.Count];
					arr.CopyTo (0, res, 0, arr.Count);

					return res;
				}
				else {
					String[] res = new String [arr.Count + 1];
					arr.CopyTo (0, res, 0, arr.Count);
					res [arr.Count] = this.Substring (pos);

					return res;
				}
			}
		}

		[ComVisible (false)]
		public String[] Split (char[] separator, StringSplitOptions options)
		{
			return Split (separator, Int32.MaxValue, options);
		}

		[ComVisible (false)]
		public String[] Split (String[] separator, StringSplitOptions options)
		{
			return Split (separator, Int32.MaxValue, options);
		}
#endif
        #endregion

        #region Substring
        public String Substring(int startIndex)
        {
            if (startIndex == 0)
                return this;

            if (startIndex < 0 || startIndex > Length)
                throw new ArgumentOutOfRangeException("startIndex");

            return new String(m_value.substring(startIndex));
        }

        public String Substring(int startIndex, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "< 0");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "< 0");
            // re-ordered to avoid possible integer overflow
            if (startIndex > Length - length)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex + length > Length");

            if (length == 0)
                return NewEmpty();

            return new String(m_value.substr(startIndex, length));
        }
        #endregion

        #region WhiteChars
        private static char[] WhiteChars
        {
            get
            {
                if (_whiteChars == null)
                {
                    _whiteChars = new char[]
                        {
                            (char)0x9, (char)0xA, (char)0xB, (char)0xC, (char)0xD,
#if NET_2_0 || PFX
                            (char)0x85, (char)0x1680, (char)0x2028, (char)0x2029,
#endif
                            (char)0x20, (char)0xA0, (char)0x2000, (char)0x2001, (char)0x2002, (char)0x2003, (char)0x2004
                            ,
                            (char)0x2005, (char)0x2006, (char)0x2007, (char)0x2008, (char)0x2009, (char)0x200A,
                            (char)0x200B,
                            (char)0x3000, (char)0xFEFF
                        };
                }
                return _whiteChars;
            }
        }
        private static char[] _whiteChars;
        #endregion

        #region Trim
        public String Trim()
        {
            return Trim(WhiteChars);
        }

        public String Trim(params char[] trimChars)
        {
            int n = Length;
            if (n == 0)
                return new String(m_value);

            if (trimChars == null || trimChars.Length == 0)
                trimChars = WhiteChars;

            int start = 0;
            while (start < n)
            {
                if (!Contains(trimChars, m_value[start]))
                    break;
                ++start;
            }

            if (start >= n)
                return NewEmpty();

            int end = n - 1;
            while (end > start)
            {
                if (!Contains(trimChars, m_value[end]))
                    break;
                --end;
            }

            return new String(m_value.substring(start, end + 1));
        }

        private static bool Contains(char[] arr, char c)
        {
            int n = arr.Length;
            for (int i = 0; i < n; ++i)
                if (arr[i] == c)
                    return true;
            return false;
        }

        public String TrimStart(params char[] trimChars)
        {
            int n = Length;
            if (n == 0)
                return new String(m_value);

            if (trimChars == null || trimChars.Length == 0)
                trimChars = WhiteChars;

            int i = 0;
            while (i < n)
            {
                if (!Contains(trimChars, m_value[i]))
                    break;
                ++i;
            }

            if (i >= n)
                return NewEmpty();

            return new String(m_value.substring(i));
        }

        public String TrimEnd(params char[] trimChars)
        {
            int n = Length;
            if (n == 0)
                return new String(m_value);

            if (trimChars == null || trimChars.Length == 0)
                trimChars = WhiteChars;

            int i = n - 1;
            while (i >= 0)
            {
                if (!Contains(trimChars, m_value[i]))
                    break;
                --i;
            }

            if (i <= 0)
                return NewEmpty();

            return new String(m_value.substring(0, i + 1));
        }
        #endregion

        #region Compare
        public static int Compare(String strA, String strB)
        {
            return Compare(strA, strB, false, CultureInfo.CurrentCulture);
        }

        public static int Compare(String strA, String strB, bool ignoreCase)
        {
            return Compare(strA, strB, ignoreCase, CultureInfo.CurrentCulture);
        }

        public static int Compare(String strA, String strB, bool ignoreCase, CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");

            if (strA == null)
            {
                if (strB == null) return 0;
                return -1;
            }
            if (strB == null)
                return 1;

            CompareOptions compopts = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;

            return culture.CompareInfo.Compare(strA, strB, compopts);
        }

        public static int Compare(String strA, int indexA, String strB, int indexB, int length)
        {
            return Compare(strA, indexA, strB, indexB, length, false, CultureInfo.CurrentCulture);
        }

        public static int Compare(String strA, int indexA, String strB, int indexB, int length, bool ignoreCase)
        {
            return Compare(strA, indexA, strB, indexB, length, ignoreCase, CultureInfo.CurrentCulture);
        }

        public static int Compare(String strA, int indexA, String strB, int indexB, int length, bool ignoreCase, CultureInfo culture)
        {
            if (length == 0)
                return 0;

            if (strA == null)
            {
                if (strB == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (strB == null)
            {
                return 1;
            }

            if (culture == null)
                throw new ArgumentNullException("culture");

            if ((indexA > strA.Length) || (indexB > strB.Length) || (indexA < 0) || (indexB < 0) || (length < 0))
                throw new ArgumentOutOfRangeException();

            CompareOptions compopts = ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None;

            /* Need to cap the requested length to the
             * length of the string, because
             * CompareInfo.Compare will insist that length
             * <= (string.Length - offset)
             */
            int len1 = length;
            int len2 = length;

            if (length > (strA.Length - indexA))
            {
                len1 = strA.Length - indexA;
            }

            if (length > (strB.Length - indexB))
            {
                len2 = strB.Length - indexB;
            }

            return culture.CompareInfo.Compare(strA, indexA, len1, strB, indexB, len2, compopts);
        }
#if NET_2_0
		public static int Compare (string strA, string strB, StringComparison comparisonType)
		{
			switch (comparisonType) {
			case StringComparison.CurrentCulture:
				return Compare (strA, strB, false, CultureInfo.CurrentCulture);
			case StringComparison.CurrentCultureIgnoreCase:
				return Compare (strA, strB, true, CultureInfo.CurrentCulture);
			case StringComparison.InvariantCulture:
				return Compare (strA, strB, false, CultureInfo.InvariantCulture);
			case StringComparison.InvariantCultureIgnoreCase:
				return Compare (strA, strB, true, CultureInfo.InvariantCulture);
			case StringComparison.Ordinal:
				return CompareOrdinal (strA, strB, CompareOptions.Ordinal);
			case StringComparison.OrdinalIgnoreCase:
				return CompareOrdinal (strA, strB, CompareOptions.Ordinal | CompareOptions.IgnoreCase);
			default:
				string msg = Locale.GetText ("Invalid value '{0}' for StringComparison", comparisonType);
				throw new ArgumentException ("comparisonType", msg);
			}
		}

		public static int Compare (string strA, int indexA, string strB, int indexB, int length, StringComparison comparisonType)
		{
			switch (comparisonType) {
			case StringComparison.CurrentCulture:
				return Compare (strA, indexA, strB, indexB, length, false, CultureInfo.CurrentCulture);
			case StringComparison.CurrentCultureIgnoreCase:
				return Compare (strA, indexA, strB, indexB, length, true, CultureInfo.CurrentCulture);
			case StringComparison.InvariantCulture:
				return Compare (strA, indexA, strB, indexB, length, false, CultureInfo.InvariantCulture);
			case StringComparison.InvariantCultureIgnoreCase:
				return Compare (strA, indexA, strB, indexB, length, true, CultureInfo.InvariantCulture);
			case StringComparison.Ordinal:
				return CompareOrdinal (strA, indexA, strB, indexB, length, CompareOptions.Ordinal);
			case StringComparison.OrdinalIgnoreCase:
				return CompareOrdinal (strA, indexA, strB, indexB, length, CompareOptions.Ordinal | CompareOptions.IgnoreCase);
			default:
				string msg = Locale.GetText ("Invalid value '{0}' for StringComparison", comparisonType);
				throw new ArgumentException ("comparisonType", msg);
			}
		}

		public static bool Equals (string a, string b, StringComparison comparisonType)
		{
			return String.Compare (a, b, comparisonType) == 0;
		}

		public bool Equals (string value, StringComparison comparisonType)
		{
			return String.Equals (this, value, comparisonType);
		}
#endif
        public int CompareTo(object value)
        {
            if (value == null) return 1;

            string s = value as string;
            if (s == null)
                throw new ArgumentException();

            return Compare(this, s, false);
        }

        public int CompareTo(String strB)
        {
            if (strB == null) return 1;
            return Compare(this, strB, false);
        }

        public static int CompareOrdinal(String strA, String strB)
        {
            if (strA == null)
            {
                if (strB == null) return 0;
                return -1;
            }
            if (strB == null)
                return 1;

            //fixed (char* aptr = strA, bptr = strB)
            //{
            //    char* ap = aptr;
            //    char* end = ap + Math.Min(strA.Length, strB.Length);
            //    char* bp = bptr;
            //    while (ap < end)
            //    {
            //        if (*ap != *bp)
            //            return *ap - *bp;
            //        ap++;
            //        bp++;
            //    }
            //    return strA.Length - strB.Length;
            //}
            int n1 = strA.Length;
            int n2 = strB.Length;
            int n = n1 < n2 ? n1 : n2;
            for (int i = 0; i < n; ++i)
            {
                char c1 = strA[i];
                char c2 = strB[i];
                if (c1 != c2)
                    return c1 - c2;
            }
            return n1 - n2;
        }

        internal static int CompareOrdinal(String strA, String strB, CompareOptions options)
        {
            if (strA == null)
            {
                if (strB == null)
                    return 0;
                else
                    return -1;
            }
            else if (strB == null)
            {
                return 1;
            }

            /* Invariant, because that is cheaper to
             * instantiate (and chances are it already has
             * been.)
             */
            return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, strB, options);
        }

        public static int CompareOrdinal(String strA, int indexA, String strB, int indexB, int length)
        {
            return CompareOrdinal(strA, indexA, strB, indexB, length, CompareOptions.Ordinal);
        }

        internal static int CompareOrdinal(String strA, int indexA, String strB, int indexB, int length, CompareOptions options)
        {
            if (strA == null)
            {
                if (strB == null)
                    return 0;
                else
                    return -1;
            }
            else if (strB == null)
            {
                return 1;
            }

            if ((indexA > strA.Length) || (indexB > strB.Length) || (indexA < 0) || (indexB < 0) || (length < 0))
                throw new ArgumentOutOfRangeException();

            /* Need to cap the requested length to the
             * length of the string, because
             * CompareInfo.Compare will insist that length
             * <= (string.Length - offset)
             */
            int len1 = length;
            int len2 = length;

            if (length > (strA.Length - indexA))
            {
                len1 = strA.Length - indexA;
            }

            if (length > (strB.Length - indexB))
            {
                len2 = strB.Length - indexB;
            }

            return CultureInfo.InvariantCulture.CompareInfo.Compare(strA, indexA, len1, strB, indexB, len2, options);
        }
        #endregion

        #region StartsWith, EndsWith
        public bool EndsWith(String value)
        {
            return EndsWith(value, false, CultureInfo.CurrentCulture);
        }

#if NET_2_0
		public
#else
        internal
#endif
 bool EndsWith(String value, bool ignoreCase, CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            return (culture.CompareInfo.IsSuffix(this, value,
                ignoreCase ? CompareOptions.IgnoreCase :
                CompareOptions.None));
        }

        public bool StartsWith(String value)
        {
            return StartsWith(value, false, CultureInfo.CurrentCulture);
        }

#if NET_2_0
		public bool StartsWith (string value, StringComparison comparisonType)
		{
			switch (comparisonType) {
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix (this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix (this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.IsPrefix (this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.IsPrefix (this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix (this, value, CompareOptions.Ordinal);
			case StringComparison.OrdinalIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsPrefix (this, value, CompareOptions.OrdinalIgnoreCase);
			default:
				return false;
			}
		}

		public bool EndsWith (string value, StringComparison comparisonType)
		{
			switch (comparisonType) {
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix (this, value, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix (this, value, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.IsSuffix (this, value, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.IsSuffix (this, value, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix (this, value, CompareOptions.Ordinal);
			case StringComparison.OrdinalIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IsSuffix (this, value, CompareOptions.OrdinalIgnoreCase);
			default:
				return false;
			}
		}

#endif

#if NET_2_0
		public
#else
        internal
#endif
 bool StartsWith(String value, bool ignoreCase, CultureInfo culture)
        {
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            return (culture.CompareInfo.IsPrefix(this, value,
                ignoreCase ? CompareOptions.IgnoreCase :
                CompareOptions.None));
        }
        #endregion

        #region IndexOf
        public int IndexOfAny(char[] anyOf)
        {
            if (anyOf == null)
                throw new ArgumentNullException("anyOf");
            if (Length == 0)
                return -1;
            return InternalIndexOfAny(anyOf, 0, Length);
        }

        public int IndexOfAny(char[] anyOf, int startIndex)
        {
            if (anyOf == null)
                throw new ArgumentNullException("anyOf");
            if (startIndex < 0 || startIndex > Length)
                throw new ArgumentOutOfRangeException("startIndex");

            return InternalIndexOfAny(anyOf, startIndex, Length - startIndex);
        }

        public int IndexOfAny(char[] anyOf, int startIndex, int count)
        {
            if (anyOf == null)
                throw new ArgumentNullException("anyOf");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "< 0");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "< 0");
            // re-ordered to avoid possible integer overflow
            if (startIndex > Length - count)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex + count > Length");

            return InternalIndexOfAny(anyOf, startIndex, count);
        }

        int InternalIndexOfAny(char[] anyOf, int startIndex, int count)
        {
            int n = anyOf.Length;
            if (n == 0)
                return -1;

            if (n == 1)
                return IndexOfImpl(anyOf[0], startIndex, count);

            for (int k = 0; k < n; ++k)
            {
                char c = anyOf[k];
                int i = IndexOfImpl(c, startIndex, count);
                if (i >= 0) return i;
            }
            return -1;
        }


        #region IndexOf
#if NET_2_0
		public int IndexOf (string value, StringComparison comparison)
		{
			return IndexOf (value, 0, this.Length, comparison);
		}

		public int IndexOf (string value, int startIndex, StringComparison comparison)
		{
			return IndexOf (value, startIndex, this.Length - startIndex, comparison);
		}

		public int IndexOf (string value, int startIndex, int count, StringComparison comparison)
		{
			switch (comparison) {
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf (this, value, startIndex, count, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.IndexOf (this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.IndexOf (this, value, startIndex, count, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.IndexOf (this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return CultureInfo.InvariantCulture.CompareInfo.IndexOf (this, value, startIndex, count, CompareOptions.Ordinal);
			case StringComparison.OrdinalIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.IndexOf (this, value, startIndex, count, CompareOptions.OrdinalIgnoreCase);
			}
			throw new SystemException ("INTERNAL ERROR: should not reach here ...");
		}

		public int LastIndexOf (string value, StringComparison comparison)
		{
			return LastIndexOf (value, value.Length - 1, value.Length, comparison);
		}

		public int LastIndexOf (string value, int startIndex, StringComparison comparison)
		{
			return LastIndexOf (value, startIndex, startIndex + 1, comparison);
		}

		public int LastIndexOf (string value, int startIndex, int count, StringComparison comparison)
		{
			switch (comparison) {
			case StringComparison.CurrentCulture:
				return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf (this, value, startIndex, count, CompareOptions.None);
			case StringComparison.CurrentCultureIgnoreCase:
				return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf (this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.InvariantCulture:
				return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf (this, value, startIndex, count, CompareOptions.None);
			case StringComparison.InvariantCultureIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf (this, value, startIndex, count, CompareOptions.IgnoreCase);
			case StringComparison.Ordinal:
				return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf (this, value, startIndex, count, CompareOptions.Ordinal);
			case StringComparison.OrdinalIgnoreCase:
				return CultureInfo.InvariantCulture.CompareInfo.LastIndexOf (this, value, startIndex, count, CompareOptions.OrdinalIgnoreCase);
			}
			throw new SystemException ("INTERNAL ERROR: should not reach here ...");
		}
#endif

        public int IndexOf(char value)
        {
            int n = Length;
            if (n == 0) return -1;
            return IndexOf(value, 0, n);
        }

        public int IndexOf(String value)
        {
            return IndexOf(value, 0, Length);
        }

        public int IndexOf(char value, int startIndex)
        {
            return IndexOf(value, startIndex, Length - startIndex);
        }

        public int IndexOf(String value, int startIndex)
        {
            return IndexOf(value, startIndex, Length - startIndex);
        }

        /* This method is culture-insensitive */
        public int IndexOf(char value, int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "< 0");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "< 0");

            int n = Length;
            // re-ordered to avoid possible integer overflow
            if (startIndex > n - count)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex + count > Length");

            if ((startIndex == 0 && n == 0) || (startIndex == n) || (count == 0))
                return -1;

            return IndexOfImpl(value, startIndex, count);
        }

        int IndexOfImpl(char value, int startIndex, int count)
        {
            while (count-- > 0)
            {
                if (this[startIndex] == value)
                    return startIndex;
                ++startIndex;
            }
            return -1;
        }

        /* But this one is culture-sensitive */
        public int IndexOf(String value, int startIndex, int count)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "< 0");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "< 0");
            // re-ordered to avoid possible integer overflow
            if (startIndex > Length - count)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex + count > Length");

            if (value.Length == 0)
                return startIndex;

            if (startIndex == 0 && Length == 0)
                return -1;

            if (count == 0)
                return -1;

            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count);
        }
        #endregion

        #region LastIndexOfAny
        public int LastIndexOfAny(char[] anyOf)
        {
            if (anyOf == null)
                throw new ArgumentNullException("anyOf");

            int n = Length;
            if (n == 0) return -1;

            return InternalLastIndexOfAny(anyOf, n - 1, 0);
        }

        public int LastIndexOfAny(char[] anyOf, int startIndex)
        {
            if (anyOf == null)
                throw new ArgumentNullException("anyOf");

            if (startIndex < 0 || startIndex >= Length)
                throw new ArgumentOutOfRangeException();

            if (Length == 0)
                return -1;

            return InternalLastIndexOfAny(anyOf, startIndex, 0);
        }

        public int LastIndexOfAny(char[] anyOf, int startIndex, int count)
        {
            if (anyOf == null)
                throw new ArgumentNullException("anyOf");

            if ((startIndex < 0) || (startIndex >= Length))
                throw new ArgumentOutOfRangeException("startIndex", "< 0 || > this.Length");
            if ((count < 0) || (count > Length))
                throw new ArgumentOutOfRangeException("count", "< 0 || > this.Length");

            int last = startIndex - count + 1;
            if (last < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex - count + 1 < 0");

            return InternalLastIndexOfAny(anyOf, startIndex, last);
        }

        private int InternalLastIndexOfAny(char[] anyOf, int start, int last)
        {
            int m = anyOf.Length;
            for (int i = start; i >= last; --i)
            {
                char c = this[i];
                for (int k = 0; k < m; ++k)
                {
                    if (c == anyOf[k])
                        return i;
                }
            }
            return -1;
        }
        #endregion

        #region LastIndexOf
        public int LastIndexOf(char value)
        {
            if (Length == 0)
                return -1;

            return LastIndexOfImpl(value, Length - 1, Length);
        }

        public int LastIndexOf(String value)
        {
            if (Length == 0)
                /* This overload does additional checking */
                return LastIndexOf(value, 0, 0);
            else
                return LastIndexOf(value, Length - 1, Length);
        }

        public int LastIndexOf(char value, int startIndex)
        {
            return LastIndexOf(value, startIndex, startIndex + 1);
        }

        public int LastIndexOf(String value, int startIndex)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            int max = startIndex;
            if (max < Length)
                max++;
            return LastIndexOf(value, startIndex, max);
        }

        /* This method is culture-insensitive */
        public int LastIndexOf(char value, int startIndex, int count)
        {
            if (startIndex == 0 && Length == 0)
                return -1;

            // >= for char (> for string)
            if ((startIndex < 0) || (startIndex >= Length))
                throw new ArgumentOutOfRangeException("startIndex", "< 0 || >= this.Length");
            if ((count < 0) || (count > Length))
                throw new ArgumentOutOfRangeException("count", "< 0 || > this.Length");
            if (startIndex - count + 1 < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex - count + 1 < 0");

            return LastIndexOfImpl(value, startIndex, count);
        }

        /* This method is culture-insensitive */
        int LastIndexOfImpl(char value, int startIndex, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                if (this[startIndex - i] == value)
                    return startIndex - i;
            }
            return -1;
        }

        /* But this one is culture-sensitive */
        public int LastIndexOf(String value, int startIndex, int count)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            // -1 > startIndex > for string (0 > startIndex >= for char)
            if ((startIndex < -1) || (startIndex > Length))
                throw new ArgumentOutOfRangeException("startIndex", "< 0 || > this.Length");
            if ((count < 0) || (count > Length))
                throw new ArgumentOutOfRangeException("count", "< 0 || > this.Length");
            if (startIndex - count + 1 < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex - count + 1 < 0");

            if (value.Length == 0)
                return startIndex;

            if (startIndex == 0 && Length == 0)
                return -1;

            // This check is needed to match undocumented MS behaviour
            if (Length == 0 && value.Length > 0)
                return -1;

            if (count == 0)
                return -1;

            if (startIndex == Length)
                startIndex--;
            return CultureInfo.CurrentCulture.CompareInfo.LastIndexOf(this, value, startIndex, count);
        }
        #endregion
        #endregion

        public static bool IsNullOrEmpty(String value)
        {
            return (value == null) || (value.Length == 0);
        }

#if NET_2_0
		public bool Contains (String value)
		{
			return IndexOf (value) != -1;
		}

		public string Normalize ()
		{
			return Normalize (NormalizationForm.FormC);
		}

		public string Normalize (NormalizationForm form)
		{
			switch (form) {
			default:
				return Normalization.Normalize (this, 0);
			case NormalizationForm.FormD:
				return Normalization.Normalize (this, 1);
			case NormalizationForm.FormKC:
				return Normalization.Normalize (this, 2);
			case NormalizationForm.FormKD:
				return Normalization.Normalize (this, 3);
			}
		}

		public bool IsNormalized ()
		{
			return IsNormalized (NormalizationForm.FormC);
		}

		public bool IsNormalized (NormalizationForm form)
		{
			switch (form) {
			default:
				return Normalization.IsNormalized (this, 0);
			case NormalizationForm.FormD:
				return Normalization.IsNormalized (this, 1);
			case NormalizationForm.FormKC:
				return Normalization.IsNormalized (this, 2);
			case NormalizationForm.FormKD:
				return Normalization.IsNormalized (this, 3);
			}
		}

		public string Remove (int startIndex)
		{
			if (startIndex < 0)
				throw new ArgumentOutOfRangeException ("startIndex", "StartIndex can not be less than zero");
			if (startIndex >= Length)
				throw new ArgumentOutOfRangeException ("startIndex", "StartIndex must be less than the length of the string");

			return Remove (startIndex, Length - startIndex);
		}
#endif

        #region Pad
        public String PadLeft(int totalWidth)
        {
            return PadLeft(totalWidth, ' ');
        }

        public String PadLeft(int totalWidth, char paddingChar)
        {
            if (totalWidth < 0)
                throw new ArgumentOutOfRangeException("totalWidth", "< 0");

            int n = Length;
            if (totalWidth < n)
                return Copy(this);

            return new String(paddingChar, totalWidth - n) + this;
        }

        public String PadRight(int totalWidth)
        {
            return PadRight(totalWidth, ' ');
        }

        public String PadRight(int totalWidth, char paddingChar)
        {
            if (totalWidth < 0)
                throw new ArgumentOutOfRangeException("totalWidth", "< 0");

            int n = Length;
            if (totalWidth < n)
                return Copy(this);

            return this + new String(paddingChar, totalWidth - n);
        }
        #endregion

        #region Replace
        /* This method is culture insensitive */
        public String Replace(char oldChar, char newChar)
        {
            int len = Length;
            if (len == 0 || oldChar == newChar)
                return this;

            avmstr s = avm.EmptyString;
            for (int i = 0; i < len; ++i)
            {
                char c = m_value[i];
                if (c == oldChar)
                    s = avm.Concat(s, avmstr.fromCharCode(newChar));
                else
                    s = avm.Concat(s, avmstr.fromCharCode(c));
            }
            return new string(s);
        }

        public String Replace(String oldValue, String newValue)
        {
            return Replace(oldValue, newValue, 0, m_value.length);
        }

        /* This method is culture sensitive */
        public String Replace(String oldValue, String newValue, int startIndex, int count)
        {
            if (oldValue == null)
                throw new ArgumentNullException("oldValue");

            int oldLen = oldValue.Length;
            if (oldLen == 0)
                throw new ArgumentException("oldValue is the empty string.");

            int n = m_value.length;

            if (startIndex < 0 || count < 0 || startIndex > n - count)
                throw new ArgumentOutOfRangeException();

            if (n == 0)
                return new String(m_value);

            avmstr ns = avm.EmptyString;
            if (newValue != null)
                ns = newValue.m_value;

            avmstr old = oldValue.m_value;
            avmstr res = avm.EmptyString;

            int n2 = startIndex + count;
            int i;
            for (i = 0; i < startIndex; ++i)
            {
                res = avm.Concat(res, m_value.charAt(i));
            }

            for (i = startIndex; i < n2; )
            {
                bool ok = true;
                if (i + oldLen > n2)
                {
                    ok = false;
                }
                else
                {
                    for (int j = 0; j < oldLen; ++j)
                    {
                        if (m_value[i + j] != old[j])
                        {
                            ok = false;
                            break;
                        }
                    }
                }
                if (ok)
                {
                    if (ns != null)
                        res = avm.Concat(res, ns);
                    i += oldLen;
                }
                else
                {
                    res = avm.Concat(res, m_value.charAt(i));
                    ++i;
                }
            }

            for (; i < n; ++i)
                res = avm.Concat(res, m_value.charAt(i));

            return new String(res);
        }
        #endregion

        #region Remove
        public String Remove(int startIndex, int count)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "< 0");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "< 0");
            // re-ordered to avoid possible integer overflow
            if (startIndex > Length - count)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex + count > Length");

            if (startIndex == 0)
                return Substring(startIndex + count);
            return Substring(0, startIndex) + Substring(startIndex + count);
        }
        #endregion

        #region ToLower & ToUpper
        public String ToLower()
        {
            return ToLower(CultureInfo.CurrentCulture);
        }

        public String ToLower(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");

            if (culture.LCID == 0x007F)
            { // Invariant
                return ToLowerInvariant();
            }
            return culture.TextInfo.ToLower(this);
        }

#if NET_2_0
		public String ToLowerInvariant ()
#else
        internal String ToLowerInvariant()
#endif
        {
            int n = Length;
            if (n == 0) return this;
            string res = "";
            for (int i = 0; i < n; ++i)
            {
                char c = Char.ToLowerInvariant(this[i]);
                res += c;
            }
            return res;
        }

        public String ToUpper()
        {
            return ToUpper(CultureInfo.CurrentCulture);
        }

        public String ToUpper(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");

            if (culture.LCID == 0x007F)
            { // Invariant
                return ToUpperInvariant();
            }
            return culture.TextInfo.ToUpper(this);
        }

#if NET_2_0
		public String ToUpperInvariant ()
#else
        internal String ToUpperInvariant()
#endif
        {
            int n = Length;
            if (n == 0) return this;
            string res = "";
            for (int i = 0; i < n; ++i)
            {
                char c = Char.ToUpperInvariant(this[i]);
                res += c;
            }
            return res;
        }
        #endregion

        public override String ToString()
        {
            return new string(m_value);
        }

        public String ToString(IFormatProvider provider)
        {
            return this;
        }

        #region Format
        public static String Format(String format, object arg0)
        {
            return Format(null, format, new object[] { arg0 });
        }

        public static String Format(String format, object arg0, object arg1)
        {
            return Format(null, format, new object[] { arg0, arg1 });
        }

        public static String Format(String format, object arg0, object arg1, object arg2)
        {
            return Format(null, format, new object[] { arg0, arg1, arg2 });
        }

        public static string Format(string format, params object[] args)
        {
            return Format(null, format, args);
        }

        public static string Format(IFormatProvider provider, string format, params object[] args)
        {
            StringBuilder b = new StringBuilder();
            FormatHelper(b, provider, format, args);
            return b.ToString();
        }

        internal static void FormatHelper(StringBuilder result, IFormatProvider provider, string format, params object[] args)
        {
            if (format == null || args == null)
                throw new ArgumentNullException();

            int ptr = 0;
            int start = ptr;
            int formatLen = format.Length;
            while (ptr < formatLen)
            {
                char c = format[ptr++];

                if (c == '{')
                {
                    result.Append(format, start, ptr - start - 1);

                    // check for escaped open bracket

                    if (format[ptr] == '{')
                    {
                        start = ptr++;
                        continue;
                    }

                    // parse specifier

                    int n, width;
                    bool left_align;
                    string arg_format;

                    ParseFormatSpecifier(format, ref ptr, out n, out width, out left_align, out arg_format);
                    if (n >= args.Length)
                        throw new FormatException("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");

                    // format argument

                    object arg = args[n];

                    string str;
                    if (arg == null)
                    {
                        str = Empty;
                    }
                    else
                    {
                        ICustomFormatter formatter = null;
                        if (provider != null)
                            formatter = provider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
                        if (formatter != null)
                            str = formatter.Format(arg_format, arg, provider);
                        else if (arg is IFormattable)
                            str = ((IFormattable)arg).ToString(arg_format, provider);
                        else
                            str = arg.ToString();
                    }

                    // pad formatted string and append to result
                    if (width > str.Length)
                    {
                        const char padchar = ' ';
                        int padlen = width - str.Length;

                        if (left_align)
                        {
                            result.Append(str);
                            result.Append(padchar, padlen);
                        }
                        else
                        {
                            result.Append(padchar, padlen);
                            result.Append(str);
                        }
                    }
                    else
                    {
                        result.Append(str);
                    }

                    start = ptr;
                }
                else if (c == '}' && ptr < formatLen && format[ptr] == '}')
                {
                    result.Append(format, start, ptr - start - 1);
                    start = ptr++;
                }
                else if (c == '}')
                {
                    throw new FormatException("Input string was not in a correct format.");
                }
            }

            if (start < formatLen)
                result.Append(format, start, formatLen - start);
        }
        #endregion

        #region Concat
        internal void Append(avmstr s)
        {
            m_value = avm.Concat(m_value, s);
        }

        internal void Append(String s)
        {
            if (s == null || s.Length == 0) return;
            m_value = avm.Concat(m_value, s.m_value);
        }

        internal void Append(char c)
        {
            m_value = avm.Concat(m_value, avmstr.fromCharCode(c));
        }

        internal void Prepend(String s)
        {
            if (s == null || s.Length == 0) return;
            m_value = avm.Concat(s.m_value, m_value);
        }

        internal void Prepend(char c)
        {
            m_value = avm.Concat(avmstr.fromCharCode(c), m_value);
        }

        public static String Concat(String s1, String s2)
        {
            avmstr v1 = s1 != null ? s1.m_value : avm.EmptyString;
            avmstr v2 = s2 != null ? s2.m_value : avm.EmptyString;
            return new String(avm.Concat(v1, v2));
        }

        public static String Concat(String s1, String s2, String s3)
        {
            avmstr v1 = s1 != null ? s1.m_value : avm.EmptyString;
            avmstr v2 = s2 != null ? s2.m_value : avm.EmptyString;
            avmstr v3 = s3 != null ? s3.m_value : avm.EmptyString;
            return new String(avm.Concat(v1, v2, v3));
        }

        public static String Concat(String s1, String s2, String s3, String s4)
        {
            avmstr v1 = s1 != null ? s1.m_value : avm.EmptyString;
            avmstr v2 = s2 != null ? s2.m_value : avm.EmptyString;
            avmstr v3 = s3 != null ? s3.m_value : avm.EmptyString;
            avmstr v4 = s4 != null ? s4.m_value : avm.EmptyString;
            return new String(avm.Concat(v1, v2, v3, v4));
        }

        public static String Concat(object obj)
        {
            if (obj == null) return NewEmpty();
            return obj.ToString();
        }

        public static String Concat(object obj1, object obj2)
        {
            string s1 = (obj1 != null) ? obj1.ToString() : null;
            string s2 = (obj2 != null) ? obj2.ToString() : null;
            return Concat(s1, s2);
        }

        public static String Concat(object obj1, object obj2, object obj3)
        {
            string s1 = (obj1 != null) ? obj1.ToString() : null;
            string s2 = (obj2 != null) ? obj2.ToString() : null;
            string s3 = (obj3 != null) ? obj3.ToString() : null;
            return Concat(s1, s2, s3);
        }

        public static String Concat(params object[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            int n = args.Length;
            if (n == 0)
                return Empty;

            string[] strings = new string[n];
            int len = 0;
            for (int i = 0; i < n; ++i)
            {
                object arg = args[i];
                if (arg != null)
                {
                    string s = arg.ToString();
                    strings[i] = s;
                    len += s.Length;
                }
            }
            
            if (len == 0)
                return Empty;

            return Concat(strings);
        }

        public static String Concat(params String[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            return InternalJoin(Empty, values, 0, values.Length);
        }
        #endregion

        #region Insert
        public String Insert(int startIndex, String value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (startIndex < 0 || startIndex > Length)
                throw new ArgumentOutOfRangeException();

            if (value.Length == 0)
                return this;

            if (Length == 0)
                return value;

            if (startIndex == 0)
                return value + this;

            return Substring(0, startIndex) + value + Substring(startIndex);
        }
        #endregion

        public static string Intern(string str)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            return str;
        }

        public static string IsInterned(string str)
        {
            if (str == null)
                throw new ArgumentNullException("str");
            return str;
        }

        #region Join
        public static string Join(string separator, string[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return Join(separator, value, 0, value.Length);
        }

        public static string Join(string separator, string[] value, int startIndex, int count)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "< 0");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "< 0");
            // re-ordered to avoid possible integer overflow
            if (startIndex > value.Length - count)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex + count > value.length");

            if (startIndex == value.Length)
                return Empty;
            if (separator == null)
                separator = Empty;

            return InternalJoin(separator, value, startIndex, count);
        }

        private static string InternalJoin(string separator, string[] value, int startIndex, int count)
        {
            avmstr sep = null;
            if (separator != null && separator.Length != 0)
                sep = separator.m_value;
            bool dosep = false;
            avmstr str = avm.EmptyString;
            while (count-- > 0)
            {
                if (dosep && sep != null)
                {
                    str = avm.Concat(str, sep);
                }
                string s = value[startIndex];
                if (s != null)
                    str = avm.Concat(str, s.m_value);
                dosep = true;
                ++startIndex;
            }
            return new string(str);
        }
        #endregion

        #region IConvertible Members
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(this, provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this, provider);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(this, provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(this, provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(this, provider);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(this, provider);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this, provider);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this, provider);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this, provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this, provider);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this, provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ToType(this, conversionType, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this, provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this, provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this, provider);
        }
        #endregion

        #region GetEnumerator
        public CharEnumerator GetEnumerator()
        {
            return new CharEnumerator(this);
        }

#if NET_2_0
		IEnumerator<char> IEnumerable<char>.GetEnumerator ()
		{
			return GetEnumerator ();
		}
#endif

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CharEnumerator(this);
        }
        #endregion

        #region Parse Utils
        private static void ParseFormatSpecifier(string str, ref int ptr, out int n, out int width,
                                                 out bool left_align, out string format)
        {
            // parses format specifier of form:
            //   N,[\ +[-]M][:F]}
            //
            // where:

            try
            {
                // N = argument number (non-negative integer)

                n = ParseDecimal(str, ref ptr);
                if (n < 0)
                {
                    //Console.WriteLine("can not parse argument number");
                    throw new FormatException("Input string was not in a correct format.");
                }

                // M = width (non-negative integer)

                if (str[ptr] == ',')
                {
                    // White space between ',' and number or sign.
                    ++ptr;
                    while (Char.IsWhiteSpace(str[ptr]))
                        ++ptr;

                    if (str[ptr] == '+')
                    {
                        left_align = false;
                        ++ptr;
                    }
                    else
                    {
                        left_align = str[ptr] == '-';
                        if (left_align)
                            ++ptr;
                    }

                    width = ParseDecimal(str, ref ptr);
                    if (width < 0)
                    {
                        //Console.WriteLine("can not parse width");
                        throw new FormatException("Input string was not in a correct format.");
                    }
                }
                else
                {
                    width = 0;
                    left_align = false;
                }

                // F = argument format (string)

                if (str[ptr] == ':')
                {
                    int start = ++ptr;
                    while (str[ptr] != '}')
                        ++ptr;

                    format = str.Substring(start, ptr - start);
                }
                else
                    format = null;

                if (str[ptr++] != '}')
                {
                    //Console.WriteLine("no }");
                    throw new FormatException("Input string was not in a correct format.");
                }
            }
            catch (IndexOutOfRangeException)
            {
                //Console.WriteLine("IndexOutOfRangeException");
                throw new FormatException("Input string was not in a correct format.");
            }
        }

        private static int ParseDecimal(string str, ref int ptr)
        {
            int p = ptr;
            int n = 0;
            while (true)
            {
                char c = str[p];
                if (c < '0' || '9' < c)
                    break;

                n = n * 10 + c - '0';
                ++p;
            }

            if (p == ptr)
                return -1;

            ptr = p;
            return n;
        }
        #endregion

        internal void InternalSetChar(int idx, char val)
        {
            if ((uint)idx >= (uint)Length)
                throw new ArgumentOutOfRangeException("idx");
            this[idx] = val;
        }

        // When modifying it, GetCaseInsensitiveHashCode() should be modified as well.
        public override int GetHashCode()
        {
            int h = 0;
            for (int i = Length - 1; i >= 0; --i)
                h = (h << 5) - h + m_value[i];
            return h;
        }

        //internal int GetCaseInsensitiveHashCode()
        //{
        //    TextInfo ti = CultureInfo.InvariantCulture.TextInfo;
        //    fixed (char* c = this)
        //    {
        //        char* cc = c;
        //        char* end = cc + Length - 1;
        //        int h = 0;
        //        for (; cc < end; cc += 2)
        //        {
        //            h = (h << 5) - h + ti.ToUpper(*cc);
        //            h = (h << 5) - h + ti.ToUpper(cc[1]);
        //        }
        //        ++end;
        //        if (cc < end)
        //            h = (h << 5) - h + ti.ToUpper(*cc);
        //        return h;
        //    }
        //}
    }
}