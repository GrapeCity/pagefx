//
// System.Net.WebConnectionStream
//
// Authors:
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// (C) 2003 Ximian, Inc (http://www.ximian.com)
// (C) 2004 Novell, Inc (http://www.novell.com)
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

using System.IO;
using System.Text;
using System.Threading;

namespace System.Net
{
	class WebConnectionStream : Stream
	{
		static byte [] crlf = new byte [] { 13, 10 };
		bool isRead;
		WebConnection cnc;
		HttpWebRequest request;
		byte [] readBuffer;
		int readBufferOffset;
		int readBufferSize;
		int contentLength;
		int totalRead;
		bool nextReadCalled;
		int pendingReads;
		int pendingWrites;
		ManualResetEvent pending;
		bool allowBuffering;
		bool sendChunked;
		MemoryStream writeBuffer;
		bool requestWritten;
		byte [] headers;
		bool disposed;
		bool headersSent;
		object locker = new object ();
		bool initRead;
		bool read_eof;
		bool complete_request_written;
		long max_buffer_size;

		public WebConnectionStream (WebConnection cnc)
		{
			isRead = true;
			pending = new ManualResetEvent (true);
			this.request = cnc.Data.request;
			this.cnc = cnc;
			string contentType = cnc.Data.Headers ["Transfer-Encoding"];
			bool chunkedRead = (contentType != null && contentType.ToLower ().IndexOf ("chunked") != -1);
			string clength = cnc.Data.Headers ["Content-Length"];
			if (!chunkedRead && clength != null && clength != "") {

				try {
					contentLength = Int32.Parse (clength);
				} catch {
					contentLength = Int32.MaxValue;
				}
			} else {
				contentLength = Int32.MaxValue;
			}
		}

		public WebConnectionStream (WebConnection cnc, HttpWebRequest request)
		{
			isRead = false;
			this.cnc = cnc;
			this.request = request;
			allowBuffering = request.InternalAllowBuffering;
			sendChunked = request.SendChunked;
			if (allowBuffering) {
				writeBuffer = new MemoryStream ();
				max_buffer_size = request.ContentLength;
			} else {
				max_buffer_size = -1;
			}

			if (sendChunked)
				pending = new ManualResetEvent (true);
		}
#if NET_2_0
		public override bool CanTimeout {
			get { return true; }
		}
#endif

		internal bool CompleteRequestWritten {
			get { return complete_request_written; }
		}

		internal bool SendChunked {
			set { sendChunked = value; }
		}

		internal byte [] ReadBuffer {
			set { readBuffer = value; }
		}

		internal int ReadBufferOffset {
			set { readBufferOffset = value;}
		}
		
		internal int ReadBufferSize {
			set { readBufferSize = value; }
		}
		
		internal byte[] WriteBuffer {
			get { return writeBuffer.GetBuffer (); }
		}

		internal int WriteBufferLength {
			get { return (int) writeBuffer.Length; }
		}

		internal void ForceCompletion ()
		{
			nextReadCalled = true;
			cnc.NextRead ();
		}
		
		internal void CheckComplete ()
		{
			bool nrc = nextReadCalled;
			if (!nrc && readBufferSize - readBufferOffset == contentLength) {
				nextReadCalled = true;
				cnc.NextRead ();
			}
		}

		internal void ReadAll ()
		{
			if (!isRead || read_eof || totalRead >= contentLength || nextReadCalled) {
				if (isRead && !nextReadCalled) {
					nextReadCalled = true;
					cnc.NextRead ();
				}
				return;
			}

			pending.WaitOne ();
			lock (locker) {
				if (totalRead >= contentLength)
					return;
				
				byte [] b = null;
				int diff = readBufferSize - readBufferOffset;
				int new_size;

				if (contentLength == Int32.MaxValue) {
					MemoryStream ms = new MemoryStream ();
					byte [] buffer = null;
					if (readBuffer != null && diff > 0) {
						ms.Write (readBuffer, readBufferOffset, diff);
						if (readBufferSize >= 8192)
							buffer = readBuffer;
					}

					if (buffer == null)
						buffer = new byte [8192];

					int read;
					while ((read = cnc.Read (buffer, 0, buffer.Length)) != 0)
						ms.Write (buffer, 0, read);

					b = ms.GetBuffer ();
					new_size = (int) ms.Length;
					contentLength = new_size;
				} else {
					new_size = contentLength - totalRead;
					b = new byte [new_size];
					if (readBuffer != null && diff > 0) {
						if (diff > new_size)
							diff = new_size;

						Buffer.BlockCopy (readBuffer, readBufferOffset, b, 0, diff);
					}
					
					int remaining = new_size - diff;
					int r = -1;
					while (remaining > 0 && r != 0) {
						r = cnc.Read (b, diff, remaining);
						remaining -= r;
						diff += r;
					}
				}

				readBuffer = b;
				readBufferOffset = 0;
				readBufferSize = new_size;
				totalRead = 0;
				nextReadCalled = true;
			}

			cnc.NextRead ();
		}

	   	void WriteCallbackWrapper (IAsyncResult r)
		{
			WebAsyncResult result;
			if (r.AsyncState != null) {
				result = (WebAsyncResult) r.AsyncState;
				result.InnerAsyncResult = r;
				result.DoCallback ();
			} else {
				EndWrite (r);
			}
		}

	   	void ReadCallbackWrapper (IAsyncResult r)
		{
			WebAsyncResult result;
			if (r.AsyncState != null) {
				result = (WebAsyncResult) r.AsyncState;
				result.InnerAsyncResult = r;
				result.DoCallback ();
			} else {
				EndRead (r);
			}
		}

		public override int Read (byte [] buffer, int offset, int size)
		{
			if (!isRead)
				throw new NotSupportedException ("this stream does not allow reading");

			if (totalRead >= contentLength)
				return 0;

			AsyncCallback cb = new AsyncCallback (ReadCallbackWrapper);
			WebAsyncResult res = (WebAsyncResult) BeginRead (buffer, offset, size, cb, null);
			if (!res.IsCompleted && !res.WaitUntilComplete (request.ReadWriteTimeout, false)) {
				nextReadCalled = true;
				cnc.Close (true);
				throw new WebException ("The operation has timed out.",
					WebExceptionStatus.Timeout);
			}

			return EndRead (res);
		}

		public override IAsyncResult BeginRead (byte [] buffer, int offset, int size,
							AsyncCallback cb, object state)
		{
			if (!isRead)
				throw new NotSupportedException ("this stream does not allow reading");

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			int length = buffer.Length;
			if (size < 0 || offset < 0 || length < offset || length - offset < size)
				throw new ArgumentOutOfRangeException ();

			lock (locker) {
				pendingReads++;
				pending.Reset ();
			}

			WebAsyncResult result = new WebAsyncResult (cb, state, buffer, offset, size);
			if (totalRead >= contentLength) {
				result.SetCompleted (true, -1);
				result.DoCallback ();
				return result;
			}
			
			int remaining = readBufferSize - readBufferOffset;
			if (remaining > 0) {
				int copy = (remaining > size) ? size : remaining;
				Buffer.BlockCopy (readBuffer, readBufferOffset, buffer, offset, copy);
				readBufferOffset += copy;
				offset += copy;
				size -= copy;
				totalRead += copy;
				if (size == 0 || totalRead >= contentLength) {
					result.SetCompleted (true, copy);
					result.DoCallback ();
					return result;
				}
				result.NBytes = copy;
			}

			if (cb != null)
				cb = new AsyncCallback (ReadCallbackWrapper);

			if (contentLength != Int32.MaxValue && contentLength - totalRead < size)
				size = contentLength - totalRead;

			if (!read_eof) {
				result.InnerAsyncResult = cnc.BeginRead (buffer, offset, size, cb, result);
			} else {
				result.SetCompleted (true, result.NBytes);
				result.DoCallback ();
			}
			return result;
		}

		public override int EndRead (IAsyncResult r)
		{
			WebAsyncResult result = (WebAsyncResult) r;
			if (result.EndCalled) {
				int xx = result.NBytes;
				return (xx >= 0) ? xx : 0;
			}

			result.EndCalled = true;

			if (!result.IsCompleted) {
				int nbytes = -1;
				try {
					nbytes = cnc.EndRead (result);
				} catch (Exception exc) {
					lock (locker) {
						pendingReads--;
						if (pendingReads == 0)
							pending.Set ();
					}

					nextReadCalled = true;
					cnc.Close (true);
					result.SetCompleted (false, exc);
					throw;
				}

				if (nbytes < 0) {
					nbytes = 0;
					read_eof = true;
				}

				totalRead += nbytes;
				result.SetCompleted (false, nbytes + result.NBytes);
				result.DoCallback ();
				if (nbytes == 0)
					contentLength = totalRead;
			}

			lock (locker) {
				pendingReads--;
				if (pendingReads == 0)
					pending.Set ();
			}

			if (totalRead >= contentLength && !nextReadCalled)
				ReadAll ();

			int nb = result.NBytes;
			return (nb >= 0) ? nb : 0;
		}
		
		public override IAsyncResult BeginWrite (byte [] buffer, int offset, int size,
							AsyncCallback cb, object state)
		{
			if (isRead)
				throw new NotSupportedException ("this stream does not allow writing");

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			int length = buffer.Length;
			if (size < 0 || offset < 0 || length < offset || length - offset < size)
				throw new ArgumentOutOfRangeException ();

			if (sendChunked) {
				lock (locker) {
					pendingWrites++;
					pending.Reset ();
				}
			}

			WebAsyncResult result = new WebAsyncResult (cb, state);
			if (allowBuffering) {
				if (max_buffer_size >= 0) {
					long avail = max_buffer_size - writeBuffer.Length;
					if (size > avail) {
						if (requestWritten)
							throw new ProtocolViolationException (
							"The number of bytes to be written is greater than " +
							"the specified ContentLength.");
					}
				}
				writeBuffer.Write (buffer, offset, size);
				if (!sendChunked) {
					result.SetCompleted (true, 0);
					result.DoCallback ();
					return result;
				}
			}

			AsyncCallback callback = null;
			if (cb != null)
				callback = new AsyncCallback (WriteCallbackWrapper);

			if (sendChunked) {
				WriteRequest ();

				string cSize = String.Format ("{0:X}\r\n", size);
				byte [] head = Encoding.ASCII.GetBytes (cSize);
				int chunkSize = 2 + size + head.Length;
				byte [] newBuffer = new byte [chunkSize];
				Buffer.BlockCopy (head, 0, newBuffer, 0, head.Length);
				Buffer.BlockCopy (buffer, offset, newBuffer, head.Length, size);
				Buffer.BlockCopy (crlf, 0, newBuffer, head.Length + size, crlf.Length);

				buffer = newBuffer;
				offset = 0;
				size = chunkSize;
			}

			result.InnerAsyncResult = cnc.BeginWrite (buffer, offset, size, callback, result);
			return result;
		}

		public override void EndWrite (IAsyncResult r)
		{
			if (r == null)
				throw new ArgumentNullException ("r");

			WebAsyncResult result = r as WebAsyncResult;
			if (result == null)
				throw new ArgumentException ("Invalid IAsyncResult");

			if (result.EndCalled)
				return;

			result.EndCalled = true;

			if (allowBuffering && !sendChunked)
				return;

			if (result.GotException)
				throw result.Exception;

			try { 
				cnc.EndWrite (result.InnerAsyncResult);
				result.SetCompleted (false, 0);
			} catch (Exception e) {
				result.SetCompleted (false, e);
			}

			if (sendChunked) {
				lock (locker) {
					pendingWrites--;
					if (pendingWrites == 0)
						pending.Set ();
				}
			}
		}
		
		public override void Write (byte [] buffer, int offset, int size)
		{
			if (isRead)
				throw new NotSupportedException ("This stream does not allow writing");

			AsyncCallback cb = new AsyncCallback (WriteCallbackWrapper);
			WebAsyncResult res = (WebAsyncResult) BeginWrite (buffer, offset, size, cb, null);
			if (!res.IsCompleted && !res.WaitUntilComplete (request.ReadWriteTimeout, false)) {
				nextReadCalled = true;
				cnc.Close (true);
				throw new IOException ("Write timed out.");
			}

			EndWrite (res);
		}

		public override void Flush ()
		{
		}

		internal void SetHeaders (byte [] buffer, int offset, int size)
		{
			if (headersSent)
				return;

			if (!allowBuffering || sendChunked) {
				headersSent = true;
				if (!cnc.Connected)
					throw new WebException ("Not connected", null, WebExceptionStatus.SendFailure, null);

				
				if (!cnc.Write (buffer, offset, size))
					throw new WebException ("Error writing request.", null, WebExceptionStatus.SendFailure, null);

				if (!initRead) {
					initRead = true;
					WebConnection.InitRead (cnc);
				}
			} else {
				headers = new byte [size];
				Buffer.BlockCopy (buffer, offset, headers, 0, size);
			}
		}

		internal bool RequestWritten {
			get { return requestWritten; }
		}

		internal void WriteRequest ()
		{
			if (requestWritten)
				return;

			if (sendChunked) {
				request.SendRequestHeaders ();
				requestWritten = true;
				return;
			}

			if (!allowBuffering || writeBuffer == null)
				return;

			byte [] bytes = writeBuffer.GetBuffer ();
			int length = (int) writeBuffer.Length;
			if (request.ContentLength != -1 && request.ContentLength < length) {
				throw new WebException ("Specified Content-Length is less than the number of bytes to write", null,
							WebExceptionStatus.ServerProtocolViolation, null);
			}

			request.InternalContentLength = length;
			request.SendRequestHeaders ();
			requestWritten = true;
			if (!cnc.Write (headers, 0, headers.Length))
				throw new WebException ("Error writing request.", null, WebExceptionStatus.SendFailure, null);

			headersSent = true;
			if (cnc.Data.StatusCode != 0 && cnc.Data.StatusCode != 100)
				return;

			IAsyncResult result = null;
			if (length > 0)
				result = cnc.BeginWrite (bytes, 0, length, null, null);

			if (!initRead) {
				initRead = true;
				WebConnection.InitRead (cnc);
			}

			if (length > 0) 
				complete_request_written = cnc.EndWrite (result);
			else
				complete_request_written = true;
		}

		internal void InternalClose ()
		{
			disposed = true;
		}

		internal void ForceCloseConnection ()
		{
			if (!disposed) {
				disposed = true;
				cnc.Close (true);
			}
		}

		public override void Close ()
		{
			if (sendChunked) {
				pending.WaitOne ();
				byte [] chunk = Encoding.ASCII.GetBytes ("0\r\n\r\n");
				cnc.Write (chunk, 0, chunk.Length);
				return;
			}

			if (isRead) {
				if (!nextReadCalled) {
					CheckComplete ();
					// If we have not read all the contents
					if (!nextReadCalled) {
						nextReadCalled = true;
						cnc.Close (true);
					}
				}
				return;
			} else if (!allowBuffering) {
				complete_request_written = true;
				if (!initRead) {
					initRead = true;
					WebConnection.InitRead (cnc);
				}
				return;
			}

			if (disposed)
				return;

			long length = request.ContentLength;
			if (length != -1 && length > writeBuffer.Length)
				throw new IOException ("Cannot close the stream until all bytes are written");

			WriteRequest ();
			disposed = true;
		}

		public override long Seek (long a, SeekOrigin b)
		{
			throw new NotSupportedException ();
		}
		
		public override void SetLength (long a)
		{
			throw new NotSupportedException ();
		}
		
		public override bool CanSeek {
			get { return false; }
		}

		public override bool CanRead {
			get { return isRead; }
		}

		public override bool CanWrite {
			get { return !isRead; }
		}

		public override long Length {
			get { throw new NotSupportedException (); }
		}

		public override long Position {
			get { throw new NotSupportedException (); }
			set { throw new NotSupportedException (); }
		}
	}
}

