//
// Comparer
//
// Authors:
//	Ben Maurer (bmaurer@ximian.com)
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

#if NET_2_0
using System;
using System.Runtime.InteropServices;

namespace System.Collections.Generic {
	[Serializable]
	public abstract class EqualityComparer <T> : IEqualityComparer, IEqualityComparer <T> {
		
		static EqualityComparer ()
		{
#if NOT_PFX
if (typeof (IEquatable <T>).IsAssignableFrom (typeof (T)))
				_default = (EqualityComparer <T>) Activator.CreateInstance (typeof (GenericEqualityComparer <>).MakeGenericType (typeof (T)));
			else
#endif
				_default = new DefaultComparer ();
		}
		
		
		public abstract int GetHashCode (T obj);
		public abstract bool Equals (T x, T y);
	
		static readonly EqualityComparer <T> _default;
		
		public static EqualityComparer <T> Default {
			get {
				return _default;
			}
		}

		int IEqualityComparer.GetHashCode (object obj)
		{
			return GetHashCode ((T)obj);
		}

		bool IEqualityComparer.Equals (object x, object y)
		{
			return Equals ((T)x, (T)y);
		}
		
		[Serializable]
		class DefaultComparer : EqualityComparer<T> {
	

			public override int GetHashCode (T obj)
			{
                if (obj == null)
                    return 0;
				return obj.GetHashCode ();
			}

            public override bool Equals(T x, T y)
            {
                if (x is IEquatable<T>)
                    return ((IEquatable<T>) x).Equals(y);

                return Object.Equals(x, y);

                //if (x != null)
                //{
                //    return ((y != null) && x.Equals(y));
                //}
                //if (y != null)
                //{
                //    return false;
                //}
                //return true;

                //if (x == null)
                //    return y == null;

                //return x.Equals (y);
            }
		}
	}
	
#if NOT_PFX
	[Serializable]
	class GenericEqualityComparer <T> : EqualityComparer <T> where T : IEquatable <T> {

		public override int GetHashCode (T obj)
		{
			return obj.GetHashCode ();
		}

		public override bool Equals (T x, T y)
		{
			if (x == null)
				return y == null;
			
			return x.Equals (y);
		}
	}
#endif
}
#endif
