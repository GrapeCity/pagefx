using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal class SignatureResolver
	{
		private readonly AssemblyLoader _loader;

		public SignatureResolver(AssemblyLoader loader)
		{
			_loader = loader;
		}

		private TypeFactory TypeFactory
		{
			get { return _loader.TypeFactory; }
		}

		private SystemTypes SystemTypes
		{
			get { return _loader.SystemTypes; }
		}

		public IType ResolveType(TypeSignature sig, Context context)
		{
			switch (sig.Element)
			{
				case ElementType.Void:
					return SystemTypes.Void;
				case ElementType.Boolean:
					return SystemTypes.Boolean;
				case ElementType.Char:
					return SystemTypes.Char;
				case ElementType.Int8:
					return SystemTypes.SByte;
				case ElementType.UInt8:
					return SystemTypes.Byte;
				case ElementType.Int16:
					return SystemTypes.Int16;
				case ElementType.UInt16:
					return SystemTypes.UInt16;
				case ElementType.Int32:
					return SystemTypes.Int32;
				case ElementType.UInt32:
					return SystemTypes.UInt32;
				case ElementType.Int64:
					return SystemTypes.Int64;
				case ElementType.UInt64:
					return SystemTypes.UInt64;
				case ElementType.Single:
					return SystemTypes.Single;
				case ElementType.Double:
					return SystemTypes.Double;
				case ElementType.String:
					return SystemTypes.String;
				case ElementType.TypedReference:
					return SystemTypes.TypedReference;
				case ElementType.IntPtr:
					return SystemTypes.IntPtr;
				case ElementType.UIntPtr:
					return SystemTypes.UIntPtr;
				case ElementType.Object:
					return SystemTypes.Object;

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
						var type = ResolveType(sig.Type, context);
						if (type == null)
							throw BadTypeSig(sig);

						var args = ResolveGenericArgs(sig, context);
						return TypeFactory.MakeGenericType(type, args);
					}

				case ElementType.Var:
					{
						int index = sig.GenericParamNumber;
						var gi = context.Type as IGenericInstance;
						if (gi != null)
							return gi.GenericArguments[index];
						var gt = context.Type;
						if (gt != null)
							return gt.GenericParameters[index];
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

		private static Exception BadTypeSig(TypeSignature sig)
		{
			return new BadSignatureException(string.Format("Unable to resolve type signature {0}", sig));
		}

		private IEnumerable<IType> ResolveGenericArgs(TypeSignature sig, Context context)
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
