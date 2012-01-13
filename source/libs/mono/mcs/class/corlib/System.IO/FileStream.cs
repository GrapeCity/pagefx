//CHANGED

//
// System.IO/FileStream.cs
//
// Authors:
// 	Dietmar Maurer (dietmar@ximian.com)
// 	Dan Lewis (dihlewis@yahoo.co.uk)
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// (C) 2001-2003 Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
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

using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.IO
{
	public class FileStream : Stream
    {
        #region construct from handle
#if NET_2_0_SAFEFILEHANDLE_ENABLED
		[Obsolete ("Use FileStream(SafeFileHandle handle, FileAccess access) instead")]
#endif
        public FileStream(IntPtr handle, FileAccess access)
            : this(handle, access, true, DefaultBufferSize, false) { }

#if NET_2_0_SAFEFILEHANDLE_ENABLED
		[Obsolete ("Use FileStream(SafeFileHandle handle, FileAccess access) instead")]
#endif
        public FileStream(IntPtr handle, FileAccess access, bool ownsHandle)
            : this(handle, access, ownsHandle, DefaultBufferSize, false) { }

#if NET_2_0_SAFEFILEHANDLE_ENABLED
		[Obsolete ("Use FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) instead")]
#endif
        public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
            : this(handle, access, ownsHandle, bufferSize, false) { }

#if NET_2_0_SAFEFILEHANDLE_ENABLED
		[Obsolete ("Use FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead")]
#endif
        public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
            : this(handle, access, ownsHandle, bufferSize, isAsync, false) { }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        internal FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync, bool noBuffering)
        {
            //this.handle = MonoIO.InvalidHandle;
            //if (handle == this.handle)
            //    throw new ArgumentException("handle", Locale.GetText("Invalid."));

            //if (access < FileAccess.Read || access > FileAccess.ReadWrite)
            //    throw new ArgumentOutOfRangeException("access");

            //MonoIOError error;
            //MonoFileType ftype = MonoIO.GetFileType(handle, out error);

            //if (error != MonoIOError.ERROR_SUCCESS)
            //{
            //    throw MonoIO.GetException(name, error);
            //}

            //if (ftype == MonoFileType.Unknown)
            //{
            //    throw new IOException("Invalid handle.");
            //}
            //else if (ftype == MonoFileType.Disk)
            //{
            //    this.canseek = true;
            //}
            //else
            //{
            //    this.canseek = false;
            //}

            //this.handle = handle;
            //this.access = access;
            //this.owner = ownsHandle;
            //this.async = isAsync;
            //this.anonymous = false;

            //InitBuffer(bufferSize, noBuffering);

            //if (canseek)
            //{
            //    buf_start = MonoIO.Seek(handle, 0, SeekOrigin.Current, out error);
            //    if (error != MonoIOError.ERROR_SUCCESS)
            //    {
            //        throw MonoIO.GetException(name, error);
            //    }
            //}

            ///* Can't set append mode */
            //this.append_startpos = 0;
            throw new NotSupportedException();
        }
        #endregion

        #region construct from filename
        public FileStream (string name, FileMode mode)
			: this (name, mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite), FileShare.Read, DefaultBufferSize, false, FileOptions.None)
		{
		}

		public FileStream (string name, FileMode mode, FileAccess access)
			: this (name, mode, access, access == FileAccess.Write ? FileShare.None : FileShare.Read, DefaultBufferSize, false, false)
		{
		}

		public FileStream (string name, FileMode mode, FileAccess access, FileShare share)
			: this (name, mode, access, share, DefaultBufferSize, false, FileOptions.None)
		{
		}
		
		public FileStream (string name, FileMode mode, FileAccess access, FileShare share, int bufferSize)
			: this (name, mode, access, share, bufferSize, false, FileOptions.None)
		{
		}

		public FileStream (string name, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool isAsync)
			: this (name, mode, access, share, bufferSize, isAsync, FileOptions.None)
		{
		}

#if NET_2_0
		public FileStream (string name, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
			: this (name, mode, access, share, bufferSize, false, options)
		{
		}

		public FileStream (SafeFileHandle handle, FileAccess access)
			:this(handle, access, DefaultBufferSize, false)
		{
		}
		
		public FileStream (SafeFileHandle handle, FileAccess access,
				   int bufferSize)
			:this(handle, access, bufferSize, false)
		{
		}
		
		[MonoLimitationAttribute("Need to use SafeFileHandle instead of underlying handle")]
		public FileStream (SafeFileHandle handle, FileAccess access,
				   int bufferSize, bool isAsync)
			:this (handle.DangerousGetHandle (), access, false, bufferSize, isAsync)
		{
		}

		public FileStream (string path, FileMode mode,
				   FileSystemRights rights, FileShare share,
				   int bufferSize, FileOptions options)
		{
			throw new NotImplementedException ();
		}
		
		public FileStream (string path, FileMode mode,
				   FileSystemRights rights, FileShare share,
				   int bufferSize, FileOptions options,
				   FileSecurity fileSecurity)
		{
			throw new NotImplementedException ();
		}
#endif

		internal FileStream (string name, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool isAsync, bool anonymous)
			: this (name, mode, access, share, bufferSize, anonymous, isAsync ? FileOptions.Asynchronous : FileOptions.None)
		{
		}

		internal FileStream (string name, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool anonymous, FileOptions options)
		{
			if (name == null) {
				throw new ArgumentNullException ("name");
			}
			
			if (name.Length == 0) {
				throw new ArgumentException ("Name is empty");
			}

#if NET_2_0
			// ignore the Inheritable flag
			share &= ~FileShare.Inheritable;

#endif

			if (bufferSize <= 0)
				throw new ArgumentOutOfRangeException ("bufferSize", "Positive number required.");

			if (mode < FileMode.CreateNew || mode > FileMode.Append)
				throw new ArgumentOutOfRangeException ("mode", "Enum value was out of legal range.");

			if (access < FileAccess.Read || access > FileAccess.ReadWrite)
				throw new ArgumentOutOfRangeException ("access", "Enum value was out of legal range.");

#if NET_2_0
			if (share < FileShare.None || share > (FileShare.ReadWrite | FileShare.Delete))
#else
			if (share < FileShare.None || share > FileShare.ReadWrite)
#endif
				throw new ArgumentOutOfRangeException ("share", "Enum value was out of legal range.");

			if (name.IndexOfAny (Path.InvalidPathChars) != -1) {
				throw new ArgumentException ("Name has invalid chars");
			}

			if (Directory.Exists (name)) {
				// don't leak the path information for isolated storage
				string msg = Locale.GetText ("Access to the path '{0}' is denied.");
				string fname = (anonymous) ? Path.GetFileName (name) : Path.GetFullPath (name);
				throw new UnauthorizedAccessException (String.Format (msg, fname));
			}

			/* Append streams can't be read (see FileMode
			 * docs)
			 */
			if (mode==FileMode.Append &&
				(access&FileAccess.Read)==FileAccess.Read) {
				throw new ArgumentException("Append access can be requested only in write-only mode.");
			}

			if ((access & FileAccess.Write) == 0 &&
				(mode != FileMode.Open && mode != FileMode.OpenOrCreate)) {
				string msg = Locale.GetText ("Combining FileMode: {0} with " +
					"FileAccess: {1} is invalid.");
				throw new ArgumentException (string.Format (msg, access, mode));
			}

			string dname = Path.GetDirectoryName (name);
			if (dname.Length > 0) {
				string fp = Path.GetFullPath (dname);
				if (!Directory.Exists (fp)) {
					// don't leak the path information for isolated storage
					string msg = Locale.GetText ("Could not find a part of the path \"{0}\".");
					string fname = (anonymous) ? dname : Path.GetFullPath (name);
					throw new DirectoryNotFoundException (String.Format (msg, fname));
				}
			}

			if (access == FileAccess.Read && mode != FileMode.Create && mode != FileMode.OpenOrCreate &&
					mode != FileMode.CreateNew && !File.Exists (name)) {
				// don't leak the path information for isolated storage
				string msg = Locale.GetText ("Could not find file \"{0}\".");
				string fname = (anonymous) ? Path.GetFileName (name) : Path.GetFullPath (name);
				throw new FileNotFoundException (String.Format (msg, fname), fname);
			}

			// IsolatedStorage needs to keep the Name property to the default "[Unknown]"
			if (!anonymous)
				this.name = name;

			// TODO: demand permissions

            throw new NotImplementedException();
            //MonoIOError error;

            //this.handle = MonoIO.Open (name, mode, access, share, options, out error);
            //if (handle == MonoIO.InvalidHandle) {
            //    // don't leak the path information for isolated storage
            //    string fname = (anonymous) ? Path.GetFileName (name) : Path.GetFullPath (name);
            //    throw MonoIO.GetException (fname, error);
            //}

            //this.access = access;
            //this.owner = true;
            //this.anonymous = anonymous;

            ///* Can we open non-files by name? */
			
            //if (MonoIO.GetFileType (handle, out error) == MonoFileType.Disk) {
            //    this.canseek = true;
            //    this.async = (options & FileOptions.Asynchronous) != 0;
            //} else {
            //    this.canseek = false;
            //    this.async = false;
            //}


            //if (access == FileAccess.Read && canseek && (bufferSize == DefaultBufferSize)) {
            //    /* Avoid allocating a large buffer for small files */
            //    long len = Length;
            //    if (bufferSize > len) {
            //        bufferSize = (int)(len < 1000 ? 1000 : len);
            //    }
            //}

            //InitBuffer (bufferSize, false);

            //if (mode==FileMode.Append) {
            //    this.Seek (0, SeekOrigin.End);
            //    this.append_startpos=this.Position;
            //} else {
            //    this.append_startpos=0;
            //}
        }
        #endregion

        // properties
		
		public override bool CanRead {
			get {
				return access == FileAccess.Read ||
				       access == FileAccess.ReadWrite;
			}
		}

                public override bool CanWrite {
                        get {
				return access == FileAccess.Write ||
				       access == FileAccess.ReadWrite;
                        }
                }
		
		public override bool CanSeek {
                        get {
                                return(canseek);
                        }
                }

		public virtual bool IsAsync {
			get {
				return (async);
			}
		}

		public string Name {
			get {
				return name; 
			}
		}

		public override long Length {
			get {
                //if (handle == MonoIO.InvalidHandle)
                //    throw new ObjectDisposedException ("Stream has been closed");

                //if (!CanSeek)
                //    throw new NotSupportedException ("The stream does not support seeking");

                //// Buffered data might change the length of the stream
                //FlushBufferIfDirty ();

                //MonoIOError error;
                //long length;
				
                //length = MonoIO.GetLength (handle, out error);
                //if (error != MonoIOError.ERROR_SUCCESS) {
                //    // don't leak the path information for isolated storage
                //    string fname = (anonymous) ? Path.GetFileName (name) : name;
                //    throw MonoIO.GetException (fname, error);
                //}

                //return(length);
                throw new NotImplementedException();
			}
		}

		public override long Position {
			get {
                //if (handle == MonoIO.InvalidHandle)
                //    throw new ObjectDisposedException ("Stream has been closed");

                //if(CanSeek == false)
                //    throw new NotSupportedException("The stream does not support seeking");
				
                //return(buf_start + buf_offset);
                throw new NotImplementedException();
			}
			set {
                //if (handle == MonoIO.InvalidHandle)
                //    throw new ObjectDisposedException ("Stream has been closed");

                //if(CanSeek == false) {
                //    throw new NotSupportedException("The stream does not support seeking");
                //}

                //if(value < 0) {
                //    throw new ArgumentOutOfRangeException("Attempt to set the position to a negative value");
                //}
				
                //Seek (value, SeekOrigin.Begin);
                throw new NotImplementedException();
			}
		}

#if NET_2_0_SAFEFILEHANDLE_ENABLED
		[Obsolete ("Use SafeFileHandle instead")]
#endif
		public virtual IntPtr Handle {
			//[SecurityPermission (SecurityAction.LinkDemand, UnmanagedCode = true)]
			//[SecurityPermission (SecurityAction.InheritanceDemand, UnmanagedCode = true)]
			get {
				return handle;
			}
		}

#if NET_2_0
		public virtual SafeFileHandle SafeFileHandle {
			[SecurityPermission (SecurityAction.LinkDemand, UnmanagedCode = true)]
			[SecurityPermission (SecurityAction.InheritanceDemand, UnmanagedCode = true)]
			get { throw new NotImplementedException (); }
		}
#endif

		// methods

		public override int ReadByte ()
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");

            //if (!CanRead)
            //    throw new NotSupportedException ("Stream does not support reading");
			
            //if (buf_size == 0) {
            //    int n = ReadData (handle, buf, 0, 1);
            //    if (n == 0) return -1;
            //    else return buf[0];
            //}
            //else if (buf_offset >= buf_length) {
            //    RefillBuffer ();

            //    if (buf_length == 0)
            //        return -1;
            //}
			
            //return buf [buf_offset ++];
            throw new NotImplementedException();
		}

		public override void WriteByte (byte value)
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");

            //if (!CanWrite)
            //    throw new NotSupportedException ("Stream does not support writing");

            //if (buf_offset == buf_size)
            //    FlushBuffer ();

            //if (buf_size == 0) { // No buffering
            //    buf [0] = value;
            //    buf_dirty = true;
            //    buf_length = 1;
            //    FlushBuffer ();
            //    return;
            //}

            //buf [buf_offset ++] = value;
            //if (buf_offset > buf_length)
            //    buf_length = buf_offset;

            //buf_dirty = true;
            throw new NotImplementedException();
		}

		public override int Read ([In,Out] byte[] dest, int dest_offset, int count)
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");
            //if (dest == null)
            //    throw new ArgumentNullException ("destFile");
            //if (!CanRead)
            //    throw new NotSupportedException ("Stream does not support reading");
            //int len = dest.Length;
            //if (dest_offset < 0)
            //    throw new ArgumentOutOfRangeException ("dest_offset", "< 0");
            //if (count < 0)
            //    throw new ArgumentOutOfRangeException ("count", "< 0");
            //if (dest_offset > len)
            //    throw new ArgumentException ("destination offset is beyond array size");
            //// reordered to avoid possible integer overflow
            //if (dest_offset > len - count)
            //    throw new ArgumentException ("Reading would overrun buffer");

            //if (async) {
            //    IAsyncResult ares = BeginRead (dest, dest_offset, count, null, null);
            //    return EndRead (ares);
            //}

            //return ReadInternal (dest, dest_offset, count);
            throw new NotImplementedException();
		}

		int ReadInternal (byte [] dest, int dest_offset, int count)
		{
			int copied = 0;

			int n = ReadSegment (dest, dest_offset, count);
			copied += n;
			count -= n;
			
			if (count == 0) {
				/* If there was already enough
				 * buffered, no need to read
				 * more from the file.
				 */
				return (copied);
			}

			if (count > buf_size) {
				/* Read as much as we can, up
				 * to count bytes
				 */
				FlushBuffer();
				n = ReadData (handle, dest,
					      dest_offset+copied,
					      count);
			
				/* Make the next buffer read
				 * start from the right place
				 */
				buf_start += n;
			} else {
				RefillBuffer ();
				n = ReadSegment (dest,
						 dest_offset+copied,
						 count);
			}

			copied += n;

			return copied;
		}

		delegate int ReadDelegate (byte [] buffer, int offset, int count);

		public override IAsyncResult BeginRead (byte [] buffer, int offset, int count,
							AsyncCallback cback, object state)
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");

			if (!CanRead)
				throw new NotSupportedException ("This stream does not support reading");

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (count < 0)
				throw new ArgumentOutOfRangeException ("count", "Must be >= 0");

			if (offset < 0)
				throw new ArgumentOutOfRangeException ("offset", "Must be >= 0");

			// reordered to avoid possible integer overflow
			if (count > buffer.Length - offset)
				throw new ArgumentException ("Buffer too small. count/offset wrong.");

			if (!async)
				return base.BeginRead (buffer, offset, count, cback, state);

            //ReadDelegate r = new ReadDelegate (ReadInternal);
            //return r.BeginInvoke (buffer, offset, count, cback, state);			
            throw new NotImplementedException();
		}
		
		public override int EndRead (IAsyncResult async_result)
		{
            //if (async_result == null)
            //    throw new ArgumentNullException ("async_result");

            //if (!async)
            //    return base.EndRead (async_result);

            //AsyncResult ares = async_result as AsyncResult;
            //if (ares == null)
            //    throw new ArgumentException ("Invalid IAsyncResult", "async_result");

            //ReadDelegate r = ares.AsyncDelegate as ReadDelegate;
            //if (r == null)
            //    throw new ArgumentException ("Invalid IAsyncResult", "async_result");

            //return r.EndInvoke (async_result);
            throw new NotImplementedException();
		}

		public override void Write (byte[] src, int src_offset, int count)
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");

			if (src == null)
				throw new ArgumentNullException ("src");
			if (src_offset < 0)
				throw new ArgumentOutOfRangeException ("src_offset", "< 0");
			if (count < 0)
				throw new ArgumentOutOfRangeException ("count", "< 0");
			// ordered to avoid possible integer overflow
			if (src_offset > src.Length - count)
				throw new ArgumentException ("Reading would overrun buffer");
			if (!CanWrite)
				throw new NotSupportedException ("Stream does not support writing");

			if (async) {
				IAsyncResult ares = BeginWrite (src, src_offset, count, null, null);
				EndWrite (ares);
				return;
			}

			WriteInternal (src, src_offset, count);
		}

		void WriteInternal (byte [] src, int src_offset, int count)
		{
            //if (count > buf_size) {
            //    // shortcut for long writes
            //    MonoIOError error;

            //    FlushBuffer ();

            //    MonoIO.Write(handle, src, src_offset, count, out error);
            //    if (error != MonoIOError.ERROR_SUCCESS)
            //    {
            //        // don't leak the path information for isolated storage
            //        string fname = (anonymous) ? Path.GetFileName(name) : name;
            //        throw MonoIO.GetException(fname, error);
            //    }
				
            //    buf_start += count;
            //} else {

            //    int copied = 0;
            //    while (count > 0) {
					
            //        int n = WriteSegment (src, src_offset + copied, count);
            //        copied += n;
            //        count -= n;

            //        if (count == 0) {
            //            break;
            //        }

            //        FlushBuffer ();
            //    }
            //}
            throw new NotImplementedException();
		}

		delegate void WriteDelegate (byte [] buffer, int offset, int count);

		public override IAsyncResult BeginWrite (byte [] buffer, int offset, int count,
							AsyncCallback cback, object state)
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");

			if (!CanWrite)
				throw new NotSupportedException ("This stream does not support writing");

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (count < 0)
				throw new ArgumentOutOfRangeException ("count", "Must be >= 0");

			if (offset < 0)
				throw new ArgumentOutOfRangeException ("offset", "Must be >= 0");

			// reordered to avoid possible integer overflow
			if (count > buffer.Length - offset)
				throw new ArgumentException ("Buffer too small. count/offset wrong.");

			if (!async)
				return base.BeginWrite (buffer, offset, count, cback, state);

			FileStreamAsyncResult result = new FileStreamAsyncResult (cback, state);
			result.BytesRead = -1;
			result.Count = count;
			result.OriginalCount = count;

			if (buf_dirty) {
				MemoryStream ms = new MemoryStream ();
				FlushBufferToStream (ms);
				ms.Write (buffer, offset, count);
				offset = 0;
				count = (int) ms.Length;
			}

			WriteDelegate w = new WriteDelegate (WriteInternal);
			return w.BeginInvoke (buffer, offset, count, cback, state);			
		}
		
		public override void EndWrite (IAsyncResult async_result)
		{
            //if (async_result == null)
            //    throw new ArgumentNullException ("async_result");

            //if (!async) {
            //    base.EndWrite (async_result);
            //    return;
            //}

            //AsyncResult ares = async_result as AsyncResult;
            //if (ares == null)
            //    throw new ArgumentException ("Invalid IAsyncResult", "async_result");

            //WriteDelegate w = ares.AsyncDelegate as WriteDelegate;
            //if (w == null)
            //    throw new ArgumentException ("Invalid IAsyncResult", "async_result");

            //w.EndInvoke (async_result);
            //return;
            throw new NotImplementedException();
		}

		public override long Seek (long offset, SeekOrigin origin)
		{
			long pos;

            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");
			
			// make absolute

			if(CanSeek == false) {
				throw new NotSupportedException("The stream does not support seeking");
			}

			switch (origin) {
			case SeekOrigin.End:
				pos = Length + offset;
				break;

			case SeekOrigin.Current:
				pos = Position + offset;
				break;

			case SeekOrigin.Begin:
				pos = offset;
				break;

			default:
				throw new ArgumentException ("origin", "Invalid SeekOrigin");
			}

			if (pos < 0) {
				/* LAMESPEC: shouldn't this be
				 * ArgumentOutOfRangeException?
				 */
				throw new IOException("Attempted to Seek before the beginning of the stream");
			}

			if(pos < this.append_startpos) {
				/* More undocumented crap */
				throw new IOException("Can't seek back over pre-existing data in append mode");
			}

			FlushBuffer ();

            //MonoIOError error;
		
            //buf_start = MonoIO.Seek (handle, pos,
            //             SeekOrigin.Begin,
            //             out error);

            //if (error != MonoIOError.ERROR_SUCCESS) {
            //    // don't leak the path information for isolated storage
            //    string fname = (anonymous) ? Path.GetFileName (name) : name;
            //    throw MonoIO.GetException (fname, error);
            //}
            throw new NotImplementedException();
			
			return(buf_start);
		}

		public override void SetLength (long length)
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");

			if(CanSeek == false)
				throw new NotSupportedException("The stream does not support seeking");

			if(CanWrite == false)
				throw new NotSupportedException("The stream does not support writing");

			if(length < 0)
				throw new ArgumentOutOfRangeException("Length is less than 0");
			
			Flush ();

            //MonoIOError error;
			
            //MonoIO.SetLength (handle, length, out error);
            //if (error != MonoIOError.ERROR_SUCCESS) {
            //    // don't leak the path information for isolated storage
            //    string fname = (anonymous) ? Path.GetFileName (name) : name;
            //    throw MonoIO.GetException (fname, error);
            //}
            throw new NotImplementedException();

			if (Position > length)
				Position = length;
		}

		public override void Flush ()
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");

			FlushBuffer ();
			
			// The flushing is not actually required, in
			//the mono runtime we were mapping flush to
			//`fsync' which is not the same.
			//
			//MonoIO.Flush (handle);
		}

		public override void Close ()
		{
			Dispose (true);
			//GC.SuppressFinalize (this);	// remove from finalize queue
		}

		public virtual void Lock (long position, long length)
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");
			if (position < 0) {
				throw new ArgumentOutOfRangeException ("position must not be negative");
			}
			if (length < 0) {
				throw new ArgumentOutOfRangeException ("length must not be negative");
			}
            //if (handle == MonoIO.InvalidHandle) {
            //    throw new ObjectDisposedException ("Stream has been closed");
            //}
				
            //MonoIOError error;
            //MonoIO.Lock (handle, position, length, out error);
            //if (error != MonoIOError.ERROR_SUCCESS) {
            //    // don't leak the path information for isolated storage
            //    string fname = (anonymous) ? Path.GetFileName (name) : name;
            //    throw MonoIO.GetException (fname, error);
            //}
            throw new NotImplementedException();
		}

		public virtual void Unlock (long position, long length)
		{
            //if (handle == MonoIO.InvalidHandle)
            //    throw new ObjectDisposedException ("Stream has been closed");

			if (position < 0) {
				throw new ArgumentOutOfRangeException ("position must not be negative");
			}
			if (length < 0) {
				throw new ArgumentOutOfRangeException ("length must not be negative");
			}
				
            //MonoIOError error;

            //MonoIO.Unlock (handle, position, length, out error);
            //if (error != MonoIOError.ERROR_SUCCESS) {
            //    // don't leak the path information for isolated storage
            //    string fname = (anonymous) ? Path.GetFileName (name) : name;
            //    throw MonoIO.GetException (fname, error);
            //}
            throw new NotImplementedException();
		}

		// protected

		~FileStream ()
		{
			Dispose (false);
		}

#if NET_2_0
		protected override void Dispose (bool disposing)
#else
		protected virtual void Dispose (bool disposing)
#endif
		{
            //if (handle != MonoIO.InvalidHandle) 
            //{
            //    FlushBuffer ();

            //    if (owner) {
            //        MonoIOError error;
				
            //        MonoIO.Close (handle, out error);
            //        if (error != MonoIOError.ERROR_SUCCESS) {
            //            // don't leak the path information for isolated storage
            //            string fname = (anonymous) ? Path.GetFileName (name) : name;
            //            throw MonoIO.GetException (fname, error);
            //        }

            //        handle = MonoIO.InvalidHandle;
            //    }
            //}
            throw new NotImplementedException();

			canseek = false;
			access = 0;
			if (disposing) {
				buf = null;
			}
		}

#if NET_2_0
		public FileSecurity GetAccessControl ()
		{
			throw new NotImplementedException ();
		}
		
		public void SetAccessControl (FileSecurity fileSecurity)
		{
			throw new NotImplementedException ();
		}
#endif

		// private.

		// ReadSegment, WriteSegment, FlushBuffer,
		// RefillBuffer and ReadData should only be called
		// when the Monitor lock is held, but these methods
		// grab it again just to be safe.

		private int ReadSegment (byte [] dest, int dest_offset, int count)
		{
			if (count > buf_length - buf_offset) {
				count = buf_length - buf_offset;
			}
			
			if (count > 0) {
				Buffer.BlockCopy (buf, buf_offset,
						  dest, dest_offset,
						  count);
				buf_offset += count;
			}
			
			return(count);
		}

		private int WriteSegment (byte [] src, int src_offset,
					  int count)
		{
			if (count > buf_size - buf_offset) {
				count = buf_size - buf_offset;
			}
			
			if (count > 0) {
				Buffer.BlockCopy (src, src_offset,
						  buf, buf_offset,
						  count);
				buf_offset += count;
				if (buf_offset > buf_length) {
					buf_length = buf_offset;
				}
				
				buf_dirty = true;
			}
			
			return(count);
		}

		void FlushBufferToStream (Stream st)
		{
			if (buf_dirty) {
				if (CanSeek == true) {
                    //MonoIOError error;
                    //MonoIO.Seek (handle, buf_start,
                    //         SeekOrigin.Begin,
                    //         out error);
                    //if (error != MonoIOError.ERROR_SUCCESS) {
                    //    // don't leak the path information for isolated storage
                    //    string fname = (anonymous) ? Path.GetFileName (name) : name;
                    //    throw MonoIO.GetException (fname, error);
                    //}
                    throw new NotImplementedException();
				}
				st.Write (buf, 0, buf_length);
			}

			buf_start += buf_offset;
			buf_offset = buf_length = 0;
			buf_dirty = false;
		}

		private void FlushBuffer ()
		{
            //if (buf_dirty) {
            //    MonoIOError error;
				
            //    if (CanSeek == true) {
            //        MonoIO.Seek(handle, buf_start,
            //                 SeekOrigin.Begin,
            //                 out error);
            //        if (error != MonoIOError.ERROR_SUCCESS)
            //        {
            //            // don't leak the path information for isolated storage
            //            string fname = (anonymous) ? Path.GetFileName(name) : name;
            //            throw MonoIO.GetException(fname, error);
            //        }
            //    }
            //    MonoIO.Write(handle, buf, 0,
            //              buf_length, out error);

            //    if (error != MonoIOError.ERROR_SUCCESS)
            //    {
            //        // don't leak the path information for isolated storage
            //        string fname = (anonymous) ? Path.GetFileName(name) : name;
            //        throw MonoIO.GetException(fname, error);
            //    }
            //}

            //buf_start += buf_offset;
            //buf_offset = buf_length = 0;
            //buf_dirty = false;
            throw new NotImplementedException();
		}

		private void FlushBufferIfDirty ()
		{
			if (buf_dirty)
				FlushBuffer ();
		}

		private void RefillBuffer ()
		{
			FlushBuffer();
			
			buf_length = ReadData (handle, buf, 0,
					       buf_size);
		}

		private int ReadData (IntPtr handle, byte[] buf, int offset,
				      int count)
		{
            //MonoIOError error;
            //int amount = 0;

            ///* when async == true, if we get here we don't suport AIO or it's disabled
            // * and we're using the threadpool */
            //amount = MonoIO.Read (handle, buf, offset, count, out error);
            //if (error == MonoIOError.ERROR_BROKEN_PIPE) {
            //    amount = 0; // might not be needed, but well...
            //} else if (error != MonoIOError.ERROR_SUCCESS) {
            //    // don't leak the path information for isolated storage
            //    string fname = (anonymous) ? Path.GetFileName (name) : name;
            //    throw MonoIO.GetException (fname, error);
            //}
			
            ///* Check for read error */
            //if(amount == -1) {
            //    throw new IOException ();
            //}
			
            //return(amount);
            throw new NotImplementedException();
		}
				
		private void InitBuffer (int size, bool noBuffering)
		{
			if (noBuffering) {
				size = 0;
				// We need a buffer for the ReadByte method. This buffer won't
				// be used for anything else since buf_size==0.
				buf = new byte [1];
			}
			else {
				if (size <= 0)
					throw new ArgumentOutOfRangeException ("bufferSize", "Positive number required.");
				if (size < 8)
					size = 8;
				buf = new byte [size];
			}
					
			buf_size = size;
			buf_start = 0;
			buf_offset = buf_length = 0;
			buf_dirty = false;
		}

		// fields

		internal const int DefaultBufferSize = 8192;

		private FileAccess access;
		private bool owner;
		private bool async;
		private bool canseek;
		private long append_startpos;
		private bool anonymous;

		private byte [] buf;			// the buffer
		private int buf_size;			// capacity in bytes
		private int buf_length;			// number of valid bytes in buffer
		private int buf_offset;			// position of next byte
		private bool buf_dirty;			// true if buffer has been written to
		private long buf_start;			// location of buffer in file
		private string name = "[Unknown]";	// name of file.

		IntPtr handle;				// handle to underlying file
	}
}

