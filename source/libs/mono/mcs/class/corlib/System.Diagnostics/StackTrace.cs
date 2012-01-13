//
// System.Diagnostics.StackTrace.cs
//
// Author:
//      Alexander Klyubin (klyubin@aqris.com)
//      Dietmar Maurer (dietmar@ximian.com)
//
// (C) 2001
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Diagnostics {

	[Serializable]
#if NET_2_0
	[ComVisible (true)]
#endif
	[MonoTODO ("Serialized objects are not compatible with .NET")]
	public class StackTrace {

		public const int METHODS_TO_SKIP = 0;

		private StackFrame[] frames;
		private bool debug_info;

		public StackTrace ()
		{
			init_frames (METHODS_TO_SKIP, false);
		}

		public StackTrace (bool needFileInfo)
		{
			init_frames (METHODS_TO_SKIP, needFileInfo);
		}

		public StackTrace (int skipFrames)
		{
			init_frames (skipFrames, false);
		}

		public StackTrace (int skipFrames, bool needFileInfo)
		{
			init_frames (skipFrames, needFileInfo);
		}

		void init_frames (int skipFrames, bool needFileInfo)
		{
			if (skipFrames < 0)
				throw new ArgumentOutOfRangeException ("< 0", "skipFrames");

			StackFrame sf;
			ArrayList al = new ArrayList ();

			skipFrames += 2;
			
			while ((sf = new StackFrame (skipFrames, needFileInfo)) != null &&
			       sf.GetMethod () != null) {
				
				al.Add (sf);
				skipFrames++;
			};

			debug_info = needFileInfo;
			frames = (StackFrame [])al.ToArray (typeof (StackFrame));	
		}
		
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern static StackFrame [] get_trace (Exception e, int skipFrames, bool needFileInfo);

		public StackTrace (Exception e)
			: this (e, METHODS_TO_SKIP, false)
		{
		}

		public StackTrace (Exception e, bool needFileInfo)
			: this (e, METHODS_TO_SKIP, needFileInfo)
		{
		}

		public StackTrace (Exception e, int skipFrames)
			: this (e, skipFrames, false)
		{
		}

		public StackTrace (Exception e, int skipFrames, bool needFileInfo)
			: this (e, skipFrames, needFileInfo, false)
		{
		}

		internal StackTrace (Exception e, int skipFrames, bool needFileInfo, bool returnNativeFrames)
		{
			if (e == null)
				throw new ArgumentNullException ("e");
			if (skipFrames < 0)
				throw new ArgumentOutOfRangeException ("< 0", "skipFrames");

			frames = get_trace (e, skipFrames, needFileInfo);

			if (!returnNativeFrames) {
				bool resize = false;
				for (int i = 0; i < frames.Length; ++i)
					if (frames [i].GetMethod () == null)
						resize = true;

				if (resize) {
					ArrayList al = new ArrayList ();

					for (int i = 0; i < frames.Length; ++i)
						if (frames [i].GetMethod () != null)
							al.Add (frames [i]);

					frames = (StackFrame [])al.ToArray (typeof (StackFrame));
				}
			}
		}

#if ONLY_1_1
		[ReflectionPermission (SecurityAction.Demand, TypeInformation = true)]
#endif
		public StackTrace (StackFrame frame)
		{
			this.frames = new StackFrame [1];
			this.frames [0] = frame;
		}

#if ONLY_1_1
		[ReflectionPermission (SecurityAction.Demand, TypeInformation = true)]
#endif
		[MonoTODO ("Not possible to create StackTraces from other threads")]
		public StackTrace (Thread targetThread, bool needFileInfo)
		{
			throw new NotImplementedException ();
		}

		public virtual int FrameCount {
			get {
				return (frames == null) ? 0 : frames.Length;
			}
		}

		public virtual StackFrame GetFrame (int index)
		{
			if ((index < 0) || (index >= FrameCount)) {
				return null;
			}

			return frames [index];
		}

#if NET_2_0
		[ComVisibleAttribute (false)]
		public virtual
#else
		// used for CAS implementation (before Fx 2.0)
		internal
#endif
		StackFrame[] GetFrames ()
		{
			return frames;
		}

		public override string ToString ()
		{
			string newline = String.Format ("{0}   {1} ", Environment.NewLine, Locale.GetText ("at"));
			string unknown = Locale.GetText ("<unknown method>");
			string debuginfo = Locale.GetText (" in {0}:line {1}");
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < FrameCount; i++) {
				StackFrame frame = GetFrame (i);
				if (i > 0)
					sb.Append (newline);
				else
					sb.AppendFormat ("   {0} ", Locale.GetText ("at"));
				MethodBase method = frame.GetMethod ();
				if (method != null) {
					// Method information available
					sb.AppendFormat ("{0}.{1}", method.DeclaringType.FullName, method.Name);
					/* Append parameter information */
					sb.Append ("(");
					ParameterInfo[] p = method.GetParameters ();
					for (int j = 0; j < p.Length; ++j) {
						if (j > 0)
							sb.Append (", ");
						Type pt = p[j].ParameterType;
						bool byref = pt.IsByRef;
						if (byref)
							pt = pt.GetElementType ();
						if (pt.IsClass && pt.Namespace != String.Empty) {
							sb.Append (pt.Namespace);
							sb.Append (".");
						}
						sb.Append (pt.Name);
						if (byref)
							sb.Append (" ByRef");
						sb.AppendFormat (" {0}", p [j].Name);
					}
					sb.Append (")");
				}
				else {
					// Method information not available
					sb.Append (unknown);
				}

				if (debug_info) {
					// we were asked for debugging informations
					try {
						// but that doesn't mean we have the debug information available
						string fname = frame.GetFileName ();
						if ((fname != null) && (fname.Length > 0))
							sb.AppendFormat (debuginfo, fname, frame.GetFileLineNumber ());
					}
					catch (SecurityException) {
						// don't leak information (about the filename) if the security 
						// manager doesn't allow it (but don't loop on this exception)
						debug_info = false;
					}
				}
			}
			return sb.ToString ();
		}
	}
}
