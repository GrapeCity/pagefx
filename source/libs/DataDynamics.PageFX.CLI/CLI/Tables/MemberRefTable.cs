using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class MemberRefTable
	{
		private readonly AssemblyLoader _loader;
		private ITypeMember[] _members;

		public MemberRefTable(AssemblyLoader loader)
		{
			_loader = loader;
		}

		public ITypeMember Get(int index, Context context)
		{
			if (_members == null)
			{
				int n = _loader.Mdb.GetRowCount(MdbTableId.MemberRef);
				_members = new ITypeMember[n];
			}

			return _members[index] ?? (_members[index] = Resolve(index, context));
		}

		private ITypeMember Resolve(int index, Context context)
		{
			var row = _loader.Mdb.GetRow(MdbTableId.MemberRef, index);

			string name = row[MDB.MemberRef.Name].String;
			var sigBlob = row[MDB.MemberRef.Signature].Blob;
			var sig = MdbSignature.DecodeSignature(sigBlob);

			MdbIndex ownerIndex = row[MDB.MemberRef.Class].Value;
			var owner = GetMemberOwner(ownerIndex, context);

			var member = FindMember(owner, name, sig, context);

			if (member == null)
			{
				//TODO: Report warning
#if DEBUG
				if (DebugHooks.BreakInvalidMemberReference)
				{
					Debugger.Break();
					FindMember(owner, name, sig, context);
				}
#endif
				throw new BadMetadataException(string.Format("Unable to resolve member ref {0}", ownerIndex));
			}

			return member;
		}

		private IType GetMemberOwner(MdbIndex owner, Context context)
		{
			int index = owner.Index - 1;
			switch (owner.Table)
			{
				case MdbTableId.TypeDef:
					return _loader.Types[index];

				case MdbTableId.TypeRef:
					return _loader.TypeRefs[index];

				case MdbTableId.ModuleRef:
					throw new NotImplementedException();

				case MdbTableId.TypeSpec:
					return _loader.GetTypeSpec(index, context);

				case MdbTableId.MethodDef:
					throw new NotImplementedException();
			}
			return null;
		}

		private ITypeMember FindMember(IType type, string name, MdbSignature sig, Context context)
		{
			if (type == null) return null;

			//special multidimensional array methods
			if (type.IsArray)
			{
				if (name == CLRNames.Constructor)
					return CreateArrayCtor(type, (MdbMethodSignature)sig);

				if (name == CLRNames.Array.Getter)
					return GetArrayGetter(type, (MdbMethodSignature)sig);

				if (name == CLRNames.Array.Address)
					return GetArrayAddress(type, (MdbMethodSignature)sig);

				if (name == CLRNames.Array.Setter)
					return GetArraySetter(type, (MdbMethodSignature)sig);
			}

			IType[] types = null;
			MemberType kind;

			var typeContext = new Context(type, context != null && context.IsGeneric);
			switch (sig.Kind)
			{
				case MdbSignatureKind.Field:
					kind = MemberType.Field;
					break;

				case MdbSignatureKind.Method:
					return FindMethod(type, name, (MdbMethodSignature)sig, typeContext);

				case MdbSignatureKind.Property:
					types = ResolveMethodSignature((MdbMethodSignature)sig, typeContext, true);
					kind = MemberType.Property;
					break;

				default:
					throw new BadSignatureException(string.Format("Invalid member signature {0}", sig));
			}

			while (type != null)
			{
				var member = FindMember(type, kind, name, types);
				if (member != null)
					return member;
				type = type.BaseType;
			}

			return null;
		}

		private IType[] ResolveMethodSignature(MdbMethodSignature sig, Context context, bool withReturnType)
		{
			int n = sig.Params.Length + (withReturnType ? 1 : 0);
			
			int i = 0;
			var types = new IType[n];
			if (withReturnType)
			{
				types[i++] = _loader.ResolveType(sig.Type, context);	
			}
			
			for (; i < n; ++i)
			{
				types[i] = _loader.ResolveType(sig.Params[i], context);
			}

			return types;
		}

		private static ITypeMember FindMember(IType type, MemberType kind, string name, IType[] types)
		{
			switch (kind)
			{
				case MemberType.Field:
					return type.Fields[name];

				case MemberType.Property:
					{
						var properties = type.Properties.Find(name);
						if (properties == null) return null;

						if (types == null || types.Length == 0)
						{
							return properties.FirstOrDefault();
						}

						return properties.FirstOrDefault(p => Signature.CheckSignature(p, name, types));
					}
			}

			return null;
		}

		private static void AddParams(IMethod method, IType[] types, string prefix)
		{
			int n = types.Length;
			for (int i = 0; i < n; ++i)
			{
				var p = new Parameter(types[i], prefix + i, i + 1);
				method.Parameters.Add(p);
			}
		}

		private IMethod CreateArrayCtor(IType type, MdbMethodSignature sig)
		{
			var types = ResolveMethodSignature(sig, new Context(type), false);

			var arrType = (ArrayType)type;
			var ctor = arrType.FindConstructor(types);
			if (ctor != null) return ctor;

			var m = new Method
				{
					Name = CLRNames.Constructor,
					Type = _loader.FindSystemType("System.Void"),
					DeclaringType = type
				};

			AddParams(m, types, "n");

			m.IsSpecialName = true;
			m.IsInternalCall = true;

			arrType.Constructors.Add(m);

			return m;
		}

		private IMethod GetArrayGetter(IType type, MdbMethodSignature sig)
		{
			var arrType = (ArrayType)type;
			if (arrType.Getter != null)
				return arrType.Getter;

			var m = new Method
				{
					Name = CLRNames.Array.Getter,
					Type = arrType.ElementType,
					IsInternalCall = true,
					DeclaringType = type,
				};
			arrType.Getter = m;

			var contextType = FixContextType(arrType.ElementType);

			var types = ResolveMethodSignature(sig, new Context(contextType), false);
			AddParams(m, types, "i");

			return m;
		}

		private IMethod GetArrayAddress(IType type, MdbMethodSignature sig)
		{
			var arrType = (ArrayType)type;
			if (arrType.Address != null)
				return arrType.Address;

			var m = new Method
				{
					Name = CLRNames.Array.Address,
					Type = _loader.ResolveType(sig.Type, new Context(type)),
					IsInternalCall = true,
					DeclaringType = type
				};

			arrType.Address = m;

			var contextType = FixContextType(arrType.ElementType);

			var types = ResolveMethodSignature(sig, new Context(contextType), false);
			AddParams(m, types, "i");

			return m;
		}

		private IMethod GetArraySetter(IType type, MdbMethodSignature sig)
		{
			var arrType = (ArrayType)type;

			if (arrType.Setter != null)
				return arrType.Setter;

			var m = new Method
				{
					Name = CLRNames.Array.Setter,
					Type = _loader.FindSystemType("System.Void"),
					IsInternalCall = true,
					DeclaringType = type
				};

			arrType.Setter = m;

			var contextType = FixContextType(arrType.ElementType);

			var types = ResolveMethodSignature(sig, new Context(contextType), false);

			int n = types.Length;
			for (int i = 0; i < n - 1; ++i)
			{
				m.Parameters.Add(new Parameter(types[i], "i" + i, i + 1));
			}
			m.Parameters.Add(new Parameter(types[n - 1], "value", n));

			return m;
		}

		private static IType FixContextType(IType type)
		{
			var gp = type as IGenericParameter;
			if (gp != null)
			{
				if (gp.DeclaringMethod != null)
					return gp.DeclaringMethod.DeclaringType;
				return gp.DeclaringType;
			}
			return type;
		}

		private IEnumerable<IMethod> GetMatchedMethods(IType type, string name, MdbMethodSignature sig, Context context)
		{
			int paramNum = sig.Params.Length;
			var set = type.Methods.Find(name);
			foreach (var method in set)
			{
				if (method.Parameters.Count != paramNum) continue;

				var methodContext = new Context(method, context.IsGeneric);

				if (context.IsGeneric)
				{
					if (!method.IsGeneric)
						continue;
					if (method.GenericParameters.Count != sig.GenericParamCount)
						continue;
				}

				if (ProbeMethodSig(method, sig, methodContext))
				{
					yield return method;
				}
			}
		}

		private IMethod FindMethod(IType type, string name, MdbMethodSignature sig, Context context)
		{
			IMethod result = null;
			while (type != null)
			{
				int curSpec = 0;
				foreach (var method in GetMatchedMethods(type, name, sig, context))
				{
					if (!method.SignatureChanged)
						return method;

					int spec = Method.GetSpecificity(method);
					if (result == null || spec > curSpec)
					{
						result = method;
						curSpec = spec;
					}
				}
				if (result != null) break;
				type = type.BaseType;
			}

			return result;
		}

		private bool ProbeMethodSig(IMethod method, MdbMethodSignature sig, Context context)
		{
			var t = _loader.ResolveType(sig.Type, context);
			if (!Signature.TypeEquals(method.Type, t))
				return false;

			int n = sig.Params.Length;
			for (int i = 0; i < n; ++i)
			{
				var p = method.Parameters[i];
				var psig = sig.Params[i];
				t = _loader.ResolveType(psig, context);

				if (!Signature.TypeEquals(p.Type, t))
					return false;
			}

			return true;
		}
	}
}
