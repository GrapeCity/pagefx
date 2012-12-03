//
// System.StringComparer
//
// Authors:
//	Marek Safar (marek.safar@seznam.cz)
//
// (C) 2005 Novell, Inc (http://www.novell.com)
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

#if NET_2_0

using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace System
{
	[Serializable, ComVisible(true)]
	public abstract class StringComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
	{
		static StringComparer invariantCultureIgnoreCase = new CultureAwareComparer (CultureInfo.InvariantCulture, true);
		static StringComparer invariantCulture = new CultureAwareComparer (CultureInfo.InvariantCulture, false);
		static StringComparer ordinalIgnoreCase = new OrdinalComparer (true);
		static StringComparer ordinal = new OrdinalComparer (false);

		// Constructors
		protected StringComparer ()
		{
		}

		// Properties
		public static StringComparer CurrentCulture {
			get {
				return new CultureAwareComparer (CultureInfo.CurrentCulture, false);
			}
		}

		public static StringComparer CurrentCultureIgnoreCase {
			get {
				return new CultureAwareComparer (CultureInfo.CurrentCulture, true);
			}
		}

		public static StringComparer InvariantCulture {
			get {
				return invariantCulture;
			}
		}

		public static StringComparer InvariantCultureIgnoreCase {
			get {
				return invariantCultureIgnoreCase;
			}
		}

		public static StringComparer Ordinal {
			get { return ordinal; }
		}

		public static StringComparer OrdinalIgnoreCase {
			get { return ordinalIgnoreCase; }
		}

		// Methods
		public static StringComparer Create (CultureInfo culture, bool ignoreCase)
		{
			return new CultureAwareComparer (culture, ignoreCase);
		}

		public int Compare (object x, object y)
		{
			if (x == y)
				return 0;
			if (x == null)
				return -1;
			if (y == null)
				return 1;

			string s_x = x as string;
			if (s_x != null) {
				string s_y = y as string;
				if (s_y != null)
					return Compare (s_x, s_y);
			}

			IComparable ic = x as IComparable;
			if (ic == null)
				throw new ArgumentException ();

			return ic.CompareTo (y);
		}

		public new bool Equals (object x, object y)
		{
			if (x == y)
				return true;
			if (x == null || y == null)
				return false;

			string s_x = x as string;
			if (s_x != null) {
				string s_y = y as string;
				if (s_y != null)
					return Equals (s_x, s_y);
			}
			return x.Equals (y);
		}

		public int GetHashCode (object o)
		{
			if (o == null)
				throw new ArgumentNullException("o");

			string s = o as string;
			return s == null ? o.GetHashCode (): GetHashCode(s);
		}

		public abstract int Compare (string x, string y);
		public abstract bool Equals (string x, string y);
		public abstract int GetHashCode (string s);
	}

	[Serializable]
	class CultureAwareComparer : StringComparer
	{
		readonly bool _ignoreCase;
		readonly CompareInfo _compareInfo;

		public CultureAwareComparer (CultureInfo ci, bool ignore_case)
		{
			_compareInfo = ci.CompareInfo;
			_ignoreCase = ignore_case;
		}

		public override int Compare (string x, string y)
		{
			CompareOptions co = _ignoreCase ? CompareOptions.IgnoreCase : 
				CompareOptions.None;
			return _compareInfo.Compare (x, y, co);
		}

		public override bool Equals (string x, string y)
		{
			return Compare (x, y) == 0;
		}

	
        public override int GetHashCode (string s)
		{
            return base.GetHashCode((object) s);
#if NOT_PFX	
			if (s == null)
				throw new ArgumentNullException("s");

			CompareOptions co = _ignoreCase ? CompareOptions.IgnoreCase : 
				CompareOptions.None;
			return _compareInfo.GetSortKey (s, co).GetHashCode ();
#endif
        }
	}

	[Serializable]
	internal class OrdinalComparer : StringComparer
	{
		public OrdinalComparer (bool ignoreCase)
		{
			_ignoreCase = ignoreCase;
		}

		public override int Compare (string x, string y)
		{
			if (!_ignoreCase)
				return String.CompareOrdinal (x, y);

			// copied from String.CompareOrdinal()
			if (x == null) {
				if (y == null)
					return 0;
				else
					return -1;
			}
			else if (y == null) {
				return 1;
			}

			int max = x.Length > y.Length ? y.Length : x.Length;
			for (int i = 0; i < max; i++) {
				if (x [i] == y [i])
					continue;
				char xc = Char.ToUpperInvariant (x [i]);
				char yc = Char.ToUpperInvariant (y [i]);
				if (xc != yc)
					return xc - yc;
			}
			return max < x.Length ? -1 :
				max == y.Length ? 0 : 1;
		}

		public override bool Equals (string x, string y)
		{
			if (!_ignoreCase)
				return x == y;
			return Compare (x, y) == 0;
		}

		public override int GetHashCode (string s)
		{
		    return s.GetHashCode();
#if NOT_PFX
			if (!_ignoreCase)
				return s.GetHashCode ();

			return s.GetCaseInsensitiveHashCode ();
           
#endif
		}


		readonly bool _ignoreCase;
	}
}

#endif
