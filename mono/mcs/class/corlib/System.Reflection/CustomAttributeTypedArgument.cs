//
// System.Reflection/CustomAttributeTypedArgument.cs
//
// Author:
//   Zoltan Varga (vargaz@gmail.com)
//   Carlos Alberto Cortez (calberto.cortez@gmail.com)
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

namespace System.Reflection {

	[ComVisible (true)]
	[Serializable]
	internal struct CustomAttributeTypedArgument {
		Type argumentType;
		object value;

		internal CustomAttributeTypedArgument (Type argumentType, object value)
		{
			this.argumentType = argumentType;
			this.value = value;
		}

		public Type ArgumentType {
			get {
				return argumentType;
			}
		}

		public object Value {
			get {
				return value;
			}
		}

		public override string ToString ()
		{
			string val = value.ToString ();
			if (argumentType == typeof (string))
				return "\"" + val + "\"";
			if (argumentType == typeof (Type)) 
				return "typeof (" + val + ")";
			if (argumentType.IsEnum)
				return "(" + argumentType.Name + ")" + val;

			return val;
		}

		public override bool Equals (object obj)
		{
			if (!(obj is CustomAttributeTypedArgument))
				return false;
			CustomAttributeTypedArgument other = (CustomAttributeTypedArgument) obj;
			return  other.argumentType == argumentType &&
				value != null ? value.Equals (other.value) : (object) other.value == null;
		}

		public override int GetHashCode ()
		{
			return (argumentType.GetHashCode () << 16) + (value != null ? value.GetHashCode () : 0);
		}

		public static bool operator == (CustomAttributeTypedArgument obj1, CustomAttributeTypedArgument obj2)
		{
			return obj1.Equals (obj2);
		}

		public static bool operator != (CustomAttributeTypedArgument obj1, CustomAttributeTypedArgument obj2)
		{
			return !obj1.Equals (obj2);
		}
	}

}

#endif

