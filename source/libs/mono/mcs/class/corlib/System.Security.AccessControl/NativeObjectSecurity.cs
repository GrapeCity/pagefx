//
// System.Security.AccessControl.NativeObjectSecurity implementation
//
// Author:
//	Dick Porter  <dick@ximian.com>
//
// Copyright (C) 2005, 2006 Novell, Inc (http://www.novell.com)
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

using System.Runtime.InteropServices;

namespace System.Security.AccessControl {

	public abstract class NativeObjectSecurity : CommonObjectSecurity {

		protected internal delegate Exception ExceptionFromErrorCode (int errorCode, string name, SafeHandle handle, object context);
		
		internal NativeObjectSecurity ()
		{
			/* Give it a 0-param constructor */
		}
		
		protected NativeObjectSecurity (bool isContainer,
						ResourceType resourceType)
		{
		}

		protected NativeObjectSecurity (bool isContainer,
						ResourceType resourceType,
						ExceptionFromErrorCode exceptionFromErrorCode,
						object exceptionContext)
		{
		}
		
		protected NativeObjectSecurity (bool isContainer,
						ResourceType resourceType,
						SafeHandle handle,
						AccessControlSections includeSections)
		{
		}
		
		protected NativeObjectSecurity (bool isContainer,
						ResourceType resourceType,
						string name,
						AccessControlSections includeSections)
		{
		}
		
		protected NativeObjectSecurity (bool isContainer,
						ResourceType resourceType,
						SafeHandle handle,
						AccessControlSections includeSections,
						ExceptionFromErrorCode exceptionFromErrorCode,
						object exceptionContext)
		{
		}
		
		protected NativeObjectSecurity (bool isContainer,
						ResourceType resourceType,
						string name,
						AccessControlSections includeSections,
						ExceptionFromErrorCode exceptionFromErrorCode,
						object exceptionContext)
		{
		}
		
		protected override sealed void Persist (SafeHandle handle,
							AccessControlSections includeSections)
		{
			throw new NotImplementedException ();
		}
		
		protected override sealed void Persist (string name,
							AccessControlSections includeSections)
		{
			throw new NotImplementedException ();
		}
		
		protected void Persist (SafeHandle handle,
					AccessControlSections includeSections,
					object exceptionContext)
		{
			throw new NotImplementedException ();
		}
		
		protected void Persist (string name,
					AccessControlSections includeSections,
					object exceptionContext)
		{
			throw new NotImplementedException ();
		}
	}
}

#endif
