//
// System.OperatingSystem.cs 
//
// Author:
//   Jim Richardson (develop@wtfo-guru.com)
//
// (C) 2001 Moonlight Enterprises, All Rights Reserved
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

namespace System
{

#if NET_2_0
	[ComVisible (true)]
#endif
    [Serializable]
    internal sealed class OperatingSystem : ICloneable
    {
        private readonly PlatformID _platform;
        private readonly Version _version;

        public OperatingSystem(PlatformID platform, Version version)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }

            _platform = platform;
            _version = version;
        }

        public PlatformID Platform
        {
            get
            {
                return _platform;
            }
        }

        public Version Version
        {
            get
            {
                return _version;
            }
        }

#if NET_2_0
		public string ServicePack {
			get { return String.Empty; }
		}

		public string VersionString {
			get { return ToString (); }
		}
#endif

        public object Clone()
        {
            return new OperatingSystem(_platform, _version);
        }

        public override string ToString()
        {
            string str;

            switch ((int)_platform)
            {
                case (int)PlatformID.Win32NT:
                    str = "Microsoft Windows NT";
                    break;
                case (int)PlatformID.Win32S:
                    str = "Microsoft Win32S";
                    break;
                case (int)PlatformID.Win32Windows:
                    str = "Microsoft Windows 98";
                    break;
#if NET_1_1
                case (int)PlatformID.WinCE:
                    str = "Microsoft Windows CE";
                    break;
#endif
                case 4: /* PlatformID.Unix */
                case 128: /* reported for 1.1 mono */
                    str = "Unix";
                    break;
                default:
                    str = Locale.GetText("<unknown>");
                    break;
            }

            return str + " " + _version;
        }
    }
}
