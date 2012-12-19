using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Tables
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
				int n = _loader.Metadata.GetRowCount(TableId.MemberRef);
				_members = new ITypeMember[n];
			}

			return _members[index] ?? (_members[index] = Resolve(index, context));
		}

		private ITypeMember Resolve(int index, Context context)
		{
			var row = _loader.Metadata.GetRow(TableId.MemberRef, index);

			string name = row[Schema.MemberRef.Name].String;
			var sigBlob = row[Schema.MemberRef.Signature].Blob;
			var sig = MetadataSignature.DecodeMember(sigBlob);

			SimpleIndex ownerIndex = row[Schema.MemberRef.Class].Value;
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

		private IType GetMemberOwner(SimpleIndex owner, Context context)
		{
			int index = owner.Index - 1;
			switch (owner.Table)
			{
				case TableId.TypeDef:
					return _loader.Types[index];

				case TableId.TypeRef:
					return _loader.TypeRefs[index];

				case TableId.ModuleRef:
					throw new NotImplementedException();

				case TableId.TypeSpec:
					return _loader.GetTypeSpec(index, context);

				case TableId.MethodDef:
					throw new NotImplementedException();
			}
			return null;
		}

		private ITypeMember FindMember(IType type, string name, MetadataSignature sig, Context context)
		{
			if (type == null) return null;

			//special multidimensional array methods
			if (type.IsArray)
			{
				switch (name)
				{
					case CLRNames.Constructor:
						return CreateArrayCtor(new Context(type, context.Method), (MethodSignature)sig);

					case CLRNames.Array.Getter:
						return GetArrayGetter(new Context(type, context.Method), (MethodSignature)sig);

					case CLRNames.Array.Address:
						return GetArrayAddress(new Context(type, context.Method), (MethodSignature)sig);

					case CLRNames.Array.Setter:
						return GetArraySetter(new Context(type, context.Method), (MethodSignature)sig);
				}					
			}

			IType[] types = null;
			MemberType kind;

			var typeContext = new Context(type, context != null && context.IsGeneric);
			switch (sig.Kind)
			{
				case SignatureKind.Field:
					kind = MemberType.Field;
					break;

				case SignatureKind.Method:
					return FindMethod(type, name, (MethodSignature)sig, typeContext);

				case SignatureKind.Property:
					types = ResolveMethodSignature((MethodSignature)sig, typeContext, true);
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

		private IType[] ResolveMethodSignature(MethodSignature sig, Context context, bool withReturnType)
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

		private IMethod CreateArrayCtor(Context context, MethodSignature sig)
		{
			var type = context.Type;
			var types = ResolveMethodSignature(sig, context, false);

			var arrType = (ArrayType)type;
			var ctor = arrType.FindConstructor(types);
			if (ctor != null) return ctor;

			var m = new Method
				{
					Name = CLRNames.Constructor,
					Type = _loader.SystemTypes.Void,
					DeclaringType = type
				};

			AddParams(m, types, "n");

			m.IsSpecialName = true;
			m.IsInternalCall = true;

			arrType.Constructors.Add(m);

			return m;
		}

		private IMethod GetArrayGetter(Context context, MethodSignature sig)
		{
			var type = context.Type;
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

			var types = ResolveMethodSignature(sig, new Context(contextType, context.Method), false);
			AddParams(m, types, "i");

			return m;
		}

		private IMethod GetArrayAddress(Context context, MethodSignature sig)
		{
			var type = context.Type;
			var arrType = (ArrayType)type;
			if (arrType.Address != null)
				return arrType.Address;

			var m = new Method
				{
					Name = CLRNames.Array.Address,
					Type = _loader.ResolveType(sig.Type, context),
					IsInternalCall = true,
					DeclaringType = type
				};

			arrType.Address = m;

			var contextType = FixContextType(arrType.ElementType);

			var types = ResolveMethodSignature(sig, new Context(contextType, context.Method), false);
			AddParams(m, types, "i");

			return m;
		}

		private IMethod GetArraySetter(Context context, MethodSignature sig)
		{
			var type = context.Type;
			var arrType = (ArrayType)type;

			if (arrType.Setter != null)
				return arrType.Setter;

			var m = new Method
				{
					Name = CLRNames.Array.Setter,
					Type = _loader.SystemTypes.Void,
					IsInternalCall = true,
					DeclaringType = type
				};

			arrType.Setter = m;

			var contextType = FixContextType(arrType.ElementType);

			var types = ResolveMethodSignature(sig, new Context(contextType, context.Method), false);

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

		private IEnumerable<IMethod> GetMatchedMethods(IType type, string name, MethodSignature sig, Context context)
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

		private IMethod FindMethod(IType type, string name, MethodSignature sig, Context context)
		{
			IMethod result = null;
			while (type != null)
			{
				int curSpec = 0;
				foreach (var method in GetMatchedMethods(type, name, sig, context))
				{
					if (!method.SignatureChanged())
						return method;

					int spec = method.GetSpecificity();
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

		private bool ProbeMethodSig(IMethod method, MethodSignature sig, Context context)
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
