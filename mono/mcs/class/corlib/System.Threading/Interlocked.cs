//CHANGED
//
// System.Threading.Interlocked.cs
//
// Author:
//	  Patrik Torstensson (patrik.torstensson@labs2.com)
//   Dick Porter (dick@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
//

//
// Copyright (C) 2004, 2005 Novell, Inc (http://www.novell.com)
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

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Threading
{
	public
#if NET_2_0
	static
#else
	sealed
#endif
	class Interlocked 
	{
#if !NET_2_0
		private Interlocked () {}
#endif

		public static int CompareExchange(ref int location, int value, int comparand)
		{
		    int old = location;
		    if (old == comparand)
		        location = value;
		    return old;
		}

		internal static object CompareExchange(ref object location, object value, object comparand)
		{
		    object old = location;
		    if (Equals(old, comparand))
		        location = value;
		    return old;
		}

        internal static float CompareExchange(ref float location, float value, float comparand)
        {
            float old = location;
            if (old == comparand)
                location = value;
            return old;
        }

	    public static int Decrement(ref int location)
		{
		    --location;
		    return location;
		}

		public static long Decrement(ref long location)
		{
		    --location;
		    return location;
		}

		public static int Increment(ref int location)
		{
		    --location;
		    return location;
		}

		public static long Increment(ref long location)
		{
		    --location;
		    return location;
		}

		public static int Exchange(ref int location, int value)
		{
		    int old = location;
		    location = value;
		    return old;
		}

		internal static object Exchange(ref object location, object value)
		{
            object old = location;
            location = value;
            return old;
		}

		internal static float Exchange(ref float location, float value)
		{
            float old = location;
            location = value;
            return old;
		}

//#if NET_2_0
		public static long CompareExchange(ref long location, long value, long comparand)
        {
		    long old = location;
		    if (old == comparand)
		        location = value;
		    return old;
        }

#if NOT_PFX
#if NET_2_0
		[ReliabilityContractAttribute (Consistency.WillNotCorruptState, Cer.Success)]
#endif
		public static IntPtr CompareExchange(ref IntPtr location, IntPtr value, IntPtr comparand)
		{
		    throw new NotSupportedException();
		}
#endif

		internal static double CompareExchange(ref double location, double value, double comparand)
		{
		    double old = location;
            if (old == comparand)
                location = value;
		    return old;
		}

		[ComVisible (false)]
		public static T CompareExchange<T> (ref T location, T value, T comparand) where T:class
		{
		    T old = location;
            if (Equals(old, comparand))
                location = value;
		    return old;
		}

		public static long Exchange(ref long location, long value)
		{
		    long old = location;
		    location = value;
		    return old;
		}

#if NOT_PFX
#if NET_2_0
		[ReliabilityContractAttribute (Consistency.WillNotCorruptState, Cer.Success)]
#endif
		public static IntPtr Exchange(ref IntPtr location, IntPtr value)
		{
		    throw new NotSupportedException();
		}
#endif

		internal static double Exchange(ref double location, double value)
		{
		    double old = location;
		    location = value;
		    return old;
		}

		[ComVisible (false)]
		public static T Exchange<T> (ref T location, T value) where T:class
		{
            T old = location;
		    location = value;
		    return old;
		}

		internal static long Read(ref long location)
		{
		    return location;
		}

		public static int Add(ref int location, int add)
		{
		    location += add;
		    return location;
		}

		public static long Add(ref long location, long add)
		{
		    location += add;
		    return location;
		}
//#endif
	}
}

