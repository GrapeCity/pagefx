// System.ComponentModel.Design.DesignerTransactionCloseEventArgs.cs
//
// Author:
// 	Alejandro Sánchez Acosta  <raciel@es.gnu.org>
//	Ivan N. Zlatev <contact i-nZ.net>
//
// (C) Alejandro Sánchez Acosta
// 

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

using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	[ComVisible(true)]
	public class DesignerTransactionCloseEventArgs : EventArgs
	{
		private bool commit;
#if NET_2_0		
		private bool last_transaction;
#endif

#if NET_2_0
		public DesignerTransactionCloseEventArgs (bool commit, bool lastTransaction)
		{
			this.commit = commit;
			this.last_transaction = lastTransaction;
		}
#endif
		
#if NET_2_0
		[Obsolete ("Use another constructor that indicates lastTransaction")]
#endif
		public DesignerTransactionCloseEventArgs (bool commit)
		{
			this.commit = commit;
		}

#if NET_2_0		
		public bool LastTransaction {
			get { return last_transaction; }
		}
#endif

		public bool TransactionCommitted {
			get { return commit; }
		}
	}
}
