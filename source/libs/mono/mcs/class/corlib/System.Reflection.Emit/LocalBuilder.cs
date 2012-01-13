
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

//
// System.Reflection.Emit/LocalBuilder.cs
//
// Authors:
//   Paolo Molaro (lupus@ximian.com)
//   Martin Baulig (martin@gnome.org)
//   Miguel de Icaza (miguel@ximian.com)
//
// (C) 2001, 2002 Ximian, Inc.  http://www.ximian.com
//

using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics.SymbolStore;

namespace System.Reflection.Emit {
#if NET_2_0
	[ComVisible (true)]
	[ComDefaultInterface (typeof (_LocalBuilder))]
#endif
	[ClassInterface (ClassInterfaceType.None)]
#if NET_2_0
	public sealed class LocalBuilder : LocalVariableInfo, _LocalBuilder {
#else
	public sealed class LocalBuilder : _LocalBuilder {
#endif

#if NET_2_0
		// Some fields are already defined in LocalVariableInfo
		#region Sync with reflection.h
		private string name;
		#endregion
#else
		#region Sync with reflection.h
		private Type type;
		internal bool is_pinned;
		internal ushort position;
		private string name;
		#endregion
#endif
		
		internal ILGenerator ilgen;
		int startOffset;
		int endOffset;

		internal LocalBuilder (Type t, ILGenerator ilgen)
		{
			this.type = t;
			this.ilgen = ilgen;
		}

		public void SetLocalSymInfo (string lname, int startOffset, int endOffset)
		{
			name = lname;
			this.startOffset = startOffset;
			this.endOffset = endOffset;
		}

		public void SetLocalSymInfo (string lname)
		{
			SetLocalSymInfo (lname, 0, 0);
		}

#if NET_2_0
		override
#endif
		public Type LocalType
		{
			get {
				return type;
			}
		}

#if NET_2_0
		public override bool IsPinned
		{
			get {
				return is_pinned;
			}
		}

		public override int LocalIndex
		{
			get {
				return position;
			}
		}
#endif

		internal static int Mono_GetLocalIndex (LocalBuilder builder)
		{
			return builder.position;
		}

		internal string Name {
			get { return name; }
		}
		
		internal int StartOffset {
			get { return startOffset; }
		}
		
		internal int EndOffset {
			get { return endOffset; }
		}

		void _LocalBuilder.GetIDsOfNames ([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException ();
		}

		void _LocalBuilder.GetTypeInfo (uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException ();
		}

		void _LocalBuilder.GetTypeInfoCount (out uint pcTInfo)
		{
			throw new NotImplementedException ();
		}

		void _LocalBuilder.Invoke (uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException ();
		}
	}
}
