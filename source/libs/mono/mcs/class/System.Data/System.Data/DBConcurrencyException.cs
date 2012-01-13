//
// System.Data.DBConcurrencyException.cs
//
// Author: Duncan Mak  (duncan@ximian.com)
//
// (C) Ximian, Inc.

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
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Data {
	[Serializable]
	public sealed class DBConcurrencyException : SystemException
	{
		DataRow row;

		#region Constructors
#if NET_1_1
                public DBConcurrencyException ()
                        : base ()
                {
                }
#endif
		public DBConcurrencyException (string message)
			: base (message)
		{
		}

		public DBConcurrencyException (string message, Exception innerException)
			: base (message, innerException)
		{
		}

#if NET_2_0
		public DBConcurrencyException (string message, Exception inner, DataRow[] dataRows)
			: base (message, inner)
		{
		}
#endif

#if NOT_PFX
private DBConcurrencyException (SerializationInfo si, StreamingContext sc) : base(si, sc)
		{
		}
#endif

		#endregion // Constructors

		#region Properties

		public DataRow Row {
			get { return row; }
			set { row = value;} // setting the row has no effect
		}

#if NET_2_0
		[MonoTODO]
		public int RowCount {
			get { throw new NotImplementedException (); }
		}
#endif

		#endregion // Properties

		#region Methods

#if NET_2_0
		[MonoTODO]
		public void CopyToRows (DataRow[] array)
		{
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public void CopyToRows (DataRow[] array, int ArrayIndex)
		{
			throw new NotImplementedException ();
		}
#endif

#if NOT_PFX
public override void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException ("info");

			base.GetObjectData (info, context);
		}
#endif

		#endregion // Methods
	}
}
