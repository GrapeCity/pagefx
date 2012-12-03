//CHANGED

//
// System.AppDomain.cs
//
// Author:
//   Duco Fijma (duco@lorentz.xs4all.nl)
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

using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
#if NET_2_0
	[ComVisible (true)]
#endif
	[CLSCompliant (false)]
	[Guid ("05F696DC-2B29-3663-AD8B-C4389CF2A713")]
	public interface _AppDomain
	{
		string BaseDirectory {get; }
		string DynamicDirectory {get; }
		//Evidence Evidence {get; }
		string FriendlyName {get; }
		string RelativeSearchPath {get; }
		bool ShadowCopyFiles {get; }

		//void AppendPrivatePath (string path);
		//void ClearPrivatePath ();
		//void ClearShadowCopyPath ();

		void DoCallBack (CrossAppDomainDelegate theDelegate);
		bool Equals (object other);

        //int ExecuteAssembly (string assemblyFile);
        //int ExecuteAssembly (string assemblyFile, Evidence assemblySecurity);
        //int ExecuteAssembly (string assemblyFile, Evidence assemblySecurity, string[] args);

		Assembly[] GetAssemblies ();
		object GetData (string name);
		int GetHashCode();

		object GetLifetimeService ();

		Type GetType ();

		object InitializeLifetimeService ();

        //Assembly Load (AssemblyName assemblyRef);
        //Assembly Load (byte[] rawAssembly);
        //Assembly Load (string assemblyString);
        //Assembly Load (AssemblyName assemblyRef, Evidence assemblySecurity);
        //Assembly Load (byte[] rawAssembly, byte[] rawSymbolStore);
        //Assembly Load (string assemblyString, Evidence assemblySecurity);
        //Assembly Load (byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence);

		//void SetAppDomainPolicy (PolicyLevel domainPolicy);
		//void SetCachePath (string s);

		//void SetData (string name, object data);

		//void SetPrincipalPolicy (PrincipalPolicy policy);

		//void SetShadowCopyPath (string s);

		//void SetThreadPrincipal (IPrincipal principal);
		string ToString ();

#if BOOTSTRAP_WITH_OLDLIB
		// older MCS/corlib returns:
		// _AppDomain.cs(138) error CS0592: Attribute 'SecurityPermission' is not valid on this declaration type.
		// It is valid on 'assembly' 'class' 'constructor' 'method' 'struct'  declarations only.
		event AssemblyLoadEventHandler AssemblyLoad;
		event ResolveEventHandler AssemblyResolve;
		event EventHandler DomainUnload;
		event EventHandler ProcessExit;
		event ResolveEventHandler ResourceResolve;
		event ResolveEventHandler TypeResolve;
		event UnhandledExceptionEventHandler UnhandledException;
#else
		event AssemblyLoadEventHandler AssemblyLoad;

		event ResolveEventHandler AssemblyResolve;

		event EventHandler DomainUnload;

		event EventHandler ProcessExit;

		event ResolveEventHandler ResourceResolve;

		event ResolveEventHandler TypeResolve;

		event UnhandledExceptionEventHandler UnhandledException;
#endif

#if NET_1_1
		void GetIDsOfNames ([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		void GetTypeInfo (uint iTInfo, uint lcid, IntPtr ppTInfo);

		void GetTypeInfoCount (out uint pcTInfo);

		void Invoke (uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams,
			IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
#endif
	}
}
