//CHANGED

//
// System.BadImageFormatException.cs
//
// Authors:
//   Sean MacIsaac (macisaac@ximian.com)
//   Duncan Mak (duncan@ximian.com)
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//
// (C) 2001 Ximian, Inc.
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

using System.Globalization;
using System.Text;

namespace System
{
	[Serializable]
	public class BadImageFormatException : SystemException
	{
		const int Result = unchecked ((int)0x8007000B);

		// Fields
		private string fileName;
		private string fusionLog;

		// Constructors
		public BadImageFormatException ()
			: base (Locale.GetText ("Format of the executable (.exe) or library (.dll) is invalid."))
		{
			HResult = Result;
		}

		public BadImageFormatException (string message)
			: base (message)
		{
			HResult = Result;
		}

		public BadImageFormatException (string message, Exception innerException)
			: base (message, innerException)
		{
			HResult = Result;
		}

		internal BadImageFormatException (string message, string fileName)
			: base (message)
		{
			this.fileName = fileName;
			HResult = Result;
		}

		internal BadImageFormatException (string message, string fileName, Exception innerException)
			: base (message, innerException)
		{
			this.fileName = fileName;
			HResult = Result;
		}

		// Properties
#if NOT_PFX
		public override string Message
		{
			get {
				if (base.Message == null) {
#if NET_2_0
					return string.Format (CultureInfo.CurrentCulture,
						"Could not load file or assembly '{0}' or one of"
						+ " its dependencies. An attempt was made to load"
						+ " a program with an incorrect format.", fileName);
#else
					if (fileName == null) {
						return "Format of the executable (.exe) or library"
							+ " (.dll) is invalid.";
					} else {
						return string.Format (CultureInfo.CurrentCulture,
							"The format of the file '{0}' is invalid.",
							FileName);
					}
#endif
				}
				return base.Message;
			}
		}
#endif

        protected override string FormatMessage()
        {
#if NET_2_0
            return string.Format(CultureInfo.CurrentCulture,
                "Could not load file or assembly '{0}' or one of"
                + " its dependencies. An attempt was made to load"
                + " a program with an incorrect format.", fileName);
#else
					if (fileName == null) {
						return "Format of the executable (.exe) or library"
							+ " (.dll) is invalid.";
					} else {
						return string.Format (CultureInfo.CurrentCulture,
							"The format of the file '{0}' is invalid.",
							FileName);
					}
#endif
        }

		internal string FileName
		{
			get { return fileName; }
		}

		[MonoTODO ("Probably not entirely correct. fusionLog needs to be set somehow (we are probably missing internal constuctor)")]
		internal string FusionLog	
        {
			// note: MS runtime throws a SecurityException when the Exception is created
			// but a FileLoadException once the exception as been thrown. Mono always
			// throw a SecurityException in both case (anyway fusionLog is currently empty)
			get { return fusionLog; }
		}

		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder (GetType ().FullName);
			sb.AppendFormat (": {0}", Message);

			if (fileName != null && fileName.Length > 0) {
				sb.Append (Environment.NewLine);
#if NET_2_0
				sb.AppendFormat ("File name: '{0}'", fileName);
#else
				sb.AppendFormat ("File name: \"{0}\"", fileName);
#endif
			}

			if (this.InnerException != null)
				sb.AppendFormat (" ---> {0}", InnerException);

			if (this.StackTrace != null) {
				sb.Append (Environment.NewLine);
				sb.Append (StackTrace);
			}

			return sb.ToString ();
		}
	}
}
