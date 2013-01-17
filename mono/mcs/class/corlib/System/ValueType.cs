//CHANGED
    
//
// System.ValueType.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//   Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
// (C) 2003 Novell, Inc.  http://www.novell.com
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

namespace System
{
    [Serializable]
#if NET_2_0
	[ComVisible (true)]
#endif
    public abstract class ValueType
    {
        protected ValueType()
        {
        }

        internal static bool DefaultEquals(object o1, object o2)
        {
            if (o1 == null) return o2 == null;
            if (o2 == null) return false;

            ValueType v1 = o1 as ValueType;
            if (v1 == null) return false;

            ValueType v2 = o2 as ValueType;
            if (v2 == null) return false;

            Type type1 = o1.GetType();
            Type type2 = o2.GetType();
            if (type1 != type2) return false;

            object[] vals1 = type1.GetFieldValues(o1);
			object[] vals2 = type2.GetFieldValues(o2);
            if (vals1 == null) return vals2 == null;
            if (vals2 == null) return false;

            int n = vals1.Length;
            if (n != vals2.Length) return false;
            for (int i = 0; i < n; ++i)
            {
                if (!Equals(vals1[i], vals2[i]))
                    return false;
            }
            return true;
        }

		internal object[] GetFieldValues()
        {
            return GetType().GetFieldValues(this);
        }

        /// <summary>
        ///   True if this instance and o represent the same type
        ///   and have the same value.
        /// </summary>
        public override bool Equals(object o)
        {
            return DefaultEquals(this, o);
        }

        /// <summary>
        ///   Gets a hashcode for this value type using the
        ///   bits in the structure
        /// </summary>
        public override int GetHashCode()
        {
            int h = 0;
			object[] vals = GetFieldValues();
            if (vals != null)
            {
                int n = vals.Length;
                for (int i = 0; i < n; ++i)
                {
                    object v = vals[i];
                    if (v != null)
                        h ^= v.GetHashCode();
                }
            }
            return h;
        }
    }
}
