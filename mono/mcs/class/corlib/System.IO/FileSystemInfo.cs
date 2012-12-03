//CHANGED

//------------------------------------------------------------------------------
// 
// System.IO.FileSystemInfo.cs 
//
// Copyright (C) 2001 Moonlight Enterprises, All Rights Reserved
// 
// Author:         Jim Richardson, develop@wtfo-guru.com
//                 Dan Lewis (dihlewis@yahoo.co.uk)
// Created:        Monday, August 13, 2001 
//
//------------------------------------------------------------------------------

//
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


#if NOT_PFX
using System.Runtime.Serialization;
#endif

namespace System.IO {

    [Serializable]
    public abstract class FileSystemInfo
#if NOT_PFX
: MarshalByRefObject
#endif
    {
        // public properties

        public abstract bool Exists { get; }

        public abstract string Name { get; }

        public virtual string FullName
        {
            get
            {
                return FullPath;
            }
        }

        public string Extension
        {
            get
            {
                return Path.GetExtension(Name);
            }
        }

        public FileAttributes Attributes
        {
            get
            {
                Refresh(false);
                //return stat.Attributes;
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
                //MonoIOError error;
                //if (!MonoIO.SetFileAttributes (FullName,
                //                   value,
                //                   out error))
                //    throw MonoIO.GetException (FullName,
                //                   error);
                Refresh(true);
            }
        }

        public DateTime CreationTime
        {
            get
            {
                Refresh(false);
                //return DateTime.FromFileTime(stat.CreationTime);
                throw new NotImplementedException();
            }

            set
            {
                //long filetime = value.ToFileTime ();

                //MonoIOError error;
                //if (!MonoIO.SetFileTime (FullName, filetime,
                //             -1, -1, out error))
                //    throw MonoIO.GetException (FullName,
                //                   error);
                //Refresh (true);
                throw new NotImplementedException();
            }
        }

        public DateTime CreationTimeUtc
        {
            get
            {
                return CreationTime.ToUniversalTime();
            }

            set
            {
                CreationTime = value.ToLocalTime();
            }
        }

        public DateTime LastAccessTime
        {
            get
            {
                Refresh(false);
                //return DateTime.FromFileTime(stat.LastAccessTime);
                throw new NotImplementedException();
            }

            set
            {
                //long filetime = value.ToFileTime ();

                //MonoIOError error;

                //if (!MonoIO.SetFileTime (FullName, -1,
                //             filetime, -1,
                //             out error))
                //    throw MonoIO.GetException (FullName,
                //                   error);
                //Refresh (true);
                throw new NotImplementedException();
            }
        }

        public DateTime LastAccessTimeUtc
        {
            get
            {
                Refresh(false);
                return LastAccessTime.ToUniversalTime();
            }

            set
            {
                LastAccessTime = value.ToLocalTime();
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                Refresh(false);
                //return DateTime.FromFileTime(stat.LastWriteTime);
                throw new NotImplementedException();
            }

            set
            {
                //long filetime = value.ToFileTime ();

                //MonoIOError error;

                //if (!MonoIO.SetFileTime (FullName, -1, -1,
                //             filetime, out error))
                //    throw MonoIO.GetException (FullName,
                //                   error);
                //Refresh (true);
                throw new NotImplementedException();
            }
        }

        public DateTime LastWriteTimeUtc
        {
            get
            {
                Refresh(false);
                return LastWriteTime.ToUniversalTime();
            }

            set
            {
                LastWriteTime = value.ToLocalTime();
            }
        }

        // public methods

        public abstract void Delete();

        public void Refresh()
        {
            Refresh(true);
        }

        // protected

        protected FileSystemInfo()
        {
            this.valid = false;
            this.FullPath = null;
        }

#if NOT_PFX
protected FileSystemInfo(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            FullPath = info.GetString("FullPath");
            OriginalPath = info.GetString("OriginalPath");
        }
#endif

        protected string FullPath;
        protected string OriginalPath;

        // internal

        internal void Refresh(bool force)
        {
            if (valid && !force)
                return;

            //MonoIOError error;

            //TODO:
            //MonoIO.GetFileStat (FullName, out stat, out error);
            /* Don't throw on error here, too much other
             * stuff relies on it not doing so...
             */

            valid = true;

            InternalRefresh();
        }

        internal virtual void InternalRefresh()
        {
        }

        internal void CheckPath(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (path.Length == 0)
                throw new ArgumentException("path", Locale.GetText("Empty path."));

            if (path.IndexOfAny(Path.InvalidPathChars) != -1)
                throw new ArgumentException("path", Locale.GetText("Invalid characters in path."));
        }

        //internal MonoIOStat stat;
        internal bool valid;
    }
}
