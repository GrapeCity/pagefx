//
// System.Runtime.InteropServices/GCHandle.cs
//
// Authors:
//   Ajay kumar Dwivedi (adwiv@yahoo.com) ??
//   Paolo Molaro (lupus@ximian.com)
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Runtime.InteropServices
{

#if NET_2_0
	[ComVisible(true)]
#endif
	[MonoTODO("Struct should be [StructLayout(LayoutKind.Sequential)] but will need to be reordered for that.")]
	public struct GCHandle 
	{
		// fields
		private int handle;

		private GCHandle(IntPtr h)
		{
			handle = (int)h;
		}
		
		// Constructors
		private GCHandle(object obj)
			: this(obj, GCHandleType.Normal)
		{}

		private GCHandle(object value, GCHandleType type)
		{
			handle = GetTargetHandle (value, 0, type);
		}

		// Properties

		public bool IsAllocated 
		{ 
			get
			{
				return (handle != 0);
			}
		}

		public object Target
		{ 
			get
			{
				return GetTarget (handle);
			} 
			set
			{
				handle = GetTargetHandle (value, handle, (GCHandleType)(-1));
			} 
		}

		// Methods
		public IntPtr AddrOfPinnedObject()
		{
			IntPtr res = GetAddrOfPinnedObject(handle);
			if (res == IntPtr.Zero)
				throw new InvalidOperationException("The handle is not of Pinned type");
			if (res == (IntPtr)(-1))
				throw new ArgumentException ("Object contains non-primitive or non-blittable data.");
			return res;
		}

		public static System.Runtime.InteropServices.GCHandle Alloc(object value)
		{
			return new GCHandle (value);
		}

		public static System.Runtime.InteropServices.GCHandle Alloc(object value, GCHandleType type)
		{
			return new GCHandle (value,type);
		}

		public void Free()
		{
			FreeHandle(handle);
			handle = 0;
		}
		
		public static explicit operator IntPtr (GCHandle value)
		{
			return (IntPtr) value.handle;
		}
		
		public static explicit operator GCHandle(IntPtr value)
		{
			if (value == IntPtr.Zero)
				throw new ArgumentException ("GCHandle value cannot be zero");
			if (!CheckCurrentDomain ((int)value))
				throw new ArgumentException ("GCHandle value belongs to a different domain");
			return new GCHandle (value);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static bool CheckCurrentDomain (int handle);
		
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static object GetTarget(int handle);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static int GetTargetHandle(object obj, int handle, GCHandleType type);
		
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void FreeHandle(int handle);
		
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static IntPtr GetAddrOfPinnedObject(int handle);

#if NET_2_0
		public static bool operator ==(GCHandle a, GCHandle b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(GCHandle a, GCHandle b)
		{
			return (!(a.Equals(b)));
		}
		
		public override bool Equals(object o)
		{
			if (o == null || !(o is GCHandle))
				return false;

			return (handle == ((GCHandle)o).handle);
		}

		public override int GetHashCode()
		{
			return handle.GetHashCode ();
		}

		public static GCHandle FromIntPtr (IntPtr value)
		{
			return (GCHandle)value;
		}

		public static IntPtr ToIntPtr (GCHandle value)
		{
			return (IntPtr)value;
		}
#endif
	} 
}

