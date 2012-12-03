//------------------------------------------------------------------------------
// 
// System.IO.FileInfo.cs 
//
// Copyright (C) 2001 Moonlight Enterprises, All Rights Reserved
// 
// Author:         Jim Richardson, develop@wtfo-guru.com
//                 Dan Lewis (dihlewis@yahoo.co.uk)
// Created:        Monday, August 13, 2001 
//
//------------------------------------------------------------------------------

//
// Copyright (C) 2004, 2006 Novell, Inc (http://www.novell.com)
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

#if NET_2_0
using System.Security.AccessControl;
#endif

namespace System.IO {

	[Serializable]
#if NET_2_0
	[ComVisible (true)]
#endif
	public sealed class FileInfo : FileSystemInfo {
	

		private bool exists;

		public FileInfo (string path) {
			CheckPath (path);
		
			OriginalPath = path;
			FullPath = Path.GetFullPath (path);
		}
		
		internal override void InternalRefresh ()
		{
			exists = File.Exists (FullPath);
		}


		// public properties

		public override bool Exists {
			get {
                //Refresh (false);

                //if (stat.Attributes == MonoIO.InvalidFileAttributes)
                //    return false;

                //if ((stat.Attributes & FileAttributes.Directory) != 0)
                //    return false;

                //return exists;
                throw new NotImplementedException();
			}
		}

		public override string Name {
			get {
				return Path.GetFileName (FullPath);
			}
		}

#if NET_2_0
		public bool IsReadOnly {
			get {
				if (!Exists)
					throw new FileNotFoundException ("Could not find file \"" + OriginalPath + "\".", OriginalPath);
					
				return ((stat.Attributes & FileAttributes.ReadOnly) != 0);
			}
				
			set {
				if (!Exists)
					throw new FileNotFoundException ("Could not find file \"" + OriginalPath + "\".", OriginalPath);
					
				FileAttributes attrs = File.GetAttributes(FullPath);

				if (value) 
					attrs |= FileAttributes.ReadOnly;
				else
					attrs &= ~FileAttributes.ReadOnly;					

				File.SetAttributes(FullPath, attrs);
			}
		}

		[MonoLimitation ("File encryption isn't supported (even on NTFS).")]
		[ComVisible (false)]
		public void Encrypt ()
		{
			// MS.NET support this only on NTFS file systems, i.e. it's a file-system (not a framework) feature.
			// otherwise it throws a NotSupportedException (or a PlatformNotSupportedException on older OS).
			// we throw the same (instead of a NotImplementedException) because most code should already be
			// handling this exception to work properly.
			throw new NotSupportedException (Locale.GetText ("File encryption isn't supported on any file system."));
		}

		[MonoLimitation ("File encryption isn't supported (even on NTFS).")]
		[ComVisible (false)]
		public void Decrypt ()
		{
			// MS.NET support this only on NTFS file systems, i.e. it's a file-system (not a framework) feature.
			// otherwise it throws a NotSupportedException (or a PlatformNotSupportedException on older OS).
			// we throw the same (instead of a NotImplementedException) because most code should already be
			// handling this exception to work properly.
			throw new NotSupportedException (Locale.GetText ("File encryption isn't supported on any file system."));
		}
#endif

		public long Length
        {
			get
            {
				if (!Exists)
					throw new FileNotFoundException ("Could not find file \"" + OriginalPath + "\".", OriginalPath);

				//return stat.Length;
                return 0;
			}
		}

		public string DirectoryName {
			get {
				return Path.GetDirectoryName (FullPath);
			}
		}

		public DirectoryInfo Directory {
			get {
				return new DirectoryInfo (DirectoryName);
			}
		}

		// streamreader methods

        public StreamReader OpenText()
        {
            return new StreamReader(Open(FileMode.Open, FileAccess.Read));
        }

        public StreamWriter CreateText()
        {
            return new StreamWriter(Open(FileMode.Create, FileAccess.Write));
        }

        public StreamWriter AppendText()
        {
            return new StreamWriter(Open(FileMode.Append, FileAccess.Write));
        }

		// filestream methods

        public FileStream Create()
        {
            return File.Create(FullPath);
        }


        public FileStream OpenRead()
        {
            return Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public FileStream OpenWrite()
        {
            return Open(FileMode.OpenOrCreate, FileAccess.Write);
        }

        public FileStream Open(FileMode mode)
        {
            return Open(mode, FileAccess.ReadWrite);
        }

        public FileStream Open(FileMode mode, FileAccess access)
        {
            return Open(mode, access, FileShare.None);
        }

        public FileStream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return new FileStream(FullPath, mode, access, share);
        }

		// file methods

		public override void Delete ()
        {
            //MonoIOError error;
						
            //if (!MonoIO.Exists (FullPath, out error)) {
            //    // a weird MS.NET behaviour
            //    return;
            //}

            //if (MonoIO.ExistsDirectory (FullPath, out error)) {
            //    throw new UnauthorizedAccessException ("Access to the path \"" + FullPath + "\" is denied.");
            //}
			
            //if (!MonoIO.DeleteFile (FullPath, out error)) {
            //    throw MonoIO.GetException (OriginalPath,
            //                   error);
            //}
            throw new NotImplementedException();
		}
		
		public void MoveTo (string dest) {

            //if (dest == null)
            //    throw new ArgumentNullException ();

            //            if (dest == Name || dest == FullName)
            //                    return;

            //MonoIOError error;
            //if (MonoIO.Exists (dest, out error) ||
            //    MonoIO.ExistsDirectory (dest, out error))
            //    throw new IOException ();
            //File.Move (FullPath, dest);
            //this.FullPath = Path.GetFullPath (dest);
            throw new NotImplementedException();
		}

		public FileInfo CopyTo (string path) {
			return CopyTo (path, false);
		}

		public FileInfo CopyTo (string path, bool overwrite) {
			string dest = Path.GetFullPath (path);

			if (overwrite && File.Exists (path))
				File.Delete (path);

			File.Copy (FullPath, dest);
		
			return new FileInfo (dest);
		}

		public override string ToString () {
			return OriginalPath;
		}

#if NET_2_0
		public FileSecurity GetAccessControl ()
		{
			throw new NotImplementedException ();
		}
		
		public FileSecurity GetAccessControl (AccessControlSections includeSections)
		{
			throw new NotImplementedException ();
		}

		[ComVisible (false)]
		public FileInfo Replace (string destinationFileName,
					 string destinationBackupFileName)
		{
			throw new NotImplementedException ();
		}
		
		[ComVisible (false)]
		public FileInfo Replace (string destinationFileName,
					 string destinationBackupFileName,
					 bool ignoreMetadataErrors)
		{
			throw new NotImplementedException ();
		}

		public void SetAccessControl (FileSecurity fileSecurity)
		{
			throw new NotImplementedException ();
		}
#endif
	}
}
