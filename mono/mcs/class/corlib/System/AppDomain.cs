//CHANGED
//
// System.AppDomain.cs
//
// Authors:
//   Paolo Molaro (lupus@ximian.com)
//   Dietmar Maurer (dietmar@ximian.com)
//   Miguel de Icaza (miguel@ximian.com)
//   Gonzalo Paniagua (gonzalo@ximian.com)
//   Patrik Torstensson
//   Sebastien Pouliot (sebastien@ximian.com)
//
// (C) 2001, 2002 Ximian, Inc.  http://www.ximian.com
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
using System.IO;
using System.Reflection;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if NOT_PFX
using System.Reflection.Emit;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
#endif
using System.Security;
using System.Security.Permissions;
#if NOT_PFX
using System.Security.Principal;
#endif
using System.Configuration.Assemblies;

#if NET_2_0
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
#endif

namespace System {

#if NET_2_0
	[ComVisible (true)]
	[ComDefaultInterface (typeof (_AppDomain))]
#endif
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class AppDomain : MarshalByRefObject , _AppDomain 
#if NOT_PFX        
        , IEvidenceFactory
#endif
	{
		IntPtr _mono_app_domain;
		static string _process_guid;

		[ThreadStatic]
		static Hashtable type_resolve_in_progress;

		[ThreadStatic]
		static Hashtable assembly_resolve_in_progress;

#if NOT_PFX
// CAS
		private Evidence _evidence;
		private PermissionSet _granted;

		// non-CAS
		private PrincipalPolicy _principalPolicy;


		[ThreadStatic]
		private static IPrincipal _principal;
#endif
        static AppDomain default_domain;

		private AppDomain ()
		{
		}

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private extern AppDomainSetup getSetup ();

		AppDomainSetup SetupInformationNoCopy {
			get { return getSetup (); }
		}

		public AppDomainSetup SetupInformation {
			get {
				AppDomainSetup setup = getSetup ();
				return new AppDomainSetup (setup);
			}
		}

#if NET_2_0
		[MonoTODO]
		public ApplicationTrust ApplicationTrust {
			get { throw new NotImplementedException (); }
		}
#endif

		public string BaseDirectory {
			get {
				string path = SetupInformationNoCopy.ApplicationBase;
#if NOT_PFX
				if (SecurityManager.SecurityEnabled && (path != null) && (path.Length > 0)) {
					// we cannot divulge local file informations
					new FileIOPermission (FileIOPermissionAccess.PathDiscovery, path).Demand ();
				}
#endif
				return path;
			}
		}

		public string RelativeSearchPath {
			get {
				string path = SetupInformationNoCopy.PrivateBinPath;
#if NOT_PFX
				if (SecurityManager.SecurityEnabled && (path != null) && (path.Length > 0)) {
					// we cannot divulge local file informations
					new FileIOPermission (FileIOPermissionAccess.PathDiscovery, path).Demand ();
				}
#endif
				return path;
			}
		}

		public string DynamicDirectory {
			get {
				AppDomainSetup setup = SetupInformationNoCopy;
				if (setup.DynamicBase == null)
					return null;

				string path = Path.Combine (setup.DynamicBase, setup.ApplicationName);
#if NOT_PFX
                if (SecurityManager.SecurityEnabled && (path != null) && (path.Length > 0)) {
					// we cannot divulge local file informations
					new FileIOPermission (FileIOPermissionAccess.PathDiscovery, path).Demand ();
				}
#endif
                return path;
			}
		}

		public bool ShadowCopyFiles {
			get {
				return (SetupInformationNoCopy.ShadowCopyFiles == "true");
			}
		}

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private extern string getFriendlyName ();

		public string FriendlyName {
			get {
				return getFriendlyName ();
			}
		}


#if NOT_PFX
		public Evidence Evidence {
			get {
				// if the host (runtime) hasn't provided it's own evidence...
				if (_evidence == null) {
					// ... we will provide our own
					lock (this) {
						// the executed assembly from the "default" appdomain
						// or null if we're not in the default appdomain
						Assembly a = Assembly.GetEntryAssembly ();
						if (a == null)
							_evidence = AppDomain.DefaultDomain.Evidence;
							_evidence = Evidence.GetDefaultHostEvidence (a);
					}
				}
				return new Evidence (_evidence);	// return a copy
			}
		}

        internal IPrincipal DefaultPrincipal {
			get {
				if (_principal == null) {

					switch (_principalPolicy) {
						case PrincipalPolicy.UnauthenticatedPrincipal:
							_principal = new GenericPrincipal (
								new GenericIdentity (String.Empty, String.Empty), null);
							break;
						case PrincipalPolicy.WindowsPrincipal:
							_principal = new WindowsPrincipal (WindowsIdentity.GetCurrent ());
							break;
					}

				}
				return _principal; 
			}
		}


    // for AppDomain there is only an allowed (i.e. granted) set
    // http://msdn.microsoft.com/library/en-us/cpguide/html/cpcondetermininggrantedpermissions.asp
		internal PermissionSet GrantedPermissionSet {
			get { return _granted; }
		}

#endif
        [MethodImplAttribute (MethodImplOptions.InternalCall)]
		private static extern AppDomain getCurDomain ();
		
		public static AppDomain CurrentDomain {
			get {
				return getCurDomain ();
			}
		}

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private static extern AppDomain getRootDomain ();

		internal static AppDomain DefaultDomain {
			get {
				if (default_domain == null) {
					AppDomain rd = getRootDomain ();
					if (rd == CurrentDomain)
						default_domain = rd;
#if NOT_PFX
					default_domain = (AppDomain) RemotingServices.GetDomainProxy (rd);
#endif
				}
				return default_domain;
			}
		}

#if NET_2_0
		[Obsolete ("AppDomain.AppendPrivatePath has been deprecated. Please investigate the use of AppDomainSetup.PrivateBinPath instead.")]
#endif
		[SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void AppendPrivatePath (string path)
		{
			if (path == null || path.Length == 0)
				return;

			AppDomainSetup setup = SetupInformationNoCopy;

			string pp = setup.PrivateBinPath;
			if (pp == null || pp.Length == 0) {
				setup.PrivateBinPath = path;
				return;
			}

			pp = pp.Trim ();
			if (pp [pp.Length - 1] != Path.PathSeparator)
				pp += Path.PathSeparator;

			setup.PrivateBinPath = pp + path;
		}

#if NET_2_0
		[Obsolete ("AppDomain.ClearPrivatePath has been deprecated. Please investigate the use of AppDomainSetup.PrivateBinPath instead.")]
#endif
		[SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void ClearPrivatePath ()
		{
			SetupInformationNoCopy.PrivateBinPath = String.Empty;
		}

#if NET_2_0
		[Obsolete ("Use AppDomainSetup.ShadowCopyDirectories")]
#endif
		[SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void ClearShadowCopyPath ()
		{
			SetupInformationNoCopy.ShadowCopyDirectories = String.Empty;
		}

#if NOT_PFX
public ObjectHandle CreateComInstanceFrom (string assemblyFile, string typeName)
		{
			return Activator.CreateComInstanceFrom (assemblyFile, typeName);
		}



#if NET_1_1
		public ObjectHandle CreateComInstanceFrom (string assemblyFile, string typeName,
			byte [] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
			return Activator.CreateComInstanceFrom (assemblyFile, typeName, hashValue ,hashAlgorithm);
		}
#endif


	public ObjectHandle CreateInstance (string assemblyFile, string typeName)
		{
			if (assemblyFile == null)
				throw new ArgumentNullException ("assemblyFile");

			return Activator.CreateInstance (assemblyFile, typeName);
		}


		public ObjectHandle CreateInstance (string assemblyFile, string typeName, object[] activationAttributes)
		{
			if (assemblyFile == null)
				throw new ArgumentNullException ("assemblyFile");

			return Activator.CreateInstance (assemblyFile, typeName, activationAttributes);
		}

		public ObjectHandle CreateInstance (string assemblyFile, string typeName, bool ignoreCase, BindingFlags bindingAttr,
		                                    Binder binder, object[] args, CultureInfo culture, object[] activationAttributes,
		                                    Evidence securityAttributes)
		{
			if (assemblyFile == null)
				throw new ArgumentNullException ("assemblyFile");

			return Activator.CreateInstance (assemblyFile, typeName, ignoreCase, bindingAttr, binder, args,
				culture, activationAttributes, securityAttributes);
		}

		public object CreateInstanceAndUnwrap (string assemblyFile, string typeName)
		{
			ObjectHandle oh = CreateInstance (assemblyFile, typeName);
			return (oh != null) ? oh.Unwrap () : null;
		}

        public object CreateInstanceAndUnwrap (string assemblyFile, string typeName, object [] activationAttributes)
		{
			ObjectHandle oh = CreateInstance (assemblyFile, typeName, activationAttributes);
			return (oh != null) ? oh.Unwrap () : null;
		}

		public object CreateInstanceAndUnwrap (string assemblyFile, string typeName, bool ignoreCase,
		                                       BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture,
		                                       object[] activationAttributes, Evidence securityAttributes)
		{
			ObjectHandle oh = CreateInstance (assemblyFile, typeName, ignoreCase, bindingAttr, binder, args,
				culture, activationAttributes, securityAttributes);
			return (oh != null) ? oh.Unwrap () : null;
		}

		public ObjectHandle CreateInstanceFrom (string assemblyFile, string typeName)
		{
			if (assemblyFile == null)
				throw new ArgumentNullException ("assemblyFile");

			return Activator.CreateInstanceFrom (assemblyFile, typeName);
		}

		public ObjectHandle CreateInstanceFrom (string assemblyFile, string typeName, object[] activationAttributes)
		{
			if (assemblyFile == null)
				throw new ArgumentNullException ("assemblyFile");

			return Activator.CreateInstanceFrom (assemblyFile, typeName, activationAttributes);
		}

		public ObjectHandle CreateInstanceFrom (string assemblyName, string typeName, bool ignoreCase,
		                                        BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture,
		                                        object[] activationAttributes, Evidence securityAttributes)
		{
			if (assemblyName == null)
				throw new ArgumentNullException ("assemblyName");

			return Activator.CreateInstanceFrom (assemblyName, typeName, ignoreCase, bindingAttr, binder, args,
			                                     culture, activationAttributes, securityAttributes);
		}

        public object CreateInstanceFromAndUnwrap (string assemblyName, string typeName)
		{
			ObjectHandle oh = CreateInstanceFrom (assemblyName, typeName);
			return (oh != null) ? oh.Unwrap () : null;
		}

		public object CreateInstanceFromAndUnwrap (string assemblyName, string typeName, object [] activationAttributes)
		{
			ObjectHandle oh = CreateInstanceFrom (assemblyName, typeName, activationAttributes);
			return (oh != null) ? oh.Unwrap () : null;
		}

		public object CreateInstanceFromAndUnwrap (string assemblyName, string typeName, bool ignoreCase,
		                                           BindingFlags bindingAttr, Binder binder, object[] args,
		                                           CultureInfo culture, object[] activationAttributes,
		                                           Evidence securityAttributes)
		{
			ObjectHandle oh = CreateInstanceFrom (assemblyName, typeName, ignoreCase, bindingAttr, binder, args,
				culture, activationAttributes, securityAttributes);

			return (oh != null) ? oh.Unwrap () : null;
		}

		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access)
		{
			return DefineDynamicAssembly (name, access, null, null, null, null, null, false);
		}

		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access, Evidence evidence)
		{
			return DefineDynamicAssembly (name, access, null, evidence, null, null, null, false);
		}

		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access, string dir)
		{
			return DefineDynamicAssembly (name, access, dir, null, null, null, null, false);
		}

		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access, string dir,
		                                              Evidence evidence)
		{
			return DefineDynamicAssembly (name, access, dir, evidence, null, null, null, false);
		}


		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access,
		                                              PermissionSet requiredPermissions,
		                                              PermissionSet optionalPermissions,
		                                              PermissionSet refusedPermissions)
		{
			return DefineDynamicAssembly (name, access, null, null, requiredPermissions, optionalPermissions,
				refusedPermissions, false);
		}

		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access, Evidence evidence,
		                                              PermissionSet requiredPermissions,
		                                              PermissionSet optionalPermissions,
		                                              PermissionSet refusedPermissions)
		{
			return DefineDynamicAssembly (name, access, null, evidence, requiredPermissions, optionalPermissions,
				refusedPermissions, false);
		}

		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access, string dir,
		                                              PermissionSet requiredPermissions,
		                                              PermissionSet optionalPermissions,
		                                              PermissionSet refusedPermissions)
		{
			return DefineDynamicAssembly (name, access, dir, null, requiredPermissions, optionalPermissions,
				refusedPermissions, false);
		}

		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access, string dir,
		                                              Evidence evidence,
		                                              PermissionSet requiredPermissions,
		                                              PermissionSet optionalPermissions,
		                                              PermissionSet refusedPermissions)
		{
			return DefineDynamicAssembly (name, access, dir, evidence, requiredPermissions, optionalPermissions,
				refusedPermissions, false);
		}

		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access, string dir,
		                                              Evidence evidence,
		                                              PermissionSet requiredPermissions,
		                                              PermissionSet optionalPermissions,
		                                              PermissionSet refusedPermissions, bool isSynchronized)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			ValidateAssemblyName (name.Name);

			// FIXME: examine all other parameters
			
			AssemblyBuilder ab = new AssemblyBuilder (name, dir, access, false);
			ab.AddPermissionRequests (requiredPermissions, optionalPermissions, refusedPermissions);
			return ab;
		}
#endif

#if NET_2_0
		// NET 3.5 method
		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access, string dir,
		                                              Evidence evidence,
		                                              PermissionSet requiredPermissions,
		                                              PermissionSet optionalPermissions,
		                                              PermissionSet refusedPermissions, bool isSynchronized, IEnumerable<CustomAttributeBuilder> assemblyAttributes)
		{
			AssemblyBuilder ab = DefineDynamicAssembly (name, access, dir, evidence, requiredPermissions, optionalPermissions, refusedPermissions, isSynchronized);
			if (assemblyAttributes != null)
				foreach (CustomAttributeBuilder cb in assemblyAttributes) {
					ab.SetCustomAttribute (cb);
				}
			return ab;
		}

		// NET 3.5 method
		public AssemblyBuilder DefineDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access, IEnumerable<CustomAttributeBuilder> assemblyAttributes) {
			return DefineDynamicAssembly (name, access, null, null, null, null, null, false, assemblyAttributes);
		}
#endif

#if NOT_PFX
		internal AssemblyBuilder DefineInternalDynamicAssembly (AssemblyName name, AssemblyBuilderAccess access)
		{
			return new AssemblyBuilder (name, null, access, true);
		}
#endif

		//
		// AppDomain.DoCallBack works because AppDomain is a MarshalByRefObject
		// so, when you call AppDomain.DoCallBack, that's a remote call
		//
		public void DoCallBack (CrossAppDomainDelegate callBackDelegate)
		{
			if (callBackDelegate != null)
				callBackDelegate ();
        }

#if NOT_PFX
		public int ExecuteAssembly (string assemblyFile)
		{
			return ExecuteAssembly (assemblyFile, null, null);
        }


		public int ExecuteAssembly (string assemblyFile, Evidence assemblySecurity)
		{
			return ExecuteAssembly (assemblyFile, assemblySecurity, null);
		}

		public int ExecuteAssembly (string assemblyFile, Evidence assemblySecurity, string[] args)
		{
			Assembly a = Assembly.LoadFrom (assemblyFile, assemblySecurity);
			return ExecuteAssemblyInternal (a, args);
		}

		public int ExecuteAssembly (string assemblyFile, Evidence assemblySecurity, string[] args, byte[] hashValue, AssemblyHashAlgorithm hashAlgorithm)
		{
#if AVM
            throw new NotSupportedException();
#else
			Assembly a = Assembly.LoadFrom (assemblyFile, assemblySecurity, hashValue, hashAlgorithm);
			return ExecuteAssemblyInternal (a, args);
#endif
		}
#endif
        int ExecuteAssemblyInternal (Assembly a, string[] args)
		{
			if (a.EntryPoint == null)
#if NET_2_0
				throw new MissingMethodException ("Entry point not found in assembly '" + a.FullName + "'.");
#else
				throw new COMException ("Unspecified error.", -2147467259);
#endif
			return ExecuteAssembly (a, args);
		}

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private extern int ExecuteAssembly (Assembly a, string[] args);
		
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private extern Assembly [] GetAssemblies (bool refOnly);

		public Assembly [] GetAssemblies ()
		{
			return GetAssemblies (false);
		}

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		public extern object GetData (string name);

        //public new Type GetType()
        //{
        //    return base.GetType ();
        //}

		public override object InitializeLifetimeService ()
		{
			return null;
        }

#if NOT_PFX
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal extern Assembly LoadAssembly (string assemblyRef, Evidence securityEvidence, bool refOnly);


		public Assembly Load (AssemblyName assemblyRef)
		{
			return Load (assemblyRef, null);
		}

		public Assembly Load (AssemblyName assemblyRef, Evidence assemblySecurity)
		{
			if (assemblyRef == null)
				throw new ArgumentNullException ("assemblyRef");

			if (assemblyRef.Name == null || assemblyRef.Name.Length == 0) {
				if (assemblyRef.CodeBase != null)
					return Assembly.LoadFrom (assemblyRef.CodeBase, assemblySecurity);
				else
					throw new ArgumentException (Locale.GetText ("assemblyRef.Name cannot be empty."), "assemblyRef");
			}

			return LoadAssembly (assemblyRef.FullName, assemblySecurity, false);
		}

        public Assembly Load (string assemblyString)
		{
			if (assemblyString == null)
				throw new ArgumentNullException ("assemblyString");

			return LoadAssembly (assemblyString, null, false);
		}

		public Assembly Load (string assemblyString, Evidence assemblySecurity)
		{
			return Load (assemblyString, assemblySecurity, false);
		}
		
		internal Assembly Load (string assemblyString, Evidence assemblySecurity, bool refonly)
		{
			if (assemblyString == null)
				throw new ArgumentNullException ("assemblyString");

			return LoadAssembly (assemblyString, assemblySecurity, refonly);
		}

		public Assembly Load (byte[] rawAssembly)
		{
			return Load (rawAssembly, null, null);
		}

		public Assembly Load (byte[] rawAssembly, byte[] rawSymbolStore)
		{
			return Load (rawAssembly, rawSymbolStore, null);
		}

        [MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal extern Assembly LoadAssemblyRaw (byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence, bool refonly);

		public Assembly Load (byte[] rawAssembly, byte[] rawSymbolStore, Evidence securityEvidence)
		{
			return Load (rawAssembly, rawSymbolStore, securityEvidence, false);
		}

		internal Assembly Load (byte [] rawAssembly, byte [] rawSymbolStore, Evidence securityEvidence, bool refonly)
		{
			if (rawAssembly == null)
				throw new ArgumentNullException ("rawAssembly");

			Assembly assembly = LoadAssemblyRaw (rawAssembly, rawSymbolStore, securityEvidence, refonly);
			assembly.FromByteArray = true;
			return assembly;
		}

		[SecurityPermission (SecurityAction.Demand, ControlPolicy = true)]
		public void SetAppDomainPolicy (PolicyLevel domainPolicy)
		{
			if (domainPolicy == null)
				throw new ArgumentNullException ("domainPolicy");
			if (_granted != null) {
				throw new PolicyException (Locale.GetText (
					"An AppDomain policy is already specified."));
			}
			if (IsFinalizingForUnload ())
				throw new AppDomainUnloadedException ();

			PolicyStatement ps = domainPolicy.Resolve (_evidence);
			_granted = ps.PermissionSet;
		}
#endif

#if NET_2_0
		[Obsolete ("Use AppDomainSetup.SetCachePath")]
#endif
		[SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetCachePath (string path)
		{
			SetupInformationNoCopy.CachePath = path;
		}

#if NOT_PFX
		[SecurityPermission (SecurityAction.Demand, ControlPrincipal = true)]
		public void SetPrincipalPolicy (PrincipalPolicy policy)
		{
			if (IsFinalizingForUnload ())
				throw new AppDomainUnloadedException ();

			_principalPolicy = policy;
			_principal = null;
		}
#endif

#if NET_2_0
		[Obsolete ("Use AppDomainSetup.ShadowCopyFiles")]
#endif
		[SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetShadowCopyFiles()
		{
			SetupInformationNoCopy.ShadowCopyFiles = "true";
		}

#if NET_2_0
		[Obsolete ("Use AppDomainSetup.ShadowCopyDirectories")]
#endif
		[SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetShadowCopyPath (string path)
		{
			SetupInformationNoCopy.ShadowCopyDirectories = path;
        }

#if NOT_PFX
		[SecurityPermission (SecurityAction.Demand, ControlPrincipal = true)]
public void SetThreadPrincipal (IPrincipal principal)
		{
			if (principal == null)
				throw new ArgumentNullException ("principal");

            if (_principal != null)
				throw new PolicyException (Locale.GetText ("principal already present."));

			if (IsFinalizingForUnload ())
				throw new AppDomainUnloadedException ();

			_principal = principal;
		}
#endif

        [MethodImplAttribute (MethodImplOptions.InternalCall)]
		private static extern AppDomain InternalSetDomainByID (int domain_id);
 
		// Changes the active domain and returns the old domain
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private static extern AppDomain InternalSetDomain (AppDomain context);

		// Notifies the runtime that this thread references 'domain'.
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern void InternalPushDomainRef (AppDomain domain);

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern void InternalPushDomainRefByID (int domain_id);

		// Undoes the effect of the last PushDomainRef call
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern void InternalPopDomainRef ();

#if NOT_PFX
		// Changes the active context and returns the old context
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern Context InternalSetContext (Context context);

		// Returns the current context
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern Context InternalGetContext ();

		// Returns the current context
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern Context InternalGetDefaultContext ();
#endif
        [MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern string InternalGetProcessGuid (string newguid);

		// This method is handled specially by the runtime
		// It is the only managed method which is allowed to set the current
		// appdomain
		internal static object InvokeInDomain (AppDomain domain, MethodInfo method, object obj, object [] args)
		{
#if AVM
            throw new NotSupportedException();
#else
			AppDomain current = CurrentDomain;
			bool pushed = false;

			try {
				InternalPushDomainRef (domain);
				pushed = true;
				InternalSetDomain (domain);
				return ((MonoMethod) method).InternalInvoke (obj, args);
			}
			finally {
				InternalSetDomain (current);
				if (pushed)
					InternalPopDomainRef ();
			}
#endif
		}

		internal static object InvokeInDomainByID (int domain_id, MethodInfo method, object obj, object [] args)
		{
#if AVM
            throw new NotSupportedException();
#else
			AppDomain current = CurrentDomain;
			bool pushed = false;

			try {
				InternalPushDomainRefByID (domain_id);
				pushed = true;
				InternalSetDomainByID (domain_id);
				return ((MonoMethod) method).InternalInvoke (obj, args);
			}
			finally {
				InternalSetDomain (current);
				if (pushed)
					InternalPopDomainRef ();
			}
#endif
		}

		internal static String GetProcessGuid ()
		{
			if (_process_guid == null) {
				_process_guid = InternalGetProcessGuid (Guid.NewGuid().ToString ());
			}
			return _process_guid;
        }

#if NOT_PFX
		public static AppDomain CreateDomain (string friendlyName)
		{
			return CreateDomain (friendlyName, null, null);
		}

		public static AppDomain CreateDomain (string friendlyName, Evidence securityInfo)
		{
			return CreateDomain (friendlyName, securityInfo, null);
		}
#endif
        [MethodImplAttribute (MethodImplOptions.InternalCall)]
		private static extern AppDomain createDomain (string friendlyName, AppDomainSetup info);

#if NOT_PFX
		[MonoTODO ("Currently it does not allow the setup in the other domain")]
		[SecurityPermission (SecurityAction.Demand, ControlAppDomain = true)]
		public static AppDomain CreateDomain (string friendlyName, Evidence securityInfo, AppDomainSetup info)
		{
			if (friendlyName == null)
				throw new System.ArgumentNullException ("friendlyName");

			AppDomain def = AppDomain.DefaultDomain;
			if (info == null) {
				// if null, get default domain's SetupInformation	
				if (def == null)
					info = new AppDomainSetup ();	// we're default!
				else
					info = def.SetupInformation;
			}
			else
				info = new AppDomainSetup (info);	// copy

			// todo: allow setup in the other domain
			if (def != null) {
				if (!info.Equals (def.SetupInformation)) {
					// If not specified use default domain's app base.
					if (info.ApplicationBase == null)
						info.ApplicationBase = def.SetupInformation.ApplicationBase;
					if (info.ConfigurationFile == null)
						info.ConfigurationFile = Path.GetFileName (def.SetupInformation.ConfigurationFile);
				}
			} else if (info.ConfigurationFile == null)
				info.ConfigurationFile = "[I don't have a config file]";


			AppDomain ad = (AppDomain) RemotingServices.GetDomainProxy (createDomain (friendlyName, info));
			if (securityInfo == null) {
				// get default domain's Evidence (unless we're are the default!)
				if (def == null)
					ad._evidence = null;		// we'll get them later (GetEntryAssembly)
				else
					ad._evidence = def.Evidence;	// new (shallow) copy
			}
			else
				ad._evidence = new Evidence (securityInfo);	// copy
#endif
#if NET_2_0
			if (info.AppDomainInitializer != null) {
				if ((info.AppDomainInitializer.Method.Attributes & MethodAttributes.Static) == 0)
					throw new ArgumentException ("Non-static methods cannot be invoked as an appdomain initializer");
				info.AppDomainInitializer (info.AppDomainInitializerArguments);
			}


			return ad;
		}


        public static AppDomain CreateDomain (string friendlyName, Evidence securityInfo,string appBasePath,
		                                      string appRelativeSearchPath, bool shadowCopyFiles)
		{
			AppDomainSetup info = new AppDomainSetup ();

			info.ApplicationBase = appBasePath;
			info.PrivateBinPath = appRelativeSearchPath;

			if (shadowCopyFiles)
				info.ShadowCopyFiles = "true";
			else
				info.ShadowCopyFiles = "false";

			return CreateDomain (friendlyName, securityInfo, info);
		}
#endif

#if NET_2_0
		[MonoTODO]
		public static AppDomain CreateDomain (string friendlyName, Evidence securityInfo, AppDomainSetup info,
		                                      PermissionSet grantSet, params StrongName [] fullTrustAssemblies)
		{
			throw new NotImplementedException ();
		}
#endif

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private static extern bool InternalIsFinalizingForUnload (int domain_id);

		public bool IsFinalizingForUnload()
		{
			return InternalIsFinalizingForUnload (getDomainID ());
		}

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		static extern void InternalUnload (int domain_id);

		// We do this because if the domain is a transparant proxy this
		// will still return the correct domain id.
		private int getDomainID ()
		{
			return Thread.GetDomainID ();
		}

		[SecurityPermission (SecurityAction.Demand, ControlAppDomain = true)]
#if NET_2_0
		[ReliabilityContractAttribute (Consistency.MayCorruptAppDomain, Cer.MayFail)]
#endif
		public static void Unload (AppDomain domain)
		{
			if (domain == null)
				throw new ArgumentNullException ("domain");

			InternalUnload (domain.getDomainID());
		}

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		[SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public extern void SetData (string name, object data);

#if NET_2_0
		[MonoTODO]
		public void SetData (string name, object data, IPermission permission)
		{
			throw new NotImplementedException ();
		}
#endif

#if NET_2_0
		[Obsolete ("Use AppDomainSetup.DynamicBase")]
#endif
		[SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public void SetDynamicBase (string path)
		{
			SetupInformationNoCopy.DynamicBase = path;
		}

#if NET_2_0
		[Obsolete ("AppDomain.GetCurrentThreadId has been deprecated"
			+ " because it does not provide a stable Id when managed"
			+ " threads are running on fibers (aka lightweight"
			+ " threads). To get a stable identifier for a managed"
			+ " thread, use the ManagedThreadId property on Thread.'")]
#endif
		public static int GetCurrentThreadId ()
		{
			return Thread.CurrentThreadId;
		}

		public override string ToString ()
		{
			return getFriendlyName ();
		}

		private static void ValidateAssemblyName (string name)
		{
			if (name == null || name.Length == 0)
				throw new ArgumentException ("The Name of " +
					"AssemblyName cannot be null or a " +
					"zero-length string.");

			bool isValid = true;

			for (int i = 0; i < name.Length; i++) {
				char c = name [i];

				// do not allow leading whitespace
				if (i == 0 && char.IsWhiteSpace (c)) {
					isValid = false;
					break;
				}

				// do not allow /,\ or : in name
				if (c == '/' || c == '\\' || c == ':') {
					isValid = false;
					break;
				}
			}

			if (!isValid)
				throw new ArgumentException ("The Name of " +
					"AssemblyName cannot start with " +
					"whitespace, or contain '/', '\\' " +
					" or ':'.");
		}

		// The following methods are called from the runtime. Don't change signatures.
		private void DoAssemblyLoad (Assembly assembly)
		{
			if (AssemblyLoad == null)
				return;

			AssemblyLoad (this, new AssemblyLoadEventArgs (assembly));
		}

		private Assembly DoAssemblyResolve (string name, bool refonly)
		{
			ResolveEventHandler del;
#if NET_2_0
			if (refonly)
				del = ReflectionOnlyAssemblyResolve;
			else
				del = AssemblyResolve;
#else
			del = AssemblyResolve;
#endif
			if (del == null)
				return null;
			
			/* Prevent infinite recursion */
			Hashtable ht = assembly_resolve_in_progress;
			if (ht == null) {
				ht = new Hashtable ();
				assembly_resolve_in_progress = ht;
			}

			Assembly ass = (Assembly) ht [name];
#if NET_2_0
			if (ass != null && (ass.ReflectionOnly == refonly))
				return null;
#else
			if (ass != null)
				return null;
#endif
			ht [name] = name;
			try {
				Delegate[] invocation_list = del.GetInvocationList ();

				foreach (Delegate eh in invocation_list) {
					ResolveEventHandler handler = (ResolveEventHandler) eh;
					Assembly assembly = handler (this, new ResolveEventArgs (name));
					if (assembly != null)
						return assembly;
				}
				return null;
			}
			finally {
				ht.Remove (name);
			}
		}

#if NOT_PFX
		internal Assembly DoTypeResolve (Object name_or_tb)
		{
			if (TypeResolve == null)
				return null;

			string name;


			if (name_or_tb is TypeBuilder)
				name = ((TypeBuilder) name_or_tb).FullName;
			else
				name = (string) name_or_tb;


			/* Prevent infinite recursion */
			Hashtable ht = type_resolve_in_progress;
			if (ht == null) {
				ht = new Hashtable ();
				type_resolve_in_progress = ht;
			}

			if (ht.Contains (name))
				return null;
			else
				ht [name] = name;

			try {
				foreach (Delegate d in TypeResolve.GetInvocationList ()) {
					ResolveEventHandler eh = (ResolveEventHandler) d;
					Assembly assembly = eh (this, new ResolveEventArgs (name));
					if (assembly != null)
						return assembly;
				}
				return null;
			}
			finally {
				ht.Remove (name);
			}
		}
#endif

        private void DoDomainUnload ()
		{
			if (DomainUnload != null)
				DomainUnload(this, null);
		}

#if NOT_PFX
		internal byte[] GetMarshalledDomainObjRef ()
		{
			ObjRef oref = RemotingServices.Marshal (AppDomain.CurrentDomain, null, typeof (AppDomain));
			return CADSerializer.SerializeObject (oref).GetBuffer();
		}


        internal void ProcessMessageInDomain (byte[] arrRequest, CADMethodCallMessage cadMsg,
		                                      out byte[] arrResponse, out CADMethodReturnMessage cadMrm)
		{
			IMessage reqDomMsg;

			if (null != arrRequest)
				reqDomMsg = CADSerializer.DeserializeMessage (new MemoryStream(arrRequest), null);
			else
				reqDomMsg = new MethodCall (cadMsg);

			IMessage retDomMsg = ChannelServices.SyncDispatchMessage (reqDomMsg);

			cadMrm = CADMethodReturnMessage.Create (retDomMsg);
			if (null == cadMrm) {
				arrResponse = CADSerializer.SerializeMessage (retDomMsg).GetBuffer();
			} 
			else
				arrResponse = null;
		}
#endif
        // End of methods called from the runtime
		
#if BOOTSTRAP_WITH_OLDLIB
		// older MCS/corlib returns:
		// _AppDomain.cs(138) error CS0592: Attribute 'SecurityPermission' is not valid on this declaration type.
		// It is valid on 'assembly' 'class' 'constructor' 'method' 'struct'  declarations only.
		public event AssemblyLoadEventHandler AssemblyLoad;

		public event ResolveEventHandler AssemblyResolve;

		public event EventHandler DomainUnload;

		public event EventHandler ProcessExit;

		public event ResolveEventHandler ResourceResolve;

		public event ResolveEventHandler TypeResolve;

		public event UnhandledExceptionEventHandler UnhandledException;
#else
		[method: SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event AssemblyLoadEventHandler AssemblyLoad;

		[method: SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler AssemblyResolve;

		[method: SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event EventHandler DomainUnload;

		[method: SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event EventHandler ProcessExit;

		[method: SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler ResourceResolve;

		[method: SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event ResolveEventHandler TypeResolve;

		[method: SecurityPermission (SecurityAction.LinkDemand, ControlAppDomain = true)]
		public event UnhandledExceptionEventHandler UnhandledException;
#endif

		/* Avoid warnings for events used only by the runtime */
		private void DummyUse () {
			ProcessExit += (EventHandler)null;
			ResourceResolve += (ResolveEventHandler)null;
			UnhandledException += (UnhandledExceptionEventHandler)null;
		}

#if NET_2_0

		public event ResolveEventHandler ReflectionOnlyAssemblyResolve;
		
		private ActivationContext _activation;
		private ApplicationIdentity _applicationIdentity;
		private AppDomainManager _domain_manager;

		// properties

		public ActivationContext ActivationContext {
			get { return _activation; }
		}

		public ApplicationIdentity ApplicationIdentity {
			get { return _applicationIdentity; }
		}

		// default is null
		public AppDomainManager DomainManager {
			get { return _domain_manager; }
		}

		public int Id {
			[ReliabilityContract (Consistency.WillNotCorruptState, Cer.Success)]
			get { return getDomainID (); }
		}

		// methods

		[MonoTODO ("This routine only returns the parameter currently")]
		[ComVisible (false)]
		public string ApplyPolicy (string assemblyName)
		{
			if (assemblyName == null)
				throw new ArgumentNullException ("assemblyName");
			if (assemblyName.Length == 0) // String.Empty
				throw new ArgumentException ("assemblyName");
			return assemblyName;
		}

		// static methods

		[MonoTODO ("add support for new delegate")]
		public static AppDomain CreateDomain (string friendlyName, Evidence securityInfo, string appBasePath,
			string appRelativeSearchPath, bool shadowCopyFiles, AppDomainInitializer adInit, string[] adInitArgs)
		{
			return CreateDomain (friendlyName, securityInfo, appBasePath, appRelativeSearchPath, shadowCopyFiles);
		}

		public int ExecuteAssemblyByName (string assemblyName)
		{
			return ExecuteAssemblyByName (assemblyName, null, null);
		}

		public int ExecuteAssemblyByName (string assemblyName, Evidence assemblySecurity)
		{
			return ExecuteAssemblyByName (assemblyName, assemblySecurity, null);
		}

		public int ExecuteAssemblyByName (string assemblyName, Evidence assemblySecurity, params string[] args)
		{
			Assembly a = Assembly.Load (assemblyName, assemblySecurity);

			return ExecuteAssemblyInternal (a, args);
		}

		public int ExecuteAssemblyByName (AssemblyName assemblyName, Evidence assemblySecurity, params string[] args)
		{
			Assembly a = Assembly.Load (assemblyName, assemblySecurity);

			return ExecuteAssemblyInternal (a, args);
		}

		public bool IsDefaultAppDomain ()
		{
			return Object.ReferenceEquals (this, DefaultDomain);
		}

		public Assembly[] ReflectionOnlyGetAssemblies ()
		{
			return GetAssemblies (true);
		}
#endif

#if NET_1_1
		void _AppDomain.GetIDsOfNames ([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException ();
		}

		void _AppDomain.GetTypeInfo (uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException ();
		}

		void _AppDomain.GetTypeInfoCount (out uint pcTInfo)
		{
			throw new NotImplementedException ();
		}

		void _AppDomain.Invoke (uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams,
			IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException ();
		}
#endif
	}
}
