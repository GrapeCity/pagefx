//
// System.Security.AccessControl.RawAcl implementation
//
// Author:
//	Dick Porter  <dick@ximian.com>
//
// Copyright (C) 2006 Novell, Inc (http://www.novell.com)
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

namespace System.Security.AccessControl {
	public sealed class RawAcl : GenericAcl
	{
		public RawAcl (byte revision, int capacity)
		{
		}
		
		public RawAcl (byte[] binaryForm, int offset)
		{
		}
		
		public override int BinaryLength
		{
			get {
				throw new NotImplementedException ();
			}
		}

		public override int Count
		{
			get {
				throw new NotImplementedException ();
			}
		}

		public override GenericAce this[int index]
		{
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public override byte Revision
		{
			get {
				throw new NotImplementedException ();
			}
		}
		
		public override void GetBinaryForm (byte[] binaryForm,
						    int offset)
		{
			throw new NotImplementedException ();
		}

		public void InsertAce (int index, GenericAce ace)
		{
			throw new NotImplementedException ();
		}
		
		public void RemoveAce (int index)
		{
			throw new NotImplementedException ();
		}
	}
}

#endif
