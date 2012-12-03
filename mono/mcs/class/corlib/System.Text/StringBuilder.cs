//CHANGED
//TODO: OPTIMIZE!

// -*- Mode: C; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*-
//
// System.Text.StringBuilder
//
// Authors: 
//   Marcin Szczepanski (marcins@zipworld.com.au)
//   Paolo Molaro (lupus@ximian.com)
//   Patrik Torstensson
//
// NOTE: In the case the buffer is only filled by 50% a new string
//       will be returned by ToString() is cached in the '_cached_str'
//		 cache_string will also control if a string has been handed out
//		 to via ToString(). If you are chaning the code make sure that
//		 if you modify the string data set the cache_string to null.
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

using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
    [Serializable]
    public sealed class StringBuilder
#if NOT_PFX
#if NET_2_0
		: ISerializable
#endif
#endif
    {
        private string _str;
        private readonly int _maxCapacity = Int32.MaxValue;
        private const int constDefaultCapacity = 16;

        #region Constructors
        public StringBuilder(string value, int startIndex, int length, int capacity)
        {
            // first, check the parameters and throw appropriate exceptions if needed
            if (null == value)
                value = "";

            // make sure startIndex is zero or positive
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", startIndex, "StartIndex cannot be less than zero.");

            // make sure length is zero or positive
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "Length cannot be less than zero.");

            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", capacity, "capacity must be greater than zero.");

            // make sure startIndex and length give a valid substring of value
            // re-ordered to avoid possible integer overflow
            if (startIndex > value.Length - length)
                throw new ArgumentOutOfRangeException("startIndex", startIndex, "StartIndex and length must refer to a location within the string.");

            if (capacity == 0)
                capacity = constDefaultCapacity;

            _str = value.Substring(startIndex, length);
        }

        public StringBuilder() : this("")
        {
        }

        public StringBuilder(int capacity) 
            : this("", 0, 0, capacity)
        {
        }

        public StringBuilder(int capacity, int maxCapacity)
            : this("", 0, 0, capacity)
        {
            if (maxCapacity < 1)
                throw new ArgumentOutOfRangeException("maxCapacity", "maxCapacity is less than one.");
            if (capacity > maxCapacity)
                throw new ArgumentOutOfRangeException("capacity", "Capacity exceeds maximum capacity.");

            _maxCapacity = maxCapacity;
        }

        public StringBuilder(string value)
        {
            /*
             * This is an optimization to avoid allocating the internal string
             * until the first Append () call.
             * The runtime pinvoke marshalling code needs to be aware of this.
             */
            if (value == null)
                _str = "";
            else
                _str = string.Copy(value);
        }

        public StringBuilder(string value, int capacity)
            : this(value, 0, value.Length, capacity)
        {
        }
        #endregion

        #region Properties
        internal int MaxCapacity
        {
            get
            {
                // MS runtime always returns Int32.MaxValue.
                return _maxCapacity;
            }
        }

        public int Capacity
        {
            get
            {
                if (_str.Length == 0)
                    return constDefaultCapacity;

                return _str.Length;
            }

            set
            {
                if (value < Length)
                    throw new ArgumentException("Capacity must be larger than length");
            }
        }

        public int Length
        {
            get
            {
                return _str.Length;
            }

            set
            {
                if (value < 0 || value > _maxCapacity)
                    throw new ArgumentOutOfRangeException();

                int len = _str.Length;
                if (value == len)
                    return;

                if (value == 0)
                {
                    _str = "";
                    return;
                }

                if (value < len)
                {
                    // LAMESPEC:  The spec is unclear as to what to do
                    // with the capacity when truncating the string.

                    // Do as MS, keep the capacity

                    // Make sure that we invalidate any cached string.
                    int d = len - value;
                    _str = _str.Remove(len - 1 - d, d);
                }
                else
                {
                    // Expand the capacity to the new length and
                    // pad the string with NULL characters.
                    Append('\0', value - len);
                }
            }
        }

        [IndexerName("Chars")]
        public char this[int index]
        {
            get
            {
                if (index >= Length || index < 0)
                    throw new IndexOutOfRangeException();
                return _str[index];
            }

            set
            {
                if (index >= Length || index < 0)
                    throw new IndexOutOfRangeException();

                _str = String.InternalSetChar(_str, index, value);
            }
        }
        #endregion

        #region Capacity
        public int EnsureCapacity(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", "Capacity must be greater than 0.");

            if (capacity <= _str.Length)
                return _str.Length;

            InternalEnsureCapacity(capacity);

            return _str.Length;
        }

        private void InternalEnsureCapacity(int size)
        {
            if (size > _str.Length)
            {
                int capacity = _str.Length;

                // Try double buffer, if that doesn't work, set the length as capacity
                if (size > capacity)
                {

                    // The first time a string is appended, we just set _cached_str
                    // and _str to it. This allows us to do some optimizations.
                    // Below, we take this into account.
                    if (capacity < constDefaultCapacity)
                        capacity = constDefaultCapacity;

                    capacity = capacity << 1;
                    if (size > capacity)
                        capacity = size;

                    if (capacity >= Int32.MaxValue || capacity < 0)
                        capacity = Int32.MaxValue;

                    if (capacity > _maxCapacity && size <= _maxCapacity)
                        capacity = _maxCapacity;

                    if (capacity > _maxCapacity)
                        throw new ArgumentOutOfRangeException("size", "capacity was less than the current size.");
                }
            }
        }

        private void IncCapacity(int size)
        {
            InternalEnsureCapacity(Capacity + size);
        }
        #endregion

        #region Object Overrides
        public override string ToString()
        {
            return _str;
        }

        public string ToString(int startIndex, int length)
        {
            // re-ordered to avoid possible integer overflow
            if (startIndex < 0 || length < 0 || startIndex > _str.Length - length)
                throw new ArgumentOutOfRangeException();

            return _str.Substring(startIndex, length);
        }

        public override bool Equals(object o)
        {
            if (o == this) return true;
            StringBuilder sb = o as StringBuilder;
            if (sb == null) return false;
            return Equals(sb);
        }

        public bool Equals(StringBuilder sb)
        {
            if (sb == null) return false;

            if (Length == sb.Length && _str == sb._str)
                return true;

            return false;
        }
        #endregion

        #region Remove, Replace
        public StringBuilder Remove(int startIndex, int length)
        {
            // re-ordered to avoid possible integer overflow
            if (startIndex < 0 || length < 0 || startIndex > Length - length)
                throw new ArgumentOutOfRangeException();

            _str = _str.Remove(startIndex, length);

            return this;
        }

        public StringBuilder Replace(char oldChar, char newChar)
        {
            return Replace(oldChar, newChar, 0, Length);
        }

        public StringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
        {
            // re-ordered to avoid possible integer overflow
            if (startIndex > Length - count || startIndex < 0 || count < 0)
                throw new ArgumentOutOfRangeException();

            _str = _str.Replace(oldChar, newChar, startIndex, count);

            return this;
        }

        public StringBuilder Replace(string oldValue, string newValue)
        {
            if (oldValue == null)
                throw new ArgumentNullException("oldValue", "The old value cannot be null.");

            if (oldValue.Length == 0)
                throw new ArgumentException("The old value cannot be zero length.");

            _str = _str.Replace(oldValue, newValue);

            return this;
        }

        public StringBuilder Replace(string oldValue, string newValue, int startIndex, int count)
        {
            if (oldValue == null)
                throw new ArgumentNullException("oldValue", "The old value cannot be null.");

            if (startIndex < 0 || count < 0 || startIndex > Length - count)
                throw new ArgumentOutOfRangeException();

            if (oldValue.Length == 0)
                throw new ArgumentException("The old value cannot be zero length.");

            // TODO: OPTIMIZE!
            _str = _str.Replace(oldValue, newValue, startIndex, count);

            return this;
        }
        #endregion

        #region Append
        public StringBuilder Append(char[] value)
        {
            if (value == null)
                return this;
            return Append(new string(value));
        }

        public StringBuilder Append(string value)
        {
            if (value == null)
                return this;
            IncCapacity(value.Length);
            _str += value;
            return this;
        }

        public StringBuilder Append(bool value)
        {
            return Append(value.ToString());
        }

        public StringBuilder Append(byte value)
        {
            return Append(value.ToString());
        }

        internal StringBuilder Append(decimal value)
        {
            return Append(value.ToString());
        }

        public StringBuilder Append(double value)
        {
            return Append(value.ToString());
        }

        public StringBuilder Append(short value)
        {
            return Append(value.ToString());
        }

        public StringBuilder Append(int value)
        {
            return Append(value.ToString());
        }

        public StringBuilder Append(long value)
        {
            return Append(value.ToString());
        }

        public StringBuilder Append(object value)
        {
            if (value == null)
                return this;

            return Append(value.ToString());
        }

        [CLSCompliant(false)]
        public StringBuilder Append(sbyte value)
        {
            return Append(value.ToString());
        }

        public StringBuilder Append(float value)
        {
            return Append(value.ToString());
        }

        [CLSCompliant(false)]
        public StringBuilder Append(ushort value)
        {
            return Append(value.ToString());
        }

        [CLSCompliant(false)]
        public StringBuilder Append(uint value)
        {
            return Append(value.ToString());
        }

        [CLSCompliant(false)]
        public StringBuilder Append(ulong value)
        {
            return Append(value.ToString());
        }

        public StringBuilder Append(char value)
        {
            IncCapacity(1);
            _str += value;
            return this;
        }

        public StringBuilder Append(char value, int repeatCount)
        {
            if (repeatCount < 0)
                throw new ArgumentOutOfRangeException();

            while (repeatCount-- > 0)
                Append(value);

            return this;
        }

        public StringBuilder Append(char[] value, int startIndex, int charCount)
        {
            if (value == null)
            {
                if (!(startIndex == 0 && charCount == 0))
                    throw new ArgumentNullException("value");

                return this;
            }

            if ((charCount < 0 || startIndex < 0) || (startIndex > value.Length - charCount))
                throw new ArgumentOutOfRangeException();

            while (charCount-- > 0)
            {
                Append(value[startIndex]);
                ++startIndex;
            }

            return this;
        }

        public StringBuilder Append(string value, int startIndex, int count)
        {
            if (value == null)
            {
                if (startIndex != 0 && count != 0)
                    throw new ArgumentNullException("value");

                return this;
            }

            if ((count < 0 || startIndex < 0) || (startIndex > value.Length - count))
                throw new ArgumentOutOfRangeException();

            string s = value.Substring(startIndex, count);
            return Append(s);
        }

#if NET_2_0
        [ComVisible(false)]
        public StringBuilder AppendLine()
        {
            return Append(Environment.NewLine);
        }

        [ComVisible(false)]
        public StringBuilder AppendLine(string value)
        {
            return Append(value).Append(Environment.NewLine);
        }

#endif

        public StringBuilder AppendFormat(string format, params object[] args)
        {
            return AppendFormat(null, format, args);
        }

        public StringBuilder AppendFormat(IFormatProvider provider,
                           string format,
                           params object[] args)
        {
            String.FormatHelper(this, provider, format, args);
            return this;
        }

#if NOT_PFX
        public StringBuilder AppendFormat(string format, object arg0)
        {
            return AppendFormat(null, format, new object[] { arg0 });
        }

        public StringBuilder AppendFormat(string format, object arg0, object arg1)
        {
            return AppendFormat(null, format, new object[] { arg0, arg1 });
        }

        public StringBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            return AppendFormat(null, format, new object[] { arg0, arg1, arg2 });
        }
#endif
        #endregion

        #region Insert
        public StringBuilder Insert(int index, char[] value)
        {
            return Insert(index, new string(value));
        }

        public StringBuilder Insert(int index, string value)
        {
            if (index > Length || index < 0)
                throw new ArgumentOutOfRangeException();

            if (value == null || value.Length == 0)
                return this;

            _str = _str.Insert(index, value);

            return this;
        }

#if NOT_PFX
        public StringBuilder Insert(int index, bool value)
        {
            return Insert(index, value.ToString());
        }

        public StringBuilder Insert(int index, byte value)
        {
            return Insert(index, value.ToString());
        }
#endif

        public StringBuilder Insert(int index, char value)
        {
            if (index > Length || index < 0)
                throw new ArgumentOutOfRangeException("index");

            //TODO: OPTIMIZE!
            _str = _str.Insert(index, new string(value, 1));

            return this;
        }

#if NOT_PFX
        public StringBuilder Insert(int index, decimal value)
        {
            return Insert(index, value.ToString());
        }

        public StringBuilder Insert(int index, double value)
        {
            return Insert(index, value.ToString());
        }

        public StringBuilder Insert(int index, short value)
        {
            return Insert(index, value.ToString());
        }

        public StringBuilder Insert(int index, int value)
        {
            return Insert(index, value.ToString());
        }

        public StringBuilder Insert(int index, long value)
        {
            return Insert(index, value.ToString());
        }

        public StringBuilder Insert(int index, object value)
        {
            return Insert(index, value.ToString());
        }

        [CLSCompliant(false)]
        public StringBuilder Insert(int index, sbyte value)
        {
            return Insert(index, value.ToString());
        }

        public StringBuilder Insert(int index, float value)
        {
            return Insert(index, value.ToString());
        }

        [CLSCompliant(false)]
        public StringBuilder Insert(int index, ushort value)
        {
            return Insert(index, value.ToString());
        }

        [CLSCompliant(false)]
        public StringBuilder Insert(int index, uint value)
        {
            return Insert(index, value.ToString());
        }

        [CLSCompliant(false)]
        public StringBuilder Insert(int index, ulong value)
        {
            return Insert(index, value.ToString());
        }
#endif

        public StringBuilder Insert(int index, string value, int count)
        {
            // LAMESPEC: The spec says to throw an exception if 
            // count < 0, while MS throws even for count < 1!
            if (count < 0)
                throw new ArgumentOutOfRangeException();

            if (value != null && value.Length != 0)
                for (int insertCount = 0; insertCount < count; insertCount++)
                    Insert(index, value);

            return this;
        }

        public StringBuilder Insert(int index, char[] value, int startIndex, int charCount)
        {
            if (value == null)
            {
                if (startIndex == 0 && charCount == 0)
                    return this;

                throw new ArgumentNullException("value");
            }

            if (charCount < 0 || startIndex < 0 || startIndex > value.Length - charCount)
                throw new ArgumentOutOfRangeException();

            return Insert(index, new String(value, startIndex, charCount));
        }
        #endregion


#if NET_2_0
		[ComVisible (false)]
		public void CopyTo (int sourceIndex, char [] destination, int destinationIndex, int count)
		{
			if (destination == null)
				throw new ArgumentNullException ("destination");
			if ((Length - count < sourceIndex) ||
			    (destination.Length -count < destinationIndex) ||
			    (sourceIndex < 0 || destinationIndex < 0 || count < 0))
				throw new ArgumentOutOfRangeException ();

			for (int i = 0; i < count; i++)
				destination [destinationIndex+i] = _str [sourceIndex+i];
        }


#if NOT_PFX		
		void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
		{
			info.AddValue ("m_MaxCapacity", _maxCapacity);
			info.AddValue ("Capacity", Capacity);
			info.AddValue ("m_StringValue", ToString ());
			info.AddValue ("m_currentThread", 0);
		}



        StringBuilder (SerializationInfo info, StreamingContext context)
		{
			string s = info.GetString ("m_StringValue");
			if (s == null)
				s = "";
			_length = s.Length;
			_str = _cached_str = s;
			
			_maxCapacity = info.GetInt32 ("m_MaxCapacity");
			if (_maxCapacity < 0)
				_maxCapacity = Int32.MaxValue;
			Capacity = info.GetInt32 ("Capacity");
		}
#endif
#endif
    }
}
