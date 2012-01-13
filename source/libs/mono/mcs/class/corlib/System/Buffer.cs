//CHANGED
//
// System.Buffer.cs
//
// Authors:
//   Paolo Molaro (lupus@ximian.com)
//   Dan Lewis (dihlewis@yahoo.co.uk)
//
// (C) 2001 Ximian, Inc.  http://www.ximian.com
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
using System.Runtime.InteropServices;

namespace System
{
#if NET_2_0
	[ComVisible (true)]
#endif
    public
#if NET_2_0
		static
#else
 sealed
#endif
 class Buffer
    {
#if !NET_2_0
        private Buffer()
        {
        }
#endif

        public static int ByteLength(Array array)
        {
            // note: the other methods in this class also use ByteLength to test for
            // null and non-primitive arguments as a side-effect.

            if (array == null)
                throw new ArgumentNullException("array");

            int length = ByteLengthInternal(array);
            if (length < 0)
                throw new ArgumentException(Locale.GetText("Object must be an array of primitives."));

            return length;
        }

        public static byte GetByte(Array array, int index)
        {
            if (index < 0 || index >= ByteLength(array))
                throw new ArgumentOutOfRangeException("index", Locale.GetText(
                    "Value must be non-negative and less than the size of the collection."));

            return GetByteInternal(array, index);
        }

        public static void SetByte(Array array, int index, byte value)
        {
            if (index < 0 || index >= ByteLength(array))
                throw new ArgumentOutOfRangeException("index", Locale.GetText(
                    "Value must be non-negative and less than the size of the collection."));

            SetByteInternal(array, index, value);
        }

        public static void BlockCopy(Array src, int srcOffset, Array dest, int destOffset, int count)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            if (dest == null)
                throw new ArgumentNullException("dest");

            if (srcOffset < 0)
                throw new ArgumentOutOfRangeException("srcOffset", Locale.GetText(
                    "Non-negative number required."));

            if (destOffset < 0)
                throw new ArgumentOutOfRangeException("destOffset", Locale.GetText(
                    "Non-negative number required."));

            if (count < 0)
                throw new ArgumentOutOfRangeException("count", Locale.GetText(
                    "Non-negative number required."));

            // We do the checks in unmanaged code for performance reasons
            bool res = BlockCopyInternal(src, srcOffset, dest, destOffset, count);
            if (!res)
            {
                // watch for integer overflow
                if ((srcOffset > ByteLength(src) - count) || (destOffset > ByteLength(dest) - count))
                    throw new ArgumentException(Locale.GetText(
                        "Offset and length were out of bounds for the array or count is greater than" +
                        "the number of elements from index to the end of the source collection."));
            }
        }

        // private
        private static int ByteLengthInternal(Array array)
        {
            int n = array.Length;
            return n;
            //int len = 0;
            //for (int i = 0; i < n; ++i)
            //{
            //    //TODO: Need sizeof of object
            //    //object v = array.GetElem(i);
            //    len += 1;
            //}
            //return len;
        }

        private static byte GetByteInternal(Array array, int index)
        {
            return (byte)array.GetElem(index);
        }

        private static void SetByteInternal(Array array, int index, int value)
        {
            array.SetElem(index, (byte)value);
        }

        internal static bool BlockCopyInternal(Array src, int src_offset, Array dest, int dest_offset, int count)
        {
            Array.Copy(src, src_offset, dest, dest_offset, count);
            return true;
        }
    }
}
