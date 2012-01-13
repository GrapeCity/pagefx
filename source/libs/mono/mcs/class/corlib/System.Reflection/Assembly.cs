//CHANGED

//
// System.Reflection/Assembly.cs
//
// Author:
//   Paolo Molaro (lupus@ximian.com)
//
// (C) 2001 Ximian, Inc.  http://www.ximian.com
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

using System.IO;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Security.Permissions;
#if NOT_PFX
using System.Security.Policy;
#endif

namespace System.Reflection {

    public class Assembly : ICustomAttributeProvider
    {
        internal bool FromByteArray;
        private string assemblyName;

        internal Assembly()
        {
        }

        internal static readonly Assembly Instance = new Assembly();

#if NOT_PFX
        public event ModuleResolveEventHandler ModuleResolve;
#endif

        #region Info
        public virtual string CodeBase
        {
            get { return ""; }
        }

#if NOT_PFX
        public virtual string EscapedCodeBase
        {
            get { return ""; }
        }
#endif

        public virtual string FullName
        {
            get
            {
                //
                // FIXME: This is wrong, but it gets us going
                // in the compiler for now
                //
                return ToString();
            }
        }

#if NOT_PFX
        public virtual MethodInfo EntryPoint
        {
            get { return entryPoint; }
        }
        internal MethodInfo entryPoint;
#endif

#if NOT_PFX
        public bool GlobalAssemblyCache
        {
            get { return false; }
        }
#endif

        public virtual String Location
        {
            get { return ""; }
        }

#if NET_1_1
        public virtual string ImageRuntimeVersion
        {
            get { return "1.0"; }
        }
#endif

#if NOT_PFX
        internal virtual AssemblyName UnprotectedGetName()
        {
            throw new NotSupportedException();
        }
#endif
        #endregion

        #region Security
#if NOT_PFX
        private Evidence _evidence;

        public virtual Evidence Evidence
        {
            [SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
            get { return UnprotectedGetEvidence(); }
        }


        // note: the security runtime requires evidences but may be unable to do so...
        internal Evidence UnprotectedGetEvidence()
        {
            // if the host (runtime) hasn't provided it's own evidence...
            if (_evidence == null)
            {
                // ... we will provide our own
                lock (this)
                {
                    _evidence = Evidence.GetDefaultHostEvidence(this);
                }
            }
            return _evidence;
        }
#endif
        #endregion

        #region Custom Attributes
        public virtual bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public virtual object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Resources
#if NOT_PFX
        internal IntPtr GetManifestResourceInternal(String name, out int size, out Module module)
        {
            throw new NotImplementedException();
        }

        public virtual Stream GetManifestResourceStream(String name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name == "")
                throw new ArgumentException("name cannot have zero length.");

            throw new NotImplementedException();
        }

        public virtual Stream GetManifestResourceStream(Type type, String name)
        {
            string ns;
            if (type != null)
                ns = type.Namespace;
            else
                ns = null;

            if ((ns == null) || (ns == ""))
                return GetManifestResourceStream(name);
            else
                return GetManifestResourceStream(ns + "." + name);
        }

        public virtual String[] GetManifestResourceNames()
        {
            throw new NotImplementedException();
        }

        public virtual ManifestResourceInfo GetManifestResourceInfo(String resourceName)
        {
            throw new NotImplementedException();
        }
#endif
        #endregion

        #region Files
#if NOT_PFX
public virtual FileStream GetFile(string name)
        {
            throw new NotSupportedException();
        }

        public virtual FileStream[] GetFiles(bool getResourceModules)
        {
            throw new NotSupportedException();
        }
#endif
        #endregion

        #region PageFX ReflectionInfo
        internal static Avm.Array Types;
        internal static Avm.Array ArrayTypes;

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern int GetTypeNum();

        private static void InitType(Type type, int index)
        {
            Assembly asm = Instance;
            string name = avm.Concat("__inittype__", index);
            object v = avm.GetProperty(asm, avm.GlobalPackage, name);
            Avm.Function f = v as Avm.Function;
            if (avm.IsNull(f))
                throw new InvalidOperationException(string.Format("No function to init type with id {0}", index));
            f.call(null, type);
        }
        
        internal static Type GetType(int index)
        {
            if (index < 0) return null;
            if (Types == null)
            {
                int n = GetTypeNum();
                if (n <= 0) return null;
                Types = avm.NewArray(n);
                ArrayTypes = avm.NewArray();
            }
            Type type = (Type)Types[index];
            if (type == null)
            {
                type = new Type();
                InitType(type, index);
                type.index = index;
                Types[index] = type;
                if (type.IsArray)
                    ArrayTypes.push(type);
            }
            return type;
        }

        internal static Avm.String GetArrayTypeName(Type elementType, int[] lengths, int[] bounds)
        {
            //TODO:
            string name = elementType.Name;
            name += "[";
            name += "]";
            return name;
        }

        internal static Type GetArrayType(Type elementType, int[] lengths, int[] bounds)
        {
            if (elementType == null)
                throw new ArgumentNullException("elementType");

            Type type;
            uint n = ArrayTypes.length;
            for (int i = 0; i < n; ++i)
            {
                type = (Type)ArrayTypes[i];
                //TODO: check length and bounds
                if (type.elemType == elementType.index)
                {
                    return type;
                }
            }

            n = Types.length;

            type = new Type();
            type.index = (int)n;
            type.elemType = elementType.index;
            type.nsobj = elementType.nsobj;
            type.ns = elementType.ns;
            type.name = GetArrayTypeName(elementType, lengths, bounds);
            type.kind = Type.KIND_ARRAY;
            Types.push(type);
            ArrayTypes.push(type);
            return type;
        }
        #endregion

        #region GetType, GetTypes
        private static bool InitTypes()
        {
            if (Types == null)
            {
                Type type = GetType(0);
                if (type == null) return false;
            }
            return true;
        }

        private static Type[] GetTypes(bool exportedOnly)
        {
            if (!InitTypes()) return null;
            uint n = Types.length;
            Type[] types = new Type[n];
            for (int i = 0; i < n; ++i)
                types[i] = GetType(i);
            return types;
        }

        public virtual Type[] GetTypes()
        {
            return GetTypes(false);
        }

        public virtual Type[] GetExportedTypes()
        {
            return GetTypes(true);
        }

        public virtual Type GetType(String name, Boolean throwOnError)
        {
            return GetType(name, throwOnError, false);
        }

        public virtual Type GetType(String name)
        {
            return GetType(name, false, false);
        }

        internal static Type InternalGetType(String name, bool throwOnError, bool ignoreCase)
        {
            if (!InitTypes()) return null;
            uint n = Types.length;
            for (int i = 0; i < n; ++i)
            {
                Type type = GetType(i);
                if (string.Compare(type.FullName, name, ignoreCase) == 0)
                    return type;
            }
            return null;
        }

        internal Type GetType(string name, bool throwOnError, bool ignoreCase)
        {
            if (name == null)
                throw new ArgumentNullException(name);

            return InternalGetType(name, throwOnError, ignoreCase);
        }
        #endregion

        #region Name
#if NOT_PFX
        private void FillName(AssemblyName an)
        {
            //TODO:
            an.Name = assemblyName;
        }

        [MonoTODO("copiedName == true is not supported")]
        public virtual AssemblyName GetName(bool copiedName)
        {
            AssemblyName aname = new AssemblyName();
            FillName(aname);
            return aname;
        }

        public virtual AssemblyName GetName()
        {
            return GetName(false);
        }
#endif

        public override string ToString()
        {
            // note: ToString work without requiring CodeBase (so no checks are needed)
            return assemblyName;
        }

        internal static String CreateQualifiedName(String assemblyName, String typeName)
        {
            return typeName + ", " + assemblyName;
        }
        #endregion

        #region GetAssembly
        public static Assembly GetEntryAssembly()
        {
            return Instance;
        }

        public static Assembly GetExecutingAssembly()
        {
            return Instance;
        }

        public static Assembly GetCallingAssembly()
        {
            return Instance;
        }

#if NOT_PFX
        public static Assembly GetAssembly(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return type.Assembly;
        }

        public Assembly GetSatelliteAssembly(CultureInfo culture)
        {
            return GetSatelliteAssembly(culture, null);
        }

        public Assembly GetSatelliteAssembly(CultureInfo culture, Version version)
        {
            throw new NotImplementedException();
        }

        public AssemblyName[] GetReferencedAssemblies()
        {
            return null;
        }
#endif
        #endregion

        #region Load
#if NOT_PFX
        //[MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static Assembly LoadFrom(String assemblyFile, bool refonly)
        {
            throw new NotSupportedException();
        }

        public static Assembly LoadFrom(String assemblyFile)
        {
            return LoadFrom(assemblyFile, false);
        }


        public static Assembly LoadFrom(String assemblyFile, Evidence securityEvidence)
        {
            Assembly a = LoadFrom(assemblyFile, false);
            if ((a != null) && (securityEvidence != null))
            {
                // merge evidence (i.e. replace defaults with provided evidences)
                a.Evidence.Merge(securityEvidence);
            }
            return a;
        }

#if NET_1_1
        public static Assembly LoadFile(String path)
        {
            throw new NotImplementedException();
        }
#endif

        public static Assembly Load(String assemblyString)
        {
            return AppDomain.CurrentDomain.Load(assemblyString);
        }

        public static Assembly Load(String assemblyString, Evidence assemblySecurity)
        {
            return AppDomain.CurrentDomain.Load(assemblyString, assemblySecurity);
        }

        public static Assembly Load(AssemblyName assemblyRef)
        {
            return AppDomain.CurrentDomain.Load(assemblyRef);
        }

        public static Assembly Load(AssemblyName assemblyRef, Evidence assemblySecurity)
        {
            return AppDomain.CurrentDomain.Load(assemblyRef, assemblySecurity);
        }

        public static Assembly Load(Byte[] rawAssembly)
        {
            return AppDomain.CurrentDomain.Load(rawAssembly);
        }

        public static Assembly Load(Byte[] rawAssembly, Byte[] rawSymbolStore)
        {
            return AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore);
        }

        public static Assembly Load(Byte[] rawAssembly, Byte[] rawSymbolStore, Evidence securityEvidence)
        {
            return AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore, securityEvidence);
        }

#if NET_2_0
		public static Assembly ReflectionOnlyLoad (byte[] rawAssembly)
		{
			return AppDomain.CurrentDomain.Load (rawAssembly, null, null, true);
		}

		public static Assembly ReflectionOnlyLoad (string assemblyName) 
		{
			return AppDomain.CurrentDomain.Load (assemblyName, null, true);
		}

		public static Assembly ReflectionOnlyLoadFrom (string assemblyFile) 
		{
			if (assemblyFile == null)
				throw new ArgumentNullException ("assemblyFile");
			
			return LoadFrom (assemblyFile, true);
		}
#endif

#if NET_2_0
		[Obsolete ("")]
#endif
        public static Assembly LoadWithPartialName(string partialName)
        {
            throw new NotImplementedException();
        }

    //[MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static Assembly load_with_partial_name(string name, Evidence e)
        {
            throw new NotSupportedException();
        }


#if NET_2_0
		[Obsolete ("")]
#endif
        public static Assembly LoadWithPartialName(string partialName, Evidence securityEvidence)
        {
            return LoadWithPartialName(partialName, securityEvidence, true);
        }

        /**
         * LAMESPEC: It is possible for this method to throw exceptions IF the name supplied
         * is a valid gac name and contains filesystem entry charachters at the end of the name
         * ie System/// will throw an exception. However ////System will not as that is canocolized
         * out of the name.
         */

        // FIXME: LoadWithPartialName must look cache (no CAS) or read from disk (CAS)
        internal static Assembly LoadWithPartialName(string partialName, Evidence securityEvidence, bool oldBehavior)
        {
            if (!oldBehavior)
                throw new NotImplementedException();

            if (partialName == null)
                throw new NullReferenceException();

            return load_with_partial_name(partialName, securityEvidence);
        }
#endif
        #endregion

        #region CreateInstance
        public Object CreateInstance(String typeName)
        {
            return CreateInstance(typeName, false);
        }

        internal Object CreateInstance(String typeName, bool ignoreCase)
        {
            Type t = GetType(typeName, false, ignoreCase);
            if (t == null)
                return null;

            try
            {
                return Activator.CreateInstance(t);
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("It is illegal to invoke a method on a Type loaded via ReflectionOnly methods.");
            }
        }

#if NOT_PFX
        public Object CreateInstance(String typeName, Boolean ignoreCase,
                          BindingFlags bindingAttr, Binder binder,
                          Object[] args, CultureInfo culture,
                          Object[] activationAttributes)
        {
            Type t = GetType(typeName, false, ignoreCase);
            if (t == null)
                return null;

            try
            {
                return Activator.CreateInstance(t, bindingAttr, binder, args, culture, activationAttributes);
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("It is illegal to invoke a method on a Type loaded via ReflectionOnly methods.");
            }
        }
#endif
        #endregion

        #region Modules
#if NOT_PFX
        public Module LoadModule(string moduleName, byte[] rawModule)
        {
            throw new NotImplementedException();
        }

        public Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore)
        {
            throw new NotImplementedException();
        }

        public Module[] GetLoadedModules()
        {
            return GetLoadedModules(false);
        }

        // FIXME: Currently, the two sets of modules are equal
        public Module[] GetLoadedModules(bool getResourceModules)
        {
            return GetModules(getResourceModules);
        }

        public Module[] GetModules()
        {
            return GetModules(false);
        }

        public Module GetModule(String name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name == "")
                throw new ArgumentException("Name can't be empty");

            Module[] modules = GetModules(true);
            foreach (Module module in modules)
            {
                if (module.ScopeName == name)
                    return module;
            }

            return null;
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal extern Module[] GetModulesInternal();

        public Module[] GetModules(bool getResourceModules)
        {
            Module[] modules = GetModulesInternal();

            if (!getResourceModules)
            {
                ArrayList result = new ArrayList(modules.Length);
                foreach (Module m in modules)
                    if (!m.IsResource())
                        result.Add(m);
                return (Module[])result.ToArray(typeof(Module));
            }
            else
                return modules;
        }
#endif
        #endregion

    }
}
