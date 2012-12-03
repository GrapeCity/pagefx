
//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
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

//
// System.Reflection.Emit/ModuleBuilder.cs
//
// Author:
//   Paolo Molaro (lupus@ximian.com)
//
// (C) 2001 Ximian, Inc.  http://www.ximian.com
//

using System;
using System.Reflection;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Resources;
using System.Globalization;

namespace System.Reflection.Emit {
#if NET_2_0
	[ComVisible (true)]
	[ComDefaultInterface (typeof (_ModuleBuilder))]
#endif
	[ClassInterface (ClassInterfaceType.None)]
	public class ModuleBuilder : Module
#if NOT_PFX
        , _ModuleBuilder 
#endif
    {
		#region Sync with object-internals.h
		private UIntPtr dynamic_image; /* GC-tracked */
		private int num_types;
		private TypeBuilder[] types;
		private CustomAttributeBuilder[] cattrs;
		private byte[] guid;
		private int table_idx;
		internal AssemblyBuilder assemblyb;
		private MethodBuilder[] global_methods;
		private FieldBuilder[] global_fields;
		bool is_main;
		private MonoResource[] resources;
		#endregion
		private TypeBuilder global_type;
		private Type global_type_created;
		Hashtable name_cache;
		Hashtable us_string_cache = new Hashtable ();
		private int[] table_indexes;
		bool transient;
		ModuleBuilderTokenGenerator token_gen;
		Hashtable resource_writers;
		ISymbolWriter symbolWriter;

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void basic_init (ModuleBuilder ab);

		internal ModuleBuilder (AssemblyBuilder assb, string name, string fullyqname, bool emitSymbolInfo, bool transient) {
			this.name = this.scopename = name;
			this.fqname = fullyqname;
			this.assembly = this.assemblyb = assb;
			this.transient = transient;
			// to keep mcs fast we do not want CryptoConfig wo be involved to create the RNG
			guid = Guid.FastNewGuidArray ();
			// guid = Guid.NewGuid().ToByteArray ();
			table_idx = get_next_table_index (this, 0x00, true);
			name_cache = new Hashtable ();

			basic_init (this);

			CreateGlobalType ();
			
			if (emitSymbolInfo) {
				Assembly asm = Assembly.LoadWithPartialName ("Mono.CompilerServices.SymbolWriter");
				Type t = asm.GetType ("Mono.CompilerServices.SymbolWriter.SymbolWriterImpl");
				if (t != null) {
					symbolWriter = (ISymbolWriter) Activator.CreateInstance (t, new object[] { this });
					string fileName = fqname;
					if (assemblyb.AssemblyDir != null)
						fileName = Path.Combine (assemblyb.AssemblyDir, fileName);
					symbolWriter.Initialize (IntPtr.Zero, fileName, true);
				}
			}
		}

		public override string FullyQualifiedName {get { return fqname;}}

		public bool IsTransient () {
			return transient;
		}

		public void CreateGlobalFunctions () 
		{
			if (global_type_created != null)
				throw new InvalidOperationException ("global methods already created");
			if (global_type != null)
				global_type_created = global_type.CreateType ();
		}

		public FieldBuilder DefineInitializedData( string name, byte[] data, FieldAttributes attributes) {
			if (data == null)
				throw new ArgumentNullException ("data");

			FieldBuilder fb = DefineUninitializedData (name, data.Length, 
													   attributes | FieldAttributes.HasFieldRVA);
			fb.SetRVAData (data);

			return fb;
		}

		public FieldBuilder DefineUninitializedData (string name, int size, FieldAttributes attributes)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (global_type_created != null)
				throw new InvalidOperationException ("global fields already created");
			if ((size <= 0) || (size > 0x3f0000))
				throw new ArgumentException ("size", "Data size must be > 0 and < 0x3f0000");

			if (global_type == null)
				global_type = new TypeBuilder (this, 0);

			string typeName = "$ArrayType$" + size;
			Type datablobtype = GetType (typeName, false, false);
			if (datablobtype == null) {
				TypeBuilder tb = DefineType (typeName, 
				    TypeAttributes.Public|TypeAttributes.ExplicitLayout|TypeAttributes.Sealed,
					assemblyb.corlib_value_type, null, PackingSize.Size1, size);
				tb.CreateType ();
				datablobtype = tb;
			}
			FieldBuilder fb = global_type.DefineField (name, datablobtype, attributes|FieldAttributes.Static);

			if (global_fields != null) {
				FieldBuilder[] new_fields = new FieldBuilder [global_fields.Length+1];
				System.Array.Copy (global_fields, new_fields, global_fields.Length);
				new_fields [global_fields.Length] = fb;
				global_fields = new_fields;
			} else {
				global_fields = new FieldBuilder [1];
				global_fields [0] = fb;
			}
			return fb;
		}

		private void addGlobalMethod (MethodBuilder mb) {
			if (global_methods != null) {
				MethodBuilder[] new_methods = new MethodBuilder [global_methods.Length+1];
				System.Array.Copy (global_methods, new_methods, global_methods.Length);
				new_methods [global_methods.Length] = mb;
				global_methods = new_methods;
			} else {
				global_methods = new MethodBuilder [1];
				global_methods [0] = mb;
			}
		}

		public MethodBuilder DefineGlobalMethod (string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return DefineGlobalMethod (name, attributes, CallingConventions.Standard, returnType, parameterTypes);
		}

		public MethodBuilder DefineGlobalMethod (string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) {
			return DefineGlobalMethod (name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

#if NET_2_0 || BOOTSTRAP_NET_2_0
		public
#else
		internal
#endif
		MethodBuilder DefineGlobalMethod (string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if ((attributes & MethodAttributes.Static) == 0)
				throw new ArgumentException ("global methods must be static");
			if (global_type_created != null)
				throw new InvalidOperationException ("global methods already created");
			if (global_type == null)
				global_type = new TypeBuilder (this, 0);
			MethodBuilder mb = global_type.DefineMethod (name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);

			addGlobalMethod (mb);
			return mb;
		}

		public MethodBuilder DefinePInvokeMethod (string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet) {
			return DefinePInvokeMethod (name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		public MethodBuilder DefinePInvokeMethod (string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet) {
			if (name == null)
				throw new ArgumentNullException ("name");
			if ((attributes & MethodAttributes.Static) == 0)
				throw new ArgumentException ("global methods must be static");
			if (global_type_created != null)
				throw new InvalidOperationException ("global methods already created");
			if (global_type == null)
				global_type = new TypeBuilder (this, 0);
			MethodBuilder mb = global_type.DefinePInvokeMethod (name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);

			addGlobalMethod (mb);
			return mb;
		}			

		public TypeBuilder DefineType (string name) {
			return DefineType (name, 0);
		}

		public TypeBuilder DefineType (string name, TypeAttributes attr) {
			if ((attr & TypeAttributes.Interface) != 0)
				return DefineType (name, attr, null, null);
			return DefineType (name, attr, typeof(object), null);
		}

		public TypeBuilder DefineType (string name, TypeAttributes attr, Type parent) {
			return DefineType (name, attr, parent, null);
		}

		private void AddType (TypeBuilder tb)
		{
			if (types != null) {
				if (types.Length == num_types) {
					TypeBuilder[] new_types = new TypeBuilder [types.Length * 2];
					System.Array.Copy (types, new_types, num_types);
					types = new_types;
				}
			} else {
				types = new TypeBuilder [1];
			}
			types [num_types] = tb;
			num_types ++;
		}

		private TypeBuilder DefineType (string name, TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packsize, int typesize) {
			if (name_cache.Contains (name))
				throw new ArgumentException ("Duplicate type name within an assembly.");

			TypeBuilder res = new TypeBuilder (this, name, attr, parent, interfaces, packsize, typesize, null);
			AddType (res);
			name_cache.Add (name, res);
			return res;
		}

		internal void RegisterTypeName (TypeBuilder tb, string name)
		{
			name_cache.Add (name, tb);
		}
		
		internal TypeBuilder GetRegisteredType (string name)
		{
			if (name_cache == null)
				return null;
			return (TypeBuilder)name_cache [name];
		}

#if NET_2_0
		[ComVisible (true)]
#endif
		public TypeBuilder DefineType (string name, TypeAttributes attr, Type parent, Type[] interfaces) {
			return DefineType (name, attr, parent, interfaces, PackingSize.Unspecified, TypeBuilder.UnspecifiedTypeSize);
		}

		public TypeBuilder DefineType (string name, TypeAttributes attr, Type parent, int typesize) {
			return DefineType (name, attr, parent, null, PackingSize.Unspecified, TypeBuilder.UnspecifiedTypeSize);
		}

		public TypeBuilder DefineType (string name, TypeAttributes attr, Type parent, PackingSize packsize) {
			return DefineType (name, attr, parent, null, packsize, TypeBuilder.UnspecifiedTypeSize);
		}

		public TypeBuilder DefineType (string name, TypeAttributes attr, Type parent, PackingSize packsize, int typesize) {
			return DefineType (name, attr, parent, null, packsize, typesize);
		}

		public MethodInfo GetArrayMethod( Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) {
			return new MonoArrayMethod (arrayClass, methodName, callingConvention, returnType, parameterTypes);
		}

		public EnumBuilder DefineEnum( string name, TypeAttributes visibility, Type underlyingType) {
			if (name_cache.Contains (name))
				throw new ArgumentException ("Duplicate type name within an assembly.");

			EnumBuilder eb = new EnumBuilder (this, name, visibility, underlyingType);
			TypeBuilder res = eb.GetTypeBuilder ();
			AddType (res);
			name_cache.Add (name, res);
			return eb;
		}

#if NET_2_0
		[ComVisible (true)]
#endif
		public override Type GetType( string className) {
			return GetType (className, false, false);
		}
		
#if NET_2_0
		[ComVisible (true)]
#endif
		public override Type GetType( string className, bool ignoreCase) {
			return GetType (className, false, ignoreCase);
		}

		private TypeBuilder search_in_array (TypeBuilder[] arr, int validElementsInArray, string className) {
			int i;
			for (i = 0; i < validElementsInArray; ++i) {
				if (String.Compare (className, arr [i].FullName, true, CultureInfo.InvariantCulture) == 0) {
					return arr [i];
				}
			}
			return null;
		}

		private TypeBuilder search_nested_in_array (TypeBuilder[] arr, int validElementsInArray, string className) {
			int i;
			for (i = 0; i < validElementsInArray; ++i) {
				if (String.Compare (className, arr [i].Name, true, CultureInfo.InvariantCulture) == 0)
					return arr [i];
			}
			return null;
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern Type create_modified_type (TypeBuilder tb, string modifiers);

		static readonly char [] type_modifiers = {'&', '[', '*'};

		private TypeBuilder GetMaybeNested (TypeBuilder t, string className) {
			int subt;
			string pname, rname;

			subt = className.IndexOf ('+');
			if (subt < 0) {
				if (t.subtypes != null)
					return search_nested_in_array (t.subtypes, t.subtypes.Length, className);
				return null;
			}
			if (t.subtypes != null) {
				pname = className.Substring (0, subt);
				rname = className.Substring (subt + 1);
				TypeBuilder result = search_nested_in_array (t.subtypes, t.subtypes.Length, pname);
				if (result != null)
					return GetMaybeNested (result, rname);
			}
			return null;
		}

#if NET_2_0
		[ComVisible (true)]
#endif		
		public override Type GetType (string className, bool throwOnError, bool ignoreCase) {
			int subt;
			string orig = className;
			string modifiers;
			TypeBuilder result = null;

			if (types == null && throwOnError)
				throw new TypeLoadException (className);

			subt = className.IndexOfAny (type_modifiers);
			if (subt >= 0) {
				modifiers = className.Substring (subt);
				className = className.Substring (0, subt);
			} else
				modifiers = null;

			if (!ignoreCase) {
				result =  name_cache [className] as TypeBuilder;
			} else {
				subt = className.IndexOf ('+');
				if (subt < 0) {
					if (types != null)
						result = search_in_array (types, num_types,  className);
				} else {
					string pname, rname;
					pname = className.Substring (0, subt);
					rname = className.Substring (subt + 1);
					result = search_in_array (types, num_types, pname);
					if (result != null)
						result = GetMaybeNested (result, rname);
				}
			}
			if ((result == null) && throwOnError)
				throw new TypeLoadException (orig);
			if (result != null && (modifiers != null)) {
				Type mt = create_modified_type (result, modifiers);
				if (mt is TypeBuilder)
					result = mt as TypeBuilder;
				else
					return mt;
			}
			if (result != null && result.is_created)
				return result.CreateType ();
			else
				return result;
		}

		internal int get_next_table_index (object obj, int table, bool inc) {
			if (table_indexes == null) {
				table_indexes = new int [64];
				for (int i=0; i < 64; ++i)
					table_indexes [i] = 1;
				/* allow room for .<Module> in TypeDef table */
				table_indexes [0x02] = 2;
			}
			// Console.WriteLine ("getindex for table "+table.ToString()+" got "+table_indexes [table].ToString());
			if (inc)
				return table_indexes [table]++;
			return table_indexes [table];
		}

		public void SetCustomAttribute( CustomAttributeBuilder customBuilder) {
			if (cattrs != null) {
				CustomAttributeBuilder[] new_array = new CustomAttributeBuilder [cattrs.Length + 1];
				cattrs.CopyTo (new_array, 0);
				new_array [cattrs.Length] = customBuilder;
				cattrs = new_array;
			} else {
				cattrs = new CustomAttributeBuilder [1];
				cattrs [0] = customBuilder;
			}
		}

#if NET_2_0
		[ComVisible (true)]
#endif
		public void SetCustomAttribute( ConstructorInfo con, byte[] binaryAttribute) {
			SetCustomAttribute (new CustomAttributeBuilder (con, binaryAttribute));
		}

		public ISymbolWriter GetSymWriter () {
			return symbolWriter;
		}

		public ISymbolDocumentWriter DefineDocument (string url, Guid language, Guid languageVendor, Guid documentType)
		{
			if (symbolWriter != null)
				return symbolWriter.DefineDocument (url, language, languageVendor, documentType);
			else
				return null;
		}

		public override Type [] GetTypes ()
		{
			if (types == null)
				return new TypeBuilder [0];

			int n = num_types;
			TypeBuilder [] copy = new TypeBuilder [n];
			Array.Copy (types, copy, n);

			return copy;
		}

		public IResourceWriter DefineResource (string name, string description, ResourceAttributes attribute)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (name == String.Empty)
				throw new ArgumentException ("name cannot be empty");
			if (transient)
				throw new InvalidOperationException ("The module is transient");
			if (!assemblyb.IsSave)
				throw new InvalidOperationException ("The assembly is transient");
			ResourceWriter writer = new ResourceWriter (new MemoryStream ());
			if (resource_writers == null)
				resource_writers = new Hashtable ();
			resource_writers [name] = writer;

			// The data is filled out later
			if (resources != null) {
				MonoResource[] new_r = new MonoResource [resources.Length + 1];
				System.Array.Copy(resources, new_r, resources.Length);
				resources = new_r;
			} else {
				resources = new MonoResource [1];
			}
			int p = resources.Length - 1;
			resources [p].name = name;
			resources [p].attrs = attribute;

			return writer;
		}

		public IResourceWriter DefineResource (string name, string description)
		{
			return DefineResource (name, description, ResourceAttributes.Public);
		}

		[MonoTODO]
		public void DefineUnmanagedResource (byte[] resource)
		{
			if (resource == null)
				throw new ArgumentNullException ("resource");

			throw new NotImplementedException ();
		}

		[MonoTODO]
		public void DefineUnmanagedResource (string resourceFileName)
		{
			if (resourceFileName == null)
				throw new ArgumentNullException ("resourceFileName");
			if (resourceFileName == String.Empty)
				throw new ArgumentException ("resourceFileName");
			if (!File.Exists (resourceFileName) || Directory.Exists (resourceFileName))
				throw new FileNotFoundException ("File '" + resourceFileName + "' does not exists or is a directory.");

			throw new NotImplementedException ();
		}

#if NET_2_0
		public void DefineManifestResource (string name, Stream stream, ResourceAttributes attribute) {
			if (name == null)
				throw new ArgumentNullException ("name");
			if (name == String.Empty)
				throw new ArgumentException ("name cannot be empty");
			if (stream == null)
				throw new ArgumentNullException ("stream");
			if (transient)
				throw new InvalidOperationException ("The module is transient");
			if (!assemblyb.IsSave)
				throw new InvalidOperationException ("The assembly is transient");

			if (resources != null) {
				MonoResource[] new_r = new MonoResource [resources.Length + 1];
				System.Array.Copy(resources, new_r, resources.Length);
				resources = new_r;
			} else {
				resources = new MonoResource [1];
			}
			int p = resources.Length - 1;
			resources [p].name = name;
			resources [p].attrs = attribute;
			resources [p].stream = stream;
		}
#endif

		[MonoTODO]
		public void SetSymCustomAttribute (string name, byte[] data)
		{
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public void SetUserEntryPoint (MethodInfo entryPoint)
		{
			if (entryPoint == null)
				throw new ArgumentNullException ("entryPoint");
			if (entryPoint.DeclaringType.Module != this)
				throw new InvalidOperationException ("entryPoint is not contained in this module");
			throw new NotImplementedException ();
		}

		public MethodToken GetMethodToken (MethodInfo method)
		{
			if (method == null)
				throw new ArgumentNullException ("method");
			if (method.DeclaringType.Module != this)
				throw new InvalidOperationException ("The method is not in this module");
			return new MethodToken (GetToken (method));
		}

		public MethodToken GetArrayMethodToken (Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return GetMethodToken (GetArrayMethod (arrayClass, methodName, callingConvention, returnType, parameterTypes));
		}

#if NET_2_0
		[ComVisible (true)]
#endif
		public MethodToken GetConstructorToken (ConstructorInfo con)
		{
			if (con == null)
				throw new ArgumentNullException ("con");
			return new MethodToken (GetToken (con));
		}

		public FieldToken GetFieldToken (FieldInfo field)
		{
			if (field == null)
				throw new ArgumentNullException ("field");
			if (field.DeclaringType.Module != this)
				throw new InvalidOperationException ("The method is not in this module");
			return new FieldToken (GetToken (field));
		}

		[MonoTODO]
		public SignatureToken GetSignatureToken (byte[] sigBytes, int sigLength)
		{
			throw new NotImplementedException ();
		}

		public SignatureToken GetSignatureToken (SignatureHelper sigHelper)
		{
			if (sigHelper == null)
				throw new ArgumentNullException ("sigHelper");
			return new SignatureToken (GetToken (sigHelper));
		}

		public StringToken GetStringConstant (string str)
		{
			if (str == null)
				throw new ArgumentNullException ("str");
			return new StringToken (GetToken (str));
		}

		public TypeToken GetTypeToken (Type type)
		{
			if (type == null)
				throw new ArgumentNullException ("type");
			if (type.IsByRef)
				throw new ArgumentException ("type can't be a byref type", "type");
			if (!IsTransient () && (type.Module is ModuleBuilder) && ((ModuleBuilder)type.Module).IsTransient ())
				throw new InvalidOperationException ("a non-transient module can't reference a transient module");
			return new TypeToken (GetToken (type));
		}

		public TypeToken GetTypeToken (string type)
		{
			return GetTypeToken (GetType (name));
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern int getUSIndex (ModuleBuilder mb, string str);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern int getToken (ModuleBuilder mb, object obj);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern int getMethodToken (ModuleBuilder mb, MethodInfo method,
							  Type[] opt_param_types);

		internal int GetToken (string str) {
			if (us_string_cache.Contains (str))
				return (int)us_string_cache [str];
			int result = getUSIndex (this, str);
			us_string_cache [str] = result;
			return result;
		}

		internal int GetToken (MemberInfo member) {
			return getToken (this, member);
		}

		internal int GetToken (MethodInfo method, Type[] opt_param_types) {
			return getMethodToken (this, method, opt_param_types);
		}

		internal int GetToken (SignatureHelper helper) {
			return getToken (this, helper);
		}

		internal TokenGenerator GetTokenGenerator () {
			if (token_gen == null)
				token_gen = new ModuleBuilderTokenGenerator (this);
			return token_gen;
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private static extern void build_metadata (ModuleBuilder mb);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern void WriteToFile (IntPtr handle);

		internal void Save ()
		{
			if (transient && !is_main)
				return;

			if (types != null) {
				for (int i = 0; i < num_types; ++i)
					if (!types [i].is_created)
						throw new NotSupportedException ("Type '" + types [i].FullName + "' was not completed.");
			}

			if ((global_type != null) && (global_type_created == null))
				global_type_created = global_type.CreateType ();

			if (resources != null) {
				for (int i = 0; i < resources.Length; ++i) {
					IResourceWriter rwriter;
					if (resource_writers != null && (rwriter = resource_writers [resources [i].name] as IResourceWriter) != null) {
						ResourceWriter writer = (ResourceWriter)rwriter;
						writer.Generate ();
						MemoryStream mstream = (MemoryStream)writer.Stream;
						resources [i].data = new byte [mstream.Length];
						mstream.Seek (0, SeekOrigin.Begin);
						mstream.Read (resources [i].data, 0, (int)mstream.Length);
						continue;
					}
					Stream stream = resources [i].stream;

					// According to MSDN docs, the stream is read during assembly save, not earlier
					if (stream != null) {
						try {
							long len = stream.Length;
							resources [i].data = new byte [len];
							stream.Seek (0, SeekOrigin.Begin);
							stream.Read (resources [i].data, 0, (int)len);
						} catch {
							/* do something */
						}
					}
				}
			}

			build_metadata (this);

			string fileName = fqname;
			if (assemblyb.AssemblyDir != null)
				fileName = Path.Combine (assemblyb.AssemblyDir, fileName);

			try {
				// We mmap the file, so unlink the previous version since it may be in use
				File.Delete (fileName);
			} catch {
				// We can safely ignore
			}
			using (FileStream file = new FileStream (fileName, FileMode.Create, FileAccess.Write))
				WriteToFile (file.Handle);
			
			//
			// The constant 0x80000000 is internal to Mono, it means `make executable'
			//
			File.SetAttributes (fileName, (FileAttributes) (unchecked ((int) 0x80000000)));
			
			if (types != null && symbolWriter != null) {
				for (int i = 0; i < num_types; ++i)
					types [i].GenerateDebugInfo (symbolWriter);
				symbolWriter.Close ();
			}
		}

		internal string FileName {
			get {
				return fqname;
			}
		}

		internal bool IsMain {
			set {
				is_main = value;
			}
		}

		internal void CreateGlobalType () {
			if (global_type == null)
				global_type = new TypeBuilder (this, 0);
		}

		internal static Guid Mono_GetGuid (ModuleBuilder mb)
		{
			return new Guid (mb.guid);
		}
#if NOT_PFX
		void _ModuleBuilder.GetIDsOfNames ([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException ();
		}

		void _ModuleBuilder.GetTypeInfo (uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException ();
		}

		void _ModuleBuilder.GetTypeInfoCount (out uint pcTInfo)
		{
			throw new NotImplementedException ();
		}

		void _ModuleBuilder.Invoke (uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException ();
		}
#endif
	}

	internal class ModuleBuilderTokenGenerator : TokenGenerator {

		private ModuleBuilder mb;

		public ModuleBuilderTokenGenerator (ModuleBuilder mb) {
			this.mb = mb;
		}

		public int GetToken (string str) {
			return mb.GetToken (str);
		}

		public int GetToken (MemberInfo member) {
			return mb.GetToken (member);
		}

		public int GetToken (MethodInfo method, Type[] opt_param_types) {
			return mb.GetToken (method, opt_param_types);
		}

		public int GetToken (SignatureHelper helper) {
			return mb.GetToken (helper);
		}
	}
}

