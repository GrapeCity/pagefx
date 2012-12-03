//
// System.CodeDom CodeTypeReferenceExpression Class implementation
//
// Author:
//   Daniel Stodden (stodden@in.tum.de)
//   Marek Safar (marek.safar@seznam.cz)
//
// (C) 2001 Ximian, Inc.
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

using System.Runtime.InteropServices;
using System.Text;

namespace System.CodeDom
{
	[Serializable]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	public class CodeTypeReference : CodeObject
	{
		private string baseType;
		private CodeTypeReference arrayType;
		private int rank;
		private bool isInterface;

#if NET_2_0
		CodeTypeReferenceCollection typeArguments;
		CodeTypeReferenceOptions codeTypeReferenceOption;
#endif

		//
		// Constructors
		//

#if NET_2_0
		public CodeTypeReference ()
		{
		}
#endif

#if NET_2_0
		[MonoTODO("We should parse basetype from right to left in 2.0 profile.")]
#endif
		public CodeTypeReference (string baseType)
		{
			Parse (baseType);
		}

#if NET_2_0
		[MonoTODO("We should parse basetype from right to left in 2.0 profile.")]
#endif
		public CodeTypeReference (Type baseType)
		{
#if NET_2_0
			if (baseType == null) {
				throw new ArgumentNullException ("baseType");
			}

			if (baseType.IsGenericParameter) {
				this.baseType = baseType.Name;
				this.codeTypeReferenceOption = CodeTypeReferenceOptions.GenericTypeParameter;
			}
			else if (baseType.IsGenericTypeDefinition)
				this.baseType = baseType.FullName;
			else if (baseType.IsGenericType) {
				this.baseType = baseType.GetGenericTypeDefinition ().FullName;
				foreach (Type arg in baseType.GetGenericArguments ()) {
					if (arg.IsGenericParameter)
						TypeArguments.Add (new CodeTypeReference (new CodeTypeParameter (arg.Name)));
					else
						TypeArguments.Add (new CodeTypeReference (arg));
				}
			}
			else
#endif
			if (baseType.IsArray) {
				this.rank = baseType.GetArrayRank ();
				this.arrayType = new CodeTypeReference (baseType.GetElementType ());
				this.baseType = arrayType.BaseType;
			} else {
				Parse (baseType.FullName);
			}
			this.isInterface = baseType.IsInterface;
		}

		public CodeTypeReference( CodeTypeReference arrayType, int rank )
		{
			this.baseType = null;
			this.rank = rank;
			this.arrayType = arrayType;
		}

#if NET_2_0
		[MonoTODO("We should parse basetype from right to left in 2.0 profile.")]
#endif
		public CodeTypeReference( string baseType, int rank )
			: this (new CodeTypeReference (baseType), rank)
		{
		}

#if NET_2_0
		public CodeTypeReference( CodeTypeParameter typeParameter ) :
			this (typeParameter.Name)
		{
			this.codeTypeReferenceOption = CodeTypeReferenceOptions.GenericTypeParameter;
		}

		public CodeTypeReference( string typeName, CodeTypeReferenceOptions codeTypeReferenceOption ) :
			this (typeName)
		{
			this.codeTypeReferenceOption = codeTypeReferenceOption;
		}

		public CodeTypeReference( Type type, CodeTypeReferenceOptions codeTypeReferenceOption ) :
			this (type)
		{
			this.codeTypeReferenceOption = codeTypeReferenceOption;
		}

		public CodeTypeReference( string typeName, params CodeTypeReference[] typeArguments ) :
			this (typeName)
		{
			TypeArguments.AddRange (typeArguments);
			if (this.baseType.IndexOf ('`') < 0)
				this.baseType += "`" + TypeArguments.Count;
		}
#endif

		//
		// Properties
		//

		public CodeTypeReference ArrayElementType
		{
			get {
				return arrayType;
			}
			set {
				arrayType = value;
			}
		}
		
		public int ArrayRank {
			get {
				return rank;
			}
			set {
				rank = value;
			}
		}

		public string BaseType {
			get {
				if (arrayType != null && rank > 0) {
					return arrayType.BaseType;
				}

				if (baseType == null)
					return String.Empty;

				return baseType;
			}
			set {
				baseType = value;
			}
		}

		internal bool IsInterface {
			get { return isInterface; }
		}

		private void Parse (string baseType)
		{
			if (baseType == null || baseType.Length == 0) {
				this.baseType = typeof (void).FullName;
				return;
			}

#if NET_2_0
			int array_start = baseType.IndexOf ('[');
			if (array_start == -1) {
				this.baseType = baseType;
				return;
			}

			int array_end = baseType.LastIndexOf (']');
			if (array_end < array_start) {
				this.baseType = baseType;
				return;
			}

			string[] args = baseType.Substring (array_start + 1, array_end - array_start - 1).Split (',');

			if ((array_end - array_start) != args.Length) {
				this.baseType = baseType.Substring (0, array_start);
				int escapeCount = 0;
				int scanPos = array_start;
				StringBuilder tb = new StringBuilder();
				while (scanPos < baseType.Length) {
					char currentChar = baseType[scanPos];
					
					switch (currentChar) {
						case '[':
							if (escapeCount > 1 && tb.Length > 0) {
								tb.Append (currentChar);
							}
							escapeCount++;
							break;
						case ']':
							escapeCount--;
							if (escapeCount > 1 && tb.Length > 0) {
								tb.Append (currentChar);
							}

							if (tb.Length != 0 && (escapeCount % 2) == 0) {
								TypeArguments.Add (tb.ToString ());
								tb.Length = 0;
							}
							break;
						case ',':
							if (escapeCount > 1) {
								// skip anything after the type name until we 
								// reach the next separator
								while (scanPos + 1 < baseType.Length) {
									if (baseType[scanPos + 1] == ']') {
										break;
									}
									scanPos++;
								}
							} else if (tb.Length > 0) {
								CodeTypeReference typeArg = new CodeTypeReference (tb.ToString ());
								TypeArguments.Add (typeArg);
								tb.Length = 0;
							}
							break;
						default:
							tb.Append (currentChar);
							break;
					}
					scanPos++;
				}
			} else {
				arrayType = new CodeTypeReference (baseType.Substring (0, array_start));
				rank = args.Length;
			}
#else
			int array_start = baseType.LastIndexOf ('[');
			if (array_start == -1) {
				this.baseType = baseType;
				return;
			}

			int array_end = baseType.LastIndexOf (']');
			if (array_end < array_start) {
				this.baseType = baseType;
				return;
			}

			string[] args = baseType.Substring (array_start + 1, array_end - array_start - 1).Split (',');

			bool isArray = true;
			foreach (string arg in args) {
				if (arg.Length != 0) {
					isArray = false;
					break;
				}
			}
			if (isArray) {
				arrayType = new CodeTypeReference (baseType.Substring (0, array_start));
				rank = args.Length;
			} else {
				this.baseType = baseType;
			}
#endif
		}

#if NET_2_0
		[ComVisible (false)]
		public CodeTypeReferenceOptions Options {
			get {
				return codeTypeReferenceOption;
			}
			set {
				codeTypeReferenceOption = value;
			}
		}

		[ComVisible (false)]
		public CodeTypeReferenceCollection TypeArguments {
			get {
				if (typeArguments == null)
					typeArguments = new CodeTypeReferenceCollection ();
				return typeArguments;
			}
		}
#endif

	}
}
