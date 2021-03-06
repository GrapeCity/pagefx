//CHANGED
//
// System.DBNull.cs
//
// Authors:
//   Duncan Mak (duncan@ximian.com)
//   Ben Maurer (bmaurer@users.sourceforge.net)
//
// (C) 2002 Ximian, Inc. http://www.ximian.com
// (C) 2003 Ben Maurer
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


#if NOT_PFX
using System.Runtime.Serialization;
#endif

namespace System
{
	[Serializable]
	public sealed class DBNull : 
#if NOT_PFX        
        ISerializable, 
#endif
        
        IConvertible
	{
		// Fields
		public static readonly DBNull Value = new DBNull ();

		// Private constructor
		private DBNull ()
		{
		}
#if NOT_PFX
		private DBNull (SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException ();
		}

		// Methods
		public void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			UnitySerializationHolder.GetDBNullData (this, info, context);
		}
#endif
		public TypeCode GetTypeCode ()
		{
			return TypeCode.DBNull;
		}

		bool IConvertible.ToBoolean (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}			

		byte IConvertible.ToByte (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		char IConvertible.ToChar (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		DateTime IConvertible.ToDateTime (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		decimal IConvertible.ToDecimal (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}
		
		double IConvertible.ToDouble (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		short IConvertible.ToInt16 (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		int IConvertible.ToInt32 (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		long IConvertible.ToInt64 (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		sbyte IConvertible.ToSByte (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		float IConvertible.ToSingle (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		object IConvertible.ToType (Type type, IFormatProvider provider)
		{
			if (type == typeof (string))
				return String.Empty;
			throw new InvalidCastException ();
		}

		ushort IConvertible.ToUInt16 (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		uint IConvertible.ToUInt32 (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		ulong IConvertible.ToUInt64 (IFormatProvider provider)
		{
			throw new InvalidCastException ();
		}

		public override string ToString ()
		{
			return String.Empty;
		}

		public string ToString (IFormatProvider provider)
		{
			return String.Empty;
		}
	}
}
