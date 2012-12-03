//
// System.Configuration.ConfigurationException.cs
//
// Author:
//   Duncan Mak (duncan@ximian.com)
//
// (C) Ximian, Inc.  http://www.ximian.com
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

using System;
using System.Globalization;
using System.Runtime.Serialization;

#if (XML_DEP)
using System.Xml;
#endif

namespace System.Configuration 
{
	[Serializable]
	public class ConfigurationException : SystemException
	{
		// Fields		
		string bareMessage;
		string filename;
		int line;

		//
		// Constructors
		//
#if NET_2_0
		[Obsolete ("This class is obsolete.  Use System.Configuration.ConfigurationErrorsException")]
#endif
		public ConfigurationException ()
			: base (Locale.GetText ("There is an error in a configuration setting."))
		{
			filename = null;
			bareMessage = Locale.GetText ("There is an error in a configuration setting.");
			line = 0;
		}

#if NET_2_0
		[Obsolete ("This class is obsolete.  Use System.Configuration.ConfigurationErrorsException")]
#endif
		public ConfigurationException (string message)
			: base (message)
		{
			bareMessage = message;
		}

		protected ConfigurationException (SerializationInfo info, StreamingContext context)
			: base (info, context)
		{
			filename = info.GetString ("filename");
			line = info.GetInt32 ("line");
		}

#if NET_2_0
		[Obsolete ("This class is obsolete.  Use System.Configuration.ConfigurationErrorsException")]
#endif
		public ConfigurationException (string message, Exception inner)
			: base (message, inner)
		{
			bareMessage = message;
		}

#if (XML_DEP)
#if NET_2_0
		[Obsolete ("This class is obsolete.  Use System.Configuration.ConfigurationErrorsException")]
#endif
		public ConfigurationException (string message, XmlNode node)
			: base (message)
		{
			filename = GetXmlNodeFilename(node);
			line = GetXmlNodeLineNumber(node);
			bareMessage = message;
		}

#if NET_2_0
		[Obsolete ("This class is obsolete.  Use System.Configuration.ConfigurationErrorsException")]
#endif
		public ConfigurationException (string message, Exception inner, XmlNode node)
			: base (message, inner)
		{
			filename = GetXmlNodeFilename (node);
			line = GetXmlNodeLineNumber (node);
			bareMessage = message;
		}
#endif
#if NET_2_0
		[Obsolete ("This class is obsolete.  Use System.Configuration.ConfigurationErrorsException")]
#endif
		public ConfigurationException (string message, string filename, int line)
			: base (message)
		{
			bareMessage = message;
			this.filename = filename;
			this.line= line;
		}

#if NET_2_0
		[Obsolete ("This class is obsolete.  Use System.Configuration.ConfigurationErrorsException")]
#endif
		public ConfigurationException (string message, Exception inner, string filename, int line)
			: base (message)
		{
			bareMessage = message;
			this.filename = filename;
			this.line = line;
		}
		//
		// Properties
		//
		public
#if NET_2_0
		virtual
#endif
		string BareMessage
		{
			get  { return bareMessage; }
		}

		public
#if NET_2_0
		virtual
#endif
		string Filename
		{
			get { return filename; }
		}

		public	
#if NET_2_0
		virtual
#endif
		int Line
		{
			get { return line; }
		}

		public override string Message
		{
			get {
				string baseMsg = base.Message;
				string f = (filename == null) ? String.Empty : filename;
				string l = (line == 0) ? String.Empty : (" line " + line);

				return baseMsg + " (" + f + l + ")";
			}
		}

		//
		// Methods
		//
#if (XML_DEP)
#if NET_2_0
		[Obsolete ("This class is obsolete.  Use System.Configuration.ConfigurationErrorsException")]
#endif
		public static string GetXmlNodeFilename (XmlNode node)
		{
			if (!(node is IConfigXmlNode))
				return String.Empty;

			return ((IConfigXmlNode) node).Filename;
		}

#if NET_2_0
		[Obsolete ("This class is obsolete.  Use System.Configuration.ConfigurationErrorsException")]
#endif
		public static int GetXmlNodeLineNumber (XmlNode node)
		{
			if (!(node is IConfigXmlNode))
				return 0;

			return ((IConfigXmlNode) node).LineNumber;
		}
#endif
		public override void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);
			info.AddValue ("filename", filename);
			info.AddValue ("line", line);
		}
	}
}
