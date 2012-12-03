//CHANGED
//
// System.Object.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
//
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

using System.Runtime.CompilerServices;

namespace System
{
    [Serializable]
    public class Object
    {
        // <summary>
        //   Compares this object to the specified object.
        //   Returns true if they are equal, false otherwise.
        // </summary>
        public virtual bool Equals(object o)
        {
            return this == o;
        }

        // <summary>
        //   Compares two objects for equality
        // </summary>
        public static bool Equals(object a, object b)
        {
            if (a == b)
                return true;

            if (a == null || b == null)
                return false;

            return a.Equals(b);
        }

        // <summary>
        //   Returns a hashcode for this object.  Each derived
        //   class should return a hash code that makes sense
        //   for that particular implementation of the object.
        // </summary>
        public virtual int GetHashCode()
        {
            //TODO:
            if (_myHashCode == 0)
                _myHashCode = NewHashCode();
            return _myHashCode;
        }
        int _myHashCode;
        static int _hashCode;
        

        internal static int NewHashCode()
        {
            return ++_hashCode ^ 45653674;
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public virtual extern int GetTypeId();

        // <summary>
        //   Returns the Type associated with the object.
        // </summary>
        public virtual Type GetType()
        {
            return System.Reflection.Assembly.GetType(GetTypeId());
        }

        // <summary>
        //   Shallow copy of the object.
        // </summary>
        protected object MemberwiseClone()
        {
            Type type = GetType();
            object obj = type.CreateInstanceDefault();
            while (type != null)
            {
                type.CopyFields(this, obj);
                if (type.IsValueType) break;
                type = type.BaseType;
            }
            return obj;
        }
        
        // <summary>
        //   Returns a stringified representation of the object.
        //   This is not supposed to be used for user presentation,
        //   use Format() for that and IFormattable.
        //
        //   ToString is mostly used for debugging purposes. 
        // </summary>
        public virtual string ToString()
        {
            return GetType().FullName;
        }

        // <summary>
        //   Tests whether a is equal to b.
        //   Can not figure out why this even exists
        // </summary>
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern public static bool ReferenceEquals(object a, object b);
        
        //public static bool ReferenceEquals(object a, object b)
        //{
        //    return (a == b);
        //}

        //[MethodImplAttribute(MethodImplOptions.InternalCall)]
        //internal static extern int InternalGetHashCode(object o);
    }
}
