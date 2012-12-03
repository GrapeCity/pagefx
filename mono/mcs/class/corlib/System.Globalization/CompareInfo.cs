//CHANGED

//
// System.Globalization.CompareInfo
//
// Authors:
//   Rodrigo Moya (rodrigo@ximian.com)
//   Dick Porter (dick@ximian.com)
//
// (C) Ximian, Inc. 2002
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
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

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Globalization
{
    public class CompareInfo
    {
        // Keep in synch with MonoCompareInfo in the runtime. 
        private int culture;

        /* Hide the .ctor() */
        private CompareInfo() { }

        internal CompareInfo(CultureInfo ci)
        {
        }

        #region Compare
        private static int CharCompare(char c1, char c2, CompareOptions options)
        {
            bool ignoreCase = (options & CompareOptions.IgnoreCase) != 0;
            bool ordinal = (options & CompareOptions.Ordinal) != 0;
            if (ignoreCase)
            {
                c1 = char.ToLower(c1);
                c2 = char.ToLower(c2);
            }
            else
            {
                if (!ordinal)
                {
                    if (c1 >= 'A' && c1 <= 'Z')
                    {
                        if (c2 >= 'a' && c2 <= 'z')
                            return 1;
                    }
                    if (c2 >= 'A' && c2 <= 'Z')
                    {
                        if (c1 >= 'a' && c1 <= 'z')
                            return -1;
                    }
                }
            }
            if (c1 == c2) return 0;
            if (ordinal) return c1 - c2;
            return c1 < c2 ? -1 : 1;
        }

        private static bool CharEquals(char c1, char c2, bool ignoreCase)
        {
            if (ignoreCase)
            {
                c1 = char.ToLower(c1);
                c2 = char.ToLower(c2);
            }
            return c1 == c2;
        }

        private static int InternalCompare(string str1, int offset1, int length1, string str2, int offset2, int length2, CompareOptions options)
        {
            int n = length1;
            if (length2 < n)
                n = length2;

            while (n-- > 0)
            {
                char c1 = str1[offset1];
                char c2 = str2[offset2];
                int c = CharCompare(c1, c2, options);
                if (c != 0) return c;
                ++offset1;
                ++offset2;
                --length1;
                --length2;
            }
            n = length1 - length2;
            if (n == 0) return 0;
            return n < 0 ? -1 : 1;
        }

        public virtual int Compare(string string1, string string2)
        {
            if (string1 == null)
            {
                if (string2 == null)
                    return 0;
                return -1;
            }
            if (string2 == null)
                return 1;

            /* Short cut... */
            if (string1.Length == 0 && string2.Length == 0)
                return (0);

            return InternalCompare(string1, 0, string1.Length, string2, 0, string2.Length, CompareOptions.None);
        }

        public virtual int Compare(string string1, string string2,
                        CompareOptions options)
        {
            if (string1 == null)
            {
                if (string2 == null) return 0;
                return -1;
            }
            if (string2 == null)
                return 1;

            /* Short cut... */
            if (string1.Length == 0 && string2.Length == 0)
                return 0;

            return InternalCompare(string1, 0, string1.Length, string2, 0, string2.Length, options);
        }

        public virtual int Compare(string string1, int offset1,
                        string string2, int offset2)
        {
            if (string1 == null)
            {
                if (string2 == null)
                    return 0;
                return -1;
            }
            if (string2 == null)
                return 1;

            /* Not in the spec, but ms does these short
             * cuts before checking the offsets (breaking
             * the offset >= string length specified check
             * in the process...)
             */
            if ((string1.Length == 0 || offset1 == string1.Length) &&
                (string2.Length == 0 || offset2 == string2.Length))
                return (0);

            if (offset1 < 0 || offset2 < 0)
            {
                throw new ArgumentOutOfRangeException("Offsets must not be less than zero");
            }

            if (offset1 > string1.Length)
            {
                throw new ArgumentOutOfRangeException("offset1", "Offset1 is greater than or equal to the length of string1");
            }

            if (offset2 > string2.Length)
            {
                throw new ArgumentOutOfRangeException("offset2", "Offset2 is greater than or equal to the length of string2");
            }

            return (InternalCompare(string1, offset1,
                         string1.Length - offset1,
                         string2, offset2,
                         string2.Length - offset2,
                         CompareOptions.None));
        }

        public virtual int Compare(string string1, int offset1,
                        string string2, int offset2,
                        CompareOptions options)
        {
            if (string1 == null)
            {
                if (string2 == null)
                    return 0;
                return -1;
            }
            if (string2 == null)
                return 1;

            /* Not in the spec, but ms does these short
             * cuts before checking the offsets (breaking
             * the offset >= string length specified check
             * in the process...)
             */
            if ((string1.Length == 0 || offset1 == string1.Length) &&
                (string2.Length == 0 || offset2 == string2.Length))
                return (0);

            if (offset1 < 0 || offset2 < 0)
            {
                throw new ArgumentOutOfRangeException("Offsets must not be less than zero");
            }

            if (offset1 > string1.Length)
            {
                throw new ArgumentOutOfRangeException("Offset1 is greater than or equal to the length of string1");
            }

            if (offset2 > string2.Length)
            {
                throw new ArgumentOutOfRangeException("Offset2 is greater than or equal to the length of string2");
            }

            return (InternalCompare(string1, offset1,
                         string1.Length - offset1,
                         string2, offset2,
                         string2.Length - offset1,
                         options));
        }

        public virtual int Compare(string string1, int offset1,
                        int length1, string string2,
                        int offset2, int length2)
        {
            if (string1 == null)
            {
                if (string2 == null)
                    return 0;
                return -1;
            }
            if (string2 == null)
                return 1;

            /* Not in the spec, but ms does these short
             * cuts before checking the offsets (breaking
             * the offset >= string length specified check
             * in the process...)
             */
            if ((string1.Length == 0 ||
                offset1 == string1.Length ||
                length1 == 0) &&
                (string2.Length == 0 ||
                offset2 == string2.Length ||
                length2 == 0))
                return (0);

            if (offset1 < 0 || length1 < 0 ||
               offset2 < 0 || length2 < 0)
            {
                throw new ArgumentOutOfRangeException("Offsets and lengths must not be less than zero");
            }

            if (offset1 > string1.Length)
            {
                throw new ArgumentOutOfRangeException("Offset1 is greater than or equal to the length of string1");
            }

            if (offset2 > string2.Length)
            {
                throw new ArgumentOutOfRangeException("Offset2 is greater than or equal to the length of string2");
            }

            if (length1 > string1.Length - offset1)
            {
                throw new ArgumentOutOfRangeException("Length1 is greater than the number of characters from offset1 to the end of string1");
            }

            if (length2 > string2.Length - offset2)
            {
                throw new ArgumentOutOfRangeException("Length2 is greater than the number of characters from offset2 to the end of string2");
            }

            return (InternalCompare(string1, offset1, length1,
                         string2, offset2, length2,
                         CompareOptions.None));
        }

        public virtual int Compare(string string1, int offset1,
                        int length1, string string2,
                        int offset2, int length2,
                        CompareOptions options)
        {
            if (string1 == null)
            {
                if (string2 == null)
                    return 0;
                return -1;
            }
            if (string2 == null)
                return 1;

            /* Not in the spec, but ms does these short
             * cuts before checking the offsets (breaking
             * the offset >= string length specified check
             * in the process...)
             */
            if ((string1.Length == 0 ||
                offset1 == string1.Length ||
                length1 == 0) &&
                (string2.Length == 0 ||
                offset2 == string2.Length ||
                length2 == 0))
                return (0);

            if (offset1 < 0 || length1 < 0 ||
               offset2 < 0 || length2 < 0)
            {
                throw new ArgumentOutOfRangeException("Offsets and lengths must not be less than zero");
            }

            if (offset1 > string1.Length)
            {
                throw new ArgumentOutOfRangeException("Offset1 is greater than or equal to the length of string1");
            }

            if (offset2 > string2.Length)
            {
                throw new ArgumentOutOfRangeException("Offset2 is greater than or equal to the length of string2");
            }

            if (length1 > string1.Length - offset1)
            {
                throw new ArgumentOutOfRangeException("Length1 is greater than the number of characters from offset1 to the end of string1");
            }

            if (length2 > string2.Length - offset2)
            {
                throw new ArgumentOutOfRangeException("Length2 is greater than the number of characters from offset2 to the end of string2");
            }

            return (InternalCompare(string1, offset1, length1,
                         string2, offset2, length2,
                         options));
        }
        #endregion

        public override bool Equals(object value)
        {
            CompareInfo other = value as CompareInfo;
            if (other == null)
            {
                return (false);
            }

            return (other.culture == culture);
        }

        internal static CompareInfo GetCompareInfo(int culture)
        {
            return (new CultureInfo(culture).CompareInfo);
        }

        internal static CompareInfo GetCompareInfo(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            return (new CultureInfo(name).CompareInfo);
        }

        internal static CompareInfo GetCompareInfo(int culture, Assembly assembly)
        {
            /* The assembly parameter is supposedly there
             * to allow some sort of compare algorithm
             * versioning.
             */
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (assembly != typeof(Object).Assembly)
            {
                throw new ArgumentException("Assembly is an invalid type");
            }
            return (GetCompareInfo(culture));
        }

        internal static CompareInfo GetCompareInfo(string name, Assembly assembly)
        {
            /* The assembly parameter is supposedly there
             * to allow some sort of compare algorithm
             * versioning.
             */
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (assembly != typeof(Object).Assembly)
            {
                throw new ArgumentException("Assembly is an invalid type");
            }
            return (GetCompareInfo(name));
        }

        public override int GetHashCode()
        {
            return (LCID);
        }

        #region IndexOf
        public virtual int IndexOf(string source, char value)
        {
            return (IndexOf(source, value, 0, source.Length,
                    CompareOptions.None));
        }

        public virtual int IndexOf(string source, string value)
        {
            return (IndexOf(source, value, 0, source.Length,
                    CompareOptions.None));
        }

        public virtual int IndexOf(string source, char value, CompareOptions options)
        {
            return (IndexOf(source, value, 0, source.Length, options));
        }

        internal virtual int IndexOf(string source, char value, int startIndex)
        {
            return (IndexOf(source, value, startIndex, source.Length - startIndex, CompareOptions.None));
        }

        public virtual int IndexOf(string source, string value, CompareOptions options)
        {
            return (IndexOf(source, value, 0, source.Length, options));
        }

        internal virtual int IndexOf(string source, string value, int startIndex)
        {
            return (IndexOf(source, value, startIndex, source.Length - startIndex, CompareOptions.None));
        }

        public virtual int IndexOf(string source, char value, int startIndex, CompareOptions options)
        {
            return (IndexOf(source, value, startIndex, source.Length - startIndex, options));
        }

        public virtual int IndexOf(string source, char value, int startIndex, int count) 
        {
            return IndexOf(source, value, startIndex, count, CompareOptions.None);
        }

        public virtual int IndexOf(string source, string value, int startIndex, CompareOptions options)
        {
            return (IndexOf(source, value, startIndex, source.Length - startIndex, options));
        }

        public virtual int IndexOf(string source, string value, int startIndex, int count)
        {
            return (IndexOf(source, value, startIndex, count, CompareOptions.None));
        }

        private static int InternalIndexOf(string s, int startIndex, int count, char c, CompareOptions opt, bool first)
        {
            // - forward IndexOf() icall is much faster than
            //   manged version, so always use icall. However,
            //   it does not work for OrdinalIgnoreCase, so
            //   do not avoid managed collator for that option.
            bool ignoreCase = (opt & CompareOptions.IgnoreCase) != 0;
            int inc = first ? 1 : -1;
            while (count-- > 0)
            {
                char c1 = s[startIndex];
                if (CharEquals(c1, c, ignoreCase))
                    return startIndex;
                startIndex += inc;
            }
            return -1;
        }

        public virtual int IndexOf(string source, char value, int startIndex, int count, CompareOptions options)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            if (count < 0 || (source.Length - startIndex) < count)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if ((options & CompareOptions.StringSort) != 0)
            {
                throw new ArgumentException("StringSort is not a valid CompareOption for this method");
            }

            if (count == 0)
                return -1;

            if ((options & CompareOptions.Ordinal) != 0)
            {
                while (count-- > 0)
                {
                    if (source[startIndex] == value)
                        return startIndex;
                    ++startIndex;
                }
                return -1;
            }
            else
            {
                return (InternalIndexOf(source, startIndex,
                            count, value, options,
                            true));
            }
        }

        private static int InternalIndexOf(string s1, int startIndex, int count, string s2, CompareOptions opt, bool first)
        {
            // - forward IndexOf() icall is much faster than
            //   manged version, so always use icall. However,
            //   it does not work for OrdinalIgnoreCase, so
            //   do not avoid managed collator for that option.
            int n2 = s2.Length;
            if (n2 == 0) return -1;
            if (count < n2) return -1;
            bool ignoreCase = (opt & CompareOptions.IgnoreCase) != 0;
            count -= n2 - 1;
            int inc = first ? 1 : -1;
            int i0 = first ? 0 : n2 - 1;
            while (count-- > 0)
            {
                bool ok = true;
                for (int i = i0, j = startIndex; 
                    first ? i < n2 : i >= 0;
                    i += inc, j += inc)
                {
                    if (!CharEquals(s1[j], s2[i], ignoreCase))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok)
                {
                    return first ? startIndex : startIndex - n2 + 1;
                }
                startIndex += inc;
            }
            return -1;
        }

        public virtual int IndexOf(string source, string value, int startIndex, int count, CompareOptions options)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            if (count < 0 || (source.Length - startIndex) < count)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (count == 0)
            {
                return (-1);
            }

            return (InternalIndexOf(source, startIndex, count,
                        value, options, true));
        }
        #endregion

        #region IsPrefix, IsSuffix
        public virtual bool IsPrefix(string source, string prefix)
        {
            return (IsPrefix(source, prefix, CompareOptions.None));
        }

        public virtual bool IsPrefix(string source, string prefix, CompareOptions options)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            //if (UseManagedCollation)
            //    return collator.IsPrefix(source, prefix, options);

            if (source.Length < prefix.Length)
            {
                return (false);
            }
            else
            {
                return (Compare(source, 0, prefix.Length, prefix, 0, prefix.Length, options) == 0);
            }
        }
        
        public virtual bool IsSuffix(string source, string suffix)
        {
            return (IsSuffix(source, suffix, CompareOptions.None));
        }

        public virtual bool IsSuffix(string source, string suffix, CompareOptions options)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (suffix == null)
            {
                throw new ArgumentNullException("suffix");
            }

            //if (UseManagedCollation)
            //    return collator.IsSuffix(source, suffix, options);

            if (source.Length < suffix.Length)
            {
                return (false);
            }
            else
            {
                return (Compare(source,
                        source.Length - suffix.Length,
                        suffix.Length, suffix, 0,
                        suffix.Length, options) == 0);
            }
        }
        #endregion

        #region LastIndexOf
        public virtual int LastIndexOf(string source, char value)
        {
            return (LastIndexOf(source, value, source.Length - 1, source.Length, CompareOptions.None));
        }

        public virtual int LastIndexOf(string source, string value)
        {
            return (LastIndexOf(source, value, source.Length - 1, source.Length, CompareOptions.None));
        }

        public virtual int LastIndexOf(string source, char value, CompareOptions options)
        {
            return (LastIndexOf(source, value, source.Length - 1, source.Length, options));
        }

        internal virtual int LastIndexOf(string source, char value, int startIndex)
        {
            return (LastIndexOf(source, value, startIndex, startIndex + 1, CompareOptions.None));
        }

        public virtual int LastIndexOf(string source, string value, CompareOptions options)
        {
            return (LastIndexOf(source, value, source.Length - 1, source.Length, options));
        }

        internal virtual int LastIndexOf(string source, string value, int startIndex)
        {
            return (LastIndexOf(source, value, startIndex, startIndex + 1, CompareOptions.None));
        }

        public virtual int LastIndexOf(string source, char value, int startIndex, CompareOptions options)
        {
            return (LastIndexOf(source, value, startIndex, startIndex + 1, options));
        }

        public virtual int LastIndexOf(string source, char value, int startIndex, int count)
        {
            return (LastIndexOf(source, value, startIndex, count, CompareOptions.None));
        }

        public virtual int LastIndexOf(string source, string value, int startIndex, CompareOptions options)
        {
            return (LastIndexOf(source, value, startIndex, startIndex + 1, options));
        }

        public virtual int LastIndexOf(string source, string value, int startIndex, int count)
        {
            return (LastIndexOf(source, value, startIndex, count, CompareOptions.None));
        }

        public virtual int LastIndexOf(string source, char value, int startIndex, int count, CompareOptions options)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            if (count < 0 || (startIndex - count) < -1)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if ((options & CompareOptions.StringSort) != 0)
            {
                throw new ArgumentException("StringSort is not a valid CompareOption for this method");
            }

            if (count == 0)
            {
                return (-1);
            }

            if ((options & CompareOptions.Ordinal) != 0)
            {
                for (int pos = startIndex;
                    pos > startIndex - count;
                    pos--)
                {
                    if (source[pos] == value)
                    {
                        return (pos);
                    }
                }
                return (-1);
            }
            else
            {
                return (InternalIndexOf(source, startIndex,
                            count, value, options,
                            false));
            }
        }

        public virtual int LastIndexOf(string source, string value, int startIndex, int count, CompareOptions options)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            if (count < 0 || (startIndex - count) < -1)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (count == 0)
            {
                return (-1);
            }

            if (value.Length == 0)
            {
                return (0);
            }

            return (InternalIndexOf(source, startIndex, count,
                           value, options, false));
        }
        #endregion

#if NOT_PFX
#if NET_2_0
		[ComVisible (false)]
		public static bool IsSortable (char c)
		{
			return MSCompatUnicodeTable.IsSortable (c);
		}

		[ComVisible (false)]
		public static bool IsSortable (string s)
		{
			return MSCompatUnicodeTable.IsSortable (s);
		}
#endif
#endif


        public override string ToString()
        {
            return ("CompareInfo - " + culture);
        }

        /* LAMESPEC: not mentioned in the spec, but corcompare
         * shows it.  Some documentation about what it does
         * would be nice.
         */
        internal int LCID
        {
            get
            {
                return (culture);
            }
        }

#if NOT_PFX
#if NET_2_0
		[ComVisible (false)]
		public virtual string Name 
        {
			get { return icu_name; }
		}
#endif
#endif
    }
}
