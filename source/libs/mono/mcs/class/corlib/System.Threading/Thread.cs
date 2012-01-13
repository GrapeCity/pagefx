//CHANGED
//
// System.Threading.Thread.cs
//
// Author:
//   Dick Porter (dick@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004-2006 Novell, Inc (http://www.novell.com)
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

using System.Globalization;

namespace System.Threading
{
    public sealed class Thread 
    {
        public static Thread CurrentThread
        {
            get
            {

                if (_currentThread == null)
                    _currentThread = new Thread();
                return _currentThread;
            }
        }

        private static Thread _currentThread;

        private Thread()
        {
        }

        /*
         * Thread objects are shared between appdomains, and CurrentCulture
         * should always return an object in the calling appdomain. See bug
         * http://bugzilla.ximian.com/show_bug.cgi?id=50049 for more info.
         * This is hard to implement correctly and efficiently, so the current
         * implementation is not perfect: changes made in one appdomain to the 
         * state of the current cultureinfo object are not visible to other 
         * appdomains.
         */
        public CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
            set
            {
                CultureInfo.CurrentCulture = value;
            }
        }

        public CultureInfo CurrentUICulture
        {
            get
            {
                return CultureInfo.CurrentUICulture;
            }
            set
            {
                CultureInfo.CurrentUICulture = value;
            }
        }

        //csc from .NET Framework 3.5 requires this property
        public int ManagedThreadId
        {
            get { return _mid; }
        }
        private int _mid;
    }
}
