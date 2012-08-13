//CHANGED

//
// System.Exception.cs
//
// Author:
//   Miguel de Icaza (miguel@ximian.com)
//   Patrik Torstensson
//
// (C) Ximian, Inc.  http://www.ximian.com
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

//using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if NOT_PFX
using System.Runtime.Serialization;
#endif
using System.Security.Permissions;
using System.Text;
using PageFX;

//using System.Runtime.InteropServices;

namespace System
{
    [Serializable]
#if NOT_PFX
#if NET_2_0
	[ComVisible(true)]
	[ComDefaultInterface (typeof (_Exception))]
	[ClassInterface (ClassInterfaceType.None)]
#else
    [ClassInterface(ClassInterfaceType.AutoDual)]
#endif
#endif
    public class Exception
    {
        Exception inner_exception;
        string help_link;
        internal int hresult = unchecked((int)0x80004005);
        string source;
        internal string stack_trace;

        #region ctors
        public Exception()
        {
        }

        public Exception(string msg)
        {
	        InternalMessage = msg;
        }

        public Exception(string msg, Exception e)
            : this(msg)
        {
            inner_exception = e;
        }

#if NOT_PFX
        protected Exception(SerializationInfo info, StreamingContext context)
		{
            if (info == null)
                throw new ArgumentNullException("info");

            //class_name = info.GetString("ClassName");
            message = info.GetString("Message");
            help_link = info.GetString("HelpURL");
            stack_trace = info.GetString("StackTraceString");
            //remote_stack_trace = info.GetString("RemoteStackTraceString");
            //remote_stack_index = info.GetInt32("RemoteStackIndex");
            hresult = info.GetInt32("HResult");
            source = info.GetString("Source");
            inner_exception = (Exception)info.GetValue("InnerException", typeof(Exception));
        }
#endif
        #endregion


        public Exception InnerException
        {
            get { return inner_exception; }
        }

        internal virtual string HelpLink
        {
            get { return help_link; }
            set { help_link = value; }
        }

        protected int HResult
        {
            get { return hresult; }
            set { hresult = value; }
        }

        public virtual string Message
        {
            get
            {
                string msg = InternalMessage;
                if (string.IsNullOrEmpty(msg))
                {
                    msg = FormatMessage();
					InternalMessage = msg;
                }
                return msg;
            }
        }

        protected virtual string FormatMessage()
        {
            return string.Format(Locale.GetText("Exception of type {0} was thrown."), GetType());
        }

        public virtual string Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
            }
        }

		[InlineFunction("getStackTrace")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern string GetInternalStackTrace();

	    internal string InternalMessage
	    {
			[InlineProperty("message")]
		    [MethodImpl(MethodImplOptions.InternalCall)]
			get;

			[InlineProperty("message")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
	    }

        public virtual string StackTrace
        {
            get
            {
                if (string.IsNullOrEmpty(stack_trace))
                    stack_trace = GetInternalStackTrace();
                return stack_trace;
            }
        }

        public virtual Exception GetBaseException()
        {
            Exception inner = inner_exception;

            while (inner != null)
            {
                if (inner.InnerException != null)
                    inner = inner.InnerException;
                else
                    return inner;
            }

            return this;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(GetType().FullName);
            result.Append(": ").Append(Message);

            if (inner_exception != null)
            {
                result.Append(" ---> ").Append(inner_exception.ToString());
                result.Append(Locale.GetText("--- End of inner exception stack trace ---"));
                result.AppendLine();
            }

            if (StackTrace != null)
                result.AppendLine().Append(StackTrace);
            return result.ToString();
        }
        
    }
}
