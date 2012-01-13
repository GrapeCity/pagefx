//
// System.Runtime.Serialization.SerializationInfo.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//   Duncan Mak (duncan@ximian.com)
//   Dietmar Maurer (dietmar@ximian.com)
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

using System;
using System.Collections;

namespace System.Runtime.Serialization
{
#if NET_2_0
	[System.Runtime.InteropServices.ComVisibleAttribute (true)]
#endif
	public sealed class SerializationInfo
	{
		Hashtable serialized = new Hashtable ();
		ArrayList values = new ArrayList ();

		string assemblyName; // the assembly being serialized
		string fullTypeName; // the type being serialized.

#if NOT_PFX
		IFormatterConverter converter;
#endif
		
#if NOT_PFX
		/* used by the runtime */
		private SerializationInfo (Type type)
		{
			assemblyName = type.Assembly.FullName;
			fullTypeName = type.FullName;
			converter = new FormatterConverter ();
		}
		
		/* used by the runtime */
		private SerializationInfo (Type type, SerializationEntry [] data)
		{
			int len = data.Length;

			assemblyName = type.Assembly.FullName;
			fullTypeName = type.FullName;
			converter = new FormatterConverter ();

			for (int i = 0; i < len; i++) {
				serialized.Add (data [i].Name, data [i]);
				values.Add (data [i]);
			}
		}
#endif

#if NOT_PFX
		// Constructor
		[CLSCompliant (false)]
		public SerializationInfo (Type type, IFormatterConverter converter)
		{
			if (type == null)
				throw new ArgumentNullException ("type", "Null argument");

			if (converter == null)
				throw new ArgumentNullException ("converter", "Null argument");
			
			this.converter = converter;
			assemblyName = type.Assembly.FullName;
			fullTypeName = type.FullName;
		}
#endif

		// Properties
		public string AssemblyName
		{
			get { return assemblyName; }
			
			set {
				if (value == null)
					throw new ArgumentNullException ("Argument is null.");
				assemblyName = value;
			}
		}
		
		public string FullTypeName
		{
			get { return fullTypeName; }
			
			set {
				if ( value == null)
					throw new ArgumentNullException ("Argument is null.");
				fullTypeName = value;
			}
		}
		
		public int MemberCount
		{
			get { return serialized.Count; }
		}

		// Methods
		public void AddValue (string name, object value, Type type)
		{
#if NOT_PFX
			if (name == null)
				throw new ArgumentNullException ("name is null");
			if (type == null)
				throw new ArgumentNullException ("type is null");
			
			if (serialized.ContainsKey (name))
				throw new SerializationException ("Value has been serialized already.");
			
			SerializationEntry entry = new SerializationEntry (name, type, value);

			serialized.Add (name, entry);
			values.Add (entry);
#else
            throw new NotSupportedException();
#endif
		}

		public object GetValue (string name, Type type)
		{
#if NOT_PFX
			if (name == null)
				throw new ArgumentNullException ("name is null.");
			if (type == null)
				throw new ArgumentNullException ("type");
			if (!serialized.ContainsKey (name))
				throw new SerializationException ("No element named " + name + " could be found.");
						
			SerializationEntry entry = (SerializationEntry) serialized [name];

			if (entry.Value != null && !type.IsInstanceOfType (entry.Value))
				return converter.Convert (entry.Value, type);
			else
				return entry.Value;
#else
            throw new NotSupportedException();
#endif
		}

		public void SetType (Type type)
		{
			if (type == null)
				throw new ArgumentNullException ("type is null.");

			fullTypeName = type.FullName;
			assemblyName = type.Assembly.FullName;
		}

#if NOT_PFX
		public SerializationInfoEnumerator GetEnumerator ()
		{
			return new SerializationInfoEnumerator (values);
		}
#endif
		
		public void AddValue (string name, short value)
		{
			AddValue (name, value, typeof (System.Int16));
		}

		[CLSCompliant(false)]
		public void AddValue (string name, UInt16 value)
		{
			AddValue (name, value, typeof (System.UInt16));
		}
		
		public void AddValue (string name, int value)
		{
			AddValue (name, value, typeof (System.Int32));
		}
		
		public void AddValue (string name, byte value)
		{
			AddValue (name, value, typeof (System.Byte));
		}
		
		public void AddValue (string name, bool value)
		{
			AddValue (name, value, typeof (System.Boolean));
		}
	       
		public void AddValue (string name, char value)
		{
			AddValue (name, value, typeof (System.Char));
		}

		[CLSCompliant(false)]
		public void AddValue (string name, SByte value)
		{
			AddValue (name, value, typeof (System.SByte));
		}
		
		public void AddValue (string name, double value)
		{
			AddValue (name, value, typeof (System.Double));
		}
		
		public void AddValue (string name, Decimal value)
		{
			AddValue (name, value, typeof (System.Decimal));
		}
		
		public void AddValue (string name, DateTime value)
		{
			AddValue (name, value, typeof (System.DateTime));
		}
		
		public void AddValue (string name, float value)
		{
			AddValue (name, value, typeof (System.Single));
		}

		[CLSCompliant(false)]
		public void AddValue (string name, UInt32 value)
		{
			AddValue (name, value, typeof (System.UInt32));
		}
	       
		public void AddValue (string name, long value)
		{
			AddValue (name, value, typeof (System.Int64));
		}

		[CLSCompliant(false)]
		public void AddValue (string name, UInt64 value)
		{
			AddValue (name, value, typeof (System.UInt64));
		}
		
		public void AddValue (string name, object value)
		{
			if (value == null)
				AddValue (name, value, typeof (System.Object));
			else
				AddValue (name, value, value.GetType ());
		}		
		
		public bool GetBoolean (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.Boolean));
			return converter.ToBoolean (value);
#else
            throw new NotSupportedException();
#endif
		}

        public byte GetByte(string name)
        {
#if NOT_PFX
			object value = GetValue (name, typeof (System.Byte));
			return converter.ToByte (value);
#else
            throw new NotSupportedException();
#endif
        }

	    public char GetChar (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.Char));
			return converter.ToChar (value);
#else
	        throw new NotSupportedException();
#endif
		}

		public DateTime GetDateTime (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.DateTime));
			return converter.ToDateTime (value);
#else
		    throw new NotSupportedException();
#endif
		}
		
		public Decimal GetDecimal (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.Decimal));
			return converter.ToDecimal (value);
#else
		    throw new NotSupportedException();
#endif
		}
		
		public double GetDouble (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.Double));
			return converter.ToDouble (value);
#else
		    throw new NotSupportedException();
#endif
		}
						
		public short GetInt16 (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.Int16));
			return converter.ToInt16 (value);
#else
		    throw new NotSupportedException();
#endif
		}
		
		public int GetInt32 (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.Int32));
			return converter.ToInt32 (value);
#else
		    throw new NotSupportedException();
#endif
		}
	       
		public long GetInt64 (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.Int64));
			return converter.ToInt64 (value);
#else
		    throw new NotSupportedException();
#endif
		}

		[CLSCompliant(false)]
		public SByte GetSByte (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.SByte));
			return converter.ToSByte (value);
#else
		    throw new NotSupportedException();
#endif
		}
		
		public float GetSingle (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.Single));
			return converter.ToSingle (value);
#else
		    throw new NotSupportedException();
#endif
		}
		
		public string GetString (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.String));
			if (value == null) return null;
			return converter.ToString (value);
#else
		    throw new NotSupportedException();
#endif
		}

		[CLSCompliant(false)]
		public UInt16 GetUInt16 (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.UInt16));
			return converter.ToUInt16 (value);
#else
		    throw new NotSupportedException();
#endif
		}
		
		[CLSCompliant(false)]
		public UInt32 GetUInt32 (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.UInt32));
			return converter.ToUInt32 (value);
#else
		    throw new NotSupportedException();
#endif
		}
		[CLSCompliant(false)]
		public UInt64 GetUInt64 (string name)
		{
#if NOT_PFX
			object value = GetValue (name, typeof (System.UInt64));
			return converter.ToUInt64 (value);
#else
		    throw new NotSupportedException();
#endif
		}

#if NOT_PFX
		/* used by the runtime */
		private SerializationEntry [] get_entries ()
		{
			SerializationEntry [] res = new SerializationEntry [this.MemberCount];
			int i = 0;
			
			foreach (SerializationEntry e in this)
				res [i++] = e;
			
			return res;
		}
#endif
	}
}
