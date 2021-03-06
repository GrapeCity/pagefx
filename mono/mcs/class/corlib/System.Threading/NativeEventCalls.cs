//
// System.Threading.AutoResetEvent.cs
//
// Author:
//   Dick Porter (dick@ximian.com)
//   Veronica De Santis (veron78@interfree.it)
//
// (C) Ximian, Inc.  http://www.ximian.com
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

#if NET_2_0
using System.Security.AccessControl;
using System.IO;
#endif

namespace System.Threading 
{
 	internal sealed class NativeEventCalls
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern IntPtr CreateEvent_internal(bool manual,bool initial,string name, out bool created);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern bool SetEvent_internal(IntPtr handle);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern bool ResetEvent_internal(IntPtr handle);
	
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void CloseEvent_internal (IntPtr handle);

#if NET_2_0
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		public static extern IntPtr OpenEvent_internal (string name, EventWaitHandleRights rights, out MonoIOError error);
#endif
	}
}
