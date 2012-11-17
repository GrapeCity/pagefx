using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI
{
	internal class SignatureResolver
	{
		private readonly AssemblyLoader _loader;

		public SignatureResolver(AssemblyLoader loader)
		{
			_loader = loader;
		}

		public IType ResolveType(MdbTypeSignature sig, Context context)
		{
			switch (sig.Element)
			{
				case ElementType.Void:
					return ResolveSystemType(SystemTypes.Void, "System.Void");
				case ElementType.Boolean:
					return ResolveSystemType(SystemTypes.Boolean, "System.Boolean");
				case ElementType.Char:
					return ResolveSystemType(SystemTypes.Char, "System.Char");
				case ElementType.Int8:
					return ResolveSystemType(SystemTypes.Int8, "System.SByte");
				case ElementType.UInt8:
					return ResolveSystemType(SystemTypes.UInt8, "System.Byte");
				case ElementType.Int16:
					return ResolveSystemType(SystemTypes.Int16, "System.Int16");
				case ElementType.UInt16:
					return ResolveSystemType(SystemTypes.UInt16, "System.UInt16");
				case ElementType.Int32:
					return ResolveSystemType(SystemTypes.Int32, "System.Int32");
				case ElementType.UInt32:
					return ResolveSystemType(SystemTypes.UInt32, "System.UInt32");
				case ElementType.Int64:
					return ResolveSystemType(SystemTypes.Int64, "System.Int64");
				case ElementType.UInt64:
					return ResolveSystemType(SystemTypes.UInt64, "System.UInt64");
				case ElementType.Single:
					return ResolveSystemType(SystemTypes.Single, "System.Single");
				case ElementType.Double:
					return ResolveSystemType(SystemTypes.Double, "System.Double");
				case ElementType.String:
					return ResolveSystemType(SystemTypes.String, "System.String");
				case ElementType.TypedReference:
					return ResolveSystemType(SystemTypes.TypedReference, "System.TypedReference");
				case ElementType.IntPtr:
					return ResolveSystemType(SystemTypes.IntPtr, "System.IntPtr");
				case ElementType.UIntPtr:
					return ResolveSystemType(SystemTypes.UIntPtr, "System.UIntPtr");
				case ElementType.Object:
					return ResolveSystemType(SystemTypes.Object, "System.Object");

				case ElementType.Ptr:
					{
						var type = ResolveType(sig.Type, context);
						if (type == null) return null;
						return TypeFactory.MakePointerType(type);
					}

				case ElementType.ByRef:
					{
						var type = ResolveType(sig.Type, context);
						if (type == null) return null;
						return TypeFactory.MakeReferenceType(type);
					}

				case ElementType.ValueType:
				case ElementType.Class:
				case ElementType.CustomArgsEnum:
					{
						var type = _loader.GetTypeDefOrRef(sig.TypeIndex, context);
						return type;
					}

				case ElementType.Array:
				case ElementType.ArraySz:
					{
						var type = ResolveType(sig.Type, context);
						if (type == null) return null;
						var dim = sig.ArrayShape.ToDimension();
						return TypeFactory.MakeArray(type, dim);
					}

				case ElementType.GenericInstantiation:
					{
						var type = ResolveType(sig.Type, context) as IGenericType;
						if (type == null)
							throw BadTypeSig(sig);

						var args = ResolveGenericArgs(sig, context);
						return TypeFactory.MakeGenericType(type, args);
					}

				case ElementType.Var:
					{
						int index = sig.GenericParamNumber;
						var gt = context.Type as IGenericType;
						if (gt != null)
							return gt.GenericParameters[index];
						var gi = context.Type as IGenericInstance;
						if (gi != null)
							return gi.GenericArguments[index];
						throw BadTypeSig(sig);
					}

				case ElementType.MethodVar:
					{
						int index = sig.GenericParamNumber;
						var contextMethod = context.Method;
						if (contextMethod == null)
							throw new BadMetadataException("Invalid method context");
						if (contextMethod.IsGenericInstance)
							return contextMethod.GenericArguments[index];
						return contextMethod.GenericParameters[index];
					}

				case ElementType.RequiredModifier:
				case ElementType.OptionalModifier:
					return ResolveType(sig.Type, context);

				case ElementType.Sentinel:
				case ElementType.Pinned:
					return ResolveType(sig.Type, context);

				case ElementType.MethodPtr:
					break;

				case ElementType.CustomArgsType:
					return SystemTypes.Type;

				case ElementType.CustomArgsBoxedObject:
					return SystemTypes.Object;

				case ElementType.CustomArgsField:
					break;

				case ElementType.CustomArgsProperty:
					break;
			}
			return null;
		}

		private IType ResolveSystemType(IType type, string fullName)
		{
			return type ?? _loader.FindSystemType(fullName);
		}

		private static Exception BadTypeSig(MdbTypeSignature sig)
		{
			return new BadSignatureException(string.Format("Unable to resolve type signature {0}", sig));
		}

		private IEnumerable<IType> ResolveGenericArgs(MdbTypeSignature sig, Context context)
		{
			int n = sig.GenericParams.Length;
			var args = new IType[n];
			for (int i = 0; i < n; ++i)
			{
				var arg = ResolveType(sig.GenericParams[i], context);
				if (arg == null)
					throw BadTypeSig(sig);
				args[i] = arg;
			}
			return args;
		}
	}
}
